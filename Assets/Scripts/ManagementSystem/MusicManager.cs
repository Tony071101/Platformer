using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    [SerializeField] private AudioSource gameplayAudio;
    [SerializeField] private AudioSource menuAudio;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void PlayMenuAudio() {
        menuAudio.Play();
    }

    public void StopMenuAudio() {
        menuAudio.Stop();
    }

    public void PlayGameplayAudio() {
        gameplayAudio.Play();
    }

    public void StopGameplayAudio() {
        gameplayAudio.Stop();
    }

    public void PauseGameplayAudio() {
        gameplayAudio.Pause();
    }

    public void UnpauseGameplayAudio() {
        gameplayAudio.UnPause();
    }
}
