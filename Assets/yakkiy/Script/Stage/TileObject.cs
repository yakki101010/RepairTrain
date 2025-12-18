using UnityEngine;


/// <summary>
/// タイルの移動にObjectを追従させるスクリプト
/// </summary>
public class TileObject : MonoBehaviour
{
    //オブジェクト削除エリア最後尾の距離よりちょっと内
    const float DEAD_ZONE_MAX = 90f;

    //オブジェクト削除エリア先頭の距離よりちょっと内
    const float DEAD_ZONE_MIN = -90f;

    Rigidbody rb;

    //float delta;
    float memory;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        if(transform.position.x > DEAD_ZONE_MAX || transform.position.x < DEAD_ZONE_MIN)
        {
            Destroy(gameObject);
        }
    }
}
