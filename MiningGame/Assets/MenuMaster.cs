using UnityEngine;
using System.Collections;

public class MenuCreator : MonoBehaviour {
    public GameObject playerInfoPrefab;
    public GameObject menuToInstantiate;
    void Start()
    {
        PlayerInformation newInfo = Instantiate(playerInfoPrefab).GetComponentInChildren<PlayerInformation>();
        newInfo.name = "BUBU";
        CreateMenu(newInfo);
    }
    
    public void CreateMenu(PlayerInformation newPlayer)
    {
        GameObject menu=(GameObject)Instantiate(menuToInstantiate,Vector3.zero,Quaternion.identity);
        menu.GetComponentInChildren<MenuManager>().SetMyPlayer(newPlayer);
    }
}
