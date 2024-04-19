using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(other.gameObject);
        if(other.gameObject.CompareTag("Player")){
            UIManager.Instance.ActivateGameOverPanel();
        }
    }
}
