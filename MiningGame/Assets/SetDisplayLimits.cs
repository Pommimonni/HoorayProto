using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SetDisplayLimits : MonoBehaviour {

    void Start()
    {/*
        leftPositions = new List<Vector3>();
        rightPositions = new List<Vector3>();
        GameObject[] cameras=GameObject.FindGameObjectsWithTag("MainCamera");
        if (cameras.Length < 2)
        {
            return;
        }
        foreach(GameObject camera in cameras)
        {
            GetLimit(camera);
        }

        leftPositions=Common.usefulFunctions.SortVector3ListaBasedOnx(leftPositions);
        rightPositions=Common.usefulFunctions.SortVector3ListaBasedOnx(rightPositions);
        mapMiddle = new Vector2(leftPositions[1].x, rightPositions[0].x);
        */


        mapleftBottom = GameObject.Find("mapleftBottom").transform.position;
        mapRightTop = GameObject.Find("mapRightTop").transform.position;
        mapMIddle = new Vector3(mapleftBottom.x + mapRightTop.x, mapleftBottom.y + mapRightTop.y, mapleftBottom.z * 2);
        mapMIddle = mapMIddle / 2;
        Common.mapMIddle = mapMIddle;
        Common.mapleftBottom = mapleftBottom;
        Common.mapRightTop = mapRightTop;


    }


    Vector3 mapleftBottom;
    Vector3 mapRightTop;
    Vector3 mapMIddle = Vector3.zero;
    /*
    public List<Vector3> leftPositions;
    public List<Vector3> rightPositions;
    public Vector2 mapMiddle;
    public void GetLimit(GameObject cameraGO)
    {
        Camera camera = cameraGO.GetComponent<Camera>();
        Vector3 bottomLeft = camera.ScreenToWorldPoint(new Vector3(0,0, camera.nearClipPlane));
        Vector3 topRight= camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        leftPositions.Add(bottomLeft);
        rightPositions.Add(topRight);
    }
    */
}
