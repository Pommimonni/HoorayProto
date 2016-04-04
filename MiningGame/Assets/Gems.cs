﻿
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



}

