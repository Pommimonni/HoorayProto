
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Gem
{

    public string Name;
    public Sprite gemSprite;
    public float chanceToGet = 0.1f;
    public GameObject effectWhenGot;
    public GameObject my3DGem;
    public string textDisplyed;
    public float priceMoney=1f;
     

}


[System.Serializable]
public class Gems: MonoBehaviour
{


    void Awake()
    {
        Common.gemSkins = this;

    }




    [SerializeField]
    public List<Gem> allGems;

    public void OnEnable()

    {

        hideFlags = HideFlags.HideAndDontSave;

        if (allGems == null)

            allGems = new List<Gem>();

    }

    public Gem getGemSkin(int a)
    {
        return allGems[a];
    }

    public List<Sprite> GiveGemSkinsAsSprites()
    {
        List<Sprite> sprites = new List<Sprite>();
        foreach(Gem gem in allGems)
        {
            sprites.Add(gem.gemSprite);
        }
        return sprites;
    }


    public bool IsGemEmpty(Gem gem)
    {
        if (gem != null)
        {
            if (gem.Name != null)
            {
                if (gem.Name.Length > 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public float CalculateMoneyWon(List<Gem> gems)
    {
        float total = 0f;
        foreach(Gem gem in gems)
        {
            if (!IsGemEmpty(gem))
            {
                total += gem.priceMoney;
            }
        }
        
        return AdjustBet(total);
    }

    float AdjustBet(float amount)
    {
        return amount * RoundSettings.bet;
    }

    public float CalculateMoneyWonByType(List<Gem> gems,Gem whatTypeOfGem)
    {
        float total = 0f;
        foreach (Gem gem in gems)
        {
            if (!IsGemEmpty(gem))
            {
                if (gem.Name == whatTypeOfGem.Name)
                {

                    total += gem.priceMoney;
                }
               
            }
        }
        return AdjustBet(total);
    }


    public List<Gem> EmptyGemsThatAre(List<Gem> whatToEmpty, Gem whatTypeOfGem)
    {
        List<Gem> newWonGems = new List<Gem>();
        int counter = 0;
        foreach (Gem gem in whatToEmpty)
        {
            if (IsGemEmpty(gem))
            {
                newWonGems.Add(null);
            }
            else if (gem.Name == whatTypeOfGem.Name)
            {

                newWonGems.Add(null);
            }
            else
            {
                newWonGems.Add(gem);
            }
            counter++;
        }
        return newWonGems;
    }
}

