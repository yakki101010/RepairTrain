using System;
using System.Data.Common;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    const int EARIY_LIFE = 3000;

    public static GameManager Instance;

    [SerializeField] TrainStructure initialTrain;

    [SerializeField] TrainStructure myTrain;


    public Property maxLife;//最大HP
    public Property life;//現在HP

    public Property scrap;//資材



    private void Awake()
    {
        Singleton();

        DontDestroyOnLoad(gameObject);

        maxLife.SetAmountOwned(EARIY_LIFE);
        life.SetAmountOwned(EARIY_LIFE);

        SetDefaulttrain();
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

    /// <summary>
    /// 設計図を初期状態の列車に上書きする
    /// </summary>
    [ContextMenu("デフォルト列車")]
    public void SetDefaulttrain()
    {
        myTrain.bogie = initialTrain.bogie;
    }
}

/// <summary>
/// ゲーム中プレイヤーが保有する物
/// </summary>
[System.Serializable]
public class Property
{
    int amountOwned;//保有額
    public int AmountOwned { get { return amountOwned; } }
    Action ChangeCallback;

    /// <summary>
    /// 保有料を指定
    /// </summary>
    public void SetAmountOwned(int value)
    {
        amountOwned = value;

        if (ChangeCallback != null) ChangeCallback();
    }

    /// <summary>
    /// 保有量に加算
    /// </summary>
    public void AddAmountOwned(int value)
    {
        amountOwned += value;

        if(ChangeCallback != null) ChangeCallback();
    }

    /// <summary>
    /// 保有量が変更された時コールバックを受け取る
    /// </summary>
    public void ReceiveCallback(Action function)
    {
        ChangeCallback += function;
    }
}
