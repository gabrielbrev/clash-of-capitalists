using UnityEngine;
using UnityEngine.InputSystem;

public class FullscreenToggle : MonoBehaviour
{
    private int previousWidth;
    private int previousHeight;
    private FullScreenMode previousFullScreenMode;
    private bool previousFullScreenState;

    void Awake()
    {
        // store initial values so we can restore them later
        previousWidth = Screen.width;
        previousHeight = Screen.height;
        previousFullScreenMode = Screen.fullScreenMode;
        previousFullScreenState = Screen.fullScreen;
    }

    void Update()
    {
        bool togglePressed;

        // Prefer the new Input System when available, otherwise fall back to legacy Input
        if (Keyboard.current != null)
        {
            togglePressed = Keyboard.current.f11Key.wasPressedThisFrame ||
                            Keyboard.current.escapeKey.wasPressedThisFrame;
        }
        else
        {
            togglePressed = Input.GetKeyDown(KeyCode.F11) || Input.GetKeyDown(KeyCode.Escape);
        }

        if (!togglePressed)
            return;

        // Toggle fullscreen state. When entering fullscreen, set resolution to monitor/native resolution.
        if (!Screen.fullScreen)
        {
            // Save current windowed resolution to restore later
            previousWidth = Screen.width;
            previousHeight = Screen.height;
            previousFullScreenMode = Screen.fullScreenMode;
            previousFullScreenState = Screen.fullScreen;

            Resolution monitorRes = Screen.currentResolution;

            // Set the resolution to the monitor's native resolution and enter fullscreen windowed mode
            Screen.SetResolution(monitorRes.width, monitorRes.height, FullScreenMode.FullScreenWindow);
            Screen.fullScreen = true;
        }
        else
        {
            // Restore previous resolution and fullscreen state (usually windowed)
            Screen.SetResolution(previousWidth, previousHeight, previousFullScreenMode);
            Screen.fullScreen = previousFullScreenState;
        }
    }
}
