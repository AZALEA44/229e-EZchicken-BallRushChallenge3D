using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    // Function to go back to the main menu
    public void BackToMainMenu()
    {
        // Assuming your main menu scene is named "MainMenu"
        SceneManager.LoadScene("Mainmenu");
    }
}
