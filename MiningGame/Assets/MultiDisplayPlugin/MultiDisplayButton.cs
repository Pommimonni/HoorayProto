using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MultiDisplayButton : MonoBehaviour {

    Button b;
	// Use this for initialization
	void Start () {
        b = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMultiDisplayMouseDown()
    {
        Debug.Log("MultiDisplayButton pressed");
       // PopUp pu = new PopUp(this.gameObject, Vector2.zero, this.gameObject.name, 5f, Vector2.up, 60f);//60f);
        //pu.FontSize = 45;
        b.onClick.Invoke();
        b.transform.position += Vector3.up * 5;
    }
}
