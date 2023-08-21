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
    [SerializeField] Vector2 knockBack;
    // Start is called before the first frame update
    private void Start()
    {
        playerAttack = FindObjectOfType<PlayerAttack>();
        healthSystem = GetComponent<HealthSystem>();
        anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerHitBox"))
        {
            playerDamage = playerAttack.GetDamage();
            healthSystem.Hit(playerDamage, ref health);
            anim.SetTrigger("Hurt");
            KnockBack(knockBack);
        }
    }

    private void KnockBack(Vector2 knockBack)
    {
        _rigidbody2D.velocity = new Vector2(knockBack.x, _rigidbody2D.velocity.y + knockBack.y);
    }
}
