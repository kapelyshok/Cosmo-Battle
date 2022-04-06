using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldUpgrade : MonoBehaviour
{
    public GameObject shieldUpgradeButton;
    public GameObject shieldUpgradeValue;
    public GameObject[] shieldsIcons = new GameObject[3];
    public GameObject[] shieldsSliders = new GameObject[3];
    public Sprite fullSquare;
    private int lastShieldLevel = 2;
    public void exitGame()
    {
        Application.Quit();
    }
    void Start()
    {
        //print("start");
        EventSystem.enoughGoldForShield.AddListener(buttonAvailable);
    }
    void buttonAvailable()
    {
        //print("button available");
        shieldUpgradeButton.GetComponent<Button>().interactable = true;
    }
    public void upgrade()
    {
        PlayerController.currentShieldLevel++;
        EventSystem.sendShieldWasUpgraded();
        shieldsSliders[PlayerController.currentShieldLevel].SetActive(true);
        shieldsIcons[PlayerController.currentShieldLevel].GetComponent<Image>().sprite = fullSquare;
        GameObject score = GameObject.Find("/Canvas/Score");
        int a = Int32.Parse(score.GetComponent<Text>().text);
        a -= PlayerController.shieldUpgrade[PlayerController.currentShieldLevel];
        score.GetComponent<Text>().text = a.ToString();
        if (PlayerController.currentShieldLevel != lastShieldLevel && a < PlayerController.shieldUpgrade[PlayerController.currentShieldLevel + 1]) shieldUpgradeButton.GetComponent<Button>().interactable = false;
        if (PlayerController.currentShieldLevel != lastShieldLevel)
        shieldUpgradeValue.GetComponent<Text>().text = PlayerController.shieldUpgrade[PlayerController.currentShieldLevel+1].ToString();
        if (PlayerController.currentShieldLevel == lastShieldLevel)
        {
            shieldUpgradeValue.GetComponent<Text>().text = null;
            Destroy(shieldUpgradeButton);
        }
            
    }
}