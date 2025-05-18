using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    TextMeshProUGUI timerText;
    float time = 0f;
    private void Start()
    {
        FindText();
        MainEvents.Instance.OnGameCompleted += StopCount;

    }
    private void OnDisable()
    {
        MainEvents.Instance.OnGameCompleted -= StopCount;
    }
    void FindText()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        if (timerText != null)
        {
           InvokeRepeating(nameof(StartCount),1f,1f);
        }
        else
        {
           Debug.LogError("TextMeshProUGUI component not found in children.");
        }

    }
    void StartCount()
    {
        ++time;
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        timerText.text = $"{minutes:00} : {seconds:00}";
    }
    void StopCount()
    {
        CancelInvoke(nameof(StartCount));
        if (time < PlayerPrefs.GetFloat("Time", 0f) || PlayerPrefs.GetFloat("Time", 0f) == 0f)
        {
            PlayerPrefs.SetFloat("Time", time);
            MainEvents.Instance.OnNewRecord?.Invoke();
        }
        else
        {
            Debug.Log("You have already completed the game in a shorter time.");
        }
    }
}
