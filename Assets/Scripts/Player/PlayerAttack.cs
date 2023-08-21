using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    private int damage = 10;
    private Player player;
    private Animator animator;
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
        animator.SetTrigger("attackOne");
    }

    public int GetDamage() => damage;
}