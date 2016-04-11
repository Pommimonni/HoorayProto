using UnityEngine;
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
        myInformationGUI.SetHitsLeft(gamesLeft);
    }

    void OnLevelWasLoaded(int level)
    {
        

    }

    public string playerInfoName = "Player 1";
    float moneyBalance = 0f;
    int gamesLeft = 6;
    public List<Gem> wonGems;
    public List<Gem> sameInARow;
    public InfoGUI myInformationGUI;

    public InfoGUI middleShowWinGUI;


    public Gem favouriteGem;  //
    public float favouriteGemBonus = 2f;
    public bool goForAllGems=true;  //If go for all gems is false player can only win by getting their favourite gem.
    public float bet = 1f; //How much money does game cost how much money you can gain.

    public void PlayerHits()
    {
        this.gamesLeft--;
        myInformationGUI.SetHitsLeft(this.gamesLeft);
    }
    public bool GamesOver()
    {
            return this.gamesLeft <= 0;
        
    }

    void LoadRoundValues()
    {
        RoundSettings settings = Common.roundSettings;
        bet = settings.bet;
        moneyBalance = settings.moneyInserted;        
    }

    public void WinMoney(float money)
    {
        
        StartCoroutine(CountMoneyRoutine(moneyBalance, moneyBalance + money,myInformationGUI,Common.effects.moneyCountParams.maxDuration));
        moneyBalance += money;
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
                return 0;
            }
        }
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
        float durationToGemMoveMiddle = Common.effects.durationToGemMoveMiddle;
        MoneyCounParameters moneyCountParams = Common.effects.moneyCountParams;
        MoveGemFromPositionToMiddleGUI(gemGO, 0, durationToGemMoveMiddle);
        yield return new WaitForSeconds(durationToGemMoveMiddle);
        Common.effects.PlayEffect(EffectsEnum.Finding_gem_movement_finished_to_middle, middleShowWinGUI.GetWorldPositionOfGemInfo(0));
        ShowMiddleGUI(text);
        // Debug.Log("MOWING in the start");
        // StartCoroutine(CountMoneyRoutine(0, amount, middleShowWinGUI,durationToGemMoveMiddle));
        middleShowWinGUI.SetWonMoney(amount);
        yield return new WaitForSeconds(durationToGemMoveMiddle);
        this.WinMoney(amount);
        
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
            Debug.Log("Wrong gem, win nothing");
        }
        yield return StartCoroutine(MoveGemToMidleAndCountMoney(gemGO, totalWon,tittleToShow));
        // Debug.Log("GEM ROUTINE CONTINUES");
        int howManyTimes = HowManyTimesSameGameInRow(wonGem);
        Vector3 endPos=MoveGemFromPositionToGUI(gemGO, howManyTimes-1, durationToGemMoveToGUI);
        //Debug.Log("END position is " + endPos);
        yield return new WaitForSeconds(durationToGemMoveToGUI);
        yield return new WaitForFixedUpdate();
        GameObject createdEFFect=Common.effects.PlayEffect(EffectsEnum.Finding_gem_movement_finished_to_combo,endPos);
        yield return new WaitForSeconds(gemStayDuration);
        Destroy(gemGO);
        Destroy(createdEFFect);
        this.myInformationGUI.SetNewWonGems(this.sameInARow);
        bool enterBonus = false;
        if (howManyTimes > 2)
        {
            enterBonus = true;
       //     int powder = howManyTimes - 2;
          //  float amount = Mathf.Pow(won.priceMoney, powder);  //Formula for how much is the bonus money
            float amount = wonGem.priceMoney * 2;
            Vector3 whereToSpawn = myInformationGUI.GetWorldPositionOfGemInfo(1);
            GameObject rowFinalEff=Common.effects.PlayEffect(EffectsEnum.ManyInRow, whereToSpawn);
            yield return new WaitForSeconds(whenInRowFoundItLasts);
            Destroy(rowFinalEff);
            WinMoney(amount);
            emptyInRowGems();

        }
        Common.gameMaster.PlayerGemHandlingFinish(this,enterBonus);
        yield break;
    }

    void emptyInRowGems()
    {
        this.sameInARow = new List<Gem>();
        myInformationGUI.SetNewWonGems(sameInARow);
    }

    int HowManyTimesSameGameInRow(Gem won)
    {
        int sameInARowCounter = 0;
        foreach (Gem gem in sameInARow)
        {
            if (gem.Name == won.Name)
            {
                sameInARowCounter++;
            }
        }
        if (sameInARowCounter == 0)
        {
            sameInARow = new List<Gem>();
        }
        sameInARow.Add(won);
        Debug.Log("We have gained " + won.Name + " this many times in a row " + sameInARow.Count.ToString());
        return sameInARow.Count;
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
