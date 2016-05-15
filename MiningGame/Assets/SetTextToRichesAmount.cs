using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class SetTextToRichesAmount : MonoBehaviour {

    public int player = 1;
    // Use this for initialization
    void Start () {
        updateText();

    }

    void OnEnable()
    {

        updateText();
    }

    void updateText()
    {
        if (Common.usefulFunctions)
        {
            if (player == 1)
                this.GetComponent<Text>().text = Common.usefulFunctions.FormatTOtaleAmountTOText(RoundSettings.player1Money,true); //RoundSettings.player1Money.ToString(formatting);
            else
            {
                this.GetComponent<Text>().text = Common.usefulFunctions.FormatTOtaleAmountTOText(RoundSettings.player2Money,true);
            }
        }
        else
        {
            Invoke("updateText", Time.deltaTime);
        }
    }

    }
