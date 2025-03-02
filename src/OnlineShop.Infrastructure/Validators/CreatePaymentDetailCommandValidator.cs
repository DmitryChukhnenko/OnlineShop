using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.PaymentDetails.Commands;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Infrastructure.Validators;

public class CreatePaymentDetailCommandValidator : AbstractValidator<CreatePaymentDetailCommand>
{
    public CreatePaymentDetailCommandValidator(ApplicationDbContext context)
    {
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .MustAsync(async (command, amount, cancellationToken) => 
            {
                var order = await context.Orders
                    .FirstOrDefaultAsync(o => o.Id == command.OrderId, cancellationToken);
                
                if (order == null)
                {
                    return false;
                }
                
                return amount == order.Total;
            })
            .WithMessage("Payment amount does not match order total");
        
        RuleFor(x => x.CardHolderName)
            .NotEmpty().WithMessage("Card holder name is required");
        
        RuleFor(x => x.CardNumber)
            .NotEmpty().WithMessage("Card number is required")            
            .Matches("^[0-9]+$");
        
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required");
        
        RuleFor(x => x.CVV)
            .NotEmpty().WithMessage("CVV is required");
        
        RuleFor(x => x.ExpirationDate)
            .NotEmpty().WithMessage("Expiration Date is required");
        
        RuleFor(x => x.OrderId)
            .NotEmpty().WithMessage("OrderId is required")
            .MustAsync(async (orderId, cancellationToken) => 
                await context.Orders.AnyAsync(o => o.Id == orderId, cancellationToken))
            .WithMessage("Order not found");     
    }
}