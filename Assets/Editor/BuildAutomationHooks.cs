using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

public class BuildAutomationHooks : IPostprocessBuildWithReport
{
    private const string PLAYTHROUGH_SETUP_FILE = "playthrough_setup.txt";
    
    public int callbackOrder => 0;

    // This method is called before the build starts
    public static void PreExport()
    {
        Debug.Log("Build Automation PreExport method called");
        // Any pre-build setup can go here
    }

    // This method is called after the build completes
    public static void PostExport(string path)
    {
        Debug.Log($"Build Automation PostExport method called for build at path: {path}");
        SetupPlaythrough(path);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        string buildPath = report.summary.outputPath;
        Debug.Log($"Build completed at: {buildPath}");
        SetupPlaythrough(Path.GetDirectoryName(buildPath));
    }

    private static void SetupPlaythrough(string buildPath)
    {
        // Create a file to indicate that this build should run a playthrough
        string setupFilePath = Path.Combine(buildPath, PLAYTHROUGH_SETUP_FILE);
        File.WriteAllText(setupFilePath, "Playthrough required");
        
        Debug.Log($"Playthrough setup file created at: {setupFilePath}");
        Debug.Log("The built game will run a playthrough when launched externally.");

        // Add any other setup logic here
        // For example, you could copy necessary assets or configuration files
    }
}