namespace uRADMonitorX.Commons.Controls.Extensions
{
    public static class ControlStateExtensions
    {
        public static bool ToBoolean(this ControlState controlState)
        {
            return controlState == ControlState.Enabled;
        }
    }
}
