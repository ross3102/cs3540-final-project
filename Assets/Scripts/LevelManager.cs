using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public enum GamePhase
    {
        TowerPlacement,
        CountDown,
        EnemyWave
    }

    public static bool isGameOver;
    public static int enemiesRemaining = 0;
    public static GamePhase currentPhase;

    public Text gameText;
    public GameObject helpText;
    public string nextLevel;
    public float totalCountDownTime = 5;

    float countDownTime;

    void Start()
    {
        isGameOver = false;
        currentPhase = GamePhase.TowerPlacement;
    }

    void Update()
    {
        switch (currentPhase)
        {
            case GamePhase.TowerPlacement:
                UpdateTowerPlacement();
                break;
            case GamePhase.CountDown:
                UpdateCountDown();
                break;
        }
    }

    void UpdateTowerPlacement()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPhase = GamePhase.CountDown;
            helpText.SetActive(false);
            countDownTime = totalCountDownTime;
            gameText.text = "";
            gameText.gameObject.SetActive(true);
        }
    }

    void UpdateCountDown()
    {
        countDownTime -= Time.deltaTime;
        if (countDownTime > 0)
        {
            gameText.text = ((int) countDownTime + 1).ToString();
        }
        else
        {
            gameText.text = "GO!";
            Invoke(nameof(HideText), 1);
            currentPhase = GamePhase.EnemyWave;
        }
    }

    void HideText()
    {
        gameText.gameObject.SetActive(false);
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
