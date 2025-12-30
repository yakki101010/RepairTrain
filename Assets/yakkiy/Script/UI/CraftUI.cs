using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftUI : MonoBehaviour
{
    [Header("選択肢の名前の表示場所")]
    [SerializeField] TMP_Text nameDisplay;

    [Header("選択肢の要求パーツの表示場所")]
    [SerializeField] TMP_Text priceDisplay;

    [Header("選択肢のイメージの表示場所")]
    [SerializeField] Image imageDisplay;

    [Header("選択不可時にかぶせるオブジェクト")]
    [SerializeField] GameObject hideObject;

    [Header("この選択肢の情報")]
    public PartProperty partProperty;

    [Header("選択肢統轄")]
    public CraftTable craftTable;

    private void Start()
    {
        nameDisplay.text = partProperty.craftUI.name;
        priceDisplay.text = partProperty.craftUI.price.ToString();
        imageDisplay.sprite = partProperty.craftUI.image;

        hideObject.SetActive(false);
    }

    /// <summary>
    /// 子のパーツを選択
    /// </summary>
    public void CraftPartsSelect()
    {
        craftTable.CraftPartsSelect(partProperty , this);
    }

    /// <summary>
    /// この選択肢を選択不可にする
    /// </summary>
    public void MakeUnselectable()
    {
        hideObject.SetActive(true);
    }
}
