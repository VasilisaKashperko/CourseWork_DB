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
    public class OnlyLettersRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;
            if (!Regex.Match(charString, "^[a-zA-ZА-Яа-я][a-zA-ZА-Яа-я -]*[a-zA-ZА-Яа-я]$").Success)
            {
                return new ValidationResult(false, $"Поле может содержать только буквы");
            }

            return ValidationResult.ValidResult;
        }
    }
}
