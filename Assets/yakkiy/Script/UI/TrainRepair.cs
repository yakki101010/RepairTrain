using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainRepair : MonoBehaviour
{
    [Header("基準パネル")]
    [SerializeField] RectTransform panel;
    [Header("アニメーションスピード")]
    [SerializeField] float openSpeed = 5;
    [Header("パーツオペレーションスクリプト")]
    [SerializeField] PartsOperation partsOperation;

    [Header("要求資材ディスプレイ")]
    [SerializeField] TMP_Text requestedScrapText;
    [Header("消費する資材")]
    [SerializeField] TMP_Text consumptionScrapText;

    [Header("現在HPを表示")]
    [SerializeField] TMP_Text lifeText;
    [Header("最大HPを表示")]
    [SerializeField] TMP_Text maxlifeText;
    [Header("回復後のHP表示")]
    [SerializeField] TMP_InputField repairAmountField;
    [Header("回復量を指定するバー")]
    [SerializeField] Slider repairAmountBler;

    [Header("修理にかかるパーツの要求量倍率")]
    [SerializeField] float requestMagnification = 10;

    GameManager gameManager;

    Vector3 defaultSize = Vector3.zero;

    int repairPartsCount;        //1HPの回復に必用な資材数
    int allRepairRequestedCount; //完全修理にかかる資材数

    int repairAmount;//修理量

    string repairAmountFieldMemory;
    float repairAmountBlerMemory;

    private void OnEnable()
    {
        if(defaultSize == Vector3.zero) defaultSize = panel.localScale;

        if(gameManager == null) gameManager = GameManager.Instance;

        RequestedScrapConfirmation();

        StartCoroutine(OpenAnimation());
    }

    private void Update()
    {

        if(repairAmountField.text != "")
        {
            int repairAmountFieldNum = int.Parse(repairAmountField.text);

            if (repairAmountFieldNum > gameManager.maxLife.AmountOwned)
            {
                repairAmountField.text = gameManager.maxLife.AmountOwned.ToString();
            }
            else if (repairAmountFieldNum < gameManager.life.AmountOwned)
            {
                repairAmountField.text = gameManager.life.AmountOwned.ToString();
            }

            if (repairAmountFieldMemory != repairAmountField.text)
            {
                repairAmountBlerMemory = repairAmountFieldNum;
                repairAmountBler.value = repairAmountFieldNum;

                repairAmountFieldMemory = repairAmountField.text;
            }
        }
        

        if(repairAmountBler.value != repairAmountBlerMemory)
        {
            repairAmountFieldMemory = ((int)(repairAmountBler.value)).ToString();
            repairAmountField.text = ((int)(repairAmountBler.value)).ToString();

            repairAmountBlerMemory = repairAmountBler.value;
        }

        repairAmount = (int)repairAmountBler.value - gameManager.life.AmountOwned;
        consumptionScrapText.text = $"消費資材{repairAmount * repairPartsCount}";
    }

    /// <summary>
    /// 要求資材を表示
    /// </summary>
    void RequestedScrapConfirmation()
    {

        repairPartsCount = (int)(requestMagnification * (gameManager.repairNum.AmountOwned + 1));
        allRepairRequestedCount = (gameManager.maxLife.AmountOwned - gameManager.life.AmountOwned) * repairPartsCount;

        requestedScrapText.text = $"HP１の修理に必用な資材数 {repairPartsCount}";
        requestedScrapText.text += $"\n完全な修理には {allRepairRequestedCount}個の資材が必要です。";

        maxlifeText.text = $"{gameManager.maxLife.AmountOwned.ToString()}/" ;
        lifeText.text = gameManager.life.AmountOwned.ToString() ;

        repairAmountBler.minValue = gameManager.life.AmountOwned;
        repairAmountBler.maxValue = gameManager.maxLife.AmountOwned;
    }

    /// <summary>
    /// 修理する
    /// </summary>
    public void RepairRequest()
    {
        int needScrap = repairAmount * repairPartsCount;

        if ((needScrap) > gameManager.scrap.AmountOwned)
        {
            SystemMessage.Instance.ErrorMessage("素材が足りません");

            return;
        }

        if(needScrap == 0)//修理が必要なければ以下の処理をせずにUIを閉じる
        {
            Close(); ;
            return;
        }

        gameManager.life.AddAmountOwned(repairAmount);
        gameManager.scrap.AddAmountOwned(-needScrap);

        gameManager.repairNum.AddAmountOwned(1);

        Close();
    }

    IEnumerator OpenAnimation()
    {
        partsOperation.OperationPossibleChange(false);

        Vector3 objectiveSize = new Vector3(0.2f,0,0);
        Vector3 nowSize = Vector3.zero;
        objectiveSize.y = defaultSize.y;

        for (float t = 0; t < 1; t+= openSpeed * Time.deltaTime)
        {
            panel.localScale = Vector3.Lerp(nowSize, objectiveSize, t);

            yield return null;
        }

        nowSize = objectiveSize;
        objectiveSize.x = defaultSize.x;

        for (float t = 0; t < 1; t += openSpeed * Time.deltaTime)
        {
            panel.localScale = Vector3.Lerp(nowSize, objectiveSize, t);

            yield return null;
        }
    }

    public void Close()
    {
        StartCoroutine(CloseAnimation());
    }

    IEnumerator CloseAnimation()
    {
        Vector3 objectiveSize = defaultSize;
        Vector3 nowSize = defaultSize;
        objectiveSize.y = 0.1f;

        for (float t = 0; t < 1; t += openSpeed * Time.deltaTime)
        {
            panel.localScale = Vector3.Lerp(nowSize, objectiveSize, t);

            yield return null;
        }

        nowSize = objectiveSize;
        objectiveSize.x = 0;

        for (float t = 0; t < 1; t += openSpeed * Time.deltaTime)
        {
            panel.localScale = Vector3.Lerp(nowSize, objectiveSize, t);

            yield return null;
        }

        gameObject.SetActive(false);
        partsOperation.OperationPossibleChange(true);
    }
}
