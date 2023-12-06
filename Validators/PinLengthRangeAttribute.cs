using System.ComponentModel.DataAnnotations;

namespace WebApi.CustomValidator.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PinLengthRangeAttribute : ValidationAttribute
    {
        private int _length;

        public PinLengthRangeAttribute(int length)
        {
            _length = length;
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                ErrorMessage = "Property value cannot be null.";
                return false;
            }

            if (value is not int)
            {
                ErrorMessage = "Property must be an integer.";
                return false;
            }

            string numberString = value.ToString();

            if (numberString.Length != _length)
            {
                ErrorMessage = string.Format("The property value must have a length of {0} digits.", _length);
                return false;
            }

            return true;
        }
    }
}
