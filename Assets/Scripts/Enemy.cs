using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Enemy : MonoBehaviour
{
    [Header("Info")]
    protected int hp;
    protected int reward;
    protected int damage;
    [Header("Sounds")]
    public GameObject explosion;
    public GameObject damaged;
    // Start is called before the first frame update
    public virtual void Start()
    {
        explosion = GameObject.Find("/Sounds/EnemyExplosion");
        damaged = GameObject.Find("/Sounds/EnemyDamaged");
        hp = 1;
        reward = 1;
        damage = 1;
    }

    public virtual void death(int reward)
    {
        explosion.GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(0.8f, 1.1f);
        explosion.GetComponent<AudioSource>().Play();
        GameObject score = GameObject.Find("/Canvas/Score");
        int a = Int32.Parse(score.GetComponent<Text>().text);
        a += reward;
        score.GetComponent<Text>().text = a.ToString();
        if (PlayerController.currentShieldLevel != 2&&a >= PlayerController.shieldUpgrade[PlayerController.currentShieldLevel + 1]) { EventSystem.sendEnoughGoldForShield();  }
        return;
    }
    public virtual void attack(int damag)
    {
        EventSystem.sendEventPlayerTookDamage(damag);
        return;
    }
    public virtual void takeDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            death(reward);
            Destroy(gameObject);
        }
        else damaged.GetComponent<AudioSource>().Play();
    }
    public virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody.tag == "Bullet")
        {
            print("bullet");
            takeDamage(PlayerController.getBulletDamage());
        }
        if (collision.rigidbody.tag == "Rocket")
        {
            takeDamage(PlayerController.getHardDamage());
        }
        if ((collision.rigidbody.tag == "Player" || collision.rigidbody.tag == "Bottom Edge"))
        {
            
            //if(PlayerController.shieldsLeft==0)
            {
                attack(damage);
            }
            Destroy(gameObject);
        }
        
    
}
}
