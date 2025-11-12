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

        if (Keyboard.current != null)
        {
            togglePressed = Keyboard.current.f11Key.wasPressedThisFrame || Keyboard.current.backquoteKey.wasPressedThisFrame;
        }
        else
        {
            togglePressed = Input.GetKeyDown(KeyCode.F11) || Input.GetKeyDown(KeyCode.BackQuote);
        }

        if (!togglePressed)
            return;

        if (!Screen.fullScreen)
        {
            previousWidth = Screen.width;
            previousHeight = Screen.height;
            previousFullScreenMode = Screen.fullScreenMode;
            previousFullScreenState = Screen.fullScreen;

            Resolution monitorRes = Screen.currentResolution;

            Screen.SetResolution(monitorRes.width, monitorRes.height, FullScreenMode.FullScreenWindow);
            Screen.fullScreen = true;
        }
        else
        {
            Screen.SetResolution(previousWidth, previousHeight, previousFullScreenMode);
            Screen.fullScreen = previousFullScreenState;
        }
    }
}
