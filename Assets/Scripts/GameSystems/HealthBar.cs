using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TMP_Text healthBarText;
    [SerializeField] private TMP_Text chestsText;
    Damageable damageable;

    private void Awake() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player == null) {
            Debug.Log("No Player found, make sure the player gameobject have tag Player.");
        }

        damageable = player.GetComponent<Damageable>();
    }
    
    private void Start() {
        healthSlider.value = CalculateSliderPercentage(damageable.Health, damageable.MaxHealth);
        healthBarText.text = "HP: " + damageable.Health + "/" + damageable.MaxHealth;
    }

    private void OnEnable() {
        damageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable() {
        damageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }

    private float CalculateSliderPercentage(float currentHealth, float maxHealth)
    {
        return currentHealth / maxHealth;
    }

    public void CalculateTotalChest(int chestOpened, int maxChest) {
        chestsText.text = $"CHESTS: {chestOpened}/{maxChest}";
    }

    private void OnPlayerHealthChanged(int newhealth, int maxHealth) {
        healthSlider.value = CalculateSliderPercentage(newhealth, maxHealth);
        healthBarText.text = "HP: " + newhealth + "/" + maxHealth;
    }
    
}
