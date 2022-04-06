using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2 : Enemy
{
    [SerializeField] public Slider _slider;
    private Vector3 _offset = new Vector3(0, -170, 0);
    public int shootingSpeed = 4;
    public GameObject bulletPrefab;
    bool isEnabled = true;
    public override void Start()
    {
        base.Start();
        hp = 3;
        reward = 2;
        damage = 10;
    }
    void Update()
    {
        _slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offset);
    }
    public override void attack(int damag)
    {
        base.attack(damag);
    }
    public override void death(int reward)
    {
        //print("die, " + "reward:" + reward);
        base.death(reward);
    }
    public override void takeDamage(int damage)
    {
        base.takeDamage(damage);
        _slider.value = hp;
        _slider.gameObject.SetActive(true);
    }
    IEnumerator Shooting()
    {
        for (; ; )
        {

            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, -5), transform.rotation);
            //bullet.GetComponent<Rigidbody>().AddForce(0f, 0f, 100f, ForceMode.Impulse);
            bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 15f;
            yield return new WaitForSeconds(4);
        }
    }
    private void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Upper Edge" && isEnabled) { isEnabled = false; StartCoroutine(Shooting()); }
    }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
