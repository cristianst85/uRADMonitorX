using System;

namespace uRADMonitorX
{
    public class ArgumentDescriptionAttribute : Attribute
    {
        public string Parameter { get; private set; }

        public string HelpText { get; private set; }

        public ArgumentDescriptionAttribute(string parameter, string helpText)
        {
            this.Parameter = parameter;
            this.HelpText = helpText;
        }
    }
}
