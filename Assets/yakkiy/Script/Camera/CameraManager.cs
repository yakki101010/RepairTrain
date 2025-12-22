using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform target;

    void Start()
    {
        
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (target == null) return;

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    public void SetTarget(Transform tage)
    {
        target = tage;
    }
}
