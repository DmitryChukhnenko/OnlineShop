using Hangfire;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Services;

public class BackgroundJobService
{
    private readonly IBackgroundJobClient _backgroundJob;
    private readonly IUnitOfWork _unitOfWork;

    public BackgroundJobService(
        IBackgroundJobClient backgroundJob,
        IUnitOfWork unitOfWork)
    {
        _backgroundJob = backgroundJob;
        _unitOfWork = unitOfWork;
    }

    public void ScheduleOrderStatusUpdate(Guid orderId)
    {
        _backgroundJob.Schedule(() => 
            UpdateOrderStatusAsync(orderId), 
            TimeSpan.FromMinutes(3));
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task UpdateOrderStatusAsync(Guid orderId)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(orderId)!;
        if (order == null) return;
        
        // Логика обновления статуса (пример)
        order.Statuses.Add(new OrderStatus() {
            Status = (int) OrderStatusEnum.Delivered,
            CurrentLocation = order.User.Address,
            Description = "Order has been delivered to the customer."
        });
        await _unitOfWork.CommitAsync();
    }
}