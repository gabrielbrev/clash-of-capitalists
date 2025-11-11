using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class QuitOnDoubleEsc : MonoBehaviour
{
    [SerializeField] private Text escWarningText;
    [SerializeField] private string sceneToLoad;
    [SerializeField] private float doublePressThreshold = 1f; 
    private float escPressTime = 0f;
    private bool escPressedOnce = false;

    void Start()
    {
        escWarningText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!escPressedOnce)
            {
                escPressedOnce = true;
                escPressTime = Time.time;

                escWarningText.gameObject.SetActive(true);
            }
            else
            {
                if (Time.time - escPressTime <= doublePressThreshold)
                {
                    SceneManager.LoadScene(sceneToLoad);
                }
            }
        }

        if (escPressedOnce && Time.time - escPressTime > doublePressThreshold)
        {
            escPressedOnce = false;
            escWarningText.gameObject.SetActive(false);
        }
    }
}
