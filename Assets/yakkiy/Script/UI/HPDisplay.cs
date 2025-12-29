using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPDisplay : MonoBehaviour
{
    [SerializeField] Image lifeBar;
    [SerializeField] TMP_Text lifeText;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;

        gameManager.life.ReceiveCallback(DisplayUpdate);
        gameManager.maxLife.ReceiveCallback(DisplayUpdate);

        DisplayUpdate();
    }

    private void OnDestroy()
    {
        if (gameManager == null) return;

        gameManager.life.RemoveCallback(DisplayUpdate);
        gameManager.maxLife.RemoveCallback(DisplayUpdate);
    }


    void DisplayUpdate()
    {
        lifeBar.fillAmount = (float)gameManager.life.AmountOwned / (float)gameManager.maxLife.AmountOwned;

        lifeText.text = gameManager.life.AmountOwned.ToString();
    }

    
}
