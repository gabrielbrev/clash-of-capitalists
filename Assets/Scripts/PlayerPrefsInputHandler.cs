using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefsInputHandler : MonoBehaviour
{
    [SerializeField] private InputField numPlayersInput;
    [SerializeField] private InputField numAiPlayersInput;
    [SerializeField] private Button startButton;

    private bool isNumPlayersValid = false;
    private bool isNumAiPlayersValid = false;

    private void Start()
    {
        int defaultNumPlayers = 1;
        int defaultNumAiPlayers = 3;

        PlayerPrefs.SetInt("NumPlayers", defaultNumPlayers);
        PlayerPrefs.SetInt("NumAiPlayers", defaultNumAiPlayers);
        PlayerPrefs.Save();

        numPlayersInput.text = defaultNumPlayers.ToString();
        numAiPlayersInput.text = defaultNumAiPlayers.ToString();

        isNumPlayersValid = true;
        isNumAiPlayersValid = true;
        UpdateButtonState();

        numPlayersInput.onEndEdit.AddListener(SaveNumPlayers);
        numAiPlayersInput.onEndEdit.AddListener(SaveNumAiPlayers);
    }

    private void UpdateButtonState()
    {
        startButton.interactable = isNumPlayersValid && isNumAiPlayersValid;
    }

    private void SaveNumPlayers(string value)
    {
        if (int.TryParse(value, out int numPlayers))
        {
            isNumPlayersValid = true;
            PlayerPrefs.SetInt("NumPlayers", numPlayers);
            PlayerPrefs.Save();
        }
        else
        {
            isNumPlayersValid = false;
            Debug.LogWarning("Valor inválido para NumPlayers: " + value);
        }
        UpdateButtonState();
    }

    private void SaveNumAiPlayers(string value)
    {
        if (int.TryParse(value, out int numAiPlayers))
        {
            isNumAiPlayersValid = true;
            PlayerPrefs.SetInt("NumAiPlayers", numAiPlayers);
            PlayerPrefs.Save();
        }
        else
        {
            isNumAiPlayersValid = false;
            Debug.LogWarning("Valor inválido para NumAiPlayers: " + value);
        }
        UpdateButtonState();
    }
}
