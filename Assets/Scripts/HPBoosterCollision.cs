using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBoosterCollision : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        if (collision.gameObject.tag == "Player")
        {

            GameObject health = GameObject.Find("/Canvas/Health");
            GameObject bar = GameObject.Find("/Canvas/HP");
            int a = Int32.Parse(health.GetComponent<Text>().text);
            if(StartMenu.getDifficulty() == 0)
            {
                if (a > 4980) a = 5000; else a += 20;
            }else 
            if (a > 80) a = 100;else
            a += 20;

            health.GetComponent<Text>().text = a.ToString();
            bar.GetComponent<Slider>().value = a;
        }
        
            
    }
}
