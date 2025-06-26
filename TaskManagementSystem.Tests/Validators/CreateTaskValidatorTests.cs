using System;
using FluentAssertions;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Validators;
using TaskManagementSystem.Domain.Enums;
using Xunit;

namespace TaskManagementSystem.Tests.Validators
{
    public class CreateTaskValidatorTests
    {
        private readonly CreateTaskValidator _validator;

        public CreateTaskValidatorTests()
        {
            _validator = new CreateTaskValidator();
        }

        [Fact]
        public void Should_Pass_When_ValidData()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "Valid Task",
                Description = "Valid Description",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskPriority.Medium
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_TitleIsEmpty()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "",
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskPriority.Medium
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title");
        }

        [Fact]
        public void Should_Fail_When_DueDateIsInPast()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "Valid Task",
                DueDate = DateTime.UtcNow.AddDays(-1),
                Priority = TaskPriority.Medium
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "DueDate");
        }

        [Fact]
        public void Should_Fail_When_TitleExceedsMaxLength()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = new string('a', 201),
                DueDate = DateTime.UtcNow.AddDays(1),
                Priority = TaskPriority.Medium
            };

            // Act
            var result = _validator.Validate(dto);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == "Title");
        }
    }
}
