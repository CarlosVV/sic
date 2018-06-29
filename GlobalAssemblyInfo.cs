using System.Reflection;
using System.Runtime.InteropServices;

// All the assembly information that is common
// across multiple projects
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("")]
[assembly: AssemblyCopyright("Copyright © SIC 2016")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version when a core piece is written or reworked. Static item assigned by the team. It should not change during the development cycle of a product release.
//      Minor Version when a new sprint has started. For instance, 1408 comes from the year and month index.e. 2014 for year and 08 for August.
//      Build Number the current day plus the day of year when a new build is executed from 0 to 365. For instance, 23001
//      Revision the seconds in the current day when a new build is executed.

// Where other assemblies that reference your assembly will look. If this number changes,
// other assemblies have to update their references to your assembly.
// Format used: <major>.<minor>

[assembly: AssemblyVersion("0.1605.1800")]

// Used for deployment. You can increase this number for every deployment.
// It is used by setup programs. Use it to mark assemblies that have the same AssemblyVersion,
// but are generated from different builds.
// Format used: <major>.<minor>.<build>.<revision>
// Revision is used for development stage and hot fixes.

//[assembly: AssemblyFileVersion("1.1408.20232.20693")]

// The Product version of the assembly.
// This is the version you would use when talking to customers or for display on your website.
// This version can be a string, like '1.0 Release Candidate'.
// Format used: <major>.<minor>.<maintenance>

[assembly: AssemblyInformationalVersion("0.1605.18000.0")]