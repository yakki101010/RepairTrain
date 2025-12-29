using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    //オブジェクト削除エリア最後尾
    const float DEAD_ZONE_MAX = 90f;

    //オブジェクト削除エリア先頭
    const float DEAD_ZONE_MIN = -90f;

    [SerializeField] LayerMask trainLayer;

    [Header("体力")]
    [SerializeField] int maxHp = 100;
    int hp;
    [Header("攻撃力")]
    [SerializeField] int damage = 10;
    [Header("攻撃頻度")]
    [SerializeField] float attackInterval = 1f;
    [Header("移動速度")]
    [SerializeField] float speed = 1;
    [Header("取得スクラップ")]
    [SerializeField] int obtainScrap = 27;

    TrainController trainController;
    ZombieObjectPool zombieObjectPool;

    RouteGenerator routeGenerator;

    GameManager gameManager;

    ///bool isMove = true;
    bool isStick;
    bool isAttackInterval;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        routeGenerator = RouteGenerator.Instance;
        gameManager = GameManager.Instance;
    }

    private void OnEnable()
    {
        hp = maxHp;
    }

    private void Update()
    {
        Move();

        DespawnCheck();

        FollowTheGround();

        if (isStick && !isAttackInterval) StartCoroutine(Attack());
    }

    /// <summary>
    /// 列車コントローラーを登録
    /// </summary>
    /// <param name="controller"></param>
    public void SetTrainController(TrainController controller)
    {
        trainController = controller;
    }
    /// <summary>
    /// オブジェクトプールを登録
    /// </summary>
    /// <param name="objectPool"></param>
    public void SetZombieObjectPool(ZombieObjectPool objectPool)
    {
        zombieObjectPool = objectPool;
    }

    void Move()
    {
        Vector3 vector = Vector3.zero;

        vector = (Train.Instance.transform.position - transform.position);
        vector.y = 0;

        transform.rotation = Quaternion.LookRotation(vector);

        rb.linearVelocity = vector.normalized * speed;
    }

    public void AddHP(int addHp)
    {
        hp += addHp;

        if(hp <= 0)
        {
            Dead();

        }
    }

    void Dead()
    {
        
        GameManager.Instance.scrap.AddAmountOwned(obtainScrap);//スクラップを渡す

        Leave();

        zombieObjectPool.AddInactive(gameObject);//オブジェクトプールに返す
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & trainLayer) != 0)
        {
            if (isStick) return;
            trainController.Stick();//列車のまとわりつきカウントを増やす
            isStick = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & trainLayer) != 0)
        {
            Leave();
        }
    }

    /// <summary>
    /// 列車から離れた
    /// </summary>
    public void Leave()
    {
        if (!isStick) return;

        trainController.Leave();//列車のまとわりつきカウントを減らす
        isStick = false;
    }

    /// <summary>
    /// タイルに合わせて移動
    /// </summary>
    void FollowTheGround()
    {

        //if (!isMove) return;

        Vector3 pos = rb.position;

        pos.x += routeGenerator.Speed * (RouteGenerator.TILE_SCALE) * Time.deltaTime;

        rb.MovePosition(pos);
    }

    /// <summary>
    /// 範囲外にでたら削除
    /// </summary>
    void DespawnCheck()
    {
        if (routeGenerator.Speed == 0) return;

        if (transform.position.x > DEAD_ZONE_MAX || transform.position.x < DEAD_ZONE_MIN)
        {
            zombieObjectPool.AddInactive(gameObject);//オブジェクトプールに返す
        }
    }

    /// <summary>
    /// ダメージを与えてクールタイムに入る
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        isAttackInterval = true;
        yield return new WaitForSeconds(attackInterval);

        if(isStick) gameManager.life.AddAmountOwned(-damage);
        isAttackInterval = false;
    }
}
