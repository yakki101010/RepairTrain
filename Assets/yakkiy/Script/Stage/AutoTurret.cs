using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : MonoBehaviour
{
    [Header("攻撃力")]
    [SerializeField] int damage;
    [Header("クールタイム")]
    [SerializeField] float coolTime = 0.2f;

    [Header("弾プレハブ")]
    [SerializeField] GameObject bloodPrefab;


    [SerializeField] Transform shotPoint;
    [Header("射線を阻害するレイヤー")]
    [SerializeField] LayerMask notPenetrationLayer;

    [Header("銃身")]
    [SerializeField] Transform gunBarrel;

    [Header("弾の生成位置")]
    [SerializeField] Transform bloodCreatePoint;

    List<GameObject> searchAreaObjs = new List<GameObject>();

    GameObject target;

    Zombie targetZombie;

    bool targetLockon;

    bool isCoolTime = false;

    private void Update()
    {
        if (target != null)
        {
            if (!target.activeSelf)//Targetが非表示なら消えたとみなす
            {
                searchAreaObjs.Remove(target);
                target = null;
            }
        }

        TargetRadiationCheck();

        ShotRequest();

        if (!targetLockon) TargetSelect();

    }

    /// <summary>
    /// 現状のターゲットが攻撃可能かをチェックする
    /// </summary>
    void TargetRadiationCheck()
    {
        
        if (target == null)//ターゲットが消えた
        {
            targetLockon = false;

            return;
        }

        if (!searchAreaObjs.Contains(target))//サーチ範囲外にでていた
        {
            targetLockon = false;

            return;
        }

        if (RadiationCheck(target.transform))//ターゲットとの間に障害物がある
        {
            targetLockon = false;

            return;
        }

        
    }

    /// <summary>
    /// 攻撃命令
    /// </summary>
    void ShotRequest()
    {
        if(target == null) return;

        gunBarrel.rotation = Quaternion.LookRotation(target.transform.position - shotPoint.position, transform.forward);

        if (!isCoolTime) StartCoroutine(Shot());
    }

    /// <summary>
    /// 弾を発射してクールタイムに入る
    /// </summary>
    /// <returns></returns>
    IEnumerator Shot()
    {
        isCoolTime = true;

        Transform blood = Instantiate(bloodPrefab, bloodCreatePoint.position, Quaternion.identity).transform;

        StartCoroutine(ShotCourse(blood , target.transform.position));

        yield return new WaitForSeconds(coolTime);

        isCoolTime = false;
    }

    /// <summary>
    /// 発射した弾の制御
    /// </summary>
    /// <returns></returns>
    IEnumerator ShotCourse(Transform blood , Vector3 targetPos)
    {
        const float SPEED = 8f;

        for (float t = 0; t < 1; t += SPEED * Time.deltaTime)
        {
            Vector3 pos = Vector3.Lerp(bloodCreatePoint.position, targetPos, t);

            blood.position = pos;

            yield return null;
        }

        targetZombie.AddHP(-damage);

        Destroy(blood.gameObject);
    }

    /// <summary>
    /// ターゲットを選択する
    /// </summary>
    void TargetSelect()
    {
        //サーチ範囲にゾンビがいない
        if (searchAreaObjs.Count <= 0) return;

        //現在補足中のゾンビをまだ狙える
        if (targetLockon) return;

        for (int i = 0; i < searchAreaObjs.Count; i++)
        {
            if (!RadiationCheck(searchAreaObjs[i].transform))
            {

                target = searchAreaObjs[i];
                targetZombie = target.GetComponent<Zombie>();
                targetLockon = true;
                return;
            }
        }
    }

    /// <summary>
    /// 対象までの間に障害物があればtrueを返す
    /// </summary>
    /// <returns></returns>
    bool RadiationCheck(Transform checkTarget)
    {
        Vector3 vector = checkTarget.position - shotPoint.position;

        Ray ray = new Ray(shotPoint.position , vector);

        return Physics.Raycast(ray, out RaycastHit hit, vector.magnitude, notPenetrationLayer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)//サーチエリアにゾンビがいるならリストに追加
        {
            searchAreaObjs.Add(other.gameObject);

            TargetSelect();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)//サーチエリアから離れたのがゾンビならリストから削除
        {
            searchAreaObjs.Remove(other.gameObject);

            TargetSelect();
        }
    }
}
