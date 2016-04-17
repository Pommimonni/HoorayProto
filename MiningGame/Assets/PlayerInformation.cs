﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class MoneyCounParameters{
    public float maxDuration = 1f;
    public float step = 0.5f;
}

/*
[System.Serializable]
public class MoveGemToPosParameters(){
    public float durationOfMovement=1f;
    public float durationOfStaying = 1f;
    public Vector3 whereToMove;
}*/


public class PlayerInformation : MonoBehaviour {

    void Awake()
    {
        Common.playerInfo = this;
    }
    void Start()
    {
        myInformationGUI.SetHitsLeft(RoundSettings.hits);
        gamesLeft = RoundSettings.hits;
        LoadRoundValues();
    }

    void OnLevelWasLoaded(int level)
    {
        

    }

    public string playerInfoName = "Player 1";
    public int playerNumber = 1;
    public float moneyWon = 0f;
    int gamesLeft = 6;
    public List<Gem> wonGems;
    public List<Gem> sameInARow;
    public InfoGUI myInformationGUI;

    public InfoGUI middleShowWinGUI;


    public Gem favouriteGem;  //
    public float favouriteGemBonus = 2f;
    public bool goForAllGems=true;  //If go for all gems is false player can only win by getting their favourite gem.
    public float bet = 1f; //How much money does game cost how much money you can gain.
    public float moneyTotalAmount = 0;


    public List<GameObject> allGemsTomiddleCreatedGems;  // FOR BOnus ROund
    public List<Vector3> startingPositionsOfGemMoveMiddle;


    public void PlayerHits()
    {
        this.gamesLeft--;
        Debug.Log("playerHItsBeing set" + gamesLeft.ToString());
        myInformationGUI.SetHitsLeft(this.gamesLeft);
    }
    public bool GamesOver()
    {
            return this.gamesLeft <= 0;
        
    }

    void LoadRoundValues()
    {
        bet = RoundSettings.bet;
        if (this.playerNumber == 1) {
            moneyTotalAmount = RoundSettings.moneyInsertedPLayer1;
        }else
        {
            moneyTotalAmount = RoundSettings.moneyInsertedPlayer2;
        }
        SetTotalMoneyAmount(- RoundSettings.bet);

        // moneyWon = RoundSettings.moneyInserted;        
    }

    void SetTotalMoneyAmount(float newAmountIncrease)
    {
        moneyTotalAmount += newAmountIncrease;
        if (this.playerNumber == 1)
        {
            RoundSettings.moneyInsertedPLayer1 = moneyTotalAmount;
        }
        else
        {
            RoundSettings.moneyInsertedPlayer2 = moneyTotalAmount;
        }
        myInformationGUI.SetMoneyTotal(moneyTotalAmount);
    }

    public void AddWinsToTotalMoney()
    {
        SetTotalMoneyAmount(moneyWon);
    }

    public void WinMoney(float money)
    {
        
        StartCoroutine(CountMoneyRoutine(moneyWon, moneyWon + money,myInformationGUI,Common.effects.moneyCountParams.maxDuration));
        moneyWon += money;
      //  myInformationGUI.SetWonMoney(wonMoney);
     
    }

    float CalculateHowLongtimeMoneyCountLasts(float moneyIncrease)
    {
        MoneyCounParameters moneyCountParams = Common.effects.moneyCountParams;
        float difference = moneyIncrease;
        int moneyPerStep = 1;
        if (difference * moneyCountParams.step > moneyCountParams.maxDuration)
        {
            return moneyCountParams.maxDuration;
        }
        float lasts=difference* moneyCountParams.step;
        Debug.Log("money count lasts" + lasts.ToString());
        return difference * moneyCountParams.step;
    }
    public void ChangeGames(int addHits)
    {
        gamesLeft += addHits;
        myInformationGUI.SetHitsLeft(gamesLeft);
    }
    public void HandleGem(Gem gemWon,GameObject gemGO)
    {


        wonGems.Add(gemWon);
        StartCoroutine(GemRoutine(gemWon, gemGO));
    }

 

