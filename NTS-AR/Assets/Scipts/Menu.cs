using UnityEngine;
using UnityEngine.SceneManagement; // Required for loading scenes
using UnityEngine.UI; // Optional, but often useful for UI scripts

// Make sure there's an AudioSource component on the same GameObject as this script,
// or assign one manually in the inspector.
[RequireComponent(typeof(AudioSource))]
public class MainMenuManager : MonoBehaviour
{
    [Header("Scene Loading")]
    [Tooltip("The exact name of the scene file to load for the game.")]
    public string gameSceneName = "YourGameSceneName"; // *** CHANGE THIS in the Inspector ***

    [Header("Sound Effects")]
    [Tooltip("The sound clip to play when the sound button is clicked.")]
    public AudioClip buttonSound; // Assign your sound effect audio clip here in the Inspector

    private AudioSource audioSource;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Get the AudioSource component attached to this GameObject
        audioSource = GetComponent<AudioSource>();

        // Basic error check
        if (audioSource == null)
        {
            Debug.LogError("MainMenuManager requires an AudioSource component on the same GameObject.");
        }
    }

    /// <summary>
    /// Loads the game scene specified by 'gameSceneName'.
    /// Make sure the scene is added to the Build Settings!
    /// </summary>
    public void LoadGameScene()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("Game Scene Name is not set in the MainMenuManager script!");
            return; // Stop execution if the scene name is missing
        }

        Debug.Log("Loading scene: " + gameSceneName);
        SceneManager.LoadScene(gameSceneName);
    }

    /// <summary>
    /// Plays the assigned 'buttonSound' using the attached AudioSource.
    /// </summary>
    public void PlayButtonSound()
    {
        if (buttonSound != null && audioSource != null)
        {
            // PlayOneShot is good for non-looping sound effects
            // It doesn't interrupt other sounds playing on the source
            audioSource.PlayOneShot(buttonSound);
            Debug.Log("Playing button sound.");
        }
        else
        {
            if (buttonSound == null) Debug.LogWarning("Button Sound clip is not assigned in the MainMenuManager script!");
            if (audioSource == null) Debug.LogWarning("AudioSource component is missing or not found!");
        }
    }

    // --- Optional ---
    // You might want a function to quit the application (especially for testing on PC or non-mobile builds)
    /*
    public void QuitApplication()
    {
        Debug.Log("Quitting Application...");
        Application.Quit();

        #if UNITY_EDITOR
        // Also stop playback in the editor
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    */
}                                                                                   