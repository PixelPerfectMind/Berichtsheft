using System;

namespace Berichtsheft.Classes {

    /// <summary>
    /// This class contains the default values for the application, which shouldn't<br></br>
    /// appear in the settings file.
    /// </summary>
    internal class DefaultValues {

        /// <summary>
        /// Returns a bool value if the current release is a Dev release.
        /// </summary>
        public bool IsDevRelease() {
            return true;                                            // Check or uncheck the following line to enable or disable the Dev release attribute.
        }

        /// <summary>
        /// Returns the Date when the time bomb explodes
        /// In Alpha- and Beta releases.
        /// </summary>
        public DateTime TimeBomb() {
            return new DateTime(2023, 12, 31);                      // If the current release is a Dev release, change the date to the end of the current month.
        }

        /// <summary>
        /// Returns the version of the program.
        /// </summary>
        public string ProgramVersion() {
            return "1.1.-amd64-OS-alpha";                            // Return the version string.
        }
    }
}