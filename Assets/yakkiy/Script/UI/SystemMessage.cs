using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SystemMessage : MonoBehaviour
{
    public static SystemMessage Instance;

    [SerializeField] TMP_Text textDisplay;

    GameObject textObject;
    RectTransform rectTransform;

    Vector3 defaultPos;
    Vector2 defaultSize;
    Quaternion defaultRot;

    List<Coroutine> activeCoroutine = new List<Coroutine>();



    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        textObject = textDisplay.gameObject;
        rectTransform = textObject.GetComponent<RectTransform>();
        defaultPos = rectTransform.anchoredPosition;
        defaultSize = rectTransform.rect.size;
        defaultRot = rectTransform.rotation;

        textObject.SetActive(false);
    }

    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ErrorMessage(string message)
    {
        if (activeCoroutine != null)
        {
            ActiveCoroutineStop();
        }

        ArrangementReset();
        textDisplay.color = Color.red;
        textDisplay.text = message;

        textObject.SetActive (true);

        activeCoroutine.Add(StartCoroutine(Shake(100f)));
        activeCoroutine.Add(StartCoroutine(HurdOut(1f)));

    }


    /// <summary>
    /// 初期位置に戻す
    /// </summary>
    void ArrangementReset()
    {
        rectTransform.anchoredPosition = defaultPos;
        rectTransform.sizeDelta = defaultSize;
        rectTransform.rotation = defaultRot;
    }

    void ActiveCoroutineStop()
    {
        for (int i = 0; i < activeCoroutine.Count; i++)
        {
            StopCoroutine(activeCoroutine[i]);
        }

        activeCoroutine.Clear();
    }

    //テキストのヒュードアウト
    IEnumerator HurdOut(float displayTime )
    {

        yield return new WaitForSeconds(displayTime);

        for (float t = 1; t >= 0; t -= Time.deltaTime)
        {
            Color color = textDisplay.color;

            color.a = t;

            textDisplay.color = color;

            yield return null;
        }
    }

    IEnumerator Shake(float shakePower)
    {
        const float SHAKE_SPEED = 15f;

        for (int i = 0; i < 5; i++)
        {
            for (float t = 1; t >= 0; t -= Time.deltaTime * SHAKE_SPEED)
            {
                Vector3 pos = defaultPos;
                pos.x += shakePower * t;

                rectTransform.anchoredPosition = pos;

                yield return null;
            }

            for (float t = 0; t < 0; t += Time.deltaTime * SHAKE_SPEED)
            {
                Vector3 pos = defaultPos;
                pos.x += shakePower * t;

                rectTransform.anchoredPosition = pos;

                yield return null;
            }

            shakePower *= -0.8f;
        }
    }
}
