using AutoMapper;
using OnlineShop.Application.Orders.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<Order, OrderDTO>()
            .ForMember(d => d.Items,
                o => o.MapFrom(s => s.Items.Select(c => c.Id)))
            .ForMember(d => d.Status,
                s => s.MapFrom(c => ((OrderStatusEnum)(c.Statuses.Last().Status)).ToString()));

        CreateMap<OrderItem, OrderItemDTO>()
            .ForMember(d => d.Product,
                o => o.MapFrom(s => s.Product));

        CreateMap<CreateOrderCommand, Order>();
    }
}