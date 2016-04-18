using UnityEngine;
using System.Collections;

public class BonusRoundBomb : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	
	// Update is called once per frame
	void Update () {
	
	}
    public ParticleSystem systemColourToChange;
    public Color player1Colour;
    public Color player2Colour;

    private int[] playersToHit = new int[2] { 1, 2 };
    private int hits = 0;
    private int hitsNeeded = 2;
    bool nextPlayerPlayer1=true;
    private bool isBigOne = false;
    public void Initialize(int newHitsNeeded,bool startingPlayerPlayer1, bool newIsBigOne)
    {
        if (newIsBigOne)
        {
            hitsNeeded = newHitsNeeded * 2;
        }
        else {
            hitsNeeded = newHitsNeeded;
        }
        nextPlayerPlayer1=startingPlayerPlayer1;
        isBigOne = newIsBigOne;
        ChangeGrapchis(startingPlayerPlayer1);
    }

    public void PlayerHitsMe(bool isPlayer1)
    {
        if (nextPlayerPlayer1==isPlayer1)
        {
            CorrectPlayerHits();
        }
        else
        {
            WrongPlayerHits(isPlayer1);
        }

    }

    private void CorrectPlayerHits()
    {
        hits++;
        if (hits >= hitsNeeded)
        {
            BombBreaks();
        }
        nextPlayerPlayer1 = !nextPlayerPlayer1;
        ChangeGrapchis(nextPlayerPlayer1);
    }

    private void ChangeGrapchis(bool newPlayerToHit)
    {
        if (newPlayerToHit)
        {
            ColourParticleEffect(player1Colour);
        }
        else
        {
            ColourParticleEffect(player2Colour);
        }
    }
    private void ColourParticleEffect(Color newColor)
    {
       // this.systemColourToChange.startColor = newColor;
    }


    private void BombBreaks()
    {
        Common.gameMaster.HandleBonusRoundWin(isBigOne,this.transform.position);

        HandleDestruction();

    }


    private void HandleDestruction()
    {
        Vector3 position = this.transform.position;
        if (!isBigOne)
        {
            Common.effects.PlayBombExplosionEffect(position);
            Common.lauriWrapper.BonusRoundDestroyWall(position, false);
        }
        else
        {
            Common.lauriWrapper.BonusRoundDestroyWall(position, true);
            Common.effects.PlayBigBombExplosionEffect(position);
        }
        Destroy(this.gameObject);
    }

    private void WrongPlayerHits(bool isPlayer1)
    {
        Debug.Log("WRONG PLAYER HITS Is it player 1" + isPlayer1.ToString());
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButton(1))
        {
            Debug.Log("RIght clicking bomb");
            PlayerHitsMe(false);
        }
        if (Input.GetMouseButton(0))
        {
            PlayerHitsMe(true);
            Debug.Log("Left clicking bomb");
        }
    }

}