    float HandleWonCheckAndFavouriteGem(Gem gemWon)
    {
        float toWin = gemWon.priceMoney*bet;
        if (gemWon.Name == this.favouriteGem.Name)
        {
            Debug.Log("Won favourite gem");
            return toWin*favouriteGemBonus;
        }
        else
        {
            if (!this.goForAllGems)
            {
                Debug.Log("won nothing");
                Debug.LogError("ERROROORRORORORO");
                return 0;
            }
        }
        Debug.Log("WON NORMAL GEMAAAAAAAAAA"+toWin);
        return toWin;
    }


    
    IEnumerator CountMoneyRoutine(float oldAmount,float newAmount,InfoGUI toSet,float maxDuration)
    {
        MoneyCounParameters moneyCountParams = Common.effects.moneyCountParams;
        float difference = Mathf.Abs(newAmount - oldAmount);
        float sign = Mathf.Sign(newAmount - oldAmount);
        int moneyPerStep = 1;
        if (difference * moneyCountParams.step > maxDuration)
        {
            moneyPerStep = (int)(difference* moneyCountParams.step / maxDuration);
        }
        float moneyGoer = oldAmount;
        for(int n=0; n < (int)difference; n++)
        {
            moneyGoer += sign * moneyPerStep;
            if (moneyGoer > newAmount)
            {
                break;
            }
            toSet.SetWonMoney(moneyGoer);
            yield return new WaitForSeconds(moneyCountParams.step);
            
        }
        
        toSet.SetWonMoney(newAmount);

        yield break;   
    }
    IEnumerator MoveGemToMidleAndCountMoney(GameObject gemGO,float amount,string text)
    {
        yield return new WaitForSeconds(1.2f);
        gemGO.GetComponent<Rigidbody2D>().isKinematic = true;
        float durationToGemMoveMiddle = Common.effects.durationToGemMoveMiddle;
        MoneyCounParameters moneyCountParams = Common.effects.moneyCountParams;
        MoveGemFromPositionToMiddleGUI(gemGO, 0, durationToGemMoveMiddle);
        yield return new WaitForSeconds(durationToGemMoveMiddle);
        Common.effects.PlayEffect(EffectsEnum.Finding_gem_movement_finished_to_middle, middleShowWinGUI.GetWorldPositionOfGemInfo(0));
        ShowMiddleGUI(text);
        // Debug.Log("MOWING in the start");
        // StartCoroutine(CountMoneyRoutine(0, amount, middleShowWinGUI,durationToGemMoveMiddle));
        // middleShowWinGUI.SetWonMoney(amount);
        Common.gameMaster.ShowMoneyWonAmountBoth(amount);
        yield return new WaitForSeconds(durationToGemMoveMiddle);
        Common.gameMaster.AddWonMoneyBoth(amount);

        // Debug.Log("MOWING in the middle");
        StartCoroutine(CountMoneyRoutine(amount, 0, middleShowWinGUI,moneyCountParams.maxDuration));
        float countLasts = CalculateHowLongtimeMoneyCountLasts(amount);
        if (countLasts < 1f)
        {
            countLasts = 1f;
        }
        yield return new WaitForSeconds(countLasts);
        // Debug.Log("MOWING ENDS");
        UnShowMIddleGUI();
        yield break;


    }


    void ShowMiddleGUI(string text)
    {
        Debug.Log("SHOWING GUI");
        middleShowWinGUI.ShowGUI(text);
    }
    void UnShowMIddleGUI()
    {
        Debug.Log("UNSHOWING GUI");
        middleShowWinGUI.UnShowGUI();
    }
    IEnumerator GemRoutine(Gem wonGem,GameObject gemGO)
    {
        float totalWon = HandleWonCheckAndFavouriteGem(wonGem);
        float durationToGemMoveToGUI = Common.effects.durationToGemMoveToGUI;
        float gemStayDuration = Common.effects.gemStayDuration;
        float whenInRowFoundItLasts = Common.effects.whenInRowFoundItLasts;
        //Debug.Log("GEMROUTINE STARTS");
        string tittleToShow = "Win";
        if (totalWon != 0)
        {
            
        }
        else
        {
            tittleToShow = "Wrong gem";
           // Debug.Log("Wrong gem, win nothing");
        }
        yield return StartCoroutine(MoveGemToMidleAndCountMoney(gemGO, totalWon,tittleToShow));
        // Debug.Log("GEM ROUTINE CONTINUES");
       
        Vector3 endPos=MoveGemFromPositionToGUI(gemGO, this.wonGems.Count-1, durationToGemMoveToGUI);
        //Debug.Log("END position is " + endPos);
        yield return new WaitForSeconds(durationToGemMoveToGUI);
        yield return new WaitForFixedUpdate();
       // GameObject createdEFFect=Common.effects.PlayEffect(EffectsEnum.FindingGemMovementStartingCombo,endPos);
        yield return new WaitForSeconds(gemStayDuration);
        Destroy(gemGO);
     //   Destroy(createdEFFect);
        this.myInformationGUI.SetNewWonGems(this.wonGems);
        bool enterBonus = false;
        int howManyTimes = HowManyTimesSameGameInRow(wonGem);
        if (howManyTimes > 2)
        {
            enterBonus = true;
       //     int powder = howManyTimes - 2;
          //  float amount = Mathf.Pow(won.priceMoney, powder);  //Formula for how much is the bonus money
            float amount = wonGem.priceMoney * 2;
            Vector3 whereToSpawn = myInformationGUI.GetWorldPositionOfGemInfo(1);
            GameObject rowFinalEff=Common.effects.PlayEffect(EffectsEnum.When_3_in_row_found, whereToSpawn);
            yield return new WaitForSeconds(whenInRowFoundItLasts);
            Destroy(rowFinalEff);
            //WinMoney(amount);
           // emptyInRowGems(wonGem);

        }
        Common.gameMaster.PlayerGemHandlingFinish(this,enterBonus,wonGem);
        yield break;
    }

