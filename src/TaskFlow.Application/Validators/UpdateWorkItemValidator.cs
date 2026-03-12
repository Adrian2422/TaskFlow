using FluentValidation;
using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Validators;

public class UpdateWorkItemValidator : AbstractValidator<UpdateWorkItemDto>
{
    public UpdateWorkItemValidator()
    {
        RuleFor(x => x.Title).NotNull().NotEmpty().MaximumLength(200).When(x => x.Title != null);
        RuleFor(x => x.Description).MaximumLength(1000).When(x => x.Description != null);
    }
}