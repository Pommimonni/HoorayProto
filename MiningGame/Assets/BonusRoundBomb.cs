using UnityEngine;
using System.Collections;

public class BonusRoundBomb : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	
	// Update is called once per frame
	void Update () {
	
	}
    public GameObject spotLight;
    public FMODUnity.StudioEventEmitter fuseLit;
    public FMODUnity.StudioEventEmitter explosion;
    public ParticleSystem systemColourToChange;
    public GameObject fuseEffectGO;
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
        SetNewPlayerCOlour(startingPlayerPlayer1);
        spotLight = (GameObject)GameObject.Instantiate(spotLight, this.transform.position, Quaternion.identity);
        spotLight.GetComponent<FollowTransform>().target = transform;
        spotLight.transform.localRotation = Quaternion.Euler(9, 0, 0);
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
            return;
        }
        nextPlayerPlayer1 = !nextPlayerPlayer1;
        SetNewPlayerCOlour(nextPlayerPlayer1);
        SetFuse();
        SetScaleAnimation();
        fuseLit.Play();
    }

    private void SetFuse()
    {
        fuseEffectGO.SetActive(true);
    }

    private void SetScaleAnimation()
    {
        GetComponentInChildren<BonusAnimation>().PlayAnimationEffect();
    }

    private void SetNewPlayerCOlour(bool newPlayerToHit)
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
        explosion.Play();
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
    /*
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
    */

    void OnMultiDisplayMouseDown()
    {
        Debug.Log("On mouse down on bomb");
        PlayerInformation hittingPlayer = Common.gameMaster.GetHittingPlayer(this.transform.position);
        bool isp1 = false;
        if (hittingPlayer == Common.gameMaster.player1)
        {
            isp1 = true;
        }
        PlayerHitsMe(isp1);
    }

}
