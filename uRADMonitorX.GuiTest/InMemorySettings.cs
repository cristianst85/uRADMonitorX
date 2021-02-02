using uRADMonitorX.Configuration;

namespace uRADMonitorX.GuiTest
{
    public class InMemorySettings : Settings
    {
        public InMemorySettings() : base()
        {
        }

        public override void Save()
        {
            ; // Nothing to do here.
        }
    }
}
