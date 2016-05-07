using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SetTextToRichesAmount : MonoBehaviour {

    public int player = 1;
    string formatting = "N2";
    // Use this for initialization
    void Start () {
        if(player==1)
          this.GetComponent<Text>().text = "€" + RoundSettings.player1Money.ToString(formatting);
        else
        {
            this.GetComponent<Text>().text = "€" + RoundSettings.player2Money.ToString(formatting);
        }
	}

    void OnEnable()
    {
        if (player == 1)
            this.GetComponent<Text>().text = "€" + RoundSettings.player1Money.ToString(formatting);
        else
        {
            this.GetComponent<Text>().text = "€" + RoundSettings.player2Money.ToString(formatting);
        }
    }

}
