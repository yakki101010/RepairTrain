using System;
using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Header("一日の空の移り変わり")]
    [SerializeField] Gradient oneDayCollar;
    [Header("１日の長さ")]
    [SerializeField] float oneDayTime;

    float timeScale;

    Action NightCallback;

    private void Start()
    {
        timeScale = (float)1f / oneDayTime;

        StartCoroutine(TimeFlow());
    }
    private void OnDisable()
    {
        RenderSettings.skybox.SetColor("_Tint", oneDayCollar.Evaluate(0));
    }

    public void RequestNightCallback(Action function)
    {
        NightCallback += function;
    }

    IEnumerator TimeFlow()
    {
        for (float t = 0; t < 1; t += Time.deltaTime * timeScale)
        {
            RenderSettings.skybox.SetColor("_Tint", oneDayCollar.Evaluate(t));

            yield return null;
        }

        if(NightCallback != null) NightCallback();
    }

}
