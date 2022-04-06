using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject center;
    bool flag = true, cor = false;
    public GameObject wall;
    public PostProcessProfile postProc;

    void Start()
    {
        wall.SetActive(false);
    }
    IEnumerator restart()
    {
        print("start");
        yield return new WaitForSeconds(2f);
        Camera.main.GetComponent<PostProcessVolume>().profile = postProc;
        for (int i = 0; i < 400; i++)
        {
            Camera.main.GetComponent<PostProcessVolume>().profile.GetSetting<Vignette>().intensity.value += 0.025f;
            yield return new WaitForSeconds(0.0125f);
        }
        SceneManager.LoadScene(0);
    }
    private void Update()
    {
        if (GetComponent<Rigidbody>().velocity != Vector3.zero && flag)
            transform.position = Vector3.Lerp(transform.position, center.transform.position, Time.deltaTime*1f);else
        {
            GetComponent<Rigidbody>().velocity = GetComponent<Transform>().forward * 20f;
            if (!cor)
            {
                cor = true;
                postProc.GetSetting<Vignette>().intensity.value = 0;
                StartCoroutine(restart());
                flag = false;
            }
            
        }
         
        //print(GetComponent<Rigidbody>().velocity);
    }
    // Update is called once per frame
    
}