    IEnumerator CreateGemsAndMoveToLocations(List<Gem> gems, List<Vector3> startLocations, List<Vector3> endLocations,float oneMoveDuration)
    {
        int counter = 0;
        List<GameObject> createdGemGOs =new List<GameObject>();
        foreach(Gem gem in gems)
        {
            GameObject createdGem = CreateGemAndMoveToLocation(gem, startLocations[counter], endLocations[counter], oneMoveDuration);
            createdGemGOs.Add(createdGem);      
            counter++;
            yield return new WaitForSeconds(oneMoveDuration);
        }
        
    }

    GameObject CreateGemAndMoveToLocation(Gem toCreate,Vector3 startLocation,Vector3 endLocation,float oneMoveDuration)
    {
        GameObject createdGem = Common.gameMaster.CreateGem(toCreate, startLocation);
        
        float fixedZToMove = Common.effects.fixedZToMove;
        Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(createdGem.transform, endLocation, oneMoveDuration, fixedZToMove);
        return createdGem;
    }



    void emptyInRowGems(Gem whatGemToEmpty)
    {

        wonGems = Common.gemSkins.EmptyGemsThatAre(wonGems, whatGemToEmpty) ;
        Common.gameMaster.combinedWonGems = Common.gemSkins.EmptyGemsThatAre(Common.gameMaster.combinedWonGems, whatGemToEmpty);
        this.myInformationGUI.SetNewWonGems(this.wonGems);
        // myInformationGUI.SetNewWonGems(sameInARow);
    }
    public void HandleEmptyGem()
    {
        AddMenuGems(null);
    }

    private void AddMenuGems(Gem newGem)
    {
        wonGems.Add(newGem);
        myInformationGUI.SetNewWonGems(wonGems);
    }

    int HowManyTimesSameGameInRow(Gem won)
    {
        int sameInARowCounter = 0;
        foreach (Gem gem in Common.gameMaster.combinedWonGems)
        {
            if (!Common.gemSkins.IsGemEmpty(gem))
            {
                if (gem.Name == won.Name)
                {
                    sameInARowCounter++;
                }
            }
        }
        if (sameInARowCounter == 0)
        {
            sameInARow = new List<Gem>();
        }
        sameInARow.Add(won);
       // Debug.Log("We have gained " + won.Name + " this many times in a row " + sameInARow.Count.ToString());
        return sameInARowCounter;
    }

    public Vector3 MoveGemFromPositionToGUI(GameObject gem,int index,float duration)
    {
        float fixedZToMove = Common.effects.fixedZToMove;
        Vector3 pos=myInformationGUI.GetWorldPositionOfGemInfo(index);
        Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(gem.transform, pos, duration,fixedZToMove);
        return pos;
    }

    public Vector3 MoveGemFromPositionToMiddleGUI(GameObject gem, int index, float duration)
    {
        float fixedZToMove = Common.effects.fixedZToMove;
        Vector3 pos = middleShowWinGUI.GetWorldPositionOfGemInfo(index);
        Common.usefulFunctions.MoveObjectToPlaceOverTimeFixedZ(gem.transform, pos, duration, fixedZToMove);
        return pos;
    }

}
