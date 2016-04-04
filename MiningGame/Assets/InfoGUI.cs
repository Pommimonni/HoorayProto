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
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("KEY DOWN A");
            SetNewWonGems(sameInARowForTesting);
        }
	}



    public float wonMoneyTester = 0f;
    public int hitsLeft = 10;
    public List<Gem> sameInARowForTesting;
    public Text wonAmountText;
    public Text hitsLeftText;
    public List<Image> gemImagesOnWonGems;
    public Sprite emptyWonGemSprite;
    Camera myCamera;

    public Text tittleText;

    public void SetWonMoney(float wonMoney)
    {
        wonAmountText.text = wonMoney.ToString();
    }
    public void SetHitsLeft(int newHits)
    {
        hitsLeftText.text = newHits.ToString();
    }

    public Vector3 GetWorldPositionOfGemInfo(int index)
    {
        Image img = gemImagesOnWonGems[index];
        return img.rectTransform.position;

    }


    public void SetNewWonGems(List<Gem> gems)
    {
        int counter = 0;
        Debug.Log("Setting new gems. Gem amount " + gems.Count.ToString());
        foreach(Gem gem in gems)
        {
            Debug.Log("setting new gem");
            gemImagesOnWonGems[counter].sprite = gem.gemSprite;
            counter++;
        }

        for(int n=counter; n<gemImagesOnWonGems.Count; n++)
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
