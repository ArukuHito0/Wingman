using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    [SerializeField]
    private int playerDamage = 30;

    [SerializeField]
    private float planetDamage = 20f;

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

            playerHealth.TakeDamage(playerDamage);
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

            planetHealth.TakeDamage(planetDamage);
        }
    }
}