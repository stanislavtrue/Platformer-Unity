using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection), typeof(Damageable))]
public class Player : MonoBehaviour
{
    public float loseDelay = 3f;
    private float walkSpeed = 3f;
    private float runSpeed = 6f;
    private float jumpImpulse = 15f; 
    private Vector2 moveInput;
    private TouchingDirection touchingDirection;
    private Damageable damageable;
    private bool handler = false;
    public bool CanMove
    {
        get => animator.GetBool(AnimationStrings.CanMove);
    }

    public bool IsAlive
    {
        get => animator.GetBool(AnimationStrings.IsAlive);
    }
    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirection.IsOnWall)
                {
                    if (IsRunning)
                        return runSpeed;
                    else
                        return walkSpeed;
                }
                else
                    return 0;
            }
            else
                return 0;
        }
    }

    private bool _IsMoving = false;
    private bool _IsRunning = false;

    public bool IsMoving
    {
        get => _IsMoving;
        private set
        {
            _IsMoving = value;
            animator.SetBool(AnimationStrings.IsMoving, value);
        }
    }

    public bool IsRunning
    {
        get => _IsRunning;
        private set
        {
            _IsRunning = value;
            animator.SetBool(AnimationStrings.IsRunning, value);
        }
    }

    public bool _IsFacingRight = true;

    public bool IsFacingRight
    {
        get => _IsFacingRight;
        private set
        {
            if(_IsFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }

            _IsFacingRight = value;
        }
    }


    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirection = GetComponent<TouchingDirection>();
        damageable = GetComponent<Damageable>();
        damageable.OnDied += OnPlayerDied;
    }

    private void FixedUpdate()
    {
        if(!damageable.LookVelocity)
            rb.linearVelocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.linearVelocity.y);
        animator.SetFloat(AnimationStrings.YVelocity, rb.linearVelocity.y);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
            IsMoving = false; 
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
            IsFacingRight = true;
        else if (moveInput.x < 0 && IsFacingRight)
            IsFacingRight = false;
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
            IsRunning = true;
        else if (context.canceled)
            IsRunning = false;
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && touchingDirection.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.JumpTrigger);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpImpulse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.AttackTrigger);

        }
    }

    public void OnHit(int Damage, Vector2 knockBack)
    {
        rb.linearVelocity = new Vector2(knockBack.x, rb.linearVelocity.y + knockBack.y);
    }

    private void OnDestroy()
    {
        damageable.OnDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        if (handler)
            return;
        handler = true;
        StartCoroutine(LoseAfterDelay());
    }

    private IEnumerator LoseAfterDelay()
    {
        yield return new WaitForSeconds(loseDelay);
        GameManager.Instance.LoseGame();
    }
}
