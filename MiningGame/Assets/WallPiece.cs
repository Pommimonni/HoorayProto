using UnityEngine;
using System.Collections;

public class WallPiece : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        float r = Random.Range(0, 100);
        if(r < 14)
        {
            float delay = Random.Range(0, 20);
            Invoke("SpawnShinyEffect", delay);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnMouseDown()
    {

    }

    void SpawnShinyEffect()
    {
        GameObject shine = (GameObject)GameObject.Instantiate(ShinyEffect.main.shinePrefab, transform.position + Vector3.back*5, Quaternion.identity);
        //shine.transform.parent = ShinyEffect.main.transform;
        shine.transform.parent = transform;
        shine.transform.localScale = new Vector3(1, 1, 1);
    }

    void OnMultiDisplayMouseDown()
    {
        Debug.Log("On mouse down on wall piece");
        PlayerInformation hittingPlayer = GetHittingPlayer(this.transform.position);
        if (Common.gameMaster.canHitWall(hittingPlayer))
        {
            Debug.Log("Clicked a wallpiece");
            bool isGem=Common.gameMaster.WallOpened(this.transform.position, hittingPlayer);
            ShatterPlay(isGem);
            
           // PlayerInformation hittingPlayer = GetHittingPlayer(this.transform.position);
            // Common.lauriWrapper.WallMouseClick();
        }
    }
    public void ShatterPlay(bool gemFound)
    {
        Destroy(this.gameObject);
        PlayerInformation hittingPlayer = GetHittingPlayer(this.transform.position);
        ShatterEffect.main.Play(this.transform.position, gemFound,hittingPlayer);
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
        return Common.gameMaster.GetHittingPlayer(position);
    }

    

    public void Shatter()
    {
        Destroy(this.gameObject);
    }
}
