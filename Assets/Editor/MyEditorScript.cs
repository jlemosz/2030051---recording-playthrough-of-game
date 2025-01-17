using System.Collections;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

class MyEditorScript
{
    static void PerformBuild ()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] {"Assets/Scenes/SampleScene.unity"};
        buildPlayerOptions.target = BuildTarget.iOS;
        buildPlayerOptions.options = BuildOptions.None;
        buildPlayerOptions.locationPathName = "iOSBuild";
        
        //BuildPipeline.BuildPlayer(buildPlayerOptions);
        
        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded, starting playthrough...");
            EditorCoroutineUtility.StartCoroutineOwnerless(WaitForPlaythrough());
        }
        else
        {
            Debug.LogError("Build failed!");
        }
        
    }
    
    private static IEnumerator WaitForPlaythrough()
    {
        yield return new PostBuildProcessor.WaitForPlaythroughCompletion();
        Debug.Log("Playthrough and post-processing complete.");
    }
}