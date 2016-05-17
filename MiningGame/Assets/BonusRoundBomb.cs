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
    public FMODUnity.StudioEventEmitter bombFailDropSound;
    public ParticleSystem systemColourToChange;
    public GameObject fuseEffectGO;
    public Color player1Colour;
    public Color player2Colour;
    private int hitOnScreen;


    private int[] playersToHit = new int[2] { 1, 2 };
    private int hits = 0;
    private int hitsNeeded = 2;
    bool nextPlayerPlayer1=true;
    private bool isBigOne = false;
    public void Initialize(int newHitsNeeded,bool startingPlayerPlayer1, bool isInfotext)
    {

        hitsNeeded = newHitsNeeded;
        if (isInfotext)
        {
            hitMeText.SetActive(true);
        }
        else
        {
            hitMeText.SetActive(false);
        }
        nextPlayerPlayer1=startingPlayerPlayer1;
        isBigOne = false;
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
        canBehit = false;
        Gem won=Common.gameMaster.HandleBonusRoundWin(isBigOne,this.transform.position);
        explosion.Play();
        HandleDestruction(won);

    }
    bool canBehit = true;
    public Vector3 myVelocity;
    public GameObject hitMeText;
    public float xVelForDropDown=2f;
    public void BombDropsDown()
    {
        canBehit = false;
        this.transform.GetComponent<Rigidbody>().useGravity = true;
        Vector3 currVel = this.transform.GetComponent<Rigidbody>().velocity;
        float sign = Mathf.Sign(currVel.x);
        currVel.x = sign * xVelForDropDown;
        this.transform.GetComponent<Rigidbody>().velocity = currVel;
        bombFailDropSound.Play();
        
    }


    private void HandleDestruction(Gem won)
    {
        Vector3 position = this.transform.position;
        if (!isBigOne)
        {
            Common.effects.PlayBombExplosionEffect(position);
            Common.lauriWrapper.BonusRoundDestroyWall(position, false, hitOnScreen,won);
        }
        else
        {
            Common.lauriWrapper.BonusRoundDestroyWall(position, true, hitOnScreen,won);
            Common.effects.PlayBigBombExplosionEffect(position);
        }
        // Destroy(spotLight.gameObject);
        spotLight.GetComponent<DimLight>().DimLights();
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

    void OnMultiDisplayMouseDown(int screenIndex)
    {
        if (canBehit)
        {
            Debug.Log("On mouse down on bomb");
            hitOnScreen = screenIndex;
            PlayerInformation hitter = Common.gameMaster.GetHittingPlayer(screenIndex);
            bool isp1 = false;
            if (hitter == Common.gameMaster.player1)
            {
                isp1 = true;
            }
            PlayerHitsMe(isp1);
        }
    }

}
