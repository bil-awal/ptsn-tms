using FluentValidation;
using System;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Validators
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title must not exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");

            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future");

            RuleFor(x => x.Priority)
                .IsInEnum().WithMessage("Invalid priority value");
        }
    }
}
