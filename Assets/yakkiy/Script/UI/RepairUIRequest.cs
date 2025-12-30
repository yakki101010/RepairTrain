using UnityEngine;

public class RepairUIRequest : MonoBehaviour
{
    [SerializeField] GameObject repairUI;
    void Start()
    {
        if (GameManager.Instance.maxLife == GameManager.Instance.life) return;

        repairUI.SetActive(true);
    }
}
