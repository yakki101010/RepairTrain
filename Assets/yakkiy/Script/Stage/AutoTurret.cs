using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTurret : MonoBehaviour
{

    [Header("攻撃力")]
    [SerializeField] int damage;
    [Header("連射")]
    [SerializeField] int rapidFire = 3;
    [Header("連射速度")]
    [SerializeField] float fireRate = 0.2f;
    [Header("クールタイム")]
    [SerializeField] float coolTime = 1f;

    [Header("弾プレハブ")]
    [SerializeField] GameObject bloodPrefab;
    [Header("着弾プレハブ")]
    [SerializeField] GameObject hitPrefab;

    [Header("射撃音")]
    [SerializeField] AudioClip shotSE;


    [Header("銃口位置")]
    [SerializeField] Transform shotPoint;
    [Header("射線を阻害するレイヤー")]
    [SerializeField] LayerMask notPenetrationLayer;

    [Header("ブレ")]
    [SerializeField] float bulletShake = 0.3f;

    [Header("銃身")]
    [SerializeField] Transform gunBarrel;
    [Header("回転台")]
    [SerializeField] Transform support;

    [Header("弾の生成位置")]
    [SerializeField] Transform bloodCreatePoint;

    List<GameObject> searchAreaObjs = new List<GameObject>();

    GameObject target;

    AudioSource audioSource;

    Zombie targetZombie;

    bool targetLockon;

    bool isCoolTime = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

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

        LookTarget();

        if (!isCoolTime) StartCoroutine(Shot());
    }

    /// <summary>
    /// ターゲットの方を向く
    /// </summary>
    void LookTarget()
    {
        Vector3 dir = target.transform.position - shotPoint.position;
        gunBarrel.rotation = Quaternion.LookRotation(dir, transform.forward);
        dir -= Vector3.Project(dir , transform.forward);//上下の方向を消す
        support.rotation = Quaternion.LookRotation(dir, transform.forward);
    }

    /// <summary>
    /// 弾を発射してクールタイムに入る
    /// </summary>
    /// <returns></returns>
    IEnumerator Shot()
    {
        isCoolTime = true;


        for (int i = 0; i < rapidFire; i++)
        {
            if (target == null) break;

            Transform blood = Instantiate(bloodPrefab, bloodCreatePoint.position, Quaternion.identity).transform;
            StartCoroutine(ShotCourse(blood, target.transform.position));

            ContinuousAudio.PlaySoundPitchRandom(audioSource, shotSE);

            yield return new WaitForSeconds(fireRate);
        }

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

        const float TARGET_OFFSET_Y = 1f;

        targetPos.y += TARGET_OFFSET_Y;

        targetPos.x += Random.Range(-bulletShake , bulletShake);
        targetPos.y += Random.Range(-bulletShake , bulletShake);
        targetPos.z += Random.Range(-bulletShake , bulletShake);

        for (float t = 0; t < 1; t += SPEED * Time.deltaTime)
        {
            Vector3 pos = Vector3.Lerp(bloodCreatePoint.position, targetPos, t);

            blood.position = pos;

            yield return null;
        }

        
        if(target != null)
        {
            targetZombie.AddHP(-damage);
            Instantiate(hitPrefab, targetPos, Quaternion.LookRotation(bloodCreatePoint.position - targetPos));
        }

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
