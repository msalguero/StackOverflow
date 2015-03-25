using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace StackOverflow.Web.Controllers
{
    public class CapitalAttribute : ValidationAttribute
    {
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
         {
             string stringValue = (string) value;
             string stringValueLowercase = stringValue.ToLower();
             if (stringValue == stringValueLowercase)
                  return new ValidationResult("Must contain Capital Letter");
            return ValidationResult.Success;
        }

    }

    public class MinimumAttribute : ValidationAttribute
    {
        int _min;

        public MinimumAttribute(int min)
        {
            this._min = min;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            
            string stringValue = (string)value;
            if(stringValue.Length <_min)
                return new ValidationResult("Must Contain more than "+_min);
            return ValidationResult.Success;
        }
    }
    public class MaximumAttribute : ValidationAttribute
    {
        readonly int _max;

        public MaximumAttribute(int max)
        {
            this._max = max;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string stringValue = (string)value;
            if (stringValue.Length > _max)
                return new ValidationResult("Must Contain less than " + _max);
            return ValidationResult.Success;
        }
    }

    public class VocalAttribute : ValidationAttribute
    {
        private readonly char[] vocals = {'a', 'e', 'i', 'o', 'u'};
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string stringValue = ((string)value).ToLower();

            foreach (var vocal in vocals)
            {
                if(stringValue.Contains(vocal.ToString()))
                    return ValidationResult.Success;
            }
            return new ValidationResult("Must Contain at least 1 vocal");
        }
    }

    public class NumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string stringValue = ((string)value).ToLower();

            for (int i = 0; i < 9; i++)
            {
                if(stringValue.Contains(i.ToString()))
                    return ValidationResult.Success;
            }
            return new ValidationResult("Must Contain at least 1 number");
        }
    }

    public class RepeatedCharAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string stringValue = (string)value;

            for (int i = 1; i < stringValue.Length; i++)
            {
                if(stringValue[i] == stringValue[i-1])
                    return new ValidationResult("Must not Contain repeated chars");
            }
            return ValidationResult.Success;
        }
    }

    public class ExcludeChar : ValidationAttribute
    {
        private readonly string _chars;

        public ExcludeChar(string chars)
            : base("{0} contains invalid character.")
        {
            _chars = chars;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                for (int i = 0; i < _chars.Length; i++)
                {
                    var valueAsString = value.ToString();
                    if (valueAsString.Contains(_chars[i].ToString()))
                    {
                        return new ValidationResult("Must contain only letter and numbers");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }

    public class MinimumWordsAttribute : ValidationAttribute
    {
        int _min;

        public MinimumWordsAttribute(int min)
        {
            this._min = min;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            string stringValue = (string)value;
            string[] words = stringValue.Split(' ');
            if (words.Count() < _min || (words.Last() == "" && words.Count()==_min))
                return new ValidationResult("Must Contain more than " + _min+" words");
            return ValidationResult.Success;
        }
    }
}