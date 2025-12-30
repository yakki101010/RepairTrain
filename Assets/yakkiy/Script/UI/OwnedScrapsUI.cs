using TMPro;
using UnityEngine;

public class OwnedScrapsUI : MonoBehaviour
{
    [SerializeField] TMP_Text display;

    GameManager gameManager;

    void Start()
    {
        Initialization();
    }

    private void OnDestroy()
    {
        if (gameManager == null) return;

        gameManager.scrap.RemoveCallback(DisplayUpdate);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    void Initialization()
    {
        gameManager = GameManager.Instance;

        gameManager.scrap.ReceiveCallback(DisplayUpdate);//コールバックを受け取るようにする

        DisplayUpdate();//ディスプレイ更新
    }

    /// <summary>
    /// 表示を更新
    /// </summary>
    void DisplayUpdate()
    {
        display.text = (gameManager.scrap.AmountOwned).ToString();
    }

}
