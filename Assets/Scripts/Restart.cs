using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class Restart : MonoBehaviour
{
    public Button restartButton;  // Assign in Inspector
    public Button quitButton;     // Assign in Inspector (New Quit button)
    public Button mainMenuButton; // Assign in Inspector (New Main Menu button)
    public TextMeshProUGUI pauseText;        // Assign in Inspector (Text to show when paused)
    private bool isPaused = false;
    private bool isPlayerDead = false; // Track if player is dead

    void Start()
    {
        // Ensure the buttons are linked
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
            restartButton.gameObject.SetActive(false); // Hide button at start
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
            quitButton.gameObject.SetActive(false); // Hide button at start
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.AddListener(GoToMainMenu);
            mainMenuButton.gameObject.SetActive(false); // Hide button at start
        }

        if (pauseText != null)
        {
            pauseText.gameObject.SetActive(false); // Hide "PAUSE" text at start
        }

        LockCursor(false); // Lock cursor when the game starts
    }

    void Update()
    {
        // Check if the player is dead, and if so, don't allow pausing
        if (isPlayerDead) return; // If player is dead, do nothing

        // Toggle pause with P only if the player is alive
        if (Input.GetKeyDown(KeyCode.P))
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;
            restartButton.gameObject.SetActive(isPaused); // Show/Hide UI
            quitButton.gameObject.SetActive(isPaused);   // Show/Hide Quit button
            mainMenuButton.gameObject.SetActive(isPaused); // Show/Hide Main Menu button
            pauseText.gameObject.SetActive(isPaused); // Show/Hide PAUSE text
            LockCursor(isPaused);
        }

        // Restart with R
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }
    }

    // Call this method when the player dies
    public void PlayerDied()
    {
        isPlayerDead = true; // Set the player death flag to true
        isPaused = true; // Pause the game when the player dies
        Time.timeScale = 0; // Freeze the game
        restartButton.gameObject.SetActive(true); // Show the restart button
        quitButton.gameObject.SetActive(true);   // Show the quit button
        mainMenuButton.gameObject.SetActive(true); // Show the Main Menu button
        LockCursor(true); // Show the cursor
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Resume before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene
        LockCursor(false); // Lock cursor after restarting
        isPlayerDead = false; // Reset player death flag
    }

    public void QuitGame()
    {
        Debug.Log("Game Over! Quitting...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // If in editor, stop play mode
#endif
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1; // Ensure time is running before loading the main menu
        SceneManager.LoadScene("MainMenu"); // Load the Main Menu scene
    }

    private void LockCursor(bool showCursor)
    {
        Cursor.visible = showCursor;
        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
