using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TestMousePosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public Vector3 mouseposition;
    public Vector3 mousepositio2n;
    // Update is called once per frame
    void Update () {
        Text text = GetComponentInChildren<Text>();
        float mousex = (Input.mousePosition.x);
        float mousey = (Input.mousePosition.y);
        mouseposition = Common.gameMaster.player1Camera.ScreenToWorldPoint(new Vector3(mousex, mousey, 32));
        mousepositio2n = Common.gameMaster.player2Camera.ScreenToWorldPoint(new Vector3(mousex, mousey, 32));
        text.text = "mx=" + mousex.ToString()+" my"+mousey.ToString();
        text.text +="\n camera1 "+ mouseposition.ToString();
        text.text += "\n camera2 "+mousepositio2n.ToString();
    }
}
