using UnityEngine;


/// <summary>
/// タイルの移動にObjectを追従させるスクリプト
/// </summary>
public class TileObject : MonoBehaviour
{
    //オブジェクト削除エリア最後尾の距離よりちょっと内
    float deadZoneMax;

    //オブジェクト削除エリア先頭の距離よりちょっと内
    float deadZoneMin;

    Rigidbody rb;

    //float delta;
    float memory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //オブジェクト削除エリア最後尾を1タイルの半分を加えた値に設定
        deadZoneMax = (RouteGenerator.Instance.transform.position.x + RouteGenerator.DEAD_LENGTH) + (RouteGenerator.TILE_SCALE * 0.5f);

        //オブジェクト削除エリア先端をタイル生成位置タイルの全長から1タイルの半分を引いた値に設定
        deadZoneMin = (RouteGenerator.Instance.transform.position.x) - (RouteGenerator.TILE_SCALE * 0.5f);
    }

    private void Update()
    {
        DespawnCheck();

        FollowTheGround();
    }

    void FollowTheGround()
    {
        Vector3 pos = rb.position;

        pos.x += RouteGenerator.Instance.Speed * (RouteGenerator.TILE_SCALE )* Time.deltaTime;

        rb.MovePosition(pos);
    }

    void DespawnCheck()
    {
        if (RouteGenerator.Instance.Speed == 0) return;

        if(transform.position.x > deadZoneMax || transform.position.x < deadZoneMin)
        {
            Destroy(gameObject);
        }
    }
}
