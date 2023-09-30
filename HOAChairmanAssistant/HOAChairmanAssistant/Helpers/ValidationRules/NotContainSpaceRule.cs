using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace HOAChairmanAssistant.Helpers.ValidationRules
{
    class NotContainSpaceRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            cultureInfo = CultureInfo.CurrentCulture;
            return value.ToString().Contains(" ") ? new ValidationResult(false, "Пробелы не разрешены в этом поле")
            : ValidationResult.ValidResult;
        }
    }
}
