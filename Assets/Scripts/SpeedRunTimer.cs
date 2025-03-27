using TMPro;
using UnityEngine;
using UnityEngine.UI;  // For displaying the timer on a UI Text component.

public class SpeedRunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;  // Reference to the UI Text element for displaying the timer.
    private float elapsedTime;  // Stores the elapsed time.
    private bool isRunning;  // Indicates whether the timer is running.
    private RectTransform timerRectTransform;  // Reference to the RectTransform of the timer text for positioning.

    void Start()
    {
        elapsedTime = 0f;
        isRunning = true;  // Start the timer automatically.
        timerRectTransform = timerText.GetComponent<RectTransform>();  // Get the RectTransform of the timer text.
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;  // Increment the timer by the time that has passed since the last frame.

            // Format the time to display minutes, seconds, and milliseconds.
            float minutes = Mathf.Floor(elapsedTime / 60);
            float seconds = Mathf.Floor(elapsedTime % 60);
            float milliseconds = Mathf.Floor((elapsedTime * 100) % 100);

            // Update the UI text with the formatted time.
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    // Call this method to stop the timer.
    public void StopTimer()
    {
        isRunning = false;  // Stop the timer.
        Debug.Log("Timer stopped at: " + elapsedTime);  // Log the time when the timer stops.

        // Change font size and move to the middle of the screen.
        timerText.fontSize = 100;  // Make the text larger.
        timerRectTransform.anchoredPosition = new Vector2(0, -300);
    }

    // Call this method to reset the timer and start again.
    public void ResetTimer()
    {
        elapsedTime = 0f;
        isRunning = true;

        // Reset the text size and position.
        timerText.fontSize = 36;  // Reset to original size.
        timerRectTransform.anchoredPosition = new Vector2(0, 50);  // Reset to original position.
    }

    // For debugging, check if the timer is stopped.
    public bool IsTimerRunning()
    {
        return isRunning;
    }
}
