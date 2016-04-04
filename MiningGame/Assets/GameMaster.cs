using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameMaster : MonoBehaviour {
    public List<Gem> allGems;
    public GameObject gemPrefab;
    public bool playerHandlingGem=false;
    public bool onBonusRound = false;

    public GameObject shallWePlayAgainGO;
    // Use this for initialization
    void Awake()
    {
        Common.gameMaster = this;
        allGems = GetComponentInChildren<Gems>().allGems;
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void WallOpened(Vector3 atPosition)
    {
        Debug.Log("WallOpened");
        PlayerInformation player = getPlayer(atPosition);  //On multiplayer on different positions different players
        player.PlayerHits();
        gemHandling(atPosition,player);
    }

    public void PlayerGemHandlingFinish(PlayerInformation player,bool enterBonusRound)
    {
        playerHandlingGem = false;
        if (enterBonusRound)
        {
            EnterBonusRound();
        }
        if (player.GamesOver())
        {
            EndGame();
        }
    }
    public bool canHitWall()
    {
        if (playerHandlingGem || onBonusRound)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    void EnterBonusRound()
    {
        //onBonusRound = true;
    }

    void EndGame()
    {
        Debug.Log("GAME ENDS");
        shallWePlayAgainGO.SetActive(true);
    }
    public void PlayerGemHandlingStart()
    {
        playerHandlingGem = true;
    }

    PlayerInformation getPlayer(Vector3 position)
    {
        return Common.playerInfo;
    }

    void gemHandling(Vector3 atPosition,PlayerInformation player)
    {
        PlayerGemHandlingStart();
        Gem toWin = WhatGemDoWeWin();
       // Debug.Log("We won " + toWin.Name.ToString());
        if (toWin != null)
        {
            GameObject createdGem=CreateGem(toWin, atPosition);
            GameObject createdEffect=CreateEffect(toWin.effectWhenGot, atPosition);
            SetObjectToParent(createdGem, createdEffect);
            
            player.HandleGem(toWin,createdGem);
          //TODO maybe  HandleText(toWin.textDisplyed);
        }
        else
        {
            PlayerGemHandlingFinish(player,false);
        //No Win at all
        }
    }

    void SetObjectToParent(GameObject parentGO,GameObject child)
    {
        child.transform.SetParent(parentGO.transform);
        child.transform.localPosition = new Vector3(0f, 0f, -2f);
    }
    void HandleExtraBonuses()
    {

    }
    void HandlePrice(float amount)
    {
        Common.playerInfo.WinMoney(amount);
    }
    public Gem wonned;
    GameObject CreateGem(Gem gem,Vector3 position)
    {
        wonned = gem;
        Debug.Log("Creating game named " + gem.Name);
        GameObject created = (GameObject)Instantiate(gemPrefab, position, Quaternion.identity);
        created.GetComponentInChildren<SpriteRenderer>().sprite = gem.gemSprite;
        return created;
    }

    GameObject CreateEffect(GameObject effect, Vector3 position)
    {
        GameObject created = (GameObject)Instantiate(effect, position, Quaternion.identity);
        return created;
    }


    Gem WhatGemDoWeWin()
    {
        float random=Random.Range(0f, 1f);
        float changeGoer = 0f;
        Debug.Log("Determining what we won random value " + random.ToString());
       foreach( Gem gem in allGems)
        {
            changeGoer = gem.chanceToGet + changeGoer;
            Debug.Log("random is " + random.ToString() + " changeGoer " + changeGoer.ToString());
            if (random < changeGoer)
            {
                Debug.Log("Won " + gem.Name);
                return gem;
            }
        }
        return null;
    }
}
