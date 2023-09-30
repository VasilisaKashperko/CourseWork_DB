using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace HOAChairmanAssistant.Helpers.ValidationRules
{
    class LoginValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;
            if (char.IsDigit(charString[0]))
            {
                return new ValidationResult(false, $"Поле не может начинаться с цифры");
            }
            if (!Regex.Match(charString, "^[a-zA-Z][a-zA-Z._\\d]*$").Success)
            {
                return new ValidationResult(false, $"Поле может содержать только латинские буквы, цифры и . _");

            }

            return ValidationResult.ValidResult;
        }
    }
}
