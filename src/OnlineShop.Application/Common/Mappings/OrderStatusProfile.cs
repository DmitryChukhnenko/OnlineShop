using AutoMapper;
using OnlineShop.Application.OrderStatuses.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class OrderStatusProfile : Profile
{
    public OrderStatusProfile()
    {
        CreateMap<OrderStatus, OrderStatusDTO>();
        CreateMap<CreateOrderStatusCommand, Order>();
    }
}