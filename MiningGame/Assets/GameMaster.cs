using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameMaster : MonoBehaviour {
    public List<Gem> allGems;
    public GameObject gemPrefab;
    public bool playerHandlingGem=false;
    public bool onBonusRound = false;
    public int indexOfGemWeEnteredBonusRound=0;
    public bool forceFirstMember = false;
    bool startingBonusRound = false;
    bool gameEnded = false;
    bool showingEndGame = false;

    public Transform bonusRowShowMenu;

    public List<Gem> gemsWonInBonusRound;

    public bool dublicatePlayersOnStartUp = true;
    public bool dublicateCameras = true;
    public GameObject cameraToDublicate;
    public float xIncrementToSecondCamera = 40f;
    public PlayerInformation player1;
    public PlayerInformation player2;
    public Camera player1Camera;
    public Camera player2Camera;

    List<float> tempMoneyFromFromMovedGems;
    int endShowMoneyCountCounter = 0;

    public List<Gem> combinedWonGems;
    public List<Gem> combinedWithRemovedBonusRoundGems;
    public List<int> indexesNoLongerValidForBonusRound;
    public bool allowHittingAlways = false;

    public float howlongToShowBonusRoundPopUP = 5f;
    public float howLongToShowResultsOfBonusRound = 5f;

    public List<int> debuggingGemsPLayer1;
    public List<int> debuggingGemsPLayer2;

    bool countingMoney = false;
    
    // Use this for initialization
    void Awake()
    {
        Common.gameMaster = this;
        allGems = GetComponentInChildren<Gems>().allGems;
        combinedWonGems = new List<Gem>();
        if (dublicatePlayersOnStartUp)
        {
           // GameObject created=(GameObject)Instantiate(player1, Vector3.zero, Quaternion.identity);
           player2=(PlayerInformation) Instantiate(player1, Vector3.zero, Quaternion.identity);
            Transform[] allChilds = player2.GetComponentsInChildren<Transform>();
            foreach (Transform child in allChilds)
            {
                if (child.tag == "PlayerUIs")
                {
                    Vector3 pos = child.GetComponent<RectTransform>().position;
                    child.GetComponent<RectTransform>().position = new Vector3(pos.x, pos.y - 5, pos.z);
                    //Destroy(child.gameObject);

                }
            }
            //  GameObject created = (GameObject)Instantiate(player1GO, Vector3.zero, Quaternion.identity);
            //     player2 = created.GetComponentInChildren<PlayerInformation>();
        }
        if (dublicateCameras)
        {
            GameObject createdCamera = (GameObject)Instantiate(cameraToDublicate, Vector3.zero, Quaternion.identity);
            createdCamera.transform.position = new Vector3(cameraToDublicate.transform.position.x+xIncrementToSecondCamera,cameraToDublicate.transform.position.y,cameraToDublicate.transform.position.z);
            player2 = createdCamera.GetComponentInChildren<PlayerInformation>();//(PlayerInformation)Instantiate(player1, Vector3.zero, Quaternion.identity);
            player2.playerNumber = 2;
            createdCamera.GetComponentInChildren<Camera>().targetDisplay = 1;
        }
    }

    public PlayerInformation GetHittingPlayer(Vector3 position)
    {
        if (position.x < Common.mapMIddle.x)
        {
            return Common.gameMaster.player1;
        }
        else
        {
            return Common.gameMaster.player2;
        }
    }
    public PlayerInformation GetHittingPlayer(int screenIndex)
    {
        if (screenIndex == 1 || screenIndex == 0) return Common.gameMaster.player1;
        return Common.gameMaster.player2;
    }
    public Camera GetCameraBasedOnPosition(Vector3 position)
    {
        if (position.x < Common.mapMIddle.x)
        {
            return player1Camera;
        }
        else
        {
            return player2Camera;
        }
        //TODO how to detect correct player

    }

    public bool IsPositionOnPlayer1Screen(Vector3 position)
    {
        PlayerInformation playerScreen = GetHittingPlayer(position);
        if (playerScreen == player1)
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F))
        {
            forceFirstMember = !forceFirstMember;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //BeforeBonusRoundEnterEffects();
            StartBonusRound();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            allowHittingAlways = !allowHittingAlways;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            AddDebuggingGems();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EndGameStarts();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(AddDebuggingDelayd());
        }
    }


    void AddDebuggingGems()
    {
        foreach (int index in debuggingGemsPLayer1) {
            CreateGemAndPlayerHandle(allGems[index], Vector3.zero, player1);
                }
        foreach (int index in debuggingGemsPLayer2)
        {
            CreateGemAndPlayerHandle(allGems[index], Vector3.zero, player2);
        }
    }
    IEnumerator AddDebuggingDelayd()
    {
        foreach (int index in debuggingGemsPLayer1)
        {
            playerHandlingGem = true;
            player1.onGemHandling = true;
            CreateGemAndPlayerHandle(allGems[index], Vector3.zero, player1);
            while (playerHandlingGem)
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
        //yield return new WaitForSeconds(1f);
    }

    public bool WallOpened(Vector3 atPosition,PlayerInformation player)
    {
       // Debug.Log("WallOpened");

       // PlayerInformation player = getPlayer(atPosition);  //On multiplayer on different positions different players
        player.PlayerHits();
        Gem toWin = null;
        if (forceFirstMember)
        {
            toWin = allGems[0];
        }
        else {
            toWin = WhatGemDoWeWin();
        }
        if (toWin == null)
        {
           // PlayerGemHandlingFinish(player, false);
            return false;
        }
        player.onGemHandling = true;
        player.nextGemToWin = toWin;
        return true;
        //gemHandling(atPosition,player);
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
        player.onGemHandling = false;
        if (enterBonusRound)
        {
            int indexFound = 0;
            int counter = 0;
            foreach (Gem gem in allGems)
            {
                if (gem.Name == gainedWhenEnter.Name)
                {
                    indexFound = counter;
                    break;
                }
                counter++;
            }
            indexOfGemWeEnteredBonusRound = indexFound;
            if (!onBonusRound && !startingBonusRound)
            {
                startingBonusRound = true;
                StartCoroutine(BeforeBonusRoundEnterEffects());
            }
        }


        CheckGameEnd();

    }


    void CheckGameEnd()
    {
        if (player1.IsNoHits() && player2.IsNoHits() && !onBonusRound)
        {
            EndGameStarts();
        }
    }

    public void GemRevealOver(Vector3 position, PlayerInformation player)
    {
        CreateGemAndPlayerHandle(player.nextGemToWin, position, player);

        //PlayerInformation player=this.get
    }

    public bool canHitWall(PlayerInformation info)
    {
        if ((info.IsNoHits() || IsGameOnNonNormalState(info)) && !allowHittingAlways)//info.onGemReveal || info.onGemHandling || onBonusRound || gameEnded || startingBonusRound) && !allowHittingAlways)
        {
            return false; //Change back to false
        }
        else
        {
            return true;
        }
    }

    public bool IsGameOnNonNormalState(PlayerInformation info)
    {
        if (info.onGemReveal || info.onGemHandling || onBonusRound || gameEnded || startingBonusRound || showingEndGame)
        {
            return true; //Change back to false
        }
        return false;
    }



    IEnumerator BeforeBonusRoundEnterEffects()
    {
    
        while (player1.onGemHandling || player2.onGemHandling)
        {
            yield return new WaitForFixedUpdate();
        }
        Debug.Log("Starting bonus round");
        
        //Common.usefulFunctions.ShowChildForxSeconds(bonusRowShowMenu, 5f);
        player1.allGemsTomiddleCreatedGems = new List<GameObject>();
        player2.allGemsTomiddleCreatedGems = new List<GameObject>();
        player1.allExtraCreatedGems = new List<GameObject>();
        player2.allExtraCreatedGems = new List<GameObject>();
        player2.startingPositionsOfGemMoveMiddle = new List<Vector3>();
        player1.startingPositionsOfGemMoveMiddle = new List<Vector3>();
        StartCoroutine(CreateGemMovementTypeOfGemToBothPlayers(Common.gemSkins.getGemSkin(indexOfGemWeEnteredBonusRound), "BonusRoundSpecial"));
       // Invoke("StartBonusRound", 5f);
       
    }
    void StartBonusRound()
    {
        startingBonusRound = false;
        onBonusRound = true;
        Common.gameMaster.gemsWonInBonusRound = new List<Gem>();
        Common.bonusBombSummoner.StartBonusRound();
    }

    public Vector3 TransformPlayer1PositionToPlayer2Position(Vector3 p1Pos)
    {
        float xDiff = player2Camera.transform.position.x- player1Camera.transform.position.x;
        Vector3 p2Pos = p1Pos;
        p2Pos.x += xDiff;
        Debug.Log("New POS " + p2Pos + " old pos " + p1Pos);
        return p2Pos;
    }

    public void BonusRoundEndsShowResults()
    {
        player1.myInformationGUI.ShowBBResultsSetActive(true);
        player2.myInformationGUI.ShowBBResultsSetActive(true);
        Common.effects.SpawnEffectOnBothScreens(EffectsEnum.BonusGameEnds, Vector3.zero);//Common.effects.PlayEffect(EffectsEnum.BonusGameEnds, Vector3.zero);
        StartCoroutine(CountMoneyBonusRound(player1));
        StartCoroutine(CountMoneyBonusRound(player2));
        //float total = Common.gemSkins.CalculateMoneyWon(gemsWonInBonusRound);
        //StartCoroutine(player1.myInformationGUI.CountInsertMoney(0, total, player1.myInformationGUI.moneyTotalBBResults, Common.effects.moneyCountParams.maxDuration));
        // StartCoroutine(player2.myInformationGUI.CountInsertMoney(0, total, player2.myInformationGUI.moneyTotalBBResults, Common.effects.moneyCountParams.maxDuration));
        //CheckGameEnd();
    }

    void BonusRoundCompletelyEnds()
    {
        player1.myInformationGUI.ShowBBResultsSetActive(false);
        player2.myInformationGUI.ShowBBResultsSetActive(false);
        onBonusRound = false;
        CheckGameEnd();
    }

    void EndGameStarts()
    {
        showingEndGame = true;
        StartCoroutine(ShowEndResults());


       // shallWePlayAgainGO.SetActive(true);
    }

    void EndGame()
    {
        player1.AddWinsToTotalMoneys();
        player2.AddWinsToTotalMoneys();

        Debug.Log("GAME ENDS");
        showingEndGame = false;
        gameEnded = true;
        player1.myInformationGUI.StopShowEndGameResults();
        player2.myInformationGUI.StopShowEndGameResults();
        player1.myInformationGUI.ShowPlayAgain();
        player2.myInformationGUI.ShowPlayAgain();
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

    public void CreateGemAndPlayerHandle(Gem toWin,Vector3 atPosition, PlayerInformation player)
    {
        if (toWin != null)
        {
            GameObject createdGem = CreateGem(toWin, atPosition);
            GameObject createdEffect = CreateEffect(toWin.effectWhenGot, atPosition);
            SetObjectToParent(createdGem, createdEffect, new Vector3(0f, 0f, -0.5f));

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
        combinedWithRemovedBonusRoundGems.Add(toWin);
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

    void SetObjectToParent(GameObject parentGO,GameObject child,Vector3 newLocalPos)
    {
        child.transform.SetParent(parentGO.transform);
        child.transform.localPosition = newLocalPos;
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
     //   Debug.Log("Creating game named " + gem.Name);
        GameObject created = (GameObject)Instantiate(gemPrefab, position, Quaternion.identity);
        created.GetComponentInChildren<SpriteRenderer>().sprite = gem.gemSprite;

        if (create3DGem)
        {
            GameObject created3D = (GameObject)Instantiate(gem.my3DGem, position, Quaternion.identity);
            Common.usefulFunctions.SetObjectToParent(created.transform, created3D.transform);
          //  created3D.GetComponent<Rigidbody>().AddTorque(Vector3.up * 30);
        }

        return created;
    }

    GameObject CreateEffect(GameObject effect, Vector3 position)
    {
        GameObject created = (GameObject)Instantiate(effect, position, Quaternion.identity);
        return created;
    }
    
    void CountMoneyWon()
    {

    }
    void DestroyCreatedGems(PlayerInformation toPlayer)
    {
        foreach(GameObject createdGO in toPlayer.allGemsTomiddleCreatedGems)
        {
            Destroy(createdGO);
        }
    }
    IEnumerator SpawnGemTypeAndMoveItToMiddle(PlayerInformation playerWhoseGemsMove, PlayerInformation playerToScreenMove, Gem whatTypeOfGem,bool isShowEndScreen=false)
    {
        //  return;
        int counter = 0;
        List<Gem> gemsToMove = playerWhoseGemsMove.wonGems;
        foreach (Gem gem in gemsToMove)
        {
            int amountAlreadyInGems = playerToScreenMove.allGemsTomiddleCreatedGems.Count;
            if (Common.gemSkins.IsGemEmpty(gem))
            {
                //newWonGems.Add(null);
            }
            else if (gem.Name == whatTypeOfGem.Name)
            {
                
                Debug.Log("moving gem gems in middle is " + amountAlreadyInGems);
                Vector3 endPos = Vector3.zero;
                if (isShowEndScreen)
                {
                    endPos = playerToScreenMove.myInformationGUI.GetWorldPositionOfShowEndGemLocation(amountAlreadyInGems);
                }
                else {
                     endPos = playerToScreenMove.myInformationGUI.GetWorldPositionOfMiddleGemLocation(amountAlreadyInGems);
                    endPos.z = -20f;
                }

                if (playerWhoseGemsMove == playerToScreenMove)
                {
                    if (isShowEndScreen)
                    {
                        tempMoneyFromFromMovedGems[endShowMoneyCountCounter] += gem.priceMoney;
                    }
                }
                Vector3 startPos = playerWhoseGemsMove.myInformationGUI.GetWorldPositionOfGemInfo(counter);
                //playerToScreenMove.myInformationGUI.GemShowDisableGem(counter);
                playerWhoseGemsMove.myInformationGUI.GemShowDisableGem(counter);
                GameObject createdGem = null;
                if (playerWhoseGemsMove == playerToScreenMove)
                {
                    createdGem = playerWhoseGemsMove.myInformationGUI.Get3DGem(counter);
                    Common.usefulFunctions.MoveObjectToPlaceNonFixed(createdGem.transform, endPos, 0.75f);
                }
                else
                {
                    createdGem = CreateGemAndMoveToLocation(gem, startPos, endPos, 0.75f);
                    playerWhoseGemsMove.allExtraCreatedGems.Add(createdGem);
                }
                
                
                playerToScreenMove.startingPositionsOfGemMoveMiddle.Add(startPos);
                playerToScreenMove.allGemsTomiddleCreatedGems.Add(createdGem);
                yield return new WaitForSeconds(0.75f);
                if (isShowEndScreen)
                {
                    Vector2 offsets = new Vector2(0.0f, 0.4f);
                    string popUpString = (gem.priceMoney/2).ToString();
                    PopUp pu = new PopUp(playerToScreenMove.myInformationGUI.GetGOOfGemMiddleShowEndScreen(amountAlreadyInGems), offsets, popUpString, 0.75f, Vector2.up, 60f);//60f);
                    pu.FontSize = 45;
                    pu.FillColor = Color.yellow;
                    pu.OutlineColor = Color.black;
                    PopUpManager.Instance.Pop(pu, true);

                }
            }


            counter++;
        }
        yield break;
        //return newWonGems;
    }
    void GemTypeEnds()
    {

    }

    

    IEnumerator ShowEndResults()
    {
        player1.myInformationGUI.StartShowEndGameResults();
        player2.myInformationGUI.StartShowEndGameResults();

        player1.allGemsTomiddleCreatedGems = new List<GameObject>();
        player2.allGemsTomiddleCreatedGems = new List<GameObject>();
        player2.startingPositionsOfGemMoveMiddle = new List<Vector3>();
        player1.startingPositionsOfGemMoveMiddle = new List<Vector3>();
        tempMoneyFromFromMovedGems = new List<float>();
        endShowMoneyCountCounter = 0;
        float moneyGoer = 0f;
      //  StartCoroutine(CountMoneyEndShowCountAllMoney(player1));
     //   StartCoroutine(CountMoneyEndShowCountAllMoney(player2));
        foreach (Gem gemType in allGems)
        {
            tempMoneyFromFromMovedGems.Add(0);
            float lowerMoney = moneyGoer;
            float moneyByType = Common.gemSkins.CalculateMoneyWonByType(combinedWonGems, gemType)/2;
            float upperMOney = moneyGoer + moneyByType;

            yield return StartCoroutine(CreateGemMovementTypeOfGemToBothPlayers(gemType, "GemTypeEnds",true));
            if (moneyByType != 0)
            {
              //  PopUpManager.Instance.Pop(pu, true);
                StartCoroutine(CountMoneyOneType(player1, lowerMoney, upperMOney));
                StartCoroutine(CountMoneyOneType(player2, lowerMoney, upperMOney));
            }
            while (countingMoney)
            {
                yield return new WaitForFixedUpdate();
            }
           // yield return new WaitForSeconds(0.3f);
            // moneyGoer += tempMoneyFromFromMovedGems//tempMoneyFromFromMovedGems[endShowMoneyCountCounter];
            //If wanted to count every win separately player1.myInformationGUI.CountTotalMoneyForEndText(0f,moneyGoer,4f);

            moneyGoer += moneyByType;
            endShowMoneyCountCounter++;
            
        }
        yield return new WaitForSeconds(1f);
        float totalMoney = moneyGoer;  //2;
        player1.WinMoney(totalMoney,1);
        player2.WinMoney(totalMoney,1);
        StartCoroutine(CountMoneyOneType(player1, moneyGoer, 0));
        StartCoroutine(CountMoneyOneType(player2, moneyGoer, 0));
        while (player1.myInformationGUI.AreWeCountingMoney())
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1f);
        DestroyCreatedGems(player1);
        DestroyCreatedGems(player2);
        EndGame();

    }

    IEnumerator CountMoneyEndShowCountAllMoney(PlayerInformation player)
    {
        countingMoney = true;
        float totalMoney = Common.gemSkins.CalculateMoneyWon(combinedWonGems);
        Text toChange = player.myInformationGUI.moneyTotalTextForEndShow;
        yield return StartCoroutine(player.myInformationGUI.CountInsertMoney(0, totalMoney, toChange, Common.effects.moneyCountParams.maxDuration));//CountTotalMoneyForEndText(0f, totalMoney, Common.effects.moneyCountParams.maxDuration);
                                                                                                                                                    //  float countLasts = player.CalculateHowLongtimeMoneyCountLasts(totalMoney);
                                                                                                                                                    // yield return new WaitForSeconds(countLasts);                                                                                                                                               //  Debug.Log("count ends it should have lasted "+countLasts.ToString());
        yield return new WaitForSeconds(1f);
        player.myInformationGUI.CountTotalMoneyForEndText(totalMoney, 0f, Common.effects.moneyCountParams.maxDuration);
        player.WinMoney(totalMoney);
        countingMoney = false;
    }


    IEnumerator CountMoneyOneType(PlayerInformation player,float lowerMoney,float upperMOney)
    {
        countingMoney = true;
        Text toChange = player.myInformationGUI.moneyTotalTextForEndShow;
        yield return StartCoroutine(player.myInformationGUI.CountInsertMoney(lowerMoney, upperMOney, toChange, Common.effects.moneyCountParams.maxDuration));
        countingMoney = false;
    }

    IEnumerator CountMoneyBonusRound(PlayerInformation player)
    {
        float startTime = Time.time;
        float totalMoney = Common.gemSkins.CalculateMoneyWon(gemsWonInBonusRound);
        Text toChange = player.myInformationGUI.moneyTotalBBResults;
        yield return StartCoroutine(player.myInformationGUI.CountInsertMoney(0, totalMoney, toChange, howLongToShowResultsOfBonusRound));
        yield return new WaitForSeconds(1f);
        player.WinMoney(totalMoney);
        yield return StartCoroutine(player.myInformationGUI.CountInsertMoney(totalMoney, 0f, toChange, howLongToShowResultsOfBonusRound));
        float elapsed = Time.time - startTime;
        float leftTilleBBends = this.howLongToShowResultsOfBonusRound-elapsed;
        if (leftTilleBBends < 0f)
        {
            BonusRoundCompletelyEnds();
        }
        else {
            Invoke("BonusRoundCompletelyEnds", leftTilleBBends);
        }
    }


    IEnumerator CreateGemMovementTypeOfGemToBothPlayers(Gem typeOfGem, string functionToRunAfter,bool isShowEndScreen=false)
    {

        StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player1, player2, typeOfGem,isShowEndScreen));
        yield return StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player1, player1, typeOfGem, isShowEndScreen));
        StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player2, player1, typeOfGem, isShowEndScreen));
        yield return StartCoroutine(SpawnGemTypeAndMoveItToMiddle(player2, player2, typeOfGem, isShowEndScreen));
        yield return new WaitForSeconds(0.5f);
        Invoke(functionToRunAfter, 0f);
        
        // yield return
    }

    void BonusRoundSpecial()
    {
        StartCoroutine(CreateBonusMovements());
    }

    void BonusRoundStartPopUp()
    {
        // Common.usefulFunctions.ShowChildForxSeconds(bonusRowShowMenu, howlongToShowBonusRoundPopUP);
        player1.myInformationGUI.ShowStartBonusRound(howlongToShowBonusRoundPopUP);
        player2.myInformationGUI.ShowStartBonusRound(howlongToShowBonusRoundPopUP);
        Invoke("StartBonusRound", howlongToShowBonusRoundPopUP);
    }





    IEnumerator CreateBonusMovements()
    {
         StartCoroutine(CombineGameObjects(player1.allGemsTomiddleCreatedGems, 0.5f));
          yield return StartCoroutine(CombineGameObjects(player2.allGemsTomiddleCreatedGems, 0.5f));
         StartCoroutine(MoveGemsBack(player1, 0.5f));
        //yield return new WaitForSeconds(1f);
        //StartCoroutine(MoveGemsBack(player1, 0.5f));
        yield return StartCoroutine(MoveGemsBack(player2, 0.5f));

        BonusRoundStartPopUp();

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
        foreach(GameObject gem in toPlayer.allExtraCreatedGems)
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
        Vector3[] startPoss = new Vector3[3] { ordered[0].transform.position, ordered[1].transform.position, ordered[2].transform.position };
        Common.effects.PlayEffect(EffectsEnum.Gem_movement_finishing_combo, middleObject.transform.position);
        float fixedZToMove = Common.effects.fixedZToMove;
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(ordered[0].transform, middleObject.transform.position, oneMoveDuration);
        yield return new WaitForSeconds(oneMoveDuration);
        
        Common.usefulFunctions.scaleGOOverTime(middleObject,middleObject.transform.localScale*1.4f,oneMoveDuration);
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(ordered[2].transform, middleObject.transform.position, oneMoveDuration);
        yield return new WaitForSeconds(oneMoveDuration);
        ordered[2].SetActive(false);
        Common.usefulFunctions.scaleGOOverTime(middleObject, middleObject.transform.localScale * 1.4f, oneMoveDuration);
        Vector3 newPos = middleObject.transform.position;
        newPos.z += 2f;
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(middleObject.transform,newPos , oneMoveDuration);
        yield return new WaitForSeconds(oneMoveDuration);
        ordered[0].SetActive(false);
        yield return new WaitForSeconds(1f);
        Debug.Log("settings scale back to " + startScale);
        ordered[0].SetActive(true);
        ordered[2].SetActive(true);
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(ordered[0].transform, startPoss[0], oneMoveDuration);
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(ordered[2].transform, startPoss[2], oneMoveDuration);
        yield return new WaitForSeconds(oneMoveDuration);
        middleObject.transform.localScale = startScale;
        yield return new WaitForSeconds(oneMoveDuration);


        yield break; 
        


    }

    public bool create3DGem = true;
    public GameObject CreateGemAndMoveToLocation(Gem toCreate, Vector3 startLocation, Vector3 endLocation, float oneMoveDuration)
    {

        GameObject createdGem = Common.gameMaster.CreateGem(toCreate, startLocation);
        createdGem.GetComponent<Rigidbody2D>().gravityScale = 0;
        float fixedZToMove = Common.effects.fixedZToMove;

        // Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(createdGem.transform, endLocation, oneMoveDuration, fixedZToMove);
        Common.usefulFunctions.MoveObjectToPlaceNonFixed(createdGem.transform, endLocation, oneMoveDuration);
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
