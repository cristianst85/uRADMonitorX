using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
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

            var deviceServiceResponse = ParseResponse(response);

            if (deviceServiceResponse.HasData)
            {
                deviceServiceResponse.Devices = new Collection<Device>();

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

        private DeviceServiceResponse ParseResponse(string response)
        {
            var deviceServiceResponse = new DeviceServiceResponse();

            var jToken = JToken.Parse(response);

            if (jToken is JObject jObject)
            {
                if (jObject.Properties().First().Name.Equals("error"))
                {
                    deviceServiceResponse.Error = jObject.Properties().First().Value.ToString();
                }
            }

            return deviceServiceResponse;
        }
    }
}
