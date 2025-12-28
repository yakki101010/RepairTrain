using UnityEngine;

public class Cowcatcher : MonoBehaviour
{
    [Header("基本ダメージ")]
    [SerializeField] int basicDamage = 10;
    [Header("スピードによるダメージ倍率\n初期状態の列車最高速度は0.3なので10倍くらいで考える")]
    [SerializeField] float speedDamageMultiplier = 1.5f;

    RouteGenerator routeGenerator;

    int damage;

    void Start()
    {
        routeGenerator = RouteGenerator.Instance;
    }

    private void Update()
    {
        damage = (int)(basicDamage * (speedDamageMultiplier * routeGenerator.Speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)//ゾンビに触れたらダメージを与える
        {
            other.gameObject.GetComponent<Zombie>().AddHP(-damage);

        }
    }
}
