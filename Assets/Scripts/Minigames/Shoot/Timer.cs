using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI myTimerText;
    [Space(10)]

    [Tooltip("Tiempo Inicial en Segundos")] public int startingTime;
    [Tooltip("Escala del Tiempo")] [Range(-10.0f, 10.0f)] public float timeScale = 0;

    private float timeToShowInSeconds;
    private float frameTimeWithTimeScale = 0f;
    private float puseTimeScale, startTimeScale;
    private bool pause = true;
    private int hours = -1;
    private int minutes = -1;
    private int seconds = -1;
    private float currentTime;

    void Update()
    {
        if(!pause)
        {
            frameTimeWithTimeScale = Time.deltaTime * timeScale;
            timeToShowInSeconds += frameTimeWithTimeScale;
            UpdateClock(timeToShowInSeconds);
        }
    }
    public void StartClock()
    {
        myTimerText.gameObject.SetActive(true);

        timeScale = -1f;
        startTimeScale = timeScale;
        timeToShowInSeconds = startingTime;
        pause = false;

        UpdateClock(startingTime);
    }
    public void UpdateClock(float timeInSeconds)
    {
        string textoDelReloj;
        if(timeInSeconds < 0) timeInSeconds = 0;

        hours = (int)timeInSeconds / 3600;
        minutes = (int)((timeInSeconds - hours * 3600) / 60);
        seconds = (int)timeInSeconds - (hours * 3600 + minutes * 60);

        currentTime = timeInSeconds;
        textoDelReloj = $"{minutes:00}:{seconds:00}";
        myTimerText.text = textoDelReloj;   
    }
    public void Pause()
    {
        if(!pause)
        {
            pause = true;
            puseTimeScale = timeScale;
            timeScale = 0f;        
        }
    }
    public void Continue()
    {
        if(pause)
        {
            pause = false;
            timeScale = puseTimeScale;
        }
    }
    public void Restart()
    {
        pause = false;
        timeScale = startTimeScale;
        timeToShowInSeconds = startingTime;
        UpdateClock(timeToShowInSeconds);
    }

    public float GetCurrentTime() => currentTime;
}
