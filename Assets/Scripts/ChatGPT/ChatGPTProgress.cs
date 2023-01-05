using DilmerGames.Core.Singletons;
using System.Collections;
using TMPro;
using UnityEngine;

public class ChatGPTProgress : Singleton<ChatGPTProgress>
{
    [SerializeField]
    private TextMeshProUGUI progressText = null;

    [SerializeField]
    private float frequency = 1.0f;

    [SerializeField]
    private int maxDots = 5;

    private string status;

    private int dotCount = 0;

    private Coroutine progress;

    private bool done = false;

    private void Awake()
    {
        progressText.text = string.Empty;
    }

    public void StartProgress(string status = "In Progress")
    {
        this.status = status;
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

            if(dotCount >= maxDots)
            {
                dotCount = 0;
            }

            progressText.text = $"<color=\"yellow\">{status}{Dots(dotCount)}</color>\n";

            dotCount++;
            if (done)
            {
                done = false;
                progressText.text = "Done";
                StopCoroutine(progress);
            };
        }
    }

    private string Dots(int count)
    {
        string dots = string.Empty;
        for (int i = 0; i < count; i++) dots += ".";
        return dots;
    }
}
