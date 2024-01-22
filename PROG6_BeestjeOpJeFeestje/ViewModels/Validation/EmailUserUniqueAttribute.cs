using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Domain.Data;

namespace PROG6_BeestjeOpJeFeestje.ViewModels.Validation;
[ExcludeFromCodeCoverage]

public class EmailUserUniqueAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(
        object? value, ValidationContext validationContext)
    {
        
        if (value == null)
        {
            return new ValidationResult("Email is required.");
        }
        var context = (ApplicationDbContext?)validationContext.GetService(typeof(ApplicationDbContext));
        if (context == null)
        {
            throw new NullReferenceException("DbContext is null.");
        }
        var entity = context.Users.SingleOrDefault(e => e.Email == value.ToString());

        return entity != null ? new ValidationResult(GetErrorMessage(value.ToString())) : ValidationResult.Success;
    }

    private string GetErrorMessage(string email)
    {
        return $"Email {email} is already in use.";
    }
}