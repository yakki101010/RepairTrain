using UnityEngine;

public class ZombieMove : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 vector = Vector3.zero;

        vector = (Train.Instance.transform.position - transform.position);

        rb.linearVelocity = vector.normalized;
    }
}
