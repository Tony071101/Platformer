using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int totalChest { get; private set; }
    public int chestOpened { get; private set; }
    private int stage = 0;
    [SerializeField] private GameObject portalPrefab;
    public bool canPauseGame { get; set; }
    public bool gameIsPaused { get; set; } = false;
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
    private void Start() {
        MusicManager.Instance.PlayMenuAudio();
        canPauseGame = false;
    }

    public void StartGame() {
        MusicManager.Instance.StopMenuAudio();
        LoadLevel(stage);
        canPauseGame = true;
    }

    private void LoadLevel(int stage){
        Cursor.visible = false;
        this.stage = stage;
        SceneManager.LoadScene($"Stage-{stage}");
    }

    public void NextLevel(){
        LoadLevel(stage + 1);
    }

    public void ReturnMenu() {
        stage = 0;
        canPauseGame = false;
        gameIsPaused = false;
        SceneManager.LoadScene("MenuScene");
        MusicManager.Instance.StopGameplayAudio();
        MusicManager.Instance.PlayMenuAudio();
    }

    public void Quit() {
        Application.Quit();
    }

    public void UpdateChestOpened(int value)
    {
        chestOpened = value;
    }

    public void SetTotalChest(int value)
    {
        totalChest = value;
    }

    public void CheckChestsOpened()
    {
        if (chestOpened == totalChest)
        {
            SpawnPortal();
        }
    }

    private void SpawnPortal()
    {
        //Need to add sound and effect.
        Instantiate(portalPrefab, portalPrefab.transform.position, Quaternion.identity);
    }
}
