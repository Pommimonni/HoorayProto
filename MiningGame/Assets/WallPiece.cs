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
        if (Common.gameMaster.canHitWall())
        {
            Debug.Log("Clicked a wallpiece");
            ShatterPlay(true);
           // Common.lauriWrapper.WallMouseClick();
        }
    }
    public void ShatterPlay(bool gemFound)
    {
        Destroy(this.gameObject);
        ShatterEffect.main.Play(this.transform.position, gemFound, GetHittingPlayer(this.transform.position));
        if (gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            ShatterCone.smaller.DestroyCone(this.transform.position);
        }
        else
        {
            ShatterCone.main.DestroyCone(this.transform.position);
        }
    }
    //which screen did the hit happen on?
    PlayerInformation GetHittingPlayer(Vector3 position)
    {
        if (position.x < Common.mapMIddle.x)
        {
            return Common.gameMaster.player1;
        }
        else
        {
            return Common.gameMaster.player2;
        }
        //TODO how to detect correct player
        return Common.playerInfo;
    }

    

    public void Shatter()
    {
        Destroy(this.gameObject);
    }
}
