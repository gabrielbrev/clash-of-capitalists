using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class SpeedControlPanel : MonoBehaviour
{
    private static readonly float[] SPEED_VALUES = new float[] { 1f, 2f, 5f, 10f, 20f, 50f, 100f };
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;
    [SerializeField] private Text speedText;
    private int currentSpeedIndex;

    void Awake()
    {

    }

    private void SetSpeed(int index)
    {
        float speed = SPEED_VALUES[index];
        Time.timeScale = speed;
        speedText.text = $"{speed:F0}x";
    }

    void Start()
    {
        currentSpeedIndex = 0;
        SetSpeed(currentSpeedIndex);

        plusButton.onClick.AddListener(() =>
        {
            currentSpeedIndex = Math.Min(currentSpeedIndex + 1, SPEED_VALUES.Length - 1);
            SetSpeed(currentSpeedIndex);
        });
        minusButton.onClick.AddListener(() =>
        {
            currentSpeedIndex = Math.Max(currentSpeedIndex - 1, 0);
            SetSpeed(currentSpeedIndex);
        });
    }

    void Update()
    {
        
    }
}
