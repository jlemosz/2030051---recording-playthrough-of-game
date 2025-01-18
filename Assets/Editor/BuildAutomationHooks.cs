using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildAutomationHooks : IPostprocessBuildWithReport
{
    private const string PLAYTHROUGH_COMPLETE_SYMBOL = "PLAYTHROUGH_COMPLETE";
    
    public int callbackOrder => 0;

    // This method is called before the build starts
    public static void PreExport()
    {
        Debug.Log("Build Automation PreExport method called");
        // Remove the completion symbol if it exists from a previous build
        RemoveScriptingDefineSymbol(PLAYTHROUGH_COMPLETE_SYMBOL);
    }

    // This method is called after the build completes
    public static void PostExport(string path)
    {
        Debug.Log("Build Automation PostExport method called. Starting playthrough...");
        
        // Set up for playthrough
        SetupPlaythrough();

        // The actual playthrough will happen when the game is run
        Debug.Log("Playthrough setup complete. The playthrough will occur when the game is run.");
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
        {
            string buildPath = report.summary.outputPath;
            Debug.Log($"Build completed at: {buildPath}");

            // Run the built executable
            System.Diagnostics.Process.Start(buildPath);

            // Wait for the playthrough to complete
            while (!PlayerPrefs.HasKey(PLAYTHROUGH_COMPLETE_SYMBOL))
            {
                System.Threading.Thread.Sleep(1000); // Wait for 1 second
            }

            Debug.Log("Playthrough completed.");

            // Perform any post-playthrough actions here
            // ...

            // Clean up
            PlayerPrefs.DeleteKey(PLAYTHROUGH_COMPLETE_SYMBOL);
        }
    }

    private static void SetupPlaythrough()
    {
        // Add any necessary setup for the playthrough
        AddScriptingDefineSymbol("BUILD_AUTOMATION_PLAYTHROUGH");
    }

    // Call this method from your game code when the playthrough is complete
    public static void SignalPlaythroughComplete()
    {
        Debug.Log("Playthrough signaled as complete.");
        PlayerPrefs.SetInt(PLAYTHROUGH_COMPLETE_SYMBOL, 1);
        PlayerPrefs.Save();
#if UNITY_EDITOR
        EditorApplication.Exit(0);
#else
        Application.Quit();
#endif
    }

    private static void AddScriptingDefineSymbol(string symbol)
    {
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (!defines.Contains(symbol))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                (defines + ";" + symbol).Trim(';')
            );
        }
    }

    private static void RemoveScriptingDefineSymbol(string symbol)
    {
        string defines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        if (defines.Contains(symbol))
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                defines.Replace(symbol, "").Replace(";;", ";").Trim(';')
            );
        }
    }
}