using System.Device.Location;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Extensions;
using uRADMonitorX.uRADMonitor.API.V1;

namespace uRADMonitorX.uRADMonitor.Domain
{
    public class DeviceFactory : IDeviceFactory
    {
        public Device Create(DeviceDto deviceDto)
        {
            var device = new Device
            {
                Id = deviceDto.Id,
                GeographicCoordinate = GetGeographicCoordinate(deviceDto),
                Status = deviceDto.Status.IsNullOrEmpty() ? (DeviceStatus?)null : (DeviceStatus)int.Parse(deviceDto.Status),
                Placement = deviceDto.Placement.IsNullOrEmpty() ? (DevicePlacement?)null : (DevicePlacement)int.Parse(deviceDto.Placement),
                Detector = deviceDto.DetectorName.IsNullOrEmpty() ? null : RadiationDetector.IsKnown(deviceDto.DetectorName) ? RadiationDetector.GetByName(deviceDto.DetectorName) : RadiationDetector.Unknown(deviceDto.DetectorName),
                CountryCode = deviceDto.CountryCode,
                City = deviceDto.City
            };

            return device;
        }

        private GeoCoordinate GetGeographicCoordinate(DeviceDto deviceDto)
        {
            TryParseCoordinates(deviceDto.Latitude, deviceDto.Longitude, deviceDto.Altitude, out GeoCoordinate geographicCoordinate);

            return geographicCoordinate;
        }

        private bool TryParseCoordinates(double? latitude, double? longitude, double? altitude, out GeoCoordinate geographicCoordinate)
        {
            geographicCoordinate = null;

            if (IsLatitudeValid(latitude, out string latitudeError) && IsLongitudeValid(longitude, out string longitudeError))
            {
                if (altitude.HasValue)
                {
                    geographicCoordinate = new GeoCoordinate(latitude.Value, longitude.Value, altitude.Value);
                }
                else
                {
                    geographicCoordinate = new GeoCoordinate(latitude.Value, longitude.Value);
                }

                return true;
            }

            return false;
        }

        private bool IsLatitudeValid(double? latitude, out string error)
        {
            error = null;

            if (latitude < -90.0d || 90.0d < latitude)
            {
                error = string.Format("Latitude values must be between -90 and 90 degrees. The actual value passed was {0}.", latitude);
                return false;
            }

            return true;
        }

        private bool IsLongitudeValid(double? longitude, out string error)
        {
            error = null;

            if (longitude < -180.0d || 180.0d < longitude)
            {
                error = string.Format("Longitude values must be between -180 and 180 degrees. The actual value passed was {0}.", longitude);
                return false;
            }

            return true;
        }
    }
}
