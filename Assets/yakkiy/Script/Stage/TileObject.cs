using UnityEngine;

public class TileObject : MonoBehaviour
{
    Rigidbody rb;
    RouteGenerator routeGenerator;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        routeGenerator = RouteGenerator.Instance;
    }

    private void Update()
    {
        FollowTheGround();
    }

    /// <summary>
    /// タイルに合わせて移動
    /// </summary>
    void FollowTheGround()
    {

        //if (!isMove) return;

        Vector3 pos = rb.position;

        pos.x += routeGenerator.Speed * (RouteGenerator.TILE_SCALE * 2) * Time.deltaTime;

        rb.MovePosition(pos);
    }
}
