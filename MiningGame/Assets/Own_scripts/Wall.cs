using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {


    public GameObject On_click_effect;
    public bool creatingHoleWithCropping = false;
   // public bool go_small = false;
    //public float go_small_speed = 0.9f;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // ChangeObjectSize(new Vector3(10f, 10f, 10f));
        //  moveRight();
        // if (go_small)
        //  {
        //      this.GoSmall();
        // }
        if (Input.GetKeyDown(KeyCode.M))
        {
            creatingHoleWithCropping = !creatingHoleWithCropping;
        }
    }

    void ChangeObjectSize(Vector3 newValue)
    {
        float decimaln= 1.1f;

        Vector3 newPosition = new Vector3(0f, 0f, 0f);
        //this.gameObject.transform.position = newPosition;
        this.gameObject.transform.localScale = newValue;

    }

    void GoSmall()
    {
   //     this.gameObject.transform.localScale = this.gameObject.transform.localScale * go_small_speed;
    }
    void OnMouseDown()
    {
        /*
        if (Common.gameMaster.canHitWall())
        {
            // go_small = true;
            //ChangeObjectSize(new Vector3(0f, 0f, 0f));

            Vector3 mousePosition = Common.usefulFunctions.GetMouseWorldPosition();
            mousePosition.z = 0;
            Common.effects.PlayEffect(EffectsEnum.Hitting_wall, mousePosition);
            bool succesfull = CreateHoleIfPossible(mousePosition);
            if (succesfull)
            {
                Common.gameMaster.WallOpened(mousePosition);
                Common.effects.PlayEffect(EffectsEnum.Wall_destrying, mousePosition);
                ///ExplosionEffect(mousePosition);
            }
            // Remove();

            //Destroy(this.gameObject);
        }
        */
    }
    void WallOpen(PlayerInformation player)
    {
        if (Common.gameMaster.canHitWall())
        {
            // go_small = true;
            //ChangeObjectSize(new Vector3(0f, 0f, 0f));

            Vector3 mousePosition = Common.usefulFunctions.GetMouseWorldPosition();
            mousePosition.z = 0;
            Common.effects.PlayEffect(EffectsEnum.Hitting_wall, mousePosition);
            bool succesfull = CreateHoleIfPossible(mousePosition);
            if (succesfull)
            {
                Common.gameMaster.WallOpened(mousePosition,player);
                Common.effects.PlayEffect(EffectsEnum.Wall_destrying, mousePosition);
                ///ExplosionEffect(mousePosition);
            }
            // Remove();

            //Destroy(this.gameObject);
        }
    }

    void OnMouseOver()
    {
        if (Common.gameMaster.canHitWall())
        {
         
            if (Input.GetMouseButton(1))
            {
                if (!Common.gameMaster.player2.GamesOver() )
                {
                    WallOpen(Common.gameMaster.player2);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (!Common.gameMaster.player1.GamesOver() )
                {
                    WallOpen(Common.gameMaster.player1);

                }
            }
        }
    }

    public GameObject unCroppedPrefab;
    public void CreateHoleWithOffset(Vector3 position,float newSize)
    {
        GameObject createdHole = Common.spriteCropper.CreateHoleAndMirrorBackground(position, newSize);
        Common.wallGrid.createdHoles.Add(createdHole);
    }
    public void CreateHoleWithCropping(Vector3 position,float size)
    {
        Debug.Log("creating hole with cropping");
        Common.wallGrid.createdHoles.Add( Common.spriteCropper.CropAndCreateHole(position, size));
    }

    public void CreateHoleWithoutCropping(Vector3 position,float newSize)
    {
         GameObject createdHole = (GameObject)Instantiate(unCroppedPrefab, position, Quaternion.identity);
         Material newShapeMat = Common.holeShapeHandler.GetRandomShapeMaterial();
         createdHole.GetComponentInChildren<SpriteRenderer>().material = newShapeMat;
         createdHole.transform.localScale = new Vector3(newSize, newSize, newSize);
    }

    public bool CreateHoleIfPossible(Vector3 position)
    {
        float newSize = Common.wallGrid.GetHoleSize(position);
        if (newSize != -1) {
            if (creatingHoleWithCropping)
            {
                CreateHoleWithCropping(position, 1f);
            }
            else {
                CreateHoleWithoutCropping(position, newSize);
            }
            return true;
        }
        else
        {
            Debug.Log("Not allowed to do Hole Here because other hole or wall is too close");
            return false;
        }
        //Idea hole is the same as creating object on the top that matches the picture on the bottom.
        
    }
    void Remove()
    {
        Destroy(this.gameObject);
    }
    void ExplosionEffect(Vector3 position)
    {
        position.z += 0.2f;
        GameObject createdObject=(GameObject)Instantiate(this.On_click_effect, position, this.gameObject.transform.localRotation);
    }
}
