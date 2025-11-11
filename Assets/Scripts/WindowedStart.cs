using UnityEngine;

public class WindowedStart : MonoBehaviour
{
    void Awake()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        Screen.SetResolution(1280, 720, false); // false = windowed
    }
}