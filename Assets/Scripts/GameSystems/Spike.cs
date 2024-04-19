using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private int attackDmg;
    [SerializeField] private Vector2 knockBack = Vector2.zero;
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();

            if(damageable != null) {
                bool gotHit = damageable.Hit(attackDmg, knockBack);
            }
        }
    }
}
