using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class StartMenuGUI : MonoBehaviour {

    public PlayerInformation myPlayerInfo;
    public Text tittleText;
    //public Text 
    void Start()
    {
        tittleText.text = myPlayerInfo.playerInfoName;
    }
    public void SetPlayerInfo(PlayerInformation playerInfo)
    {
        this.myPlayerInfo = playerInfo;
        tittleText.text = playerInfo.name;
    }
}
