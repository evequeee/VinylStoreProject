using FluentValidation;
using ProjectVinylStore.Business.DTOs;

namespace ProjectVinylStore.Business.Validators
{
    public class VinylSearchDtoValidator : AbstractValidator<VinylSearchDto>
    {
        public VinylSearchDtoValidator()
        {
            When(x => !string.IsNullOrEmpty(x.SearchTerm), () =>
            {
                RuleFor(x => x.SearchTerm)
                    .MaximumLength(100).WithMessage("Search term must not exceed 100 characters")
                    .MinimumLength(2).WithMessage("Search term must be at least 2 characters");
            });

            When(x => !string.IsNullOrEmpty(x.Genre), () =>
            {
                RuleFor(x => x.Genre)
                    .MaximumLength(50).WithMessage("Genre must not exceed 50 characters");
            });

            When(x => !string.IsNullOrEmpty(x.Artist), () =>
            {
                RuleFor(x => x.Artist)
                    .MaximumLength(100).WithMessage("Artist must not exceed 100 characters");
            });

            When(x => x.MinPrice.HasValue, () =>
            {
                RuleFor(x => x.MinPrice)
                    .GreaterThanOrEqualTo(0).WithMessage("Minimum price cannot be negative");
            });

            When(x => x.MaxPrice.HasValue, () =>
            {
                RuleFor(x => x.MaxPrice)
                    .GreaterThanOrEqualTo(0).WithMessage("Maximum price cannot be negative")
                    .LessThanOrEqualTo(50000).WithMessage("Maximum price cannot exceed 50000");
            });

            When(x => x.MinPrice.HasValue && x.MaxPrice.HasValue, () =>
            {
                RuleFor(x => x.MinPrice)
                    .LessThanOrEqualTo(x => x.MaxPrice).WithMessage("Minimum price cannot be greater than maximum price");
            });

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100");
        }
    }
}