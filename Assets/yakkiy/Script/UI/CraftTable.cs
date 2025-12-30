using System.Collections;
using TMPro;
using UnityEngine;

public class CraftTable : MonoBehaviour
{
    const int CHOICES_COUNT = 8;//選択肢の数

    [Header("パーツオペレーションスクリプト")]
    [SerializeField] PartsOperation partsOperation;

    [Header("候補になるパーツの情報")]
    [SerializeField] PartsData partsData;

    [Header("閉じた時のUI位置")]
    [SerializeField] float closeUIPos_x;

    [Header("開閉スピード")]
    [SerializeField] float openSpeed;
    [SerializeField] float closeSpeed;

    [Header("作成候補パネル")]
    [SerializeField] Transform choicesPanel;

    [Header("選択肢UIプレハブ")]
    [SerializeField] GameObject choicesUIPrefab;

    [Header("説明文ディスプレイ")]
    [SerializeField] TMP_Text explanationDisplay;

    PartProperty selectPartProperty;//現在選択中のパーツ情報
    CraftUI selectCraftUI;//現在選択中の選択肢

    RectTransform rectTransform;

    GameManager gameManager;

    bool isActive;//クラフト画面表示中
    public bool IsActive {  get { return isActive; } }

    bool isMoveAnimation;//開閉アニメーション中

    private void Start()
    {
        gameManager = GameManager.Instance;

        rectTransform = GetComponent<RectTransform>();

        Choicelottery();

        CraftPartsSelect(null , null);//表示物リセットのために空を選択する
    }

    /// <summary>
    /// 選択肢を用意
    /// </summary>
    void Choicelottery()
    {
        for (int i = 0; i < CHOICES_COUNT; i++)
        {
            CraftUI craftUI = Instantiate(choicesUIPrefab, choicesPanel).GetComponent<CraftUI>();

            craftUI.partProperty = partsData.PartProperties[Random.Range(0, partsData.PartProperties.Length - 1)];

            craftUI.craftTable = this;
        }
    }

    /// <summary>
    /// パーツを選択する
    /// </summary>
    /// <param name="partProperty"></param>
    public void CraftPartsSelect(PartProperty partProperty , CraftUI craftUI)
    {
        selectPartProperty = partProperty;
        selectCraftUI = craftUI;

        ExplanationDisplay();

    }

    /// <summary>
    /// 説明文表示
    /// </summary>
    void ExplanationDisplay()
    {
        if (selectPartProperty == null)//セレクトパーツがnullならTextを空にしてreturn
        {
            explanationDisplay.text = null;

            return;
        }

        explanationDisplay.text = selectPartProperty.craftUI.name;
        explanationDisplay.text += "\n\n";
        explanationDisplay.text += selectPartProperty.craftUI.explanation;
    }

    /// <summary>
    /// パーツ作成をリクエスト
    /// </summary>
    public void CraftRequest()
    {
        if (selectPartProperty == null) return;
        if (selectPartProperty.craftUI.price > gameManager.scrap.AmountOwned)//パーツ不足
        {
            SystemMessage.Instance.ErrorMessage("資材が足りません");
            return;
        }


        Vector3 createPos = Camera.main.transform.position;
        createPos += Camera.main.transform.forward;

        Instantiate(selectPartProperty.makingPrefab, createPos, Quaternion.identity);
        gameManager.scrap.AddAmountOwned(-selectPartProperty.craftUI.price);

        ActiveSwitch(false);//クラフト台を閉じる
    }


    /// <summary>
    /// 表示切替
    /// </summary>
    public void ActiveSwitch()
    {
        isActive = !isActive;

        partsOperation.OperationPossibleChange(!isActive);

        if (!isMoveAnimation) StartCoroutine(MoveAnimation());
    }

    /// <summary>
    /// 表示切替を代入
    /// </summary>
    public void ActiveSwitch(bool active)
    {
        isActive = active;

        partsOperation.OperationPossibleChange(!isActive);

        if (!isMoveAnimation) StartCoroutine(MoveAnimation());
    }

    /// <summary>
    /// 表示アニメーション
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveAnimation()
    {
        isMoveAnimation = true;

        Vector3 startPos = Vector3.zero;
        Vector3 endPos = Vector3.zero;

        if(isActive)
        {
            startPos.x += closeUIPos_x;
        }
        else
        {
            endPos.x += closeUIPos_x;
        }

        float speed = isActive ? openSpeed : closeSpeed;

        for (float t = 0; t < 1; t+= speed * Time.deltaTime)
        {
            Vector3 pos = Vector3.Lerp(startPos , endPos , t);

            rectTransform.anchoredPosition = pos;

            yield return null;
        }

        isMoveAnimation = false;
    }
}
