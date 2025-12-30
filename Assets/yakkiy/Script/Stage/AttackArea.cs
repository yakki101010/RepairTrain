using UnityEngine;

public class AttackArea : MonoBehaviour
{
    [SerializeField] int damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)//ゾンビに触れたらダメージを与える
        {
            other.gameObject.GetComponent<Zombie>().AddHP(-damage);

        }
    }
}
