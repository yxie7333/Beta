using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonScript : MonoBehaviour
{
    public GameObject OptionPanel, PauseButton, WinPanel, HelpPanel;

    public void setOptionPanelOn()
    {
        OptionPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void setOptionPanelOff()
    {
        OptionPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void setHelpPanelOn()
    {
        HelpPanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    public void setHelpPanelOff()
    {
        HelpPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void GoToLevelSelection()
    {
        SceneManager.LoadScene("LevelSelect");
        Time.timeScale = 1.0f;
        initializeStatus();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1.0f;
        initializeStatus();
    }

    public void GoToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
        Time.timeScale = 1.0f;
        initializeStatus();
    }
    public void GoToLevel1()
    {
        SceneManager.LoadScene("Level1");
        Time.timeScale = 1.0f;
        initializeStatus();
    }
    public void GoToLevel2()
    {
        SceneManager.LoadScene("Level2");
        Time.timeScale = 1.0f;
        initializeStatus();
    }


    public void RePlay()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1.0f;
        initializeStatus();
    }

    public void initializeStatus()
    {
    }

    public void setWinPanelOff()
    {
        WinPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }
}