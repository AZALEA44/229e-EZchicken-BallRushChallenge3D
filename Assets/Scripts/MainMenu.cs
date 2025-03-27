using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene loading
using UnityEngine.UI;  // Required for accessing UI elements

public class MainMenu : MonoBehaviour
{
    // Function to start the game
    public void StartGame()
    {
        // You can change "GameScene" to the name of the scene you want to load
        SceneManager.LoadScene("SampleScene");
    }

    // Function to show credits (you can display a simple message or open a new scene)
    public void ShowCredits()
    {
        // Example: Load a Credits scene
        SceneManager.LoadScene("Credits");
    }

    public void BackMainMenu()
    {
        
        SceneManager.LoadScene("Mainmenu");
    }

    // Function to quit the game
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();

        // If you're running inside the editor, you can stop it with this line
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
