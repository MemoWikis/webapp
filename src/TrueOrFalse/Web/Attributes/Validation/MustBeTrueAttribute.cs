using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

[AttributeUsage(AttributeTargets.Property)]
public class MustBeTrueAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        return value != null && value is bool && (bool)value;
    }
}