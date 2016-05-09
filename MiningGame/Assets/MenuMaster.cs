using UnityEngine;
using System.Collections;

public class MenuMaster : MonoBehaviour {
    public GameObject playerInfoPrefab;
    public GameObject menuToInstantiate;
    public Vector2 player1Pos;
    public Vector2 player2Pos;
    public string player1TittleName = "Player1";
    public string player2TittleName = "Player2";
    public GameObject menu1;
    public GameObject menu2;
    public FMODUnity.StudioEventEmitter menuMusic;
    void Start()
    {
        menuMusic.Play();
       // PlayerInformation newInfo = Instantiate(playerInfoPrefab).GetComponentInChildren<PlayerInformation>();
       // newInfo.name = "BUBU";
        menu1=CreateMenu(player1TittleName,player1Pos,1,menu1);
        menu2=CreateMenu(player2TittleName, player2Pos,2,menu2);
        menu1.GetComponentInChildren<MenuManager>().SetOtherMenu(menu2);
        menu2.GetComponentInChildren<MenuManager>().SetOtherMenu(menu1);
    }
    
    public GameObject CreateMenu(string tittleName, Vector2 pos, int playerNumber,GameObject menu)
    {
      //  GameObject menu=(GameObject)Instantiate(menuToInstantiate,Vector3.zero,Quaternion.identity);
        menu.GetComponentInChildren<MenuManager>().Initialize(tittleName,playerNumber);
       // menu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
        return menu;
    }
}
