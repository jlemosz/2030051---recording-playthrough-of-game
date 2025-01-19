using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;

public class BuildAutomationHooks : IPostprocessBuildWithReport
{
    private const string PLAYTHROUGH_CONFIG_FILE = "playthrough_config.json";
    
    public int callbackOrder => 0;

    // This method is called after the build completes
    public static void PostExport(string path)
    {
        Debug.Log($"Build Automation PostExport method called for build at path: {path}");
        CreatePlaythroughConfig(path);
    }

    public void OnPostprocessBuild(BuildReport report)
    {
        string buildPath = report.summary.outputPath;
        Debug.Log($"Build completed at: {buildPath}");
        CreatePlaythroughConfig(Path.GetDirectoryName(buildPath));
    }

    private static void CreatePlaythroughConfig(string buildPath)
    {
        var config = new PlaythroughConfig
        {
            Duration = 5f,
            // Add any other configuration parameters you need
        };

        string configJson = JsonUtility.ToJson(config, true);
        string configPath = Path.Combine(buildPath, PLAYTHROUGH_CONFIG_FILE);
        File.WriteAllText(configPath, configJson);
        
        Debug.Log($"Playthrough config created at: {configPath}");
    }
}

[System.Serializable]
public class PlaythroughConfig
{
    public float Duration = 5f;
    // Add any other configuration parameters you need
}