using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Post processing")]
    public GameObject explosionPrefab;
    public PostProcessProfile postProc;
    public PostProcessProfile postProcShield;
    [Header("Shooting")]
    static public float timeToReload=3;
    public GameObject bulletPrefab;
    static public GameObject hardAmmo;
    static public GameObject turret;
    bool RocketReady = true;
    public Sprite rocketImage;
    public Sprite rocketImageOnReload;
    Coroutine shot = null;
    Coroutine cor;
    private int rocketsLeft = 4;
    private Sprite[] sprites;
    public GameObject launcherRocketsLeft;
    private Image rocketsLeftRenderer;
    [Header("Player")]
    private float health;
    Coroutine shieldRestart = null;
    public Material shieldMaterial;
    public GameObject mainShield;
    static public int maxShields = 0;
    static public int shieldsLeft = 0;
    public GameObject[] shields=new GameObject[3];
    static public bool isDead = false;
    public GameObject player;
    private Rigidbody rb;
    public Transform tr;
    public float speed = 15f;
    static private int playerBulletDamage = 1;
    static private int playerHardDamage = 3;
    [Header("HealthBar")]
    GameObject healthBar;
    GameObject healthText;
    public GameObject heartIcon;
    [Header("Pause")]
    public GameObject bottomEdgeForEnemy;
    private bool gameIsPaused = false;
    public GameObject pauseMenu;
    public GameObject resumeButton;
    [Header("UpgradeInfo")]
    static public int[] shieldUpgrade=new int[3] {10,25,50};
    static public int currentShieldLevel=-1;
    [Header("Sounds")]
    public AudioSource shotSound;
    public AudioSource playerExplosionSound;
    public AudioSource rocketSound;
    public AudioSource canonSound;
    public AudioSource playerDamageSound;
    public AudioSource shieldTookDamage;
    public AudioSource shieldRestartSound;
    public AudioSource bgMusic;
    static public int getBulletDamage()
    {
        return playerBulletDamage;
    }
    static public int getHardDamage()
    {
        return playerHardDamage;
    }
    IEnumerator back(int a)
    {
        
        for (; ; )
        {
            
            if (Math.Abs(tr.rotation.z) <= 0.001) yield break;
            tr.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 5*a), Time.deltaTime * 20f);
            yield return null;
        }

    }
    IEnumerator Shooting()
    {
        for(; ; )
        {
           GameObject bullet = Instantiate(bulletPrefab, tr.transform.position + new Vector3(0, 0, 2), tr.rotation);
           bullet.GetComponent<Rigidbody>().velocity = transform.forward * 30f;
            shotSound.GetComponent<AudioSource>().Play();
            yield return new WaitForSeconds(0.2f);
        }
    }
    IEnumerator RocketTimer()
    {
        rocketsLeft = 0;
        rocketsLeftRenderer.sprite = sprites[0];
        timeToReload = 10;
        float time1 = timeToReload;
       
        GameObject timeText = GameObject.Find("/Canvas/RocketTime");
        GameObject image = GameObject.Find("/Canvas/Rocket");
        image.GetComponent<Image>().sprite = rocketImageOnReload;
        for (int i=0;i<10*time1;i++ )
        {
            timeToReload -= 0.1f;
            //Math.Round(timer, 1);
            timeText.GetComponent<Text>().text = timeToReload.ToString("0.0");
            yield return new WaitForSeconds(0.1f);
        }
        image.GetComponent<Image>().sprite = rocketImage;
        rocketsLeftRenderer.sprite = sprites[4];
        rocketsLeft = 4;
        RocketReady = true;
        timeText.GetComponent<Text>().text = "Ready";
    }
    IEnumerator canonTimer()
    {
        
        timeToReload = 1.5f;
        float time1 = timeToReload;
        
        GameObject timeText = GameObject.Find("/Canvas/RocketTime");
        GameObject image = GameObject.Find("/Canvas/Rocket");
        image.GetComponent<Image>().sprite = rocketImageOnReload;
        for (int i = 0; i < 10 * time1; i++)
        {
            timeToReload -= 0.1f;
            //Math.Round(timer, 1);
            timeText.GetComponent<Text>().text = timeToReload.ToString("0.0");
            yield return new WaitForSeconds(0.1f);
        }
        RocketReady = true;
        timeText.GetComponent<Text>().text = "Ready";
    }
    IEnumerator deathCoroutine()
    {
        //print("start coroutine");
        
        for (int i = 0; i <50; i++)
        {
            //print(Time.timeScale);
            Time.timeScale -= 0.01f;
            yield return new WaitForSeconds(0.05f);
        }
        Time.timeScale = 0f;
        GameObject restart = GameObject.Find("/RestartController");
        GameObject.Find("/Canvas/Dead").GetComponent<Text>().text = "You are dead";
        GameObject.Find("/Canvas/Retry").GetComponent<Text>().text = "Press any button to restart";
        restart.GetComponent<RestartGame>().enabled = true;
    }
    void finalFunction()
    {
        StopAllCoroutines();
        GetComponent<Finish>().enabled = true;
        GetComponent<PlayerController>().enabled = false;
    }
    void Start()
    {
        EventSystem.gameIsOver.AddListener(finalFunction);
        bgMusic.Play();
        maxShields = 0;
        shieldsLeft = 0;
        currentShieldLevel = -1;
        EventSystem.shieldWasUpgraded.AddListener(updateShieldInformation);
        EventSystem.playerTookDamage.AddListener(playerTakingDamage);
        shieldsLeft = maxShields;
        
        isDead = false;
        healthBar = GameObject.Find("/Canvas/HP");
        healthText = GameObject.Find("/Canvas/Health");
        rocketsLeftRenderer =launcherRocketsLeft.GetComponent<Image>();
        sprites = Resources.LoadAll<Sprite>("Launcher4");
        hardAmmo = GameObject.Find("/Rocket");
        turret = GameObject.Find("/Player/Rocket Launcher");
        postProc.GetSetting<Vignette>().intensity.value = 0.4f;
        postProcShield.GetSetting<Vignette>().intensity.value = 0.4f;
        rb = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

        if (StartMenu.getDifficulty() == 0)
        {
            healthBar.GetComponent<Slider>().maxValue = 5000;
            healthBar.GetComponent<Slider>().value = 5000;
            healthText.GetComponent<Text>().text = 5000.ToString();
        }
    }
    void death()
    {
        mainShield.transform.SetParent(null);
        Destroy(mainShield);
        var exp = Instantiate(explosionPrefab,player.transform.position,Quaternion.identity);
        Destroy(exp, 1.5f);
        bottomEdgeForEnemy.GetComponent<Collider>().enabled = false;
        healthBar.GetComponent<Slider>().value = 0;
        healthText.GetComponent<Text>().text = 0.ToString();
        print(player.transform.childCount);
        player.gameObject.layer = 13;
        playerExplosionSound.Play();
        for (int i = player.transform.childCount - 1; i >= 0; i--)
        {
            Transform child = player.transform.GetChild(i);
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.tag = "DestroyedPlayer";
            child.gameObject.layer = 13;
            child.gameObject.GetComponent<Rigidbody>().useGravity = false;
            child.gameObject.GetComponent<BoxCollider>().enabled = true;
            child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(5f, Vector3.up, 5f);
            child.SetParent(null);
        }
        //Time.timeScale = 0;
        StartCoroutine(deathCoroutine());
        
    }
    // Update is called once per frame
    private void Update()
    {
        var mousePosition=Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var difference=mousePosition - transform.position;
        float rotY = Mathf.Atan2(difference.x,difference.z)*Mathf.Rad2Deg;
        if (!isDead)
            turret.transform.rotation = Quaternion.Euler(0f, rotY, 0f);


        if(Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RocketTimer());
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            print(Camera.main.ScreenToWorldPoint(transform.position));
            /*isDead = true;
            death();*/
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            
            StartCoroutine(back(-1));
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            
            StartCoroutine(back(1));
        }
        if (Input.GetKeyDown(KeyCode.Space)&&!isDead)
        {
            shot = StartCoroutine(Shooting());
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopCoroutine(shot);
        }
        if (Input.GetMouseButtonDown(0) && rocketsLeft>0 && !gameIsPaused&&RocketReady&&!isDead)
        {
            if (turret.gameObject.name == "Rocket Launcher")
            {
                playerHardDamage = 3;
                rocketsLeft--;
                 rocketsLeftRenderer.sprite = sprites[rocketsLeft];
                 if (rocketsLeft==0)
                 StartCoroutine(RocketTimer());
                 rocketSound.Play();
                 GameObject rocket = Instantiate(hardAmmo, turret.transform.position, turret.transform.rotation);
                 rocket.GetComponent<Rigidbody>().velocity = rocket.transform.TransformDirection(Vector3.forward) * 30f;
            }
            else
            {
                RocketReady = false;
                playerHardDamage = 2;
                StartCoroutine(canonTimer());
                canonSound.Play();
                GameObject rocket = Instantiate(hardAmmo, turret.transform.position, turret.transform.rotation);
                rocket.GetComponent<Rigidbody>().velocity = rocket.transform.TransformDirection(Vector3.forward) * 45f;
            }
                
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused)
            {
                resumeGame();
            }
            else
            {
                pauseGame();
            }
        }
    }
    void FixedUpdate()
    {

        if (Input.GetKey(KeyCode.A)&&!isDead)
        {
            tr.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, 12), Time.deltaTime * 30f);
            rb.AddRelativeForce(-speed, 0f, 0f);
        }
        
        if (Input.GetKey(KeyCode.D) && !isDead)
        {
            tr.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -12), Time.deltaTime * 30f);
            rb.AddRelativeForce(speed, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.W) && !isDead)
        {
            rb.AddRelativeForce(0f, 0f, speed);
        }
        if (Input.GetKey(KeyCode.S) && !isDead)
        {
            rb.AddRelativeForce(0f, 0f, -speed);
        }
        
        
        //print(tr.position.x);
    }
    int number = 0;
    bool coro = false;
    private void playerTakingDamage(int damage)
    {
        if(coro)
        {
            coro = false;
            StopCoroutine("restartShield");
            print(shieldRestart);
            shields[shieldsLeft].GetComponent<Slider>().value = 0;
        }
        if (shieldsLeft == 0)
        {
            if (shieldsLeft < maxShields) { coro = true; StartCoroutine("restartShield"); }
            playerDamageSound.Play();
            health = healthBar.GetComponent<Slider>().value;
            health -= damage;
            healthText.GetComponent<Text>().text = health.ToString();
            healthBar.GetComponent<Slider>().value = health;
            StartCoroutine(playerTakingDamageCoroutine());
            StartCoroutine(heartBlinking());
        }
        else
        shieldTakingDamage();
        postProc.GetSetting<Vignette>().intensity.value = 0.4f;
        postProcShield.GetSetting<Vignette>().intensity.value = 0.4f;
        if (Int32.Parse(healthText.GetComponent<Text>().text) <= 0 && !isDead)
        {
            isDead = true;
            print("dead");
            death();
        }
    }
    IEnumerator playerTakingDamageCoroutine()
    {
        for (int i = 0; i < 6; i++)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.value += 0.04f;
            yield return new WaitForSeconds(0.03f);
        }

        for (int i = 0; i < 6; i++)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.value -= 0.04f;
            //print(postProc.GetSetting<Vignette>().intensity.value);
            yield return new WaitForSeconds(0.03f);
        }
        Camera.main.GetComponent<PostProcessVolume>().profile = postProc;
    }
    IEnumerator heartBlinking()
    {
        float a = 1;
        for (int i = 0; i < 10; i++)
        {
            a -= 0.1f;
            heartIcon.GetComponent<Image>().color = new Color(255f, 0f, 0f, a);
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 10; i++)
        {
            a += 0.1f;
            heartIcon.GetComponent<Image>().color = new Color(255f, 0f, 0f, a);
            yield return new WaitForSeconds(0.01f);
        }
    }
    private void shieldTakingDamage()
    {
        shieldTookDamage.Play();
        Camera.main.GetComponent<PostProcessVolume>().profile = postProcShield;
        StartCoroutine(playerTakingDamageCoroutine());
        print("shield took damage");
        shieldsLeft--;
        shields[shieldsLeft].GetComponent<Slider>().value = 0;
        //shields[shieldsLeft+1].GetComponent<Slider>().value = 0;
        //StopCoroutine(shieldRestart);
        if (!coro) { coro = true; StartCoroutine("restartShield"); }
        if(shieldsLeft==0)
        {
            mainShield.SetActive(false);
        }
        postProc.GetSetting<Vignette>().intensity.value = 0.4f;
        postProcShield.GetSetting<Vignette>().intensity.value = 0.4f;
    }
    IEnumerator restartShield()
    {
        number++;
        yield return new WaitForSeconds(5f);
            for (int i = 0; i < 50; i++)
            {
            print(number);
                shields[shieldsLeft].GetComponent<Slider>().value += 2;
                yield return new WaitForSeconds(0.02f);
            }
            shieldRestartSound.Play();
            mainShield.SetActive(true); 
            shieldsLeft++;

        if (shieldsLeft < maxShields) { coro = true; StartCoroutine("restartShield"); }
        else
        {
            //if (shieldRestart != null)
            {
                coro = false;
                StopCoroutine("restartShield");
            }

        }
            
    }
    public void updateShieldInformation()
    {
        maxShields++;
        if(!coro)
        {
            coro=true;
            StartCoroutine(restartShield());
        }
        
    }
    public void resumeGame()
    {
        resumeButton.gameObject.transform.localScale = new Vector3(4, 4, 4);
        gameIsPaused = false;
        Time.timeScale = 1f;
        pauseMenu.gameObject.SetActive(false);
    }
    void pauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0f;
        pauseMenu.gameObject.SetActive(true);
    }
}
