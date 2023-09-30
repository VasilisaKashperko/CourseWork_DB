using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace HOAChairmanAssistant
{
    public class MaximumCharacterRule : ValidationRule
    {
        public int MaximumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;
            cultureInfo = CultureInfo.CurrentCulture;

            if (charString.Length >= MaximumCharacters)
            {
                return new ValidationResult(false, $"Поле должно содержать не более {MaximumCharacters-1} символов.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
