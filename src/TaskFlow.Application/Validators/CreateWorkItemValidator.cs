using FluentValidation;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Validators;

public class CreateWorkItemValidator : AbstractValidator<CreateWorkItemDto>
{
    public CreateWorkItemValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
    }
}