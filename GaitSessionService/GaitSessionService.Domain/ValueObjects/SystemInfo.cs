namespace GaitSessionService.Domain.ValueObjects
{
    public class SystemInfo
    {
        public string Software { get; private set; }
        public string Version { get; private set; }
        public string MarkerSetup { get; private set; }

        protected SystemInfo() { } // Required by EF Core

        private SystemInfo(string software, string version, string markerSetup)
        {
            Software = software;
            Version = version;
            MarkerSetup = markerSetup;
        }

        public static SystemInfo Create(string software, string version, string markerSetup)
        {
            return new SystemInfo(software, version, markerSetup);
        }
    }
}
