using System;

namespace uRADMonitorX.Commons
{
    public class EnumRegister<Enumeration> where Enumeration : struct, IConvertible
    {
        private int register;

        public EnumRegister(Enumeration flag)
        {
            this.register = Convert.ToInt32(flag);
        }

        public Enumeration State
        {
            get
            {
                return (Enumeration)Enum.ToObject(typeof(Enumeration), this.register);
            }
        }

        public bool IsSet(Enumeration flag)
        {
            int flagValue = Convert.ToInt32(flag);

            return (this.register & flagValue) == flagValue;
        }

        public Enumeration InvertFlag(Enumeration flag)
        {
            var flagValue = Convert.ToInt32(flag);

            this.register = IsSet(flag) ? this.register &= ~(int)flagValue : this.register |= (int)flagValue;

            return State;
        }
    }
}
