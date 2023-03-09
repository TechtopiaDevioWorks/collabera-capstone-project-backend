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