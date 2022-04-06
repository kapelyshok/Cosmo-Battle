using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeWeaponController : MonoBehaviour
{
    public GameObject rocketLauncher,canon;
    GameObject rocketsLeftImage;
    private void Start()
    {
        rocketsLeftImage = GameObject.Find("/Canvas/RocketsLeft");
    }
    public void chooseLauncher()
    {

        rocketsLeftImage.SetActive(true);
        rocketLauncher.SetActive(true);
        canon.SetActive(false);
        GameObject img = GameObject.Find("/Canvas/Pause_Menu/RocketLauncherButton/Image");
        img.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        GameObject imgCanon = GameObject.Find("/Canvas/Pause_Menu/CanonButton/Image");
        imgCanon.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        PlayerController.turret = GameObject.Find("/Player/Rocket Launcher");
        PlayerController.hardAmmo = GameObject.Find("Rocket");
    }
    public void chooseCanon()
    {
        
        rocketsLeftImage.SetActive(false);
        rocketLauncher.SetActive(false);
        canon.SetActive(true);
        GameObject img = GameObject.Find("/Canvas/Pause_Menu/CanonButton/Image");
        img.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        GameObject imgRocket = GameObject.Find("/Canvas/Pause_Menu/RocketLauncherButton/Image");
        imgRocket.GetComponent<Image>().color = new Color(255, 255, 255, 1f);
        PlayerController.turret = GameObject.Find("/Player/Turret center");
        PlayerController.hardAmmo = GameObject.Find("Canon bullet");
    }
}
