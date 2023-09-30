using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Controls;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant
{
    class MinimumCharacterRule : ValidationRule
    {
        public int MinimumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"Это поле должно содержать не менее {MinimumCharacters} символов.");

            return ValidationResult.ValidResult;
        }
    }
}
