using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    const float LEADING_BOGIE_POS_X = -3f;
    const float FAST_TRAILING_BOGIE_POS_X = 6f;

    public static Train Instance;

    [SerializeField] PartsData partsData;
    public PartsData PartsData { get { return partsData; } }

    [SerializeField] TrainStructure initialTrain;

    public TrainParameter parameter;

    private void Awake()
    {
        Singleton();

        //if (parameter.structure.trailingBogie.Count <= 0)//後続車が一つもなければ1台追加
        //{
        //    PartObject trailingBogie = new PartObject();
        //    trailingBogie.partProperty = partsData.LeadingBogie;
        //    trailingBogie.pos.x = FAST_TRAILING_BOGIE_POS_X;
        //    parameter.structure.trailingBogie.Add(trailingBogie);
        //}
    }

    private void Start()
    {
        TrainReadout_Stage();
    }

    /// <summary>
    /// 設計図を初期状態の列車に上書きする
    /// </summary>
    [ContextMenu("デフォルト列車")]
    public void SetDefaulttrain()
    {
        parameter.myTrain.bogie = initialTrain.bogie;
    }

    /// <summary>
    /// ステージ用列車の生成
    /// </summary>
    public void TrainReadout_Stage()
    {
        Readout_Stage(parameter.myTrain.bogie, transform);
    }

    /// <summary>
    /// 階層を全て調べてステージプレハブを生成する
    /// </summary>
    /// <param name="childPart"></param>
    /// <param name="parent"></param>
    void Readout_Stage(List<PartObject> childPart, Transform parent)
    {
        for (int i = 0; i < childPart.Count; i++)
        {
            GameObject obj = Instantiate(childPart[i].partProperty.stagePrefab, childPart[i].pos, childPart[i].rot);

            if (obj.TryGetComponent<MakingPart>(out MakingPart makingPart))
            {
                makingPart.beStructure = true;

            }

            //partsFamily.SetParent(parent);
            obj.transform.SetParent(parent);

            if (childPart[i].childPart.Count > 0) Readout_Stage(childPart[i].childPart, obj.transform);
        }
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
