using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //std::vector<int> vec;
    public AudioSource bossSignal;
    List<int> spawnList= new List<int> { 0,1,2};
    public GameObject[] spawners = new GameObject[3];
    float speedForEnemy1 = 8f, speedForEnemy2 = 5f;
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject bossPrefab;
    public Transform tr;
    public Rigidbody rb;
    int difficulty;
    float timeToRespawn = 2f;
    int wavesCnt = 0;
    void Start()
    {
        tr=GetComponent<Transform>();
        rb=GetComponent<Rigidbody>();
        StartCoroutine(spawner());
        difficulty = StartMenu.getDifficulty();
        switch (difficulty)
        {
            case 0:
                timeToRespawn = 5f;
                speedForEnemy1 = 4f;
                speedForEnemy2 = 3f;
                break;
            case 1:
                timeToRespawn = 4f;
                speedForEnemy1 = 5f;
                speedForEnemy2 = 3f;
                break;
            case 2:
                timeToRespawn = 3f;
                speedForEnemy1 = 8f;
                speedForEnemy2 = 5f;
                break;
            case 3:
                timeToRespawn = 2f;
                speedForEnemy1 = 8f;
                speedForEnemy2 = 5f;
                break;
        }
    }
    IEnumerator spawner()
    {
        for(;;)
        {
            
            spawnList = new List<int> { 0, 1, 2 };
            yield return new WaitForSeconds(timeToRespawn);
            wavesCnt++;
            int numberOfEnemy=Random.Range(0,spawners.Length);
            for(int i=0;i<=numberOfEnemy;i++)
            {
                float randomDistance= Random.Range(0, 6)/10f;
                yield return new WaitForSeconds(randomDistance);
                int index=Random.Range(0, spawnList.Count);
                int typeOfEnemy= Random.Range(0, 3);
                
                GameObject enemy=null;
                switch (typeOfEnemy)
                {
                    case 0:
                        enemy = Instantiate(enemyPrefab1, spawners[spawnList[index]].transform.position, spawners[spawnList[index]].transform.rotation);
                        enemy.GetComponent<Rigidbody>().velocity = -transform.forward * speedForEnemy1;
                        break;
                    case 1:
                        enemy = Instantiate(enemyPrefab2, spawners[spawnList[index]].transform.position, spawners[spawnList[index]].transform.rotation);
                        enemy.GetComponent<Rigidbody>().velocity = -transform.forward * speedForEnemy2;
                        break;
                    case 2:
                        enemy = Instantiate(enemyPrefab3, spawners[spawnList[index]].transform.position, spawners[spawnList[index]].transform.rotation);
                        enemy.GetComponent<Rigidbody>().velocity = -transform.forward * speedForEnemy2;
                        break;
                }
                
                spawnList.RemoveAt(index);
                
            }
            if (wavesCnt == 30) break;
        }
        yield return new WaitForSeconds(2f);
        bossSignal.Play();
        yield return new WaitForSeconds(3f);
        GameObject boss = null;
        boss = Instantiate(bossPrefab, spawners[1].transform.position, spawners[1].transform.rotation);
        boss.GetComponent<Rigidbody>().velocity = -transform.forward * 3f;

    }
    
}
