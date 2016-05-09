using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
public class DoBlur : MonoBehaviour {

	// Use this for initialization
	void Start () {
       myPlayer= this.gameObject.transform.parent.GetComponentInChildren<PlayerInformation>();
        myBlur=this.gameObject.GetComponent<BlurOptimized>();
        myNoiseAndGrain= this.gameObject.GetComponent<NoiseAndGrain>();
        initialized = true;
    }

    public PlayerInformation myPlayer;
    BlurOptimized myBlur;
    NoiseAndGrain myNoiseAndGrain;
    bool initialized = false;
	// Update is called once per frame
	void Update () {
        if (initialized == true)
        {
            if (onBlur == false)
            {
                if (Common.gameMaster.IsGameOnNonNormalState(myPlayer) == false)
                {
                    if (myPlayer.IsNoHits())
                    {
                        StartBlur();
                    }
                }
            }
            if (onBlur)
            {
                if (Common.gameMaster.IsGameOnNonNormalState(myPlayer) == true)
                {
                    StopBlur();
                }
            }
        }
	}


    public bool onBlur = false;


    void StartBlur()
    {
        onBlur = true;
        Invoke("DoBlurEffect", 2f);
    }

    void DoBlurEffect()
    {
        if (onBlur)
        {
            BlurEffects(true);
        }
    }

    void BlurEffects(bool onOff)
    {
        myBlur.enabled = onOff;
        myPlayer.myInformationGUI.ShowOutOfHitsOnOff(onOff);
       // myNoiseAndGrain.enabled = onOff;   
    }
    public void StopBlur()
    {
        onBlur = false;
        BlurEffects(false);
    }




}
