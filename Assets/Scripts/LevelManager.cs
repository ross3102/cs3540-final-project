using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static bool isGameOver;
    public static int enemiesRemaining = 0;

    public Text gameText;
    public string nextLevel;

    void Start()
    {
        isGameOver = false;
    }

    public void LevelLost()
    {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);
        Invoke(nameof(LoadCurrentLevel), 2);
    }

    public void LevelBeat()
    {
        isGameOver = true;
        gameText.text = "YOU WIN!";
        gameText.gameObject.SetActive(true);
        Invoke(nameof(LoadNextLevel), 2);
    }

    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevel))
        {
            SceneManager.LoadScene(nextLevel);
        }
    }

    void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void EnemyDestroyed()
    {
        enemiesRemaining--;
        if (enemiesRemaining <= 0)
        {
            LevelBeat();
        }
    }
}
