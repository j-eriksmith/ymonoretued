using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    const string MAIN_GAME_SCENE = "JLScene";
    public GameObject backToMenuButton;
    public GameObject startButton;
    public GameObject mainMenu;
    public GameObject HowToPlayMenu;


    void Start()
    {
        mainMenu.SetActive(true);
        HowToPlayMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(MAIN_GAME_SCENE);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HowToPlay()
    {
        HowToPlayMenu.SetActive(true);
        mainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(backToMenuButton);
    }

    public void BackToMenu()
    {
        HowToPlayMenu.SetActive(false);
        mainMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton);
    }
}
