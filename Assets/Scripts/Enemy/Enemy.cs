using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health = 30;
    private PlayerAttack playerAttack;
    private int playerDamage;
    private HealthSystem healthSystem;
    private Animator anim;
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private Vector2 knockBack;
    private EnemyAttack enemyAttack;
    private bool _hasTarget = false;

    // Start is called before the first frame update
    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            playerDamage = playerAttack.GetDamage();
            healthSystem.Hit(playerDamage, ref health);
            anim.SetTrigger("Hurt");
            IsKnockedBack(knockBack);
        }
    }

    private void IsKnockedBack(Vector2 knockBack)
    {
        _rigidbody2D.velocity = new Vector2(knockBack.x, _rigidbody2D.velocity.y + knockBack.y);
    }
}
