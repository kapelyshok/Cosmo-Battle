using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy3 : Enemy
{
    [SerializeField] public Slider _slider;
    private Vector3 _offset = new Vector3(0, -170, 0);
    public int shootingSpeed = 4;
    public GameObject bulletPrefab;
    bool isEnabled = true;
    GameObject player;
    public GameObject laser;
    public override void Start()
    {

        base.Start();
        player = GameObject.Find("/Player");
        hp = 3;
        reward = 3;
        damage = 15;
    }
    void Update()
    {
        laser.GetComponent<Transform>().LookAt(player.GetComponent<Transform>());
        Debug.DrawLine(laser.GetComponent<Transform>().position, laser.GetComponent<Transform>().forward * 100f, Color.blue);
        _slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + _offset);
    }
    public override void attack(int damag)
    {
        base.attack(damag);
    }
    public override void death(int reward)
    {

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
        yield return new WaitForSeconds(1);
        for (; ; )
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position + new Vector3(0, 0, -5), laser.GetComponent<Transform>().rotation);
            //bullet.GetComponent<Rigidbody>().AddForce(0f, 0f, 100f, ForceMode.Impulse);
            bullet.GetComponent<Rigidbody>().velocity = bullet.GetComponent<Transform>().forward * 20f;
            yield return new WaitForSeconds(3);
        }
        
            
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Upper Edge" && isEnabled) { isEnabled = false; StartCoroutine(Shooting()); laser.SetActive(true); }
    }
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
