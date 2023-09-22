using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance { get; private set; }
    private Player playerGameObject;
    private UIManager uIManager;
    [SerializeField] private int stage;
    public int lives { get; private set; }

    private static int currentDefeatObjective;
    private int defeatObjective;

    private void Awake() {
        if (_instance != null)
        {
            Destroy(this.gameObject);
        } else{
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy() {
        if(_instance == this){
            _instance = null;
        }        
    }

    private void Start() {
        playerGameObject = FindObjectOfType<Player>();
        lives = playerGameObject.GetPlayerHealth();
        uIManager = FindObjectOfType<UIManager>();
        if (SceneManager.GetActiveScene().name != $"Stage-{stage}") {
            NewGame();
        }
        defeatObjective = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void NewGame(){
        LoadLevel(stage);
    }

    private void LoadLevel(int stage){
        this.stage = stage;
        SceneManager.LoadScene($"Stage-{stage}");
    }

    private void NextLevel(){
        currentDefeatObjective = uIManager.GetEnemyDeathValues();
        if(currentDefeatObjective == defeatObjective){
            LoadLevel(stage + 1);
        }
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

    public int GetDefeatObjective() => defeatObjective;
}
