using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        int level = PlayerPrefs.GetInt("level", -1);
        if (level == -1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    public void Continue()
    {
        int level = PlayerPrefs.GetInt("level", 1);
        MoneyManager.money = PlayerPrefs.GetInt("money", 0);
        SceneManager.LoadScene(level);
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("attempts", PlayerPrefs.GetInt("attempts", 0) + 1);
        PlayerPrefs.SetInt("level", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
