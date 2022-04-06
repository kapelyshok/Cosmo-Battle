using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy
{
    // Start is called before the first frame update
     public override void Start()
    {
        base.Start();
        hp = 1;
        reward = 1;
        damage = 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void attack(int damag)
    {
        base.attack(damag);
    }
    public override void death(int reward)
    {
        //print("die, "+"reward:"+reward);
        base.death(reward);
    }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
    }
}
