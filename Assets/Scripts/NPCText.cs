using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCText : MonoBehaviour
{

    public Text npcText;

    private string[] helpMessages = {
        "I'm King Quesadilla! Click me if you need any help.",
        "The Kingdom of El Jefes is under attack by Plomites! Kill the enemies before they reach the castle!",
        "Luckily, you are a knight in the kingdom's engineering regiment! Build towers using right click to help defeat the enemy.",
        "I suspect the Plomites are trying to steal our guacamole recipe...",
        "Kill enemies to collect gold and build more towers.",
        "Place towers strategically to maximize damage area.",
        "Awfully nice weather today isn't it. No wonder the Plomites chose to attack today.",
        "Punch enemies if they exit your tower range!",
    };

    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        npcText.text = helpMessages[0];
        currentIndex = 0;

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public void CycleText()
    {
        if (currentIndex >= helpMessages.Length - 1)
        {
            currentIndex = 0;
        } else
        {
            currentIndex++;
        }

        npcText.text = helpMessages[currentIndex];
    }

    public void ShowText(bool isShown)
    {
        npcText.enabled = isShown;
    }
}
