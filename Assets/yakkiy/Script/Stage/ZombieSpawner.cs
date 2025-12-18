using System;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    const float SPAWN_INTERVAL = 10;
    const float SPAWN_INTERVAL_SCALE = 10;//スポーン間隔に補正をかける列車のスピードにかかる補正の倍率

    [SerializeField] GameObject prefab;

    [SerializeField] Vector3 spawnOffset;

    float random_x;
    float random_z;

    float spawnIntervalTime = SPAWN_INTERVAL;

    void Start()
    {
        //RouteGenerator.Instance.TileSpawn += TileSpawn;//ルートマネージャーがタイルを生成したタイミングの関数に登録

        random_x = RouteGenerator.TILE_SCALE * 0.5f;
        random_z = RouteGenerator.TILE_SCALE * 0.5f;
    }

    private void Update()
    {
        if(spawnIntervalTime > SPAWN_INTERVAL)
        {
            spawnIntervalTime = 0;

            TileSpawn(spawnOffset);
            TileSpawn(new Vector3(-spawnOffset.x, spawnOffset.y, spawnOffset.z));
        }
        else
        {
            float correction = RouteGenerator.Instance.Speed * SPAWN_INTERVAL_SCALE;
            if (correction < 0) correction *= -1f;

            spawnIntervalTime += (1 + (correction)) * Time.deltaTime;
        }
    }

    void TileSpawn(Vector3 tilePos)
    {
        for (int i = 0; i < StageManager.Instance.ZombieNum; i++)
        {
            //Debug.Log($"ゾンビ生成、場所:{tilePos}");

            Vector3 randomness = Vector3.zero;//ランダム性

            randomness.x = UnityEngine.Random.Range(random_x , -random_x);
            randomness.z = UnityEngine.Random.Range(random_z , -random_z);

            Instantiate(prefab, tilePos + randomness, Quaternion.identity);
        }
    }
}
