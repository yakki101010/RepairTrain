using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Harvester : MonoBehaviour
{
    [Header("回転オブジェクト")]
    [SerializeField] Transform roller;
    [Header("回転速度")]
    [SerializeField] float speed = 10;

    [Header("攻撃力")]
    [SerializeField] int damage = 1;
    [Header("攻撃頻度")]
    [SerializeField] float interval = 0.1f;

    [Header("攻撃音")]
    [SerializeField] AudioClip attackSE;

    [Header("ダメージエフェクト")]
    [SerializeField] GameObject attackEffectPrefab;

    List<Zombie> hitZombies = new List<Zombie>();

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(Attack());
    }

    private void Update()
    {
        roller.Rotate(speed, 0, 0);
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        while (true)
        {
            if(hitZombies.Count > 0)
            {
                for (int i = hitZombies.Count  -1; i >= 0 ; i--)
                {
                    if(!hitZombies[i].gameObject.activeSelf) hitZombies[i] = null;//オブジェクトが非アクティブなら登録解除

                    if (hitZombies[i] == null)//登録解除されていたらリストを削除
                    {
                        hitZombies.RemoveAt(i);
                    }
                    else
                    {
                        hitZombies[i].AddHP(-damage);

                        Instantiate(attackEffectPrefab , hitZombies[i].transform.position , transform.rotation);
                    }

                }

                ContinuousAudio.PlaySoundPitchRandom(audioSource, attackSE);
            }


            yield return new WaitForSeconds(interval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            hitZombies.Add(other.gameObject.GetComponent<Zombie>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            hitZombies.Remove(other.gameObject.GetComponent<Zombie>());
        }
    }
}
