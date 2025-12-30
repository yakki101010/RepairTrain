using UnityEngine;

public class TileObject : MonoBehaviour
{
    

    RouteGenerator routeGenerator;


    private void Start()
    {
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

        Vector3 pos = transform.position;

        pos.x += routeGenerator.Speed * (RouteGenerator.TILE_SCALE) * Time.deltaTime;

        transform.position = pos;
    }

    
}
