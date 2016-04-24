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
    public void BonusRoundDestroyWall(Vector3 position,bool isBigOne)
    {
        GameObject wallPieceGO = Common.usefulFunctions.RayCastAlongCameraAndReturnhit(Common.gameMaster.player1Camera, position, 11);
        wallPieceGO.GetComponentInChildren<WallPiece>().ShatterPlay(false);
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
        if (Common.gameMaster.canHitWall())
        {

            if (Input.GetMouseButton(1))
            {
                if (!Common.gameMaster.player2.GamesOver())
                {
                    WallOpen(Common.gameMaster.player2);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (!Common.gameMaster.player1.GamesOver())
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
