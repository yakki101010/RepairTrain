using UnityEngine;

/// <summary>
/// フェーズのステージを生成するスクリプト
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] RailTile tile;

    [SerializeField] int length;//ステージの全長

    [SerializeField] int zombieNum;//1タイル当たりのゾンビ数
    public int ZombieNum { get { return zombieNum; } }

    [SerializeField] int eliteZombieNum;//このステージに出現するエリートゾンビの数

    [SerializeField] Stage stage;
    public Stage Stage { get { return stage; } }

    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        GetStage();
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

    void GetStage()
    {
        stage = new Stage();

        stage.RandomTile(length, tile.plainSeries);
    }
}

/// <summary>
/// 道中ステージの情報
/// </summary>
public class Stage
{
    public GameObject[] tiles;//タイルマップ


    /// <summary>
    /// ステージ用ランダムなタイルデータを作成
    /// </summary>
    /// <param name="length">タイルの長さ</param>
    /// <param name="rail">使用するタイルシリーズ</param>
    public void RandomTile(int length , Rail rail)
    {
        tiles = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            tiles[i] = rail.GetTile();
        }
    }
}

