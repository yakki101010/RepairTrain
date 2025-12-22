
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// ステージの情報通りに進行度からタイルを生成操作するスクリプト
/// <param name=""></param>
/// </summary>
public class RouteGenerator : MonoBehaviour
{
    /// <summary>
    /// タイルの横幅
    /// </summary>
    public const float TILE_SCALE = 45f;//タイルの横幅
    /// <summary>
    /// タイルを表示したい長さ
    /// </summary>
    public const float DEAD_LENGTH = 300f;//タイルを表示したい長さ
    /// <summary>
    /// シーン上のタイルの数
    /// </summary>
    public const int TILE_NUM = (int)(DEAD_LENGTH / TILE_SCALE);//表示したい長さとタイルの横幅から画面上のタイルの数を決定

    public static RouteGenerator Instance;

    public GameObject[] stageTiles;

    [SerializeField] float currentLocation;//現在位置

    public float CurrentLocation {  get { return currentLocation; } }

    int currentTile;

    [SerializeField] float speed;
    public float Speed { get { return speed; } }

    int currentLocationMemory = 0;
    
    List<Transform> tiles = new List<Transform>();

    public Action<Vector3> TileSpawn;//タイルがスポーンしたときに呼ばれる

    private void Awake()
    {
        Singleton();
    }

    private void Start()
    {
        stageTiles = StageManager.Instance.Stage.tiles;

        for (int i = 0; i < TILE_NUM; i++)
        {
            Transform tile = Instantiate(stageTiles[i], transform).transform;

            tile.localPosition = new Vector3(i * TILE_SCALE, 0, 0);

            tiles.Add(tile);
        }
    }

    private void Update()
    {
        TileWork();//タイルの管理

        currentLocation += Time.deltaTime * speed;
    }

    public void SetSpeed(float delta)//移動スピードをセットする
    {
        if(delta < 0 && currentLocation < 0)
        {
            speed = 0;

            return;
        }

        if (delta > 0 && currentLocation + TILE_NUM > stageTiles.Length)
        {
            speed = 0;

            return;
        }

        speed = delta;

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

    void TileWork()//タイルの管理
    {
        currentTile = (int)currentLocation;//進行度から現在のタイルに変換

        //ステージの大きさをはみ出していたらタイルは移動させない
        if (currentTile < 0) return;
        if (currentTile + TILE_NUM >= stageTiles.Length) return;

        //操作タイルのリストをいじっている間はタイルの移動は行わない
        //無くすとタイル更新のタイミングで一瞬タイルが他のタイルと入れ替わる
        if (currentLocationMemory == currentTile)
        {
            RailMove();
        }
        else
        {
            Reupholstery();
        }
    }

    void RailMove()//タイルの移動
    {
        float delta = (currentLocation % 1) * TILE_SCALE;

        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].localPosition = new Vector3((i * TILE_SCALE) + delta, 0, 0);


        }

    }

    void Reupholstery()//タイルの更新
    {
        int insertIndex;//タイルを追加する位置
        int destroyNum;//削除する番号

        int tileNum;//追加するタイルの番号を決める

        Vector3 spawnPos;//タイルの生成位置(生成に使うものではなくActionの引数として生成位置を渡す物)

        if (currentLocationMemory < currentTile)//前回より前に進んでいたら後方を削除
        {
            insertIndex = 0;
            destroyNum = tiles.Count - 1;

            //先頭の次を指定
            tileNum = currentTile + TILE_NUM;

            
            //先頭の位置つまりはこのオブジェクトの位置をスポーン位置に設定
            spawnPos = transform.position;
        }
        else
        {
            insertIndex = tiles.Count - 1;
            destroyNum = 0;

            //最後尾の前を指定
            tileNum = currentTile;

            //最後尾の位置をスポーン位置に設定
            spawnPos = transform.position + new Vector3(TILE_SCALE * TILE_NUM, 0, 0);
        }

        Destroy(tiles[destroyNum].gameObject);
        tiles.RemoveAt(destroyNum);


        Transform tile = Instantiate(stageTiles[tileNum], transform).transform;
        
        tiles.Insert(insertIndex, tile);


        if(TileSpawn != null) TileSpawn(spawnPos);//アクションに登録された関数を呼ぶ

        currentLocationMemory = currentTile;
    }
}
