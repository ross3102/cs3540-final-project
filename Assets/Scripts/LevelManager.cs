using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject helpTextBox;
    public GameObject upgradesPanel;
    public string nextLevel;
    public float totalCountDownTime = 5;

    float countDownTime;
    MoneyManager money;
    PlaceTower placeTower;
    TextMeshProUGUI helpText;
    bool isPlaceTowerDisabled = false;

    void Start()
    {
        isGameOver = false;
        currentPhase = GamePhase.TowerPlacement;
        money = GameObject.FindGameObjectWithTag("Player").GetComponent<MoneyManager>();
        placeTower = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlaceTower>();
        helpText = helpTextBox.GetComponentInChildren<TextMeshProUGUI>();
        UpgradableTower.ResetTowers();
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
        if (!money.HasAtLeast(placeTower.GetCurrentTowerCost()))
        {
            helpText.text = "Your funds are low! Press E to start the enemy wave";
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            currentPhase = GamePhase.CountDown;
            helpTextBox.SetActive(false);
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
        if (string.IsNullOrEmpty(nextLevel))
        {
            PlayerPrefs.SetInt("wins", PlayerPrefs.GetInt("wins", 0) + 1);
            var finalMoney = money.GetMoneyAmount();
            if (finalMoney > PlayerPrefs.GetInt("highScore", 0))
            {
                PlayerPrefs.SetInt("highScore", finalMoney);
            }
            PlayerPrefs.SetInt("level", -1);
            SceneManager.LoadScene(0);
        }
        else
        {
            MoneyManager.restart = false;
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
            SceneManager.LoadScene(nextLevel);
        }
    }

    void LoadCurrentLevel()
    {
        MoneyManager.restart = true;
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

    public void SetIsPlaceTowerDisabled(bool disabled)
    {
        isPlaceTowerDisabled = disabled;
    }

    public bool IsPlaceTowerDisabled()
    {
        return isPlaceTowerDisabled;
    }
}
