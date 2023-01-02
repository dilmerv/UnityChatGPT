using DilmerGames.Core.Singletons;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ChatGPTProgress : Singleton<ChatGPTProgress>
{
    [SerializeField]
    private TextMeshProUGUI progressText = null;

    [SerializeField]
    private string progressInfo;

    [SerializeField]
    private float frequency = 1.0f;

    [SerializeField]
    private int maxLines = 15;

    private Coroutine progress;

    private bool done = false;

    private void Awake()
    {
        progressText.text = string.Empty;
    }

    public void StartProgress()
    {
        progress = StartCoroutine(ProcessProgress());
    }
    public void StopProgress()
    {
        done = true;
    }

    IEnumerator ProcessProgress()
    {
        while (true)
        {
            yield return new WaitForSeconds(frequency);
            
            if (progressText.text.Split('\n').Count() >= maxLines)
            {
                progressText.text = string.Empty;
            }
            else
            {
                progressText.text += $"<color=\"yellow\">{DateTime.Now.ToString("HH:mm:ss.fff")} {progressInfo}</color>\n";
            }

            if (done)
            {
                done = false;
                StopCoroutine(progress);
            };
        }
    }
}
