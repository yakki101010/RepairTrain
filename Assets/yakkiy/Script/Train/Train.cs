using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    const float LEADING_BOGIE_POS_X = -3f;
    const float FAST_TRAILING_BOGIE_POS_X = 6f;

    public static Train Instance;

    [SerializeField] PartsData partsData;

    [SerializeField] TrainStructure initialTrain;

    public TrainParameter parameter;

    private void Awake()
    {
        Singleton();

        parameter.myTrain.bogie = initialTrain.bogie;//Debug用初期化

        //if (parameter.structure.trailingBogie.Count <= 0)//後続車が一つもなければ1台追加
        //{
        //    PartObject trailingBogie = new PartObject();
        //    trailingBogie.partProperty = partsData.LeadingBogie;
        //    trailingBogie.pos.x = FAST_TRAILING_BOGIE_POS_X;
        //    parameter.structure.trailingBogie.Add(trailingBogie);
        //}
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

    /// <summary>
    /// 列車の構造
    /// </summary>
    public TrainStructure myTrain;
}
