using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class AttackSystem : MonoBehaviour
{
    [SerializeField] private int attackDmg;
    [SerializeField] private Vector2 knockBack = Vector2.zero;
    private void OnTriggerEnter2D(Collider2D other) {
        Damageable damageable = other.GetComponent<Damageable>();

        if(damageable != null) {
            Vector2 deliveredKnockBack = transform.parent.localScale.x > 0 ? knockBack : new Vector2(-knockBack.x, knockBack.y);
            bool gotHit = damageable.Hit(attackDmg, deliveredKnockBack);
        }
    }
}
