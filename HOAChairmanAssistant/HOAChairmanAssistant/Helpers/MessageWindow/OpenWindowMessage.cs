using HOAChairmanAssistant.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HOAChairmanAssistant.Helpers.MessageWindow
{
    class OpenWindowMessage
    {
        public WindowType Type { get; set; }
        public User Argument { get; set; }
    }
}
