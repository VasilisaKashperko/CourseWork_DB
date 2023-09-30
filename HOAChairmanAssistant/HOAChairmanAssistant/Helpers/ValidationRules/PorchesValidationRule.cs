using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using HOAChairmanAssistant.Helpers.GlobalData;

namespace HOAChairmanAssistant.Helpers.ValidationRules
{
    class PorchesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int charString = Convert.ToInt32(value);
            cultureInfo = CultureInfo.CurrentCulture;
            if (GlobalData.GlobalData.NumberOfFlats < charString)
            {
                return new ValidationResult(false, $"Количество подъездов не может быть больше количества квартир.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
