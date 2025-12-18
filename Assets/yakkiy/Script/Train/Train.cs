using UnityEngine;

public class Train : MonoBehaviour
{
    public static Train Instance;

    public TrainParameter parameter;

    private void Awake()
    {
        Singleton();
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
}

/// <summary>
/// 列車のステータス
/// </summary>
[System.Serializable]
public class TrainParameter
{
    /// <summary>
    /// 最高速
    /// </summary>
    public float MaxSpeed;
    /// <summary>
    /// 加速力
    /// </summary>
    public float Acceleration;
    ///// <summary>
    ///// 重量
    ///// </summary>
    //public float Weight;
}
