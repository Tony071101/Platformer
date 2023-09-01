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

    public int Hit(int currentHealth, int damage)
    {
        currentHealth -= damage;
        // if (currentHealth <= 0)
        // {
        //     Die();
        // }
        return currentHealth;
    }

    // private void Die()
    // {
    //     _rigidbody2D.bodyType = RigidbodyType2D.Static;
    //     anim.SetTrigger("death");
    //     // string objectTag = gameObject.tag;
    //     // StartCoroutine(DieWithDelay(objectTag));
    // }

    // private IEnumerator DieWithDelay(string tag)
    // {
    //     _rigidbody2D.bodyType = RigidbodyType2D.Static;
    //     anim.SetTrigger("death");
    //     float delayTime = 0f;
    //     if (tag == "Player")
    //     {
    //         delayTime = 1f;
    //     }
    //     else if (tag == "Enemy")
    //     {
    //         delayTime = 0.2f;
    //     }
    //     yield return new WaitForSeconds(delayTime);
        
    //     Destroy(gameObject);
    // }
}
