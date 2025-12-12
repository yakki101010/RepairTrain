
using System.Collections.Generic;
using UnityEngine;

public class RouteGenerator : MonoBehaviour
{
    [SerializeField] StageGenerator stageGenerator;

    public GameObject[] stageTiles;

    [SerializeField] float currentLocation;

    [SerializeField] float speed;

    int currentLocationMemory = 0;

    const float TILE_SCALE = 45f;

    const float DEAD_LENGTH = 120;

   List<Transform> tiles = new List<Transform>();

    private void Start()
    {
        stageTiles = stageGenerator.GetStage();

        for (int i = 0; i < DEAD_LENGTH / TILE_SCALE; i++)
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

    void TileWork()//タイルの管理
    {
        //ステージの大きさをはみ出していたらタイルは移動させない
        if ((int)currentLocation > stageTiles.Length || (int)currentLocation < 0) return;

        //操作タイルのリストをいじっている間はタイルの移動は行わない
        //無くすとタイル更新のタイミングで一瞬タイルが他のタイルと入れ替わる
        if (currentLocationMemory == (int)currentLocation)
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
        for (int i = 0; i < tiles.Count; i++)
        {
            tiles[i].localPosition = new Vector3((i * TILE_SCALE) + ((currentLocation % 1) * TILE_SCALE), 0, 0);
        }

    }

    void Reupholstery()//タイルの更新
    {
        bool prepend;//先頭に追加
        int destroyNum;//削除する番号

        if (currentLocationMemory < (int)currentLocation)//前回より前に進んでいたら後方を削除
        {
            prepend = true;
            destroyNum = tiles.Count - 1;
        }
        else
        {
            prepend = false;
            destroyNum = 0;
        }

        Destroy(tiles[destroyNum].gameObject);
        tiles.RemoveAt(destroyNum);

        int tileNum;
        if (prepend)//追加するタイルの番号を決める
        {
            //先頭の次を指定
            tileNum = (int)currentLocation + (int)(DEAD_LENGTH / TILE_SCALE);
        }
        else
        {
            //最後尾の前を指定
            tileNum = (int)currentLocation;
        }

        Transform tile = Instantiate(stageTiles[tileNum], transform).transform;

        if (prepend)
        {
            tiles.Insert(0, tile);
        }
        else
        {
            tiles.Add(tile);
        }

        currentLocationMemory = (int)currentLocation;
    }
}
