using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterSpawner : MonoBehaviour
{
    public GameObject[] spawnersList =new GameObject[2];
    private int timeToSpawn;
    public GameObject[] boosters = new GameObject[3]; 
    IEnumerator spawnBoosterTimer()
    {
        for(; ; )
        {
            timeToSpawn = UnityEngine.Random.Range(15, 25);
            yield return new WaitForSeconds(timeToSpawn);
            int spawnerNum = UnityEngine.Random.Range(0, 2);
            GameObject booster =Instantiate(boosters[UnityEngine.Random.Range(0, 1)], spawnersList[spawnerNum].transform.position, spawnersList[spawnerNum].transform.rotation);
            booster.GetComponent<Rigidbody>().velocity = -transform.forward * 10f;
        }
    }
    void Start()
    {
        StartCoroutine(spawnBoosterTimer());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
