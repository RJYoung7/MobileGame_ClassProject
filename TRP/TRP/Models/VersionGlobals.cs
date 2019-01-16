
namespace TRP.Models
{
    //Global Variables
    public static class VersionGlobals
    {
        // Data Version Major and Minor update when the json formats change.  

        // Major is breaking change
        public const int VersionDataMajor = 1;

        // Minior is compatable change
        public const int VersionDataMinor = 1;

        // Code Version Major and Minor update when the code changes.  

        // Major is breaking change
        public const int VersionCodeMajor = 1;

        // Minior is compatable change
        public const int VersionCodeMinor = 1;

        // Return a combindation of the Major and Minor values
        public static string GetCodeVersion()
        {
            return VersionCodeMajor + "." + VersionCodeMinor;
        }

        // Return a combindation of the Major and Minor values
        public static string GetDataVersion()
        {
            return VersionCodeMajor + "." + VersionCodeMinor;
        }

        // Return a combindation of the Major and Minor values
        public static string GetCombinedVersion()
        {
            return "Version: " + GetCodeVersion() + " Data: " + GetDataVersion();
        }
    }
}
