using System;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public HealthBar healthBar;
    private Animator animator;

    public event Action OnDied;

    [SerializeField] private int _MaxHealth = 100;
    public int MaxHealth
    {
        get => _MaxHealth;
        set
        {
            _MaxHealth = value;
        }
    }

    private int _Health = 100;
    public int Health
    {
        get => _Health;
        set
        {
            if (_Health == value)
                return;
            _Health = value;
            if (_Health <= 0 && _IsAlive)
            {
                Die();
            }
        }
    }

    private bool _IsAlive = true;
    private bool IsInvincible = false;

    private float TimeSinceHit = 0;
    public float InvincibilityTimer = 0.25f;

    public bool IsAlive
    {
        get => _IsAlive;
        set
        {
            _IsAlive = value;
            animator.SetBool(AnimationStrings.IsAlive, value);
            Debug.Log("IsAlive set " + value);
        }
    }
    public bool LookVelocity
    {
        get => animator.GetBool(AnimationStrings.LookVelocity);
        set
        {
            animator.SetBool(AnimationStrings.LookVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Health = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        if (IsInvincible)
        {
            if (TimeSinceHit > InvincibilityTimer)
            {
                IsInvincible = false;
                TimeSinceHit = 0;
            }

            TimeSinceHit += Time.deltaTime;
        }
    }

    private void Die()
    {
        IsAlive = false;
        OnDied?.Invoke();
    }

    public bool Hit(int damage, Vector2 knockBack)
    {
        if (IsAlive && !IsInvincible)
        {
            Health -= damage;
            healthBar.SetHealth(Health);
            IsInvincible = true;
            animator.SetTrigger(AnimationStrings.HitTrigger);
            LookVelocity = true;
            damageableHit?.Invoke(damage, knockBack);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            return true;
        }
        return false;
    }

    public bool Heal(int healthRestored)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestored);
            Health += actualHeal;
            healthBar.SetHealth(Health);
            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
