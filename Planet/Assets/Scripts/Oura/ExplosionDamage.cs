using UnityEngine;
using System.Collections;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField]
    private int playerDamage = 30;

    [SerializeField]
    private int planetDamage = 20;

    // 判定時間
    [SerializeField]
    private float hitDuration = 0.05f;

    private CircleCollider2D circleCollider;

    private void Awake()
    {
        circleCollider =
            GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        // 一瞬後に判定OFF
        StartCoroutine(
            DisableCollider()
        );
    }

    IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(
            hitDuration
        );

        if (circleCollider != null)
        {
            circleCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("接触 : " + other.name);

        // プレイヤーダメージ
        PlayerHealth playerHealth =
            other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                other.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth != null)
        {
            Debug.Log(
                "<color=red>プレイヤーに爆発ダメージ！</color>"
            );
            if (PlayerHealth.Instance != null && PlayerHealth.Instance.isStarInvincible == false)
            {
                playerHealth.TakeDamage();
            }
        }

        // 惑星ダメージ
        PlanetHealth planetHealth =
            other.GetComponent<PlanetHealth>();

        if (planetHealth == null)
        {
            planetHealth =
                other.GetComponentInParent<PlanetHealth>();
        }

        if (planetHealth != null)
        {
            Debug.Log(
                "<color=yellow>惑星に爆発ダメージ！</color>"
            );

            planetHealth.TakeDamage(
                planetDamage
            );
        }
    }
}