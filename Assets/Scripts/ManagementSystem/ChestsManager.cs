using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestsManager : MonoBehaviour
{
    public static ChestsManager Instance { get; private set; }
    public int chestsCount { get; private set; }
    public int totalChest { get; private set; } = 0;
    public UnityAction<int> OnChestCountChanged;
    HealthBar healthBar;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        healthBar = FindObjectOfType<HealthBar>();
        totalChest = transform.childCount;
        if(GameManager.Instance != null) {
            GameManager.Instance.SetTotalChest(totalChest);
            CalculateTotalChest();
        }else{
            return;
        }
    }

    public void IncrementChestCount()
    {
        chestsCount++;
        if(GameManager.Instance != null) {
            GameManager.Instance.UpdateChestOpened(chestsCount);
            GameManager.Instance.CheckChestsOpened();
            CalculateTotalChest();
        } else{
            return;
        }
    }

    private void CalculateTotalChest()
    {
        healthBar.CalculateTotalChest(chestsCount, totalChest);
    }
}
