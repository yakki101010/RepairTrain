using UnityEngine;

public class Bloodstain : MonoBehaviour
{
    //オブジェクト削除エリア最後尾
    const float DEAD_ZONE_MAX = 90f;

    //オブジェクト削除エリア先頭
    const float DEAD_ZONE_MIN = -90f;

    RouteGenerator routeGenerator;

    public BloodstainObjectPool bloodstainObjectPool;


    private void Start()
    {
        routeGenerator = RouteGenerator.Instance;
    }
    private void Update()
    {
        DespawnCheck();
    }

    /// <summary>
    /// 範囲外にでたら削除
    /// </summary>
    void DespawnCheck()
    {
        if (routeGenerator.Speed == 0) return;

        if (transform.position.x > DEAD_ZONE_MAX || transform.position.x < DEAD_ZONE_MIN)
        {
            bloodstainObjectPool.AddInactive(gameObject);
        }
    }
}
