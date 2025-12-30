using UnityEngine;

/// <summary>
/// フェーズのステージを生成するスクリプト
/// </summary>
public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [SerializeField] LevelData levelData;

    Level level;
    public Level _Level { get { return level; } }

     [SerializeField] Stage stage;
    public Stage Stage { get { return stage; } }

    private void Awake()
    {
        Singleton();

        for (int i = 0; i < levelData.Levels.Length; i++)
        {
            if (levelData.Levels[i]._Level > GameManager.Instance.day.AmountOwned)
            {
                level = levelData.Levels[i];

                return;
            }
        }
    }

    private void Start()
    {
        GetStage();

        Train.Instance.TrainReadout_Stage();//列車生成
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

        stage.RandomTile(level.Length, level.Tile.plainSeries);
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
    public void RandomTile(int length, Rail rail)
    {
        tiles = new GameObject[length];

        for (int i = 0; i < length; i++)
        {
            tiles[i] = rail.GetTile();
        }

        OverwriteStartAndGoal();


        void OverwriteStartAndGoal()
        {
            int margin = (RouteGenerator.TILE_NUM / 2);

            tiles[margin] = rail.startRail;
            tiles[length - margin] = rail.goalRail;
        }
    }

}

