using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class UIManager : MonoBehaviour
{
    private GameObject playerHUD;

    public void SetPlayerPanel(PlayerPanel panel)
    {
        if (playerHUD != null) playerHUD.SetActive(false);

        playerHUD = panel.gameObject;
        playerHUD.transform.SetParent(transform, false);
        playerHUD.SetActive(true);
    }

    void Awake()
    {

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
