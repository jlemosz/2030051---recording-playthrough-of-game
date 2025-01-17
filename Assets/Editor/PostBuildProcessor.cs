using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.Collections;

public class PostBuildProcessor : IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    private static bool isPlaythroughComplete = false;

    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("Build completed. Starting playthrough...");
        
        // Reset the completion flag
        isPlaythroughComplete = false;

        // Start the game
        EditorApplication.isPlaying = true;

        // Register the update function to check for completion
        EditorApplication.update += CheckPlaythroughCompletion;
    }

    private void CheckPlaythroughCompletion()
    {
        if (isPlaythroughComplete)
        {
            // Unregister the update function
            EditorApplication.update -= CheckPlaythroughCompletion;

            // Stop the game
            EditorApplication.isPlaying = false;

            // Perform any post-playthrough actions
            Debug.Log("Playthrough completed. Performing post-playthrough actions...");
            // Add your post-playthrough logic here (e.g., processing recorded data)

            // Signal that the build process can continue
            EditorApplication.ExecuteMenuItem("File/Exit");
        }
    }

    // This custom yield instruction allows you to wait for the playthrough to complete
    public class WaitForPlaythroughCompletion : CustomYieldInstruction
    {
        public override bool keepWaiting => !isPlaythroughComplete;
    }

    // Call this method from your game code when the playthrough is complete
    public static void SignalPlaythroughComplete()
    {
        isPlaythroughComplete = true;
    }
}