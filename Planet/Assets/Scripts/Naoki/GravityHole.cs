using System;
using UnityEngine;
using UnityEngine.Pool;

public class GravityHole : MonoBehaviour
{
    public ObjectPool<GameObject> myPool;

    [SerializeField] private GameObject closeEffect;

    [Header("磁気吸着の設定")]
    [SerializeField] private float attractionForce = 15f;
    [SerializeField] private string magneticTag = "Planet";
    [SerializeField] private float residenceTime = 5;
    private float time = 0;

    private CircleCollider2D magneticRadius;

    public event Action onGravityClosed;

    private void OnEnable()
    {
        AudioManager.instance.PlaySE("UseGravity");
        time = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        magneticRadius = GetComponent<CircleCollider2D>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time > residenceTime)
        {
            time = 0;

            Instantiate(
                closeEffect,
                transform.position,
                Quaternion.identity
                );

            onGravityClosed?.Invoke();

            myPool?.Release(gameObject);
        }
    }

    void FixedUpdate()
    {
        ApplyMagneticPull();
    }

    private void ApplyMagneticPull()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(magneticRadius.transform.position, magneticRadius.radius);

        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag(magneticTag))
            {
                Vector2 direction = (transform.position - col.transform.position).normalized;

                col.gameObject.GetComponent<Rigidbody2D>().AddForce(direction * attractionForce, ForceMode2D.Force);
            }
        }
    }
}