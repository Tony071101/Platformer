using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

public class PortalController : MonoBehaviour
{
    [SerializeField] private AudioClip spawnSound;
    private SpriteRenderer spriteRenderer;
    private float fadeInDuration = 2f;
    private CinemachineVirtualCamera virtualCamera;

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        FadeInDOTWEEN();
        LookAtPortal();

        StartCoroutine(ResetCamera());
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            UIManager.Instance.ActivateGameFinishPanel();
            //Will change this to go next level if there'll be more level in future.
        }
    }

    private void FadeInDOTWEEN() {
        AudioSource.PlayClipAtPoint(spawnSound, this.transform.position, 1);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        spriteRenderer.DOFade(1f, fadeInDuration);
    }

    private void LookAtPortal()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = this.transform;
        }
    }

    private IEnumerator ResetCamera()
    {
        yield return new WaitForSeconds(3f); // Chờ 2 giây

        // Chuyển camera trở lại theo dõi player
        if (virtualCamera != null)
        {
            virtualCamera.Follow = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }
}
