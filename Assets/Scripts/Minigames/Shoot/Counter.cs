using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Animation _animation;
    [Space(5)]

    [SerializeField] private float duration;
    [Space(10)]

    [SerializeField] List<AudioSource> beepSounds = new();
    [Space(10)]

    public UnityEvent OnStartCount;
    public UnityEvent OnFinishCount;

    public void StartCount()
    {
        StartCoroutine(CountSequence(duration));
    }

    private IEnumerator CountSequence(float time)
    {
        OnStartCount.Invoke();
        countText.gameObject.SetActive(true);

        countText.text = "3";
        _animation.Play();
        beepSounds[0].Play();
        yield return new WaitForSeconds(time);

        countText.text = "2";
        _animation.Play();
        beepSounds[0].Play();
        yield return new WaitForSeconds(time);

        countText.text = "1";
        _animation.Play();
        beepSounds[0].Play();
        yield return new WaitForSeconds(time);

        countText.text = "go";
        _animation.Play();
        beepSounds[1].Play();
        yield return new WaitForSeconds(time);

        OnFinishCount.Invoke();

        StopCoroutine(nameof(CountSequence));
    }
}
