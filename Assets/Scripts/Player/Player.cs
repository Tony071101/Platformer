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
    private Animator anim;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyHitBox"))
        {
            anim.SetTrigger("hurt");
            onKnockBack?.Invoke(knockBack);
        }
    }

}
