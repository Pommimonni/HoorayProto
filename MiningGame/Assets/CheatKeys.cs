using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CheatKeys : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadScene(0);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(2);
        }
    }


}
