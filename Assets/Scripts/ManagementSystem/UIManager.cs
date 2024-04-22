using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject damageTextPref;
    [SerializeField] private GameObject healthTextPref;
    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameFinishPanel;
    [SerializeField] private GameObject playerUI;
    private Canvas gameCanvas;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        PauseGameUI();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += SceneLoaded;
        CharacterEvents.characterDamaged += CharacterTookDmg;
        CharacterEvents.characterHealed += CharacterHealed;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= SceneLoaded;
        CharacterEvents.characterDamaged -= CharacterTookDmg;
        CharacterEvents.characterHealed -= CharacterHealed;
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode) {
        gameCanvas = FindObjectOfType<Canvas>();
    }

    public void CharacterTookDmg(GameObject character, int dmgReceived) {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(damageTextPref, spawnPos, Quaternion.identity, gameCanvas.transform)
        .GetComponent<TMP_Text>();

        tmpText.text = dmgReceived.ToString();
    }

    public void CharacterHealed(GameObject character, int healthRestored) {
        Vector3 spawnPos = Camera.main.WorldToScreenPoint(character.transform.position);

        TMP_Text tmpText = Instantiate(healthTextPref, spawnPos, Quaternion.identity, gameCanvas.transform)
        .GetComponent<TMP_Text>();

        tmpText.text = healthRestored.ToString();
    }

    public void ButtonStartGame() {
        GameManager.Instance.StartGame();
        gameOverPanel.SetActive(false);
        gameFinishPanel.SetActive(false);
        startGameUI.SetActive(false);
    }

    public void ButtonReturnMenu() {
        Time.timeScale = 1f;
        GameManager.Instance.ReturnMenu();
        startGameUI.SetActive(true);
        pauseMenu.SetActive(false);
        gameOverPanel.SetActive(false);
        gameFinishPanel.SetActive(false);
    }

    public void ButtonQuitGame() {
        GameManager.Instance.Quit();
    }

    public void PauseGameUI() {
        //Lower music here too.
        if (GameManager.Instance.canPauseGame) {
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(GameManager.Instance.gameIsPaused){
                    GameResume();
                }
                else{
                    GamePause();
                }
            }
        }
    }
    
    public void GameResume(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.gameIsPaused = false;
        MusicManager.Instance.UnpauseGameplayAudio();
    }

    public void GamePause(){
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameManager.Instance.gameIsPaused = true;
        MusicManager.Instance.PauseGameplayAudio();
    }

    public void ActivateGameOverPanel() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.canPauseGame = false;
        gameOverPanel.SetActive(true);
    }

    public void ActivateGameFinishPanel() {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance.canPauseGame = false;
        gameFinishPanel.SetActive(true);
    }

    public void ActivatePlayerUI() {
        playerUI.SetActive(true);
    }
}
