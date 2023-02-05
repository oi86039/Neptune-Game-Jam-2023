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
        currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate() { 
    
    }

    public void PlayGame() {
        SceneManager.LoadScene(1); //Mission Briefing
    }

    public void ReadyToPlay() {
        SceneManager.LoadScene(3);
    }
    public void Loading() {
        SceneManager.LoadScene(2);
    }
    public void QuitToMenu() {
        SceneManager.LoadScene(0);
        }
    public void QuitToDesktop() {
        Application.Quit();
    }

}
