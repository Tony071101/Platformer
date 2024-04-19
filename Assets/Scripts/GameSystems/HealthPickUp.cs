using System.Collections;
using System.Collections.Generic;
using Cainos.PixelArtPlatformer_VillageProps;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    private int healthRestored = 20;
    private AudioSource pickupSource;
    private Animator _anim;
    private bool hasCollided = false;

    private void Awake() {
        pickupSource = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (!hasCollided) {
            Damageable damageable = other.gameObject.GetComponent<Damageable>();

            damageable.Heal(healthRestored);
            _anim.SetBool(AnimationStrings.isOpened, true);
            if (pickupSource) {
                AudioSource.PlayClipAtPoint(pickupSource.clip, gameObject.transform.position, pickupSource.volume);
            }
            ChestsManager.Instance.IncrementChestCount();

            hasCollided = true;
        }
    }
}
