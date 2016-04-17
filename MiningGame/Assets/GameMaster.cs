using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameMaster : MonoBehaviour {
    public List<Gem> allGems;
    public GameObject gemPrefab;
    public bool playerHandlingGem=false;
    public bool onBonusRound = false;
    public int indexOfGemWeEnteredBonusRound=0;
    public bool forceFirstMember = false;
    bool gameEnded = false;

    public Transform bonusRowShowMenu;

    public GameObject shallWePlayAgainGO;
    public List<Gem> gemsWonInBonusRound;
    public PlayerInformation player1;
    public PlayerInformation player2;

    public List<Gem> combinedWonGems;

    public bool allowHittingAlways = false;

    
    // Use this for initialization
    void Awake()
    {
        Common.gameMaster = this;
        allGems = GetComponentInChildren<Gems>().allGems;
        combinedWonGems = new List<Gem>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            forceFirstMember = !forceFirstMember;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            EnterBonusRound();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            allowHittingAlways = !allowHittingAlways;
        }
    }

    public void WallOpened(Vector3 atPosition,PlayerInformation player)
    {
        Debug.Log("WallOpened");
       // PlayerInformation player = getPlayer(atPosition);  //On multiplayer on different positions different players
        player.PlayerHits();
        
        gemHandling(atPosition,player);
    }
    public void RefreshTotalGemShow()
    {
        player1.myInformationGUI.SetGemShow(combinedWonGems);
        player2.myInformationGUI.SetGemShow(combinedWonGems);
    }

    public void ShowMoneyWonAmountBoth(float amount)
    {
        player1.middleShowWinGUI.SetWonMoney(amount);
        player2.middleShowWinGUI.SetWonMoney(amount);
    }
    public void AddWonMoneyBoth(float amount)
    {
        player1.WinMoney(amount);
        player2.WinMoney(amount);
    }

    public void PlayerGemHandlingFinish(PlayerInformation player,bool enterBonusRound, Gem gainedWhenEnter=null)
    {
        playerHandlingGem = false;
        if (enterBonusRound)
        {
            int indexFound = 0;
            int counter = 0;
            foreach(Gem gem in allGems)
            {
                if (gem.Name == gainedWhenEnter.Name)
                {
                    indexFound = counter;
                    break;
                }
                counter++;
            }
            indexOfGemWeEnteredBonusRound = indexFound;
            if (!onBonusRound)
            {
                EnterBonusRound();
            }
        }
        CheckGameEnd();
    }

    void CheckGameEnd()
    {
        if (player1.GamesOver() && player2.GamesOver() && !onBonusRound)
        {
            EndGame();
        }
    }

    public bool canHitWall()
    {
        if ((playerHandlingGem || onBonusRound || gameEnded) && !allowHittingAlways)
        {
            return false; //Change back to false
        }
        else
        {
            return true;
        }
    }
    void EnterBonusRound()
    {
        onBonusRound = true;
        //Common.usefulFunctions.ShowChildForxSeconds(bonusRowShowMenu, 5f);
        StartCoroutine(CreateGemMovementAllTypeOfGemToBothPlayers(Common.gemSkins.getGemSkin(indexOfGemWeEnteredBonusRound), "BonusRoundSpecial"));
       // Invoke("StartBonusRound", 5f);
    }
    void StartBonusRound()
    {
        Common.bonusBombSummoner.StartBonusRound();
    }

    public void EndBonusRound()
    {
        onBonusRound = false;
        CheckGameEnd();
    }

    void EndGame()
    {
        player1.AddWinsToTotalMoney();
        player2.AddWinsToTotalMoney();
        Debug.Log("GAME ENDS");
        gameEnded = true;
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
        Gem toWin = null;
        if (forceFirstMember)
        {
            toWin = allGems[0];
        }
        else {
            toWin = WhatGemDoWeWin();
        }
        // Debug.Log("We won " + toWin.Name.ToString());
        CreateGemAndPlayerHandle(toWin, atPosition, player);
    }

    void CreateGemAndPlayerHandle(Gem toWin,Vector3 atPosition, PlayerInformation player)
    {
        if (toWin != null)
        {
            GameObject createdGem = CreateGem(toWin, atPosition);
            GameObject createdEffect = CreateEffect(toWin.effectWhenGot, atPosition);
            SetObjectToParent(createdGem, createdEffect);

            player.HandleGem(toWin, createdGem);
            //TODO maybe  HandleText(toWin.textDisplyed);
        }
        else
        {
            player.HandleEmptyGem();
            PlayerGemHandlingFinish(player, false);
            //No Win at all
        }
        combinedWonGems.Add(toWin);
    }

    public  void HandleBonusRoundWin(bool isBigOne,Vector3 position)
    {
        Gem toWin = Common.bonusRoundChances.DetermineWin(this.indexOfGemWeEnteredBonusRound);
        if (isBigOne)
        {
            //Win 3 gems?
        }
        gemsWonInBonusRound.Add(toWin);
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
    public GameObject CreateGem(Gem gem,Vector3 position)
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


    IEnumerator SpawnGemTypeAndMoveItToMiddle(PlayerInformation playerWhoseGemsMove, PlayerInformation playerToScreenMove, Gem whatTypeOfGem)
    {
        //  return;
        int counter = 0;
        List<Gem> gemsToMove = playerWhoseGemsMove.wonGems;
        foreach (Gem gem in gemsToMove)
        {
            if (Common.gemSkins.IsGemEmpty(gem))
            {
                //newWonGems.Add(null);
            }
            else if (gem.Name == whatTypeOfGem.Name)
            {
                int amountAlreadyInGems = playerToScreenMove.allGemsTomiddleCreatedGems.Count;
                Debug.Log("moving gem gems in middle is " + amountAlreadyInGems);
                Vector3 endPos=playerToScreenMove.myInformationGUI.GetWorldPositionOfMiddleGemLocation(amountAlreadyInGems);
                Vector3 startPos = playerWhoseGemsMove.myInformationGUI.GetWorldPositionOfGemInfo(counter);
                playerToScreenMove.myInformationGUI.GemShowDisableGem(counter);
                GameObject createdGem=CreateGemAndMoveToLocation(gem, startPos, endPos, 0.75f);
                playerToScreenMove.startingPositionsOfGemMoveMiddle.Add(startPos);
                playerToScreenMove.allGemsTomiddleCreatedGems.Add(createdGem);
                yield return new WaitForSeconds(0.75f);
                
            }

            counter++;
        }
        yield break;
        //return newWonGems;
    }

    IEnumerator CreateGemMovementAllTypeOfGemToBothPlayers(Gem typeOfGem, string functionToRunAfter)
    {
        player1.allGemsTomiddleCreatedGems = new List<GameObject>();
        player2.allGemsTomiddleCreatedGems = new List<GameObject>();
        player2.startingPositionsOfGemMoveMiddle = new List<Vector3>();
        player1.startingPositionsOfGemMoveMiddle = new List<Vector3>();
        StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player1, player2, typeOfGem));
        yield return StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player1, player1, typeOfGem));
        StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player2, player1, typeOfGem));
        yield return StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player2, player2, typeOfGem));
        yield return new WaitForSeconds(0.5f);
        Invoke(functionToRunAfter, 0f);
        
        // yield return
    }

    void BonusRoundSpecial()
    {
        StartCoroutine(CreateBonusMovements());
    }

    IEnumerator CreateBonusMovements()
    {
        StartCoroutine(CombineGameObjects(player1.allGemsTomiddleCreatedGems, 0.5f));
        yield return StartCoroutine(CombineGameObjects(player2.allGemsTomiddleCreatedGems, 0.5f));
        StartCoroutine(MoveGemsBack(player1, 0.5f));
        StartCoroutine(MoveGemsBack(player2, 0.5f));
        Common.usefulFunctions.ShowChildForxSeconds(bonusRowShowMenu, 3);

    }

    IEnumerator MoveGemsBack(PlayerInformation toPlayer,float oneMoveDuration)
    {
        List<GameObject> toMove = toPlayer.allGemsTomiddleCreatedGems;
        float fixedZToMove = Common.effects.fixedZToMove;
        int counter = 0;
        foreach (GameObject gem in toMove)
        {
           
            Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(gem.transform,toPlayer.startingPositionsOfGemMoveMiddle[counter],oneMoveDuration, fixedZToMove);
            counter++;
            yield return new WaitForSeconds(oneMoveDuration);

        }
        foreach(GameObject gem in toMove)
        {
            Destroy(gem);
        }
        toPlayer.myInformationGUI.SetGemShow(toPlayer.wonGems);
    }

    IEnumerator CombineGameObjects(List<GameObject> toCombine,float oneMoveDuration)
    {
        List<GameObject> ordered = Common.usefulFunctions.OrderByTransformxPosition(toCombine); // Common.usefulFunctions.GetMeanPositionFromGameObjects(toCombine);
        GameObject middleObject = ordered[1];
        Vector3 startScale = middleObject.transform.localScale;
        float fixedZToMove = Common.effects.fixedZToMove;
        Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(ordered[0].transform, middleObject.transform.position, oneMoveDuration, fixedZToMove+0.3f);
        yield return new WaitForSeconds(oneMoveDuration);
        Common.effects.PlayEffect(EffectsEnum.Gem_movement_finishing_combo, middleObject.transform.position);
        Common.usefulFunctions.scaleGOOverTime(middleObject,middleObject.transform.localScale*2f,oneMoveDuration);
        Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(ordered[2].transform, middleObject.transform.position, oneMoveDuration, fixedZToMove+0.3f);
        yield return new WaitForSeconds(oneMoveDuration);
     //   ordered[2].SetActive(false);
        Common.usefulFunctions.scaleGOOverTime(middleObject, middleObject.transform.localScale * 2f, oneMoveDuration);
        yield return new WaitForSeconds(oneMoveDuration);
       // ordered[0].SetActive(false);
        yield return new WaitForSeconds(1f);
        Debug.Log("settings scale back to " + startScale);
        middleObject.transform.localScale = startScale;
        yield break;
        


    }


    GameObject CreateGemAndMoveToLocation(Gem toCreate, Vector3 startLocation, Vector3 endLocation, float oneMoveDuration)
    {
        GameObject createdGem = Common.gameMaster.CreateGem(toCreate, startLocation);
        createdGem.GetComponent<Rigidbody2D>().gravityScale = 0;
        float fixedZToMove = Common.effects.fixedZToMove;
        Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(createdGem.transform, endLocation, oneMoveDuration, fixedZToMove);
        return createdGem;
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
