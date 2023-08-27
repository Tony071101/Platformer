using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public List<Collider2D> detectedColliders = new List<Collider2D>();
    private Collider2D _collider2D;

    // Start is called before the first frame update
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            detectedColliders.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            detectedColliders.Remove(other);
        }
    }
}
