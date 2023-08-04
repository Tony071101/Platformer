using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //Notice that this only Die when player collide with Enemy
        //Will be adjust to can pass through Enemy
        //And only Die when take enemy hit or health return to 0.
        if (other.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }

    private void Die()
    {
        _rigidbody2D.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
