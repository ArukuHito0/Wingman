using UnityEngine;

public class PlanetMagneticTarget : MonoBehaviour
{
    [Header("磁気吸着の設定")]
    [SerializeField] private float attractionRadius = 4f;
    [SerializeField] private float attractionForce = 15f;
    [SerializeField] private string magneticTag = "Magnetic";

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        ApplyMagneticPull();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ApplyMagneticPull()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attractionRadius);

        foreach (Collider2D col in hitColliders)
        {
            if (col.CompareTag(magneticTag))
            {
                Vector2 direction = (col.transform.position - transform.position).normalized;

                rb.AddForce(direction * attractionForce, ForceMode2D.Force);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attractionRadius);
    }
}
