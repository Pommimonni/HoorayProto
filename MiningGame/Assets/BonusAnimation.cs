using UnityEngine;
using System.Collections;

public class BonusAnimation : MonoBehaviour {

	// Use this for initialization
	void Start () {
     //   PlayAnimationEffect();
	}
	
	// Update is called once per frame
	void Update () {
        if (playing)
        {
            if (CheckAreWeMiddle())
            {
                StopAnimationEffect();
            }
        }
	}
    bool playing = false;
    public void PlayAnimationEffect()
    {
        this.GetComponent<Animation>().Play();
        playing = true;
    }

    void StopAnimationEffect()
    {
        this.GetComponent<Animation>().Stop();
        this.transform.localScale = Vector3.one;
        playing = false;
    }

    bool CheckAreWeMiddle()
    {
        float x = this.transform.position.x;
        float mx = Common.mapMIddle.x;
        if (x < mx+2 &&  x>mx-2)
        {
            return true;
        }
        return false;
    }
}
