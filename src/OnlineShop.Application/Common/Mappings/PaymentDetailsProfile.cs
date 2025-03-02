using AutoMapper;
using OnlineShop.Application.PaymentDetails.Commands;
using OnlineShop.Domain.DTOs;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Mappings;

public class PaymentDetailProfile : Profile
{
    public PaymentDetailProfile()
    {
        CreateMap<PaymentDetail, PaymentDetailDTO>()
            .ForMember(d => d.OrderId,
                o => o.MapFrom(s => s.Order.Id));
        
        CreateMap<CreatePaymentDetailCommand, PaymentDetail>();
    }
}