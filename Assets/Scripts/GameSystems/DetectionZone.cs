using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DetectionZone : MonoBehaviour
{
    public UnityEvent noCollidersRemain;
    private BoxCollider2D boxCollider;
    public List<Collider2D> detectedColliders = new List<Collider2D>();

    private void Awake() {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        detectedColliders.Add(other);
    }

    private void OnTriggerExit2D(Collider2D other) {
        detectedColliders.Remove(other);

        if(detectedColliders.Count <= 0) {
            noCollidersRemain.Invoke();
        }
    }
}
