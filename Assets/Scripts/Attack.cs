using UnityEngine;

public class Attacl : MonoBehaviour
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
            Vector2 deliveredKnockback =
                transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            bool gotHit = damageable.Hit(attackDamage, deliveredKnockback);
            if(gotHit)
                Debug.Log(collision.name + " hit for " + attackDamage);
        }
    }
}
