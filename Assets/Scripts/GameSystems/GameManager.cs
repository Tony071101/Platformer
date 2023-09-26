using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }
    private Player playerGameObject;
    private Enemy[] enemies;
    private UIManager uIManager;
    [SerializeField] private int stage;
    public int lives { get; private set; }

    private int currentDefeatObjective;
    private int defeatObjective;

    private void Awake() {
        Debug.Log("Awake Game Manager");
        if (_instance != null)
        {
            Destroy(this.gameObject);
        } else{
            _instance = this;
        }
    }

    private void OnDestroy() {
        if(_instance == this){
            _instance = null;
        }        
    }

    private void Start() {
        Debug.Log("Start Game Manager");
        playerGameObject = FindObjectOfType<Player>();
        enemies = FindObjectsOfType<Enemy>();
        defeatObjective = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if(playerGameObject != null){
            lives = playerGameObject.GetPlayerHealth(); 
        }
        uIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        GetEnemyDeath();
        GameNextStage();
    }

    public void NewGame(){
        LoadLevel(stage);
    }

    private void LoadLevel(int stage){
        this.stage = stage;
        SceneManager.LoadScene($"Stage-{stage}");
    }

    public void NextLevel(){
        LoadLevel(stage + 1);
    }
    public void ResetLevel(float delay){
        Invoke(nameof(ResetLevel), delay);
    }

    private void ResetLevel(){
        lives--;
        if(lives > 0) {
            LoadLevel(stage);
        }else{
            GameOver();
        }
    }

    private void GameOver(){
        NewGame();
    }

    private void GetEnemyDeath() {
        currentDefeatObjective = 0;
        foreach (Enemy enemy in enemies)
        {
            if (enemy.GetIsDeadEnemy()) 
            {
                currentDefeatObjective++;
            }
        }
    }

    private void GameNextStage(){
        if(currentDefeatObjective == defeatObjective){
            uIManager.ShowNextStageUI();
            playerGameObject.enabled = false;
        }
    }
    public int GetDefeatObjective() => defeatObjective;
    public int GetCurrentDefeatObjective() => currentDefeatObjective;
}
