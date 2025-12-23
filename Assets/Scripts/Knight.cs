using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection),typeof(Damageable))]
public class Knight : MonoBehaviour
{
    public float walkAcceleration = 2f;
    public float maxSpeed = 2f;
    public float walkStopRate = 0.015f;
    public DetectionZone attackZone;
    public DetectionZone CliffDetectionZone;

    private Rigidbody2D rb;
    private TouchingDirection touchingDirections;
    private Animator animator;
    private Damageable damageable;

    public enum WalkableDirection
    {
        Right,
        Left
    }

    private WalkableDirection _walkDirection;
    private Vector2 WalkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get => _walkDirection;
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1,
                    gameObject.transform.localScale.y);
                if (value == WalkableDirection.Right)
                    WalkDirectionVector = Vector2.right;
                else if (value == WalkableDirection.Left)
                    WalkDirectionVector = Vector2.left;
            }
            _walkDirection = value;
        }
    }

    private bool _HasTarget = false;

    public bool HasTarget
    {
        get => _HasTarget;
        private set
        {
            _HasTarget = value;
            animator.SetBool(AnimationStrings.HasTarget, value);
        }
    }

    public bool CanMove
    {
        get => animator.GetBool(AnimationStrings.CanMove);
    }

    public float AttackCooldown
    {
        get => animator.GetFloat(AnimationStrings.AttackCooldown);
        private set
        {
            animator.SetFloat(AnimationStrings.AttackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirection>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        HasTarget = attackZone.DetectedColliders.Count > 0;
        if (AttackCooldown > 0)
            AttackCooldown -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
            FlipDirection();
        if (!damageable.LookVelocity)
        {
            if (CanMove)
                rb.linearVelocity = new Vector2(Mathf.Clamp(rb.linearVelocity.x + (walkAcceleration * WalkDirectionVector.x * Time.fixedDeltaTime),
                    -maxSpeed, maxSpeed), rb.linearVelocity.y);
            else
                rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
        }
    } 

    private void FlipDirection()
    {
        if (WalkDirection == WalkableDirection.Right)
            WalkDirection = WalkableDirection.Left;
        else if (WalkDirection == WalkableDirection.Left)
            WalkDirection = WalkableDirection.Right;
        else
            Debug.LogError("Current walkable direction isn't set to legal values or right or left.");
    }

    public void OnHit(int damage, Vector2 knockBack)
    {
        rb.linearVelocity = new Vector2(knockBack.x, rb.linearVelocity.y + knockBack.y);
    }

    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
            FlipDirection();
    }
}
