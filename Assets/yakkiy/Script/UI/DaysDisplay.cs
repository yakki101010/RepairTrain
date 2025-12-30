using TMPro;
using UnityEngine;

public class DaysDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text display;

    void Start()
    {
        display.text = $"生存{GameManager.Instance.day.AmountOwned + 1}日目";
    }
}
