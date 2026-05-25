using UnityEngine;

public class PlanetCollisionFilter : MonoBehaviour
{
    [SerializeField] private int planetID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlanetCollisionFilter[] allPlanets = FindObjectsByType<PlanetCollisionFilter>(FindObjectsSortMode.None);

        Collider2D myCollider = GetComponent<Collider2D>();

        foreach (var otherPlanet in allPlanets)
        {
            if (otherPlanet == this) continue;

            if (this.planetID != otherPlanet.planetID)
            {
                Collider2D otherCollider = otherPlanet.GetComponent<Collider2D>();
                if (otherCollider != null)
                {
                    Physics2D.IgnoreCollision(myCollider, otherCollider);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.TryGetComponent<PlanetCollisionFilter>(out var otherPlanet))
    //    {
    //        if (this.planetID == 7 && otherPlanet.planetID == 7)
    //        {
    //            Collider2D myCollider = GetComponent<Collider2D>();
    //            Collider2D otherCollider = collision.collider;
    //            Physics2D.IgnoreCollision(myCollider, otherCollider);
    //        }
    //        else
    //        if (this.planetID != otherPlanet.planetID)
    //        {
    //            Collider2D myCollider = GetComponent<Collider2D>();
    //            Collider2D otherCollider = collision.collider;

    //            Physics2D.IgnoreCollision(myCollider, otherCollider);
    //        }
    //    }
    //}
}
