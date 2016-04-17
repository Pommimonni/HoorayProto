using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class InfoGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
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
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
           // Debug.Log("KEY DOWN A");
            //SetNewWonGems(sameInARowForTesting);
        }
	}



    public float wonMoneyTester = 0f;
    public int hitsLeft = 10;
    public List<Gem> sameInARowForTesting;
    public Text wonAmountText;
    public Text hitsLeftText;
    public List<Image> gemImagesOnWonGems;
    public List<Image> gemImagesOnMiddleGems;
    public Sprite emptyWonGemSprite;
    Camera myCamera;
    public Color usedAxeColor;
    public Color unUsedAxeColor;

    public Transform hitShowHorizontal;
    public Transform combinedHitsAndGemsFound;

    public Text tittleText;

    public Text betText;
    public Text moneyTotalText;

    public void SetWonMoney(float wonMoney)
    {
       // Debug.LogError("settings money " + wonMoney);
        wonAmountText.text = wonMoney.ToString();
    }
    public void SetHitsLeft(int newHits)
    {
        // hitsLeftText.text = newHits.ToString();
        ColourHitSown(newHits);

    }
    public void SetBet()
    {
        Debug.Log(this.gameObject.name);
        betText.text = RoundSettings.bet.ToString();
    }
    public void SetMoneyTotal(float money)
    {
        if (moneyTotalText)
        {
            moneyTotalText.text = money.ToString();
        }
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

    public Vector3 GetWorldPositionOfMiddleGemLocation(int index)
    {
        Image img = gemImagesOnMiddleGems[index];
        return img.rectTransform.position;
    }

    public void SetNewWonGems(List<Gem> gems)
    {
        SetGemShow(gems);
        //  SetCombinedHitsAndDiamonds(gems);
        //   Common.gameMaster.RefreshTotalGemShow();
    }


    public void GemShowDisableGem(int counter)
    {
        gemImagesOnWonGems[counter].sprite = emptyWonGemSprite;
    }

    public void GemShowAddGem(int counter, Gem gem)
    {
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
                            Debug.Log("setting new gem");
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
            gemImagesOnWonGems[n].sprite = emptyWonGemSprite;
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
