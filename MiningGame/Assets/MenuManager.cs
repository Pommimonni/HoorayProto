using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
       // SetAllPlayerNameTittle(playerName);
       // ActivateMenu(menuGOer);
        

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GoNextMenu();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            GoBackMenu();
        }
	}
    public string playerName = "player1";
    public int playerNumber = 1;
    public int menuGOer = 0;
    public PlayerInformation myPlayer;
    public bool isAllGems = false;
    public int favouriteGem = 0;
    public float betAmount = 0;
    public float moneyInserted = 0;
    public int chosenFavouriteGemint = 0;
    public float ratio = 0;
    public Gem chosenFavouriteGem;
    public MenuManager otherMenu;
    public ToogleGroupHandler betToggle;

    public void SetIsAllGemsOn(bool value)
    {
        isAllGems = value;
    }
    public void ActivateMenu(int index)
    {

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        this.transform.GetChild(index).gameObject.SetActive(true);
        menuGOer = index;
    }
    public void SetOtherMenu(GameObject menu)
    {
        otherMenu = menu.GetComponentInChildren<MenuManager>();
    }
    public void Initialize(string newPlayer,int newplayerNumber)
    {
       // myPlayer = newPlayer;
        SetAllPlayerNameTittle(newPlayer);
        
        this.playerNumber = newplayerNumber;
        this.GetComponentInChildren<RemoveFromPlayer>().RemoveBasedOnTags(playerNumber);
        ActivateMenu(menuGOer);

    }



    public void SetMoneyInserted(string moneyAmount)
    {
        float money = float.Parse(moneyAmount);
        moneyInserted = money;
        if (playerNumber == 1)
        {
            RoundSettings.moneyInsertedPLayer1 += moneyInserted;
        }
        else
        {
            RoundSettings.moneyInsertedPlayer2 += moneyInserted;
        }

    }

    public void SetBetAmount(string newBetAmount)
    {
        float conv = float.Parse(newBetAmount);
        betAmount = conv;

    }

    public void GoNextMenu()
    {
        menuGOer++;
        if (menuGOer == transform.childCount)
        {
            EndOfMenus();
            return;
        }
        ActivateMenu(menuGOer);
    }
    public void GoBackMenu()
    {
        menuGOer--;
        if (menuGOer == transform.childCount)
        {
            EndOfMenus();
            return;
        }
        ActivateMenu(menuGOer);
    }

    public void GemChooseToggleChanged(int newChild)
    {
        this.chosenFavouriteGemint = newChild;
        this.chosenFavouriteGem = Common.gemSkins.allGems[newChild];
    }


    public void ChangeOtherBetToggle(int toggle)
    {
       // otherMenu.betToggle.SetMemberToggled(toggle);
    }

    public void BetToggleChanged(int newBet)
    {

        this.betAmount = newBet+1;
        ChangeOtherBetToggle(newBet);
        otherMenu.betAmount = newBet + 1;
    }

    public void RatioToggleChanged(int newToggle)
    {
        if (newToggle == 0)
        {
            ratio = 0.25f;
        }
        if (newToggle == 1)
        {
            ratio = 0.5f;
        }
        if (newToggle == 2)
        {
            ratio = 0.75f;
        }
    }

    void EndOfMenus()
    {
        StartGame();
    }

    void StartGame()
    {
        int sceneToLoad = (int)definedLevels.game;
        SetRoundSettings();
        Common.levelLoader.LoadLevel(sceneToLoad);
       // SceneManager.LoadScene(sceneToLoad);
        
    }

    void SetRoundSettings()
    {

        RoundSettings.bet = this.betAmount;
        RoundSettings.ratio = this.ratio;
        RoundSettings.moneyInserted = this.moneyInserted;
    }

    void SetAllPlayerNameTittle(string newTittle)
    {
        string tagToFind = "PlayerNameTittle";
        Transform[] allChilds=this.transform.GetComponentsInChildren<Transform>();
        foreach(Transform child in allChilds)
        {
            if (child.tag == tagToFind)
            {
                Text textToChange=child.GetComponentInChildren<Text>();
                textToChange.text = newTittle;
            }
        }
    }


}
