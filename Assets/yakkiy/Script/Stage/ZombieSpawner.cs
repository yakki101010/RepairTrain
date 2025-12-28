using System;
using System.Collections;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    const float SPAWN_INTERVAL = 10;
    const float SPAWN_INTERVAL_SCALE = 10;//スポーン間隔に補正をかける列車のスピードにかかる補正の倍率

    const int SPAWN_COUNT_MAX = 2500;//ゾンビの最大数

    [SerializeField] GameObject prefab;

    [SerializeField] ZombieObjectPool zombieObjectPool;

    [SerializeField] Vector3 sideSpawnOffset;
    [SerializeField] Vector3 frontAndBackSpawnOffset;

    //左右スポーン位置のランダム範囲
    float sideRandom_x;
    float sideRandom_z;

    //前後スポーン位置のランダム範囲
    float frontAndBackRandom_x;
    float frontAndBackRandom_z;


    float spawnIntervalTime = SPAWN_INTERVAL;

    StageManager stageManager;

    int sideSpaenCount;
    int frontAndBackSpaenCount;

    void Start()
    {
        //RouteGenerator.Instance.TileSpawn += TileSpawn;//ルートマネージャーがタイルを生成したタイミングの関数に登録

        //左右スポーン位置のランダム範囲
        sideRandom_x = RouteGenerator.TILE_SCALE * 0.5f;
        sideRandom_z = RouteGenerator.TILE_SCALE * 0.5f;
        //前後スポーン位置のランダム範囲
        frontAndBackRandom_x = RouteGenerator.TILE_SCALE * 0.5f;
        frontAndBackRandom_z = RouteGenerator.TILE_SCALE * 0.25f;

        stageManager = StageManager.Instance;

        zombieObjectPool.GenerateZombie(prefab, SPAWN_COUNT_MAX);

        sideSpaenCount = stageManager.ZombieNum / 3;
        frontAndBackSpaenCount = (int)(sideSpaenCount * 0.5f);
    }

    private void Update()
    {
        if(spawnIntervalTime > SPAWN_INTERVAL)
        {
            spawnIntervalTime = 0;


            StartCoroutine(TileSpawn(sideSpawnOffset, sideRandom_x, sideRandom_z, sideSpaenCount));
            StartCoroutine(TileSpawn(new Vector3(-sideSpawnOffset.x, sideSpawnOffset.y, sideSpawnOffset.z), sideRandom_x, sideRandom_z, sideSpaenCount));

            StartCoroutine(TileSpawn(frontAndBackSpawnOffset, frontAndBackRandom_x, frontAndBackRandom_z, frontAndBackSpaenCount));
            StartCoroutine(TileSpawn(new Vector3(frontAndBackSpawnOffset.x, frontAndBackSpawnOffset.y, -frontAndBackSpawnOffset.z), frontAndBackRandom_x, frontAndBackRandom_z, frontAndBackSpaenCount));
        }
        else
        {
            float correction = RouteGenerator.Instance.Speed * SPAWN_INTERVAL_SCALE;
            if (correction < 0) correction *= -1f;

            spawnIntervalTime += (1 + (correction)) * Time.deltaTime;
        }
    }


    /// <summary>
    /// 1秒かけてcount体のゾンビを生成
    /// </summary>
    /// <param name="tilePos"></param>
    /// <returns></returns>
    IEnumerator TileSpawn(Vector3 tilePos , float random_x, float random_z, int count)
    {
        for (int i = 0; i < count; i++)
        {
            //Debug.Log($"ゾンビ生成、場所:{tilePos}");

            Vector3 randomness = Vector3.zero;//ランダム性

            randomness.x = UnityEngine.Random.Range(random_x, -random_x);
            randomness.z = UnityEngine.Random.Range(random_z, -random_z);

            zombieObjectPool.ZombieInstantiate(tilePos + randomness, Quaternion.identity);

            yield return new WaitForSeconds(1 / count);
        }
    }
}
