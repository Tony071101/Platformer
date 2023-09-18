using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    private int health = 100;
    private int damage = 10;
    private int playerDamage;
    private HealthSystem healthSystem;
    private Animator anim;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private Vector2 knockBack;
    private EnemyAttack enemyAttack;
    private bool _hasTarget = false;
    private Player player;
    private bool isDead = false;
    private bool canTakeDamage = true;
    private float attackedCDTime = .1f;
    public event EventHandler onBeingHitByPlayer;

    // Start is called before the first frame update
    private void Start()
    {
        player = FindObjectOfType<Player>();
        healthSystem = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        enemyAttack = GetComponentInChildren<EnemyAttack>();
    }

    public bool hasTarget
    {
        get { return _hasTarget; }
        private set
        {
            _hasTarget = value;
            anim.SetBool("hasTarget", value);
            anim.SetTrigger("Attack");
        }
    }

    // Update is called once per frame
    void Update()
    {
        hasTarget = enemyAttack.detectedColliders.Count > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isDead && canTakeDamage && collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            canTakeDamage = false; // Prevent further damage for a short period
            StartCoroutine(EnableDamageAfterCooldown());
            
            PlayerAttack playerAttack = collision.gameObject.GetComponent<PlayerAttack>();
            if (playerAttack != null)
            {
                playerDamage = playerAttack.Damage;
            }
            else
            {
                playerDamage = 0; // Default damage if the PlayerAttack script is missing
            }
            health = healthSystem.Hit(health, playerDamage);
            if (health <= 0)
            {
                Die();
            }
            anim.SetTrigger("Hurt");
            IsKnockedBack(knockBack);
            onBeingHitByPlayer?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Die()
    {
        isDead = true;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
        Destroy(this.gameObject, 2f);
    }

    private void IsKnockedBack(Vector2 knockBack)
    {
        Vector2 knockbackDirection = ((Vector2)transform.position - player.GetPlayerPos()).normalized;
        _rigidbody2D.velocity = new Vector2(knockBack.x * -knockbackDirection.x, knockBack.y);
    }

    private IEnumerator EnableDamageAfterCooldown()
    {
        yield return new WaitForSeconds(attackedCDTime); // Adjust the cooldown time as needed
        canTakeDamage = true;
    }

    public Vector2 GetEnemyPos() => this.gameObject.transform.position;
    public int GetEnemyDamage() => damage;
    public int GetPlayerDamage() => playerDamage;
}
