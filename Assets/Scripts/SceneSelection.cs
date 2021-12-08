using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelection : MonoBehaviour
{

    /*
    public void selectScene()
    {
        switch (this.gameObject.name)
        {
            case "Fred_SceneButton":
                SceneManager.LoadScene ("Fred_Scene_Wall_Jump");
                break;
            case "Level1 1Button":
                SceneManager.LoadScene ("Level1 1");
                break;
            case "Level2Button":
                SceneManager.LoadScene ("Level2");
                break;
            case "Level3_HennButton":
                SceneManager.LoadScene ("Level3Henn");
                break;
            case "Level9_NathyButton":
                SceneManager.LoadScene ("Level9_Nathy");
                break;
            case "PushPullLevelButton":
                SceneManager.LoadScene ("PushPullLevel");
                break;
        }
    }
    */

    public void fredScene()
    {
        SceneManager.LoadScene ("Fred_Scene_Wall_Jump");
    }
    public void level1()
    {
        SceneManager.LoadScene ("Level1 1");
    }
    public void level2()
    {
        SceneManager.LoadScene ("Level2");
    }
    public void level3_Henn()
    {
        SceneManager.LoadScene ("Level3_Henn");
    }
    public void level9_Nathy()
    {
        SceneManager.LoadScene ("Level9_Nathy");
    }
    public void pushPullLevel()
    {
        SceneManager.LoadScene ("PushPullLevel");
    }

}
