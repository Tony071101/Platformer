using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text playerHealthValue;
    [SerializeField] private Text playerDamageValue;
    [SerializeField] private Text enemyDamageValue;
    [SerializeField] private Text defeatObjective;
    private Player player;
    private PlayerAttack playerAttack;
    private Enemy enemy;
    private Enemy[] enemies;
    private float timeToAppear = .5f;
    private float timeWhenDisappear;
    private int defeatObjectiveValues;
    private int defeatedEnemiesCount;
    void Start()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
        enemies = FindObjectsOfType<Enemy>();
        player.onBeingHit += PlayerHit;
        foreach (Enemy enemy in enemies)
        {
            enemy.onBeingHitByPlayer += EnemyHit;
        }
        defeatObjectiveValues = GameManager._instance.GetDefeatObjective();
        playerAttack = player.GetComponentInChildren<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHealthUpdate();
        UpdateObjective();
        if(enemyDamageValue.enabled && Time.time >= timeWhenDisappear){
            enemyDamageValue.enabled = false;
        }

        if(playerDamageValue.enabled && Time.time >= timeWhenDisappear){
            playerDamageValue.enabled = false;
        }
    }

    private void PlayerHealthUpdate(){
        playerHealthValue.text = player.GetPlayerHealth().ToString();
    }

    private void PlayerHit(object sender, EventArgs e) {
        enemyDamageValue.enabled = true;
        enemyDamageValue.text = enemy.GetEnemyDamage().ToString();
        timeWhenDisappear = Time.time + timeToAppear;
    }

    private void EnemyHit(object sender, EventArgs e) {
        Enemy enemyHit = sender as Enemy;
        if (enemyHit != null) {
            playerDamageValue.enabled = true;
            int damage = enemyHit.GetPlayerDamage();
            playerDamageValue.text = damage.ToString();
            timeWhenDisappear = Time.time + timeToAppear;
        }
    }

    private void UpdateObjective(){
        defeatedEnemiesCount = GameManager._instance.GetCurrentDefeatObjective();
        defeatObjective.text = ($"Objective: {defeatedEnemiesCount}/{defeatObjectiveValues}");
    }

}
