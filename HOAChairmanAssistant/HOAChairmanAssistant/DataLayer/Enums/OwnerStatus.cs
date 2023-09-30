using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace HOAChairmanAssistant.Model
{
    public enum OwnerStatus
    {
        [Description("Живет")]
        Lives = 0,
        [Description("Сдает")]
        Rents,
        [Description("Не задан")]
        Undefined
    }
}
