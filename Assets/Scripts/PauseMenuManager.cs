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
    public GameObject gameUI;


    public Text swordDur;
    public Text health;

    private ItemDurability itemDur;
    private HeroController hero;

    // Start is called before the first frame update
    void Start()
    {
        CloseMenus();
        // AdjustTime(1);
        itemDur = FindObjectOfType<ItemDurability>();
        hero = FindObjectOfType<HeroController>();
    }

    void Update()
    {
        swordDur.text = itemDur.getSwordDurability().ToString() + " %";
        // health.text = hero.GetHealth().ToString();
    }


    public void OpenPauseMenu()
    {
        masterPauseGameObject.SetActive(true);
        mainPauseMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
        gameUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);
        // AdjustTime(0);

    }

    public void CloseMenus()
    {
        masterPauseGameObject.SetActive(false);
        mainPauseMenu.SetActive(false);
        howToPlayMenu.SetActive(false);
        endMenu.SetActive(false);
        gameUI.SetActive(true);
    }

    public void ResumeGame()
    {
        CloseMenus();
        // AdjustTime(1);
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
        gameUI.SetActive(false);
        EventSystem.current.SetSelectedGameObject(restartButton);
        // AdjustTime(0);

        
    }

    private void AdjustTime(float scale)
    {
        Time.timeScale = scale;
    }



}
