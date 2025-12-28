using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] float knockBack = 1;
    [SerializeField] int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)//ゾンビに触れたらダメージを与える
        {
            other.gameObject.GetComponent<Zombie>().AddHP(-damage);

            if (knockBack <= 0) return;

            if(other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                //吹き飛ぶ方向を指定
                Vector3 vector = (other.gameObject.transform.position - transform.position).normalized;
                //爆心地に近いほど上方向へ跳ぶ
                vector.y = knockBack / Vector3.Distance(other.gameObject.transform.position , transform.position);
                //勢いを適用
                vector.x *= knockBack;
                vector.z *= knockBack;

                rb.AddForce(vector);

            }
        }
    }
}
