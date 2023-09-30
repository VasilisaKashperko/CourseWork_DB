using HOAChairmanAssistant.Model;
using System;
using System.Globalization;
using System.Windows.Data;

namespace HOAChairmanAssistant.Helpers.Converters
{
    class EnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
    object parameter, CultureInfo culture)
        {
            if (value == null) return "";
            if (value is OwnerStatus)
            {
                switch ((OwnerStatus)value)
                {
                    case OwnerStatus.Lives:
                        {
                            return "Живет";
                        }
                    case OwnerStatus.Rents:
                        {
                            return "Сдает";
                        }
                    case OwnerStatus.Undefined:
                        {
                            return "Не задан";
                        }
                }
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {

            if (value == null) return "";
            switch (value.ToString())
            {
                case "Живет":
                    {
                        return OwnerStatus.Lives;
                    }
                case "Сдает":
                    {
                        return OwnerStatus.Rents;
                    }
                case "Не задан":
                    {
                        return OwnerStatus.Undefined;
                    }
            }
            return null;
        }
    }
}
