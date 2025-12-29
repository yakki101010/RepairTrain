using UnityEngine;

public class Cowcatcher : MonoBehaviour
{
    const float RANDOM_PITCH_MIN = 1f;
    const float RANDOM_PITCH_MAX = 1.5f;

    [Header("基本ダメージ")]
    [SerializeField] int basicDamage = 10;
    [Header("スピードによるダメージ倍率\n初期状態の列車最高速度は0.3なので10倍くらいで考える")]
    [SerializeField] float speedDamageMultiplier = 1.5f;

    [Header("大ダメージ音")]
    [SerializeField] AudioClip criticalHitSE;

    RouteGenerator routeGenerator;

    AudioSource audioSource;

    int damage;

    void Start()
    {
        routeGenerator = RouteGenerator.Instance;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        damage = (int)(basicDamage * (speedDamageMultiplier * routeGenerator.Speed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)//ゾンビに触れたらダメージを与える
        {
            if (damage > 100)
            {
                audioSource.pitch = Random.Range(RANDOM_PITCH_MIN, RANDOM_PITCH_MAX);
                audioSource.PlayOneShot(criticalHitSE); 
            }

            other.gameObject.GetComponent<Zombie>().AddHP(-damage);

        }
    }
}
