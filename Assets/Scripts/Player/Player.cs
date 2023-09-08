using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public static Player Instance { get; private set; }
    public event EventHandler onMove;
    public event EventHandler onAttack;
    public event Action<Vector2> onKnockBack;
    private HealthSystem healthSystem;
    private Enemy enemy;
    private Animator anim;
    private int health = 100;
    private bool isDead = false;
    private int enemyDamage;
    private Rigidbody2D _rigidbody2D;

    [SerializeField] private Vector2 knockBack;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
        enemy = FindObjectOfType<Enemy>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }



    private void Move()
    {
        onMove?.Invoke(this, EventArgs.Empty);
    }

    private void Attack()
    {
        onAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isDead && other.gameObject.layer == LayerMask.NameToLayer("EnemyHitBox"))
        {
            enemyDamage = enemy.GetEnemyDamage();
            health = healthSystem.Hit(health, enemyDamage);
            Debug.LogError($"Player being hit, Current health: {health}");
            if(health <= 0 ){
                Die();
            }
            anim.SetTrigger("hurt");
            onKnockBack?.Invoke(knockBack);
        }
    }

    private void Die()
    {
        isDead = true;
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }
    public Vector2 GetPlayerPos() => this.gameObject.transform.position;
}
