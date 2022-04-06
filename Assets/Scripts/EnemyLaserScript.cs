using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class EnemyLaserScript : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag == "Player")
        {
            EventSystem.sendEventPlayerTookDamage(15);
        }
    }
}
