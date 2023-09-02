using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using uRADMonitorX.uRADMonitor.API.V1;
using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Infrastructure;

namespace uRADMonitorX.uRADMonitor.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceDataClient deviceDataClient;
        private readonly IDeviceFactory deviceFactory;

        public DeviceService(IDeviceDataClient deviceDataClient, IDeviceFactory deviceFactory)
        {
            this.deviceDataClient = deviceDataClient;
            this.deviceFactory = deviceFactory;
        }

        public DeviceServiceResponse GetAll()
        {
            var response = this.deviceDataClient.Get();

            var deviceServiceResponse = new DeviceServiceResponse();

            if (JToken.Parse(response) is JObject jObject)
            {
                var jProperty = jObject.Properties().First();

                if (jProperty.Name.Equals("error"))
                {
                    deviceServiceResponse.SetError(jProperty.Value.ToString());
                    return deviceServiceResponse;
                }
            }

            if (!deviceServiceResponse.HasError)
            {
                var jArray = JsonConvert.DeserializeObject(response) as JArray;

                foreach (var jToken in jArray)
                {
                    var deviceDto = jToken.ToObject<DeviceDto>();

                    var device = this.deviceFactory.Create(deviceDto);
                    device.RawData = jToken.ToString(Formatting.None);

                    deviceServiceResponse.Devices.Add(device);
                }
            }

            return deviceServiceResponse;
        }
    }
}
