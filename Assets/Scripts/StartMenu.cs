using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    static public int difficultuLevel;

    public void exitGame()
    {
        Application.Quit();
    }
    public void level1()
    {
        difficultuLevel = 1;
        SceneManager.LoadScene(1);
    }
    public void level2()
    {
        difficultuLevel = 2;
        SceneManager.LoadScene(1);
    }
    public void level3()
    {
        difficultuLevel = 3;
        SceneManager.LoadScene(1);
    }
    public void levelJul()
    {
        difficultuLevel =0;
        SceneManager.LoadScene(1);
    }
    static public int getDifficulty()
    {
        return difficultuLevel;
    }
}
