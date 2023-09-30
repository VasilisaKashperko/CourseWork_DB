using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace HOAChairmanAssistant.Helpers.ValidationRules
{
    class OnlyNumbersRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;
            if (!Regex.Match(charString, "^[0-9 ]+$").Success)
            {
                return new ValidationResult(false, $"Поле может содержать только цифры");
            }

            return ValidationResult.ValidResult;
        }
    }
}
