using System.Collections.Generic;
using UnityEngine;

public class ZombieObjectPool : MonoBehaviour
{
    List<GameObject> inactiveZombies = new List<GameObject>();

    TrainController trainController;

    private void Awake()
    {
        trainController = Train.Instance.gameObject.GetComponent<TrainController>();
    }

    public void GenerateZombie(GameObject zombiesPrefab , int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(zombiesPrefab ,transform);
            obj.TryGetComponent<Zombie>(out Zombie zombie);
            zombie.SetTrainController(trainController);
            zombie.SetZombieObjectPool(this);
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
