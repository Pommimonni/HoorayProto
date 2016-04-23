using UnityEngine;
using System.Collections;

public class WallPiece : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        Debug.Log("On mouse down on wall piece");
        if (Common.gameMaster.canHitWall() || true)
        {
            Debug.Log("Clicked a wallpiece");
            Destroy(this.gameObject);
            ShatterEffect.main.Play(this.transform.position, true, GetHittingPlayer());
            if (gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                ShatterCone.smaller.DestroyCone(this.transform.position);
            }
            else
            {
                ShatterCone.main.DestroyCone(this.transform.position);
            }
            Common.lauriWrapper.WallMouseClick();
        }
    }

    //which screen did the hit happen on?
    PlayerInformation GetHittingPlayer()
    {
        //TODO how to detect correct player
        return Common.playerInfo;
    }

    

    public void Shatter()
    {
        Destroy(this.gameObject);
    }
}
