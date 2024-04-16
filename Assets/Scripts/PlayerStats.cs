using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    float playtime;
    int attempts;
    int wins;
    int highScore;

    // Start is called before the first frame update
    void Start()
    {
        playtime = PlayerPrefs.GetFloat("playtime", 0);
        attempts = PlayerPrefs.GetInt("attempts", 0);
        wins = PlayerPrefs.GetInt("wins", 0);
        highScore = PlayerPrefs.GetInt("highScore", 0);

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playtime.ToString("hh':'mm':'ss") + "s";
        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = attempts.ToString();
        transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = wins.ToString();
        transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "$" + highScore;
    }
}
