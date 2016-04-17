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
    void Start()
    {
       // PlayerInformation newInfo = Instantiate(playerInfoPrefab).GetComponentInChildren<PlayerInformation>();
       // newInfo.name = "BUBU";
        menu1=CreateMenu(player1TittleName,player1Pos);
        menu2=CreateMenu(player2TittleName, player2Pos);
        menu1.GetComponentInChildren<MenuManager>().SetOtherMenu(menu2);
        menu2.GetComponentInChildren<MenuManager>().SetOtherMenu(menu1);
        menu1.GetComponentInChildren<MenuManager>().playerNumber = 1;
        menu2.GetComponentInChildren<MenuManager>().playerNumber = 2;
    }
    
    public GameObject CreateMenu(string tittleName, Vector2 pos)
    {
        GameObject menu=(GameObject)Instantiate(menuToInstantiate,Vector3.zero,Quaternion.identity);
        menu.GetComponentInChildren<MenuManager>().Initialize(tittleName);
        menu.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = pos;
        return menu;
    }
}
