using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfoGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
        myPlayer = this.transform.parent.GetComponent<PlayerInformation>();
       // wonAmountText.text = wonMoneyTester.ToString();
       // hitsLeftText.text = hitsLeft.ToString();
        foreach (Image img in gemImagesOnWonGems)
        {
         //   img.sprite = emptyWonGemSprite;
        }
        Common.playerInfoGUI = this;
        if (betText)
        {
            SetBet();
        }
        
    }

    public Text debugText;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Canvas mycanv=this.GetComponent<Canvas>();//.planeDistance.ToString();
            RenderMode rend = mycanv.renderMode;
            
            mycanv.renderMode = RenderMode.ScreenSpaceCamera;
            mycanv.worldCamera = myCamera;
            Canvas.ForceUpdateCanvases();
            debugText.text = rend.ToString() +"      "+ mycanv.worldCamera.ToString();
            //canvas.renderMode = RenderMode.OverlayCamera;
            //  debugText.text += mycanv.camera.ToString();
            //mycanv.renderMode = RenderMode.ScreenSpaceCamera;
            //mycanv.render

            // Debug.Log("KEY DOWN A");
            //SetNewWonGems(sameInARowForTesting);
        }
	}
    PlayerInformation myPlayer;
    public Transform sBBResults;
    public Transform sEndGameResults;
    public Transform sStartBB;
    public Transform sPlayAgain;
    public Transform sHeadOut;
    public Transform BonusRoundCombining;
    public Transform sOutOfHits;

    public float wonMoneyTester = 0f;
    public int hitsLeft = 10;
    public List<Gem> sameInARowForTesting;
    public Text midScreenWonAmountTextNotUsed;
    public Text hitsLeftText;
    public List<Image> gemImagesOnWonGems;
    public List<Image> gemImagesOnMiddleGemsBonusRound;
    public List<Image> gemImagesOnShowEndScreenGems;
    public Sprite emptyWonGemSprite;
    public Camera myCamera;
    public Color usedAxeColor;
    public Color unUsedAxeColor;
    public GameObject testSpawn;
    public Transform hitShowHorizontal;
    public Transform combinedHitsAndGemsFound;

    public Transform coinMoveStartlocation;

    public Text tittleText;
    public FMODUnity.StudioEventEmitter coinCountSound;
    public Text betText;
    public Text moneyTotalText;
    public Text moneyTotalTextForEndShow;
    public Text moneyTotalBBResults;

    public List<GameObject> my3DGUIGEMS;

    public CurtainDrop myCurtain;

    List<Gem> gemTypeOf3DGems;

    public RectTransform locationForCoinMovement;

    public void AddGUIGem(GameObject gem,Gem gemType)
    {
        my3DGUIGEMS.Add(gem);
       // gemTypeOf3DGems.Add(gemType);
    }

    public GameObject Get3DGem(int counter)
    {
        return my3DGUIGEMS[counter];
    }

    public void SetShowEndScreenToEmpty()
    {
        foreach(Image gemImg in gemImagesOnShowEndScreenGems)
        {
            gemImg.sprite = emptyWonGemSprite;
        }
    }

    public void SetWonMoney(float wonMoney)
    {
       // Debug.LogError("settings money " + wonMoney);
        midScreenWonAmountTextNotUsed.text = wonMoney.ToString();
    }


    public void DropMyCurtain()
    {
        myCurtain.Startmoving(-1);
    }
    public bool IsDropFinished()
    {
        return !myCurtain.moving;
    }

    public void RaiseMyCurtain()
    {
        myCurtain.Startmoving(1);
    }

    public void SetHitsLeft(int newHits)
    {
         hitsLeftText.text = "x"+newHits.ToString();
       // ColourHitSown(newHits);

    }

    public void TransformTo2D()
    {
        SetGemShow(myPlayer.wonGems);
        foreach(GameObject gem3D in my3DGUIGEMS){
            gem3D.SetActive(false);
        }
    }

    void EmptyGemShow()
    {
        foreach(Image img in gemImagesOnWonGems)
        {
            img.sprite = emptyWonGemSprite;
            // gemImagesOnWonGems[n].sprite = emptyWonGemSprite;
        }
    }

    public void TransformTo3D()
    {
        EmptyGemShow();
        foreach (GameObject gem3D in my3DGUIGEMS)
        {
            gem3D.SetActive(true);
        }

    }

    public void SetBet()
    {
        //Debug.Log(this.gameObject.name);
        betText.text = "€"+RoundSettings.bet.ToString();
    }
    public void SetMoneyTotal(float money)
    {
        if (moneyTotalText)
        {
            moneyTotalText.text = Common.usefulFunctions.FormatTOtaleAmountTOText(money);
        }
    }

    public void CountTotalMoneyForEndText(float oldAmount,float newAmount,float maxDuration)
    {
        StartCoroutine(CountInsertMoney(oldAmount, newAmount, moneyTotalTextForEndShow, maxDuration));
    }

    public bool AreWeCountingMoney()
    {
        return stillCountingMoney;
    }

    bool stillCountingMoney = false;

    public void CahsOut()
    {
        StopShowPlayAgain();
        StartCoroutine(CountInsertMoney(myPlayer.moneyTotalAmount, 0, moneyTotalText, Common.effects.moneyCountParams.maxDuration));
        RoundSettings.player1Money = RoundSettings.StartMoney;
        RoundSettings.player2Money = RoundSettings.StartMoney;

    }

    public void CountHIts()
    {
        StopShowPlayAgain();
        StartCoroutine(CountInsertMoney(0, RoundSettings.maxHits, hitsLeftText, Common.effects.moneyCountParams.maxDuration,0.4f,false,true));

    }

    public IEnumerator CountInsertMoney(float oldAmount, float newAmount, Text toSet, float maxDuration,float speed=1f,bool createCoinMovement=false,bool areWeCountinghits=false)
    {
        stillCountingMoney = true;
        MoneyCounParameters moneyCountParams = Common.effects.moneyCountParams;
        float startTime = Time.time;
        float difference = Mathf.Abs(newAmount - oldAmount);
        float sign = Mathf.Sign(newAmount - oldAmount);
        int moneyPerStep = 1;
        float lasts = difference * moneyCountParams.step;
        int modToCoinMove = Common.effects.modToCoinMove;
        if (!areWeCountinghits)
        {
            coinCountSound.Play();
        }
        if (difference * moneyCountParams.step > maxDuration)
        {
            
            float floatmoneyPerStep = (difference * moneyCountParams.step / maxDuration);
            moneyPerStep =(int)Mathf.Ceil(floatmoneyPerStep);
            Debug.Log("changing step new step is "+moneyPerStep);
        }
        float moneyGoer = oldAmount;
        float step = moneyCountParams.step/speed;
        
           
        for (int n = 0; n < (int)difference; n++)
        {
            moneyGoer += sign * moneyPerStep;
            if (moneyGoer*sign > newAmount)
            {
                break;
            }
            if (createCoinMovement)
            {
                
                if (n % modToCoinMove == 0)
                {
                    Common.effects.CreateOneCoinMove(step*modToCoinMove,coinMoveStartlocation.position,locationForCoinMovement.position);   // moneyTotalBBResults.transform.position
                                                                                                                                    // Instantiate(testSpawn, moneyTotalText.rectTransform.position, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(step);
            if (areWeCountinghits == true)
            {
                toSet.text = "x"+moneyGoer.ToString();
            }
            else {
                toSet.text = Common.usefulFunctions.FormatTOtaleAmountTOText(moneyGoer);//moneyGoer.ToString();
            }

        }
        if (!areWeCountinghits)
        {
            coinCountSound.Stop();
        }
        if (areWeCountinghits == true)
        {
            toSet.text = "x" + newAmount.ToString();
        }
        else {
            toSet.text = Common.usefulFunctions.FormatTOtaleAmountTOText(newAmount); //newAmount.ToString();
        }

      //  Debug.Log("in reality money count lasted " + (Time.time - startTime).ToString()+ "  with step "+moneyPerStep+" we caunted "+difference+ " should last"+lasts.ToString()+ " max duration is "+maxDuration.ToString());
      //  Debug.Log("New amount " + newAmount);
       // Debug.Log("Old Amount " + oldAmount);
        stillCountingMoney = false;
        yield break;
    }
    public Image line1;
    public Image line2;
    IEnumerator WaitAndDoLine(int index, float duration)
    {
        yield return new WaitForSeconds(duration);
        if (index == 0)
        {
            line1.enabled = true;
        }
        if (index == 1)
        {
            line2.enabled = true;
            line1.enabled = true;
        }
    }
    public void AddLine(int index,float duration)
    {
        StartCoroutine(WaitAndDoLine(index, duration));
    }

    public void ShowBBResultsSetActive(bool active)
    {
        GameObject objectToShow = sBBResults.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(active);
        // Invoke("StartBonusRound", duration);
    }


    public void StartShowEndGameResults()
    {
        
        GameObject objectToShow = sEndGameResults.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(true);
        // Invoke("StartBonusRound", duration);
    }
    public void StopShowEndGameResults()
    {
        GameObject objectToShow = sEndGameResults.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(false);
    }

    public void OnOFfBOnusRoundStartPopUp(bool onOff)
    {
        GameObject objectToShow = sStartBB.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(onOff);
        // Common.usefulFunctions.ShowChildForxSeconds(sStartBB, duration);
    }

    public void ShowOutOfHitsOnOff(bool onOff)
    {
        GameObject objectToShow = sOutOfHits.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(onOff);
    }

    public void ShowPlayAgain()
    {
        GameObject objectToShow = sPlayAgain.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(true);
    }

    public void DoHeadOut()
    {
        GameObject objectToShow = sHeadOut.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(true);
        TotalAmountZeroEffect();
    }

    public void StopShowPlayAgain()
    {
        GameObject objectToShow = sPlayAgain.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(false);
    }


    public void SetCombinedHitsAndDiamonds(List<Gem> gemsFound)
    {
        for (int n = gemsFound.Count - 1; n >= 0; n--)
        {
            Transform child = combinedHitsAndGemsFound.GetChild(n);
            Gem gem = gemsFound[n];
            if (child.GetComponent<Image>())
            {
                child.GetComponent<Image>().color = Color.white;
                if (Common.gemSkins.IsGemEmpty(gem))
                {
                    child.GetComponent<Image>().sprite = emptyWonGemSprite;
                }
                else
                {
                    child.GetComponent<Image>().sprite = gem.gemSprite;
                }
            }
        }
    }

    public void ColourHitSown(int amount)
    {
        int counter = 0;
        for(int n= hitShowHorizontal.childCount-1; n>=0; n--)
        {
            Transform child = hitShowHorizontal.GetChild(n);
            if (child.GetComponent<Image>())
            {
                if (amount-1 < counter)
                {
                    child.GetComponent<Image>().color = usedAxeColor;
                }
                else
                {
                    child.GetComponent<Image>().color = unUsedAxeColor;
                }
            }
            counter++;
        }
    }


    public Vector3 GetWorldPositionOfGemInfo(int index)
    {
        Image img = gemImagesOnWonGems[index];
        return img.rectTransform.position;

    }
    public GameObject GetGOOfGemMiddleShowEndScreen(int index)
    {
        Image img = gemImagesOnShowEndScreenGems[index];
        return img.gameObject;
    }

    public Vector3 GetWorldPositionOfMiddleGemLocation(int index)
    {
        Image img = gemImagesOnMiddleGemsBonusRound[index];
        return img.rectTransform.position;
    }


    public Vector3 GetWorldPositionOfShowEndGemLocation(int index)
    {
        Image img = gemImagesOnShowEndScreenGems[index];
        return img.rectTransform.position;
    }

    public void SetNewWonGems(List<Gem> gems)
    {
        //Takaisin jos 2d normi peliinSetGemShow(gems);
        //  SetCombinedHitsAndDiamonds(gems);
        //   Common.gameMaster.RefreshTotalGemShow();
    }

    public void BetDiamondPressed()
    {
        if (Common.gameMaster.gameEnded)
        {
            RoundSettings.cameFromBet = true;
            Common.levelLoader.LoadMenu();
        }
    }

    public GameObject InfoBetDiamond;
    public Image betNotification;

    bool BetScaleEffectGoing = false;
    public void BetdiamondButtonEffectStart()
    {
        BetScaleEffectGoing = true;
        StartCoroutine(ScaleEffectForUI(InfoBetDiamond));
        betNotification.enabled = true;
    }

    public void StopBetScaleEffect()
    {
        BetScaleEffectGoing = false;
    }

    public void TotalAmountZeroEffect()
    {
        GameObject amountGO = moneyTotalText.gameObject;
        moneyTotalText.color = Color.red;
        StartCoroutine(ScaleEffectForUI(amountGO));
    }

    IEnumerator ScaleEffectForUI(GameObject toScale)
    {
        Vector3 startScale = toScale.transform.localScale;
        float percentageToScale = 0.7f;
        float dur = 1f;
        Vector3 lastScale = startScale * (1.2f + percentageToScale);
        Vector3 lowScale = startScale * percentageToScale;
        while (Common.gameMaster.gameEnded)
        {
            Common.usefulFunctions.scaleGOOverTime(toScale, lowScale, dur);
            yield return new WaitForSeconds(dur);
            Common.usefulFunctions.scaleGOOverTime(toScale,  startScale, dur);
            yield return new WaitForSeconds(dur);
            Common.usefulFunctions.scaleGOOverTime(toScale, lastScale, dur);
        }
    }


    public void GemShowDisableGem(int counter)
    {
        Debug.Log("Disabling gem on counter " + counter);
        gemImagesOnWonGems[counter].sprite = emptyWonGemSprite;
    }

    public void GemShowAddGem(int counter, Gem gem)
    {
        Debug.Log("Adding gemshow on  " + counter);
        gemImagesOnWonGems[counter].sprite = gem.gemSprite;
    }

    public void SetGemShow(List<Gem> gems)
    {
        int counter = 0;
        foreach (Gem gem in gems)
        {
            bool added = false;
            if (gem != null)
            {
                if (gem.Name != null)
                {
                    {
                        if (gem.Name.Length > 1)
                        {
                         //   Debug.Log("setting new gem");
                            GemShowAddGem(counter, gem);
                            added = true;
                        }
                    }
                }
            }
            if (added == false)
            {
                GemShowDisableGem(counter);
            }
            counter++;
        }

        for (int n = counter; n < gemImagesOnWonGems.Count; n++)
        {
           // gemImagesOnWonGems[n].sprite = emptyWonGemSprite;
        }
    }

    public void UnShowGUI()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ShowGUI(string text)
    {
        this.tittleText.text = text;
        this.transform.GetChild(0).gameObject.SetActive(true);

    }

}
