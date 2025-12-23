using UnityEngine;

public class Attack : MonoBehaviour
{
    public int attackDamage = 15;
    public Vector2 knockBack = Vector2.zero;

    private Collider2D attackCollider;
    private void Awake()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        if(damageable != null)
        {
            bool gotHit = damageable.Hit(attackDamage, knockBack);
            if(gotHit)
                Debug.Log(collision.name + " hit for " + attackDamage);
        }
    }
}
