using System.Collections.Generic;
using UnityEngine;

public class ZombieObjectPool : MonoBehaviour
{
    List<GameObject> inactiveZombies = new List<GameObject>();

    TrainController trainController;

    [SerializeField] TimeManager timeManager;

    /// <summary>
    /// ステージで使用するゾンビを生成
    /// </summary>
    /// <param name="zombiesPrefab">ゾンビのプレハブ</param>
    /// <param name="attack">ゾンビの攻撃力</param>
    /// <param name="hp">ゾンビの体力</param>
    /// <param name="count">生成数</param>
    public void GenerateZombie(GameObject zombiesPrefab ,int attack ,int hp, int count)
    {
        trainController = Train.Instance.gameObject.GetComponent<TrainController>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(zombiesPrefab ,transform);
            obj.TryGetComponent<Zombie>(out Zombie zombie);
            zombie.damage = attack;
            zombie.maxHp = hp;
            zombie.SetTrainController(trainController);
            zombie.SetZombieObjectPool(this);
            timeManager.RequestNightCallback(zombie.NightFalls);
            obj.SetActive(false);
            inactiveZombies.Add(obj);
        }
    }

    public GameObject ZombieInstantiate(Vector3 pos , Quaternion rot)
    {
        if(inactiveZombies.Count <= 0) return null;

        GameObject obj = inactiveZombies[0];
        inactiveZombies.RemoveAt(0);
        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.SetActive(true);

        return obj;
    }

    /// <summary>
    /// 役目を終えたゾンビが帰ってくる場所
    /// </summary>
    /// <param name="obj"></param>
    public void AddInactive(GameObject obj)
    {
        obj.transform.position = new Vector3(0, 30, 0);//悪い影響を及ぼさないように退避させる

        obj.SetActive(false);
        inactiveZombies.Add(obj);
    }
}
