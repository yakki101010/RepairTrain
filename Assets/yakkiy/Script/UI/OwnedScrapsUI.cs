using TMPro;
using UnityEngine;

public class OwnedScrapsUI : MonoBehaviour
{
    [SerializeField] TMP_Text display;

    void Start()
    {
        Initialization();
    }
    /// <summary>
    /// 初期化
    /// </summary>
    void Initialization()
    {
        GameManager.Instance.scrap.ReceiveCallback(DisplayUpdate);//コールバックを受け取るようにする

        DisplayUpdate();//ディスプレイ更新
    }

    /// <summary>
    /// 表示を更新
    /// </summary>
    void DisplayUpdate()
    {
        display.text = (GameManager.Instance.scrap.AmountOwned).ToString();
    }
}
