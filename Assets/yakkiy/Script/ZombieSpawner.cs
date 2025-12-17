using System;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;

    [SerializeField] Vector3 spawnOffset;

    float random_x;
    float random_z;

    void Start()
    {
        RouteGenerator.Instance.TileSpawn += TileSpawn;//ルートマネージャーがタイルを生成したタイミングの関数に登録

        random_x = RouteGenerator.TILE_SCALE * 0.5f;
        random_z = RouteGenerator.TILE_SCALE * 0.5f;
    }

    void TileSpawn(Vector3 tilePos)
    {
        for (int i = 0; i < StageManager.Instance.ZombieNum; i++)
        {
            //Debug.Log($"ゾンビ生成、場所:{tilePos + spawnOffset}");

            Vector3 randomness = Vector3.zero;//ランダム性

            randomness.x = UnityEngine.Random.Range(random_x , -random_x);
            randomness.z = UnityEngine.Random.Range(random_z , -random_z);

            Instantiate(prefab, tilePos + spawnOffset + randomness, Quaternion.identity);
        }
    }
}
