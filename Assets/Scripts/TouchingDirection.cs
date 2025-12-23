using UnityEngine;
using UnityEngine.Serialization;

public class TouchingDirection : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    public float cellingDistance = 0.05f;

    private CapsuleCollider2D touchingCol;
    private Animator animator;

    private RaycastHit2D[] groundHits = new RaycastHit2D[5];
    private RaycastHit2D[] wallHits= new RaycastHit2D[5];
    private RaycastHit2D[] cellingHits= new RaycastHit2D[5];

    private bool _IsGrounded = true;

    public bool IsGrounded
    {
        get => _IsGrounded;
        private set
        {
            _IsGrounded = value;
            animator.SetBool(AnimationStrings.IsGrounded, value);
        }
    }

    private bool _IsOnWall;
    public bool IsOnWall
    {
        get => _IsOnWall;
        private set
        {
            _IsOnWall = value;
            animator.SetBool(AnimationStrings.IsOnWall, value);
        }
    }

    private bool _IsOnCelling;
    public bool IsOnCelling
    {
        get => _IsOnCelling;
        private set
        {
            _IsOnCelling = value;
            animator.SetBool(AnimationStrings.IsOnCelling, value);
        }
    }

    public Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCelling = touchingCol.Cast(Vector2.up, castFilter, cellingHits, cellingDistance) > 0;
    }
}
