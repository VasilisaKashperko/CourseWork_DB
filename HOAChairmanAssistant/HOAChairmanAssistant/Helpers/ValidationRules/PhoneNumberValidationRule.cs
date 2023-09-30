using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace HOAChairmanAssistant.Helpers.ValidationRules
{
    class PhoneNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string number = value as string;
            if (Regex.Match(number, @"^(\+375[0-9]{9})$").Success == true)
                return ValidationResult.ValidResult;
            if (Regex.Match(number, @"^(80[0-9]{9})$").Success == true)
                return ValidationResult.ValidResult;
            else
                return new ValidationResult(false, "Некорректный номер");
        }
    }
}
