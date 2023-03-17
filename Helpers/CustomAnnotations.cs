namespace CustomAnnotations;
using System.ComponentModel.DataAnnotations;

public class RequiredIfNotAttribute : RequiredAttribute
{
    private string _otherPropertyName;
    private object _otherPropertyValue;

    public RequiredIfNotAttribute(string otherPropertyName, object otherPropertyValue)
    {
        _otherPropertyName = otherPropertyName;
        _otherPropertyValue = otherPropertyValue;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var instance = validationContext.ObjectInstance;
        var otherPropertyInfo = instance.GetType().GetProperty(_otherPropertyName);
        var otherPropertyValue = otherPropertyInfo.GetValue(instance, null);
        if (otherPropertyValue != null && !otherPropertyValue.Equals(_otherPropertyValue))
        {

            return base.IsValid(value, validationContext);
        }

        return ValidationResult.Success;
    }
}
public class DateTimeAfterCurrentDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if(value == null) return ValidationResult.Success;
        var dateTime = (DateTime)value;
        if (dateTime.Date >= DateTime.Now.Date)
        {
            return ValidationResult.Success;
        }
        return new ValidationResult($"Should be after current date and time.");
    }

}

public class DateTimeAfterAttribute : ValidationAttribute
{
    private readonly string _otherPropertyName;

    public DateTimeAfterAttribute(string otherPropertyName)
    {
        _otherPropertyName = otherPropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
        if (otherPropertyInfo == null)
        {
            return new ValidationResult($"Unknown property: {_otherPropertyName}");
        }

        var otherPropertyValue = (DateTime)otherPropertyInfo.GetValue(validationContext.ObjectInstance);
        var thisPropertyValue = (DateTime)value;

        if (thisPropertyValue > otherPropertyValue)
        {
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult($"Should be after {_otherPropertyName}.");
        }
    }
}