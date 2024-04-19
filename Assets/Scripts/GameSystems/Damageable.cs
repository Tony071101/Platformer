using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;

    [HideInInspector]
    public UnityEvent<int, int> healthChanged;
    private Animator _anim;
    private float timeSinceHit = 0;
    private float invincibilityTime = 0.2f; //Can be customize
    private bool isInvincible = false;
    private int _maxHealth = 100;
    public int MaxHealth { 
        get {
            return _maxHealth;
        } 
        private set {
            _maxHealth = value;
        } 
    }

    private int _health = 100;
    public int Health {
        get {
            return _health;
        }
        private set {
            _health = value;

            healthChanged?.Invoke(_health, MaxHealth);

            if(_health <= 0) {
                IsAlive = false;
                Destroy(gameObject, 2f);
            }
        }
    }

    private bool _isAlive = true;
    public bool IsAlive {
        get {
            return _isAlive;
        }
        private set {
            _isAlive = value;
            _anim.SetBool(AnimationStrings.isAlive, value);
        }
    }

    public bool LockVelocity { 
        get {
            if(_anim == null) {
                return false;
            }
            return _anim.GetBool(AnimationStrings.lockVelocity);
        }
        set {
            _anim.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake() {
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update() {
        if(isInvincible) {
            if(timeSinceHit > invincibilityTime) {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    public bool Hit(int dmg, Vector2 knockBack) {
        if(IsAlive && !isInvincible) {
            Health -= dmg;
            isInvincible = true;

            _anim.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(dmg, knockBack);
            CharacterEvents.characterDamaged?.Invoke(gameObject, dmg);

            return true;
        }

        return false;
    }

    public bool Heal(int healthRestore) {
        if(IsAlive && Health < MaxHealth) {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;

            CharacterEvents.characterHealed.Invoke(gameObject, actualHeal);
            return true;
        }

        return false;
    }
}
