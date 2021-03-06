﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Core.Device
{
    public class RadiationDetector
    {
        private static readonly ICollection<RadiationDetector> detectors = new Collection<RadiationDetector>() {
            RadiationDetector.SBM19,
            RadiationDetector.SBM20,
            RadiationDetector.SBM20M,
            RadiationDetector.SBM21,
            RadiationDetector.SI1G,
            RadiationDetector.SI3BG,
            RadiationDetector.SI22G,
            RadiationDetector.SI29BG,
            RadiationDetector.STS5,
            RadiationDetector.LND712,
            RadiationDetector.SI8B,
            RadiationDetector.SBT10A
        };

        public string Name { get; private set; }

        public decimal? ConversionFactor { get; private set; }

        private RadiationDetector(string detectorName, decimal? conversionFactor)
        {
            this.Name = detectorName;
            this.ConversionFactor = conversionFactor;
        }

        public static RadiationDetector Unknown(string detectorName)
        {
            return new RadiationDetector(detectorName, null);
        }

        public static RadiationDetector SBM19
        {
            get
            {
                return new RadiationDetector("SBM19", 0.001500m);
            }
        }

        public static RadiationDetector SBM20
        {
            get
            {
                return new RadiationDetector("SBM20", 0.006315m);
            }
        }

        public static RadiationDetector SBM20M
        {
            get
            {
                return new RadiationDetector("SBM20M", 0.013333m);
            }
        }

        public static RadiationDetector SBM21
        {
            get
            {
                return new RadiationDetector("SBM21", 0.048000m);
            }
        }

        public static RadiationDetector SI1G
        {
            get
            {
                return new RadiationDetector("SI1G", 0.006000m);
            }
        }

        public static RadiationDetector SI3BG
        {
            get
            {
                return new RadiationDetector("SI3BG", 0.631578m);
            }
        }

        public static RadiationDetector SI22G
        {
            get
            {
                return new RadiationDetector("SI22G", 0.001714m);
            }
        }

        public static RadiationDetector SI29BG
        {
            get
            {
                return new RadiationDetector("SI29BG", 0.010000m);
            }
        }

        public static RadiationDetector STS5
        {
            get
            {
                return new RadiationDetector("STS5", 0.006666m);
            }
        }

        public static RadiationDetector LND712
        {
            get
            {
                return new RadiationDetector("LND712", 0.00594m);
            }
        }

        public static RadiationDetector SI8B
        {
            get
            {
                return new RadiationDetector("SI8B", 0.001108m);
            }
        }

        public static RadiationDetector SBT10A
        {
            get
            {
                return new RadiationDetector("SBT10A", 0.001105m);
            }
        }

        public static string Normalize(string detectorName)
        {
            if (detectorName.IsNotNullOrEmpty())
            {
                // Trim leading and trailing white-spaces and also remove dashes 
                // (e.g., in firmware version 110 the SI29BG detector is named SI29-BG)
                detectorName = detectorName.Trim().Replace("-", string.Empty);
            }

            return detectorName;
        }

        public static bool IsKnown(string detectorName)
        {
            if (detectorName.IsNull())
            {
                throw new ArgumentNullException("detectorName");
            }

            // Trim leading and trailing white-spaces.
            detectorName = detectorName.Trim();

            foreach (var detector in detectors)
            {
                if (detector.Name.Equals(detectorName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static RadiationDetector GetByName(string detectorName)
        {
            if (detectorName == null)
            {
                throw new ArgumentNullException("detectorName");
            }

            // Trim leading and trailing white-spaces.
            detectorName = detectorName.Trim();

            if (detectorName.Length == 0)
            {
                throw new ArgumentException("detectorName");
            }

            foreach (var detector in detectors)
            {
                if (detector.Name.Equals(detectorName, StringComparison.OrdinalIgnoreCase))
                {
                    return detector;
                }
            }

            throw new Exception(string.Format("Unknown radiation detector '{0}'.", detectorName));
        }

        public static bool TryGetByName(string detectorName, out RadiationDetector radiationDetector)
        {
            radiationDetector = null;

            if (detectorName.IsNullOrEmpty())
            {
                return false;
            }

            detectorName = Normalize(detectorName);

            if (IsKnown(detectorName))
            {
                radiationDetector = GetByName(detectorName);
                return true;
            }

            return false;
        }
    }
}
