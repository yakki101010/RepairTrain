using System.Collections;
using UnityEngine;

public class Mortar : MonoBehaviour
{
    const float RANDOM_PITCH_MIN = 1f;
    const float RANDOM_PITCH_MAX = 1.5f;

    [Header("クールタイム")]
    [SerializeField] float coolTime;


    [Header("ターゲット位置マーク")]
    [SerializeField] GameObject targetMarkObj;

    [Header("弾プレハブ")]
    [SerializeField] GameObject bloodPrefab;
    [Header("爆発プレハブ")]
    [SerializeField] GameObject explosionPrefab;

    [Header("発射音")]
    [SerializeField] AudioClip shotSE;
    [Header("着弾音")]
    [SerializeField] AudioClip explosionSE;

    [Header("y軸回転")]
    [SerializeField] Transform support;
    [Header("x軸回転")]
    [SerializeField] Transform gunBarrel;
    [Header("攻撃のターゲットに\n指定できるレイヤー")]
    [SerializeField] LayerMask flowLayer;

    AudioSource audioSource;

    InputManager inputManager;

    Vector3 targetPos;

    bool isFastHit = true;

    Coroutine coolingCoroutine;//クールタイム中コルーチン

    private void Start()
    {
        Initialization();

    }

    private void Update()
    {
        Targeting();
    }
    /// <summary>
    /// 初期化
    /// </summary>
    void Initialization()
    {
        inputManager = InputManager.Instance;

        inputManager.CallbackLeftClickDown(Shot);

        audioSource = GetComponent<AudioSource>();
    }
    /// <summary>
    /// 攻撃入力
    /// </summary>
    void Shot()
    {
        if(coolingCoroutine == null) coolingCoroutine = StartCoroutine(CoolTime());
    }

    void Targeting()
    {
        Ray ray = Camera.main.ScreenPointToRay(inputManager.MousePos);    // カメラからマウスカーソルの位置のRayを作成
        RaycastHit hit;
        // Rayを飛ばして当たり判定をチェック
        if (Physics.Raycast(ray, out hit, float.MaxValue, flowLayer))
        {
            targetPos = hit.point;

            targetMarkObj.transform.position = targetPos;

        }
    }


    /// <summary>
    /// クールタイムを挟む
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolTime()
    {
        StartCoroutine(MortarShot());

        yield return new WaitForSeconds(coolTime);//クールタイム

        coolingCoroutine = null;
    }

    /// <summary>
    /// 迫撃
    /// </summary>
    /// <returns></returns>
    IEnumerator MortarShot()
    {
        audioSource.PlayOneShot(shotSE);

        Transform bloodTransform = Instantiate(bloodPrefab, transform.position, Quaternion.identity).transform;

        yield return StartCoroutine(Parabola(bloodTransform, targetPos));

        Instantiate(explosionPrefab, bloodTransform.position, Quaternion.identity);

        audioSource.pitch = Random.Range(RANDOM_PITCH_MIN, RANDOM_PITCH_MAX);
        audioSource.PlayOneShot(explosionSE);
    }

    /// <summary>
    /// 発射軌道放物線(使いまわす場合クラスを分ける)
    /// </summary>
    IEnumerator Parabola(Transform projectile , Vector3 target)
    {
        float gravity = -70f;

        float time = 1f;//到達速度

        //ベクトルを指定
        Vector3 velocity;
        velocity.x = (target.x - projectile.position.x) / time;
        velocity.z = (target.z - projectile.position.z) / time;

        //上方向初速
        velocity.y = 30f;

        for (float t = 0; t < 1; t+= Time.deltaTime)
        {
            projectile.position += velocity * Time.deltaTime;

            //タイルに流されるようにする
            velocity.x += RouteGenerator.Instance.Speed * (RouteGenerator.TILE_SCALE) * Time.deltaTime;

            //重力
            velocity.y += gravity * Time.deltaTime;

            yield return null;
        }
    }
}
