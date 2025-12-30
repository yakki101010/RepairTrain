using System.Collections.Generic;
using UnityEngine;

public class BloodstainObjectPool : MonoBehaviour
{
    public static BloodstainObjectPool Instance;

    List<GameObject> inactiveBloodstain = new List<GameObject>();
    List<GameObject> workBloodstain = new List<GameObject>();

    [Header("血痕プレハブ")]
    [SerializeField] GameObject bloodstainPrefab;
    [Header("最大数")]
    [SerializeField] int maxNum;

    

    private void Awake()
    {
        Singleton();
        GenerateBlood();
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
    void GenerateBlood()
    {
        for (int i = 0; i < maxNum; i++)
        {
            GameObject obj = Instantiate(bloodstainPrefab, transform);
            obj.TryGetComponent<Bloodstain>(out Bloodstain bloodstain);
            bloodstain.bloodstainObjectPool = this;
            obj.SetActive(false);
            inactiveBloodstain.Add(obj);
        }
    }

    public GameObject BloodstainInstantiate(Vector3 pos, Quaternion rot)
    {
        GameObject obj;

        if (inactiveBloodstain.Count > 0)
        {
            obj = inactiveBloodstain[0];
            inactiveBloodstain.RemoveAt(0);
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            obj.SetActive(true);
            workBloodstain.Add(obj);
        }
        else//最大数血だまりができていたら古い血だまりを再利用
        {
            obj = workBloodstain[0];
            workBloodstain.RemoveAt(0);
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            workBloodstain.Add(obj);
        }


        return obj;
    }

    /// <summary>
    /// 役目を終えた血だまりの帰還
    /// </summary>
    /// <param name="obj"></param>
    public void AddInactive(GameObject obj)
    {
        obj.transform.position = new Vector3(0, 30, 0);//悪い影響を及ぼさないように退避させる

        obj.SetActive(false);
        workBloodstain.Remove(obj);
        inactiveBloodstain.Add(obj);
    }
}
