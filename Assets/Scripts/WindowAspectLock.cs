using UnityEngine;

public class WindowAspectLock : MonoBehaviour
{
    public float targetAspect = 16f / 9f;
    private bool adjusting = false;
    private bool wasFullScreen = false;

    void Update()
    {
        if (Screen.fullScreen != wasFullScreen)
        {
            wasFullScreen = Screen.fullScreen;
            adjusting = false;
        }

        if (Screen.fullScreen)
            return;

        if (adjusting)
            return;

        float currentAspect = (float)Screen.width / Screen.height;
        if (Mathf.Abs(currentAspect - targetAspect) > 0.01f)
        {
            adjusting = true;

            int newWidth = Screen.width;
            int newHeight = Mathf.RoundToInt(Screen.width / targetAspect);

            if (newHeight > Display.main.systemHeight)
            {
                newHeight = Display.main.systemHeight;
                newWidth = Mathf.RoundToInt(newHeight * targetAspect);
            }

            Screen.SetResolution(newWidth, newHeight, false);

            // Deixa o ajuste estabilizar
            Invoke(nameof(ResetAdjusting), 0.1f);
        }
    }

    void ResetAdjusting()
    {
        adjusting = false;
    }
}
