using UnityEngine;
using System.Collections;

public class LauriWrapper : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Common.lauriWrapper = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //laita tarvittava toiminnallisuus seinän tekoon
    public void BonusRoundDestroyWall(Vector3 position,bool isBigOne, int destroyedOnScreen,Gem gem)
    {
        Camera camera = Common.gameMaster.GetCameraBasedOnPosition(position);
        GameObject wallPieceGO = Common.usefulFunctions.RayCastAlongCameraAndReturnhit(camera, position, 11);
        wallPieceGO.GetComponentInChildren<WallPiece>().ShatterPlay(false, destroyedOnScreen);
        GameObject the3dModel= gem.my3DGem;

        //Tee gem tulee ulos räjähdys kolosta.
        GameObject createdGemGO = Common.gameMaster.CreateGem(gem, wallPieceGO.transform.position); //(GameObject)Instantiate(the3dModel,wallPieceGO.transform.position,Quaternion.identity);

        //Create dropping gem found!

        GemFall gemFall = createdGemGO.GetComponentInChildren<GemFall>();
        PlayerInformation hittingPlayer = Common.gameMaster.GetHittingPlayer(destroyedOnScreen);
        gemFall.FallBelowScreenAndWait(wallPieceGO.transform.position, hittingPlayer);

        Common.gameMaster.gemsWonInBBGOS.Add(createdGemGO);

      //  ShatterCone.smaller.DestroyCone(position); tai jotain vastaavaa
        //positio on parametri joka kertoo missä paikassa pommi tuhoutuu
        //is bigOne parametri joka kertoo jos on isompi pommi tulossa.
        //laita funktio toiminnalisuusjota kutsut kun seinää tuhotaan at bonusround
    }
    

    //

    //Laita scriptiin jossa on sun seinäs
    void OnMouseOver()
    {
        WallMouseClick();

    }



    public void WallMouseClick()
    {
        if (Common.gameMaster.canHitWall(null))
        {

            if (Input.GetMouseButton(1))
            {
                if (!Common.gameMaster.player2.IsNoHits())
                {
                    WallOpen(Common.gameMaster.player2);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (!Common.gameMaster.player1.IsNoHits())
                {
                    WallOpen(Common.gameMaster.player1);

                }
            }
        }
    }


    public void WallOpen(PlayerInformation player)
    {
        Vector3 mousePosition = Common.usefulFunctions.GetMouseWorldPosition();
        mousePosition.z = 0;
        Common.gameMaster.WallOpened(mousePosition, player);
    }


    }
