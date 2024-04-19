using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HealthUIText : MonoBehaviour
{
    private Vector3 moveSpeed = new Vector3(0, 75f, 0);
    private float timeToFade = 1.5f;
    private RectTransform textTransform;
    private TextMeshProUGUI textMeshPro;
    private bool isFading = false;

    private void Awake() {
        textTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
    }

    private void Update() {
        textTransform.position += moveSpeed * Time.deltaTime;
        if (!isFading)
        {
            StartFadeAway();
        }
    }

    private void StartFade()
    {
        // Use DOTween to fade out the TextMeshProUGUI text
        textMeshPro.DOFade(0f, timeToFade).OnComplete(() => OnFadeComplete());
    }

    // Call this method when you want to start the fade
    private void StartFadeAway()
    {
        // You can add any additional actions before starting the fade here
        isFading = true;
        StartFade();
    }

    private void OnFadeComplete()
    {
        // Check if the object is null before accessing it
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
