using System;

namespace uRADMonitorX {

    public class ArgumentDescriptionAttribute : Attribute {

        public String Parameter { get; private set; }
        public String HelpText { get; private set; }

        public ArgumentDescriptionAttribute(String parameter, String helpText) {
            this.Parameter = parameter;
            this.HelpText = helpText;
        }
    }
}
