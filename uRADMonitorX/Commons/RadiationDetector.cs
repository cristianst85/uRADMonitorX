using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace uRADMonitorX.Commons {

    /// <summary>
    /// The information about coversion factors was taken from: 
    /// https://github.com/radhoo/uradmonitor_kit1/blob/master/code/misc/detectors.cpp
    /// </summary>
    public class RadiationDetector {

        private static readonly ICollection<RadiationDetector> detectors = new Collection<RadiationDetector>() {
            RadiationDetector.SBM19,
            RadiationDetector.SBM20,
            RadiationDetector.SBM21,
            RadiationDetector.SI1G,
            RadiationDetector.SI3BG,
            RadiationDetector.SI22G,
            RadiationDetector.SI29BG,
            RadiationDetector.STS5
        };

        public String Name { get; private set; }
        public double Factor { get; private set; }

        private RadiationDetector(String detectorName, double factor) {
            this.Name = detectorName;
            this.Factor = factor;
        }

        public static RadiationDetector SBM19 {
            get {
                return new RadiationDetector("SBM19", 0.001500);
            }
        }

        public static RadiationDetector SBM20 {
            get {
                return new RadiationDetector("SBM20", 0.006315);
            }
        }

        public static RadiationDetector SBM21 {
            get {
                return new RadiationDetector("SBM21", 0.048000);
            }
        }

        public static RadiationDetector SI1G {
            get {
                return new RadiationDetector("SI1G", 0.006000);
            }
        }

        public static RadiationDetector SI3BG {
            get {
                return new RadiationDetector("SI3BG", 0.631578);
            }
        }

        public static RadiationDetector SI22G {
            get {
                return new RadiationDetector("SI22G", 0.001714);
            }
        }

        public static RadiationDetector SI29BG {
            get {
                return new RadiationDetector("SI29BG", 0.010000);
            }
        }

        public static RadiationDetector STS5 {
            get {
                return new RadiationDetector("STS5", 0.006666);
            }
        }

        public static String Normalize(String detectorName) {

            if (detectorName != null) {
                detectorName = detectorName.Trim()                      // Trim leading and trailing white-spaces.               
                                           .Replace("-", String.Empty); // Remove dashes. (e.g.: fw version 110 / SI29-BG detector)
            }

            return detectorName;
        }

        public static bool IsKnown(String detectorName) {

            if (detectorName == null) {
                throw new ArgumentNullException("detectorName");
            }

            detectorName = detectorName.Trim(); // Trim leading and trailing white-spaces.

            foreach (RadiationDetector detector in detectors) {
                if (detector.Name.Equals(detectorName, StringComparison.OrdinalIgnoreCase)) {
                    return true;
                }
            }
            return false;
        }

        public static RadiationDetector GetByName(String detectorName) {

            if (detectorName == null) {
                throw new ArgumentNullException("detectorName");
            }

            detectorName = detectorName.Trim(); // Trim leading and trailing white-spaces.

            if (detectorName.Length == 0) {
                throw new ArgumentException("detectorName");
            }

            foreach (RadiationDetector detector in detectors) {
                if (detector.Name.Equals(detectorName, StringComparison.OrdinalIgnoreCase)) {
                    return detector;
                }
            }
            throw new Exception(String.Format("Unknown radiation detector ({0}).", detectorName));
        }
    }
}
