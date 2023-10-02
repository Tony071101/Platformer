using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text playerHealthValue;
    [SerializeField] private Text playerDamageValue;
    [SerializeField] private Text enemyDamageValue;
    [SerializeField] private Text defeatObjective;
    [SerializeField] private GameObject nextStageCanvas;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject thankyouCanvas;
    private Player player;
    private PlayerAttack playerAttack;
    private Enemy enemy;
    private Enemy[] enemies;
    private float timeToAppear = .5f;
    private float timeWhenDisappear;
    private int defeatObjectiveValues;
    private int defeatedEnemiesCount;
    private static bool gameIsPaused = false;
    
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
        PauseGame();
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
        defeatObjectiveValues = GameManager._instance.GetDefeatObjective();
        defeatObjective.text = ($"Defeat all Enemies: {defeatedEnemiesCount}/{defeatObjectiveValues}");
    }

    public void ShowNextStageUI(){
        nextStageCanvas.SetActive(true);
    }

    public void ThankYouCanvas(){
        thankyouCanvas.SetActive(true);
    }

    private void PauseGame() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(gameIsPaused){
                GameResume();
            }
            else{
                GamePause();
            }
        }
    }

    public void GameResume(){
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void GamePause(){
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void ReturnHomeScene(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartLevel(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
