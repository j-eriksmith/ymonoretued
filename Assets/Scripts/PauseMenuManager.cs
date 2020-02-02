using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    const string MAIN_MENU_SCENE = "MainMenu";

    public GameObject mainPauseMenu;
    public GameObject howToPlayMenu;
    public GameObject resumeButton;
    public GameObject backButton;
    public GameObject masterPauseGameObject;
    public GameObject restartButton;
    public GameObject endMenu;


    public Text swordDur;
    public Text shieldDur;

    // Start is called before the first frame update
    void Start()
    {
        CloseMenus();
    }

    void Update()
    {
        // swordDur.text = 
        // shieldDur.text = 
    }


    public void OpenPauseMenu()
    {
        masterPauseGameObject.SetActive(true);
        mainPauseMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        // time stuff here?

    }

    public void CloseMenus()
    {
        masterPauseGameObject.SetActive(false);
        mainPauseMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    public void ResumeGame()
    {
        CloseMenus();
        // time stuff here?
    }

    public void OpenHowToPlay()
    {
        mainPauseMenu.SetActive(false);
        howToPlayMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(backButton);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EndGame()
    {
        endMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(restartButton);
        // time stuff here?
    }



}
