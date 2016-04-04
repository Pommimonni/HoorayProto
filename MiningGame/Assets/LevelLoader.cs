using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum definedLevels { menu, game, bonusRound, endGame }

public class LevelLoader : MonoBehaviour {

	// Use this for initialization


    // Update is called once per frame

    void Awake()
    {
        if (Common.levelLoader)
        {
            Destroy(this.gameObject);
        }
        else {
            Common.levelLoader= this;
            //DontDestroyOnLoad(this.gameObject);
        }

    }
    public void LoadLevel(int level)
    {

        SceneManager.LoadScene(level);
    }

    public void LoadMenu()
    {
        Debug.Log("LOAD MENU");
        LoadLevel((int)definedLevels.menu);
    }

    public void LoadGame()
    {
        Debug.Log("Load GAME");
        LoadLevel((int)definedLevels.game);
    }

}
