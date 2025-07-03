using FluentValidation;
using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Validators
{
    public class CheckoutRequestDtoValidator : AbstractValidator<CheckoutRequestDto>
    {
        public CheckoutRequestDtoValidator()
        {
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required")
                .Must(items => items.Count <= 50).WithMessage("Maximum 50 items allowed per order");

            RuleForEach(x => x.Items).SetValidator(new OrderItemDtoValidator());

            RuleFor(x => x.ShippingDetails)
                .NotNull().WithMessage("Shipping details are required")
                .SetValidator(new ShippingDetailsDtoValidator());

            RuleFor(x => x.PaymentDetails)
                .NotNull().WithMessage("Payment details are required")
                .SetValidator(new PaymentDetailsDtoValidator());
        }
    }

    public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
    {
        public OrderItemDtoValidator()
        {
            RuleFor(x => x.VinylRecordId)
                .GreaterThan(0).WithMessage("Invalid vinyl record ID");

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required")
                .MaximumLength(200).WithMessage("Product name must not exceed 200 characters");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0")
                .LessThanOrEqualTo(100).WithMessage("Maximum quantity per item is 100");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0")
                .LessThanOrEqualTo(10000).WithMessage("Price cannot exceed 10000");
        }
    }

    public class ShippingDetailsDtoValidator : AbstractValidator<ShippingDetailsDto>
    {
        public ShippingDetailsDtoValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required")
                .MaximumLength(100).WithMessage("Full name must not exceed 100 characters");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required")
                .MaximumLength(200).WithMessage("Address must not exceed 200 characters");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(50).WithMessage("City must not exceed 50 characters");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Postal code is required")
                .MaximumLength(20).WithMessage("Postal code must not exceed 20 characters");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(50).WithMessage("Country must not exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format");
        }
    }

    public class PaymentDetailsDtoValidator : AbstractValidator<PaymentDetailsDto>
    {
        public PaymentDetailsDtoValidator()
        {
            RuleFor(x => x.PaymentMethod)
                .NotEmpty().WithMessage("Payment method is required")
                .Must(method => new[] { "Credit Card", "Debit Card", "PayPal" }.Contains(method))
                .WithMessage("Invalid payment method");

            RuleFor(x => x.CardNumber)
                .NotEmpty().WithMessage("Card number is required")
                .CreditCard().WithMessage("Invalid card number format");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required")
                .Matches(@"^(0[1-9]|1[0-2])\/\d{2}$").WithMessage("Expiry date must be in MM/YY format")
                .Must(BeValidExpiryDate).WithMessage("Card has expired");

            RuleFor(x => x.CVV)
                .NotEmpty().WithMessage("CVV is required")
                .Matches(@"^\d{3,4}$").WithMessage("CVV must be 3 or 4 digits");
        }

        private static bool BeValidExpiryDate(string expiryDate)
        {
            if (string.IsNullOrEmpty(expiryDate) || !expiryDate.Contains('/'))
                return false;

            var parts = expiryDate.Split('/');
            if (parts.Length != 2)
                return false;

            if (!int.TryParse(parts[0], out var month) || !int.TryParse(parts[1], out var year))
                return false;

            var currentDate = DateTime.Now;
            var cardDate = new DateTime(2000 + year, month, 1).AddMonths(1).AddDays(-1);

            return cardDate >= currentDate;
        }
    }
}