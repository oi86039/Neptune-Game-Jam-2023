using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenemanage : MonoBehaviour
{

    Scene currentScene;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() { 
    
    }

    public void PlayGame() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1); //Mission Briefing
    }

    public void ReadyToPlay() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }
    public void Loading() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    public void QuitToMenu() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    public void QuitToDesktop() {
        Application.Quit();
    }

}
