using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    private Player player;
    private Animator animator;
    [SerializeField] private int damage;
    public int Damage { get { return damage; } }
    // Start is called before the first frame update
    private void Start()
    {
        player = GetComponentInParent<Player>();
        player.onAttack += Attack;
        animator = GetComponentInParent<Animator>();
    }

    private void OnDisable()
    {
        player.onAttack -= Attack;
    }

    private void Attack(object sender, EventArgs e)
    {
        animator.SetTrigger("attack");
    }
}
