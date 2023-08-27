using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Hit(int damage, ref int health)
    {
        // health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

}
