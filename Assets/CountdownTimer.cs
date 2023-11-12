using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CountdownTimer : MonoBehaviour
{
    public float countdownDuration = 30f; // Duration in seconds
    public TextMeshProUGUI countdownText; // Assign your TextMeshProUGUI object here

    private float countdownTime;
    private bool isCountingDown = false;

    // Public events
    public UnityEvent OnCountdownStart;
    public UnityEvent OnCountdownEnd;

    void Start()
    {
        if (countdownText == null)
        {
            Debug.LogError("CountdownTimer: No TextMeshProUGUI component assigned.");
            return;
        }

        StartCountdown();
    }

    void Update()
    {
        if (isCountingDown)
        {
            if (countdownTime > 0)
            {
                countdownTime -= Time.deltaTime;
                countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
               // Debug.Log(countdownText.text);
            }
            else
            {
                countdownText.text = "0";
                isCountingDown = false;
                OnCountdownEnd?.Invoke();
            }
        }
    }

    public void StartCountdown()
    {
        if (!isCountingDown)
        {
            ResetCountdown();
            isCountingDown = true;
            OnCountdownStart?.Invoke();
        }
    }

    private void ResetCountdown()
    {
        countdownTime = countdownDuration;
        countdownText.text = Mathf.CeilToInt(countdownTime).ToString();
    }
}
