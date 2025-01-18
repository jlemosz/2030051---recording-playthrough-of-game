using UnityEngine;
using UnityEditor;
using System.Collections;
using Unity.EditorCoroutines.Editor;

public static class BuildAutomationHooks
{
    private static bool isPlaythroughComplete = false;
    private static EditorCoroutine playthroughCoroutine;

    // This method is called before the build starts
    public static void PreExport()
    {
        Debug.Log("Cloud Build PreExport method called");
        // You can do any pre-build setup here
    }

    // This method is called after the build completes
    public static void PostExport(string path)
    {
        Debug.Log("Cloud Build PostExport method called. Starting playthrough...");
        
        // Reset the completion flag
        isPlaythroughComplete = false;

        // Start the game
        EditorApplication.isPlaying = true;

        // Start a coroutine to wait for the playthrough to complete
        playthroughCoroutine = EditorCoroutineUtility.StartCoroutine(WaitForPlaythrough(path), "BuildAutomationPlaythrough");
    }

    private static IEnumerator WaitForPlaythrough(string buildPath)
    {
        Debug.Log("Waiting for playthrough to complete...");
        while (!isPlaythroughComplete)
        {
            yield return new EditorWaitForSeconds(3f);
        }

        // Stop the game
        EditorApplication.isPlaying = false;

        // Perform any post-playthrough actions
        Debug.Log("Playthrough completed. Performing post-playthrough actions...");
        // Add your post-playthrough logic here (e.g., processing recorded data)
        
        // You might want to do something with the build at buildPath here
        Debug.Log($"Build path: {buildPath}");

        // Signal that the post-export process is complete
        EditorCoroutineUtility.StopCoroutine(playthroughCoroutine);
    }

    // Call this method from your game code when the playthrough is complete
    public static void SignalPlaythroughComplete()
    {
        Debug.Log("Playthrough signaled as complete.");
        isPlaythroughComplete = true;
    }
}
