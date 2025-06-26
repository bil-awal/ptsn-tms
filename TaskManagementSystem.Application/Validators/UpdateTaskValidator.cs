using FluentValidation;
using System;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Validators
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Title)
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Title));

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters")
                .When(x => x.Description != null);

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future")
                .When(x => x.DueDate.HasValue);

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value")
                .When(x => x.Priority.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value")
                .When(x => x.Status.HasValue);
        }
    }
}
