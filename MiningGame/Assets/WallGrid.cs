using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallGrid : MonoBehaviour {
    public bool[][] takenPixels;
    public List<GameObject> createdHoles;
    // Use this for initialization
    public List<float> holeSizes = new List<float>();
    public List<float> allowedSizess = new List<float>();
    public float maxSize = 0f;
    public void Start()
    {
        Common.wallGrid = this;
    }
    public float increment=0;
    public void CreateHoleSizes()
    {
        GameSettings settings = Common.gameSettings;
        int iterations = Common.gameSettings.amountOfHoleSizes;
        increment =   (settings.maxHoleSize-settings.minHoleSize)/(iterations-1);
        
        for(int n=0; n<iterations; n++)
        {
            float size = settings.minHoleSize + increment*n;
            holeSizes.Add(size);
        }

    }

    public float GetHoleSize(Vector3 position)
    {
        if (increment == 0f)
        {
            CreateHoleSizes();
        }
        float maxAllowedSize = GetAllowedMaxSize(position);
        maxSize = maxAllowedSize;
        List<float> allowedSizesToUse = new List<float>();
        foreach(float holeSize in holeSizes)
        {
            if (holeSize > maxAllowedSize)
                break;
            
            allowedSizesToUse.Add(holeSize);
        }
        allowedSizess = allowedSizesToUse;
        if (allowedSizesToUse.Count == 0)
        {
            return -1;
        }
        int randomIndex = Random.Range(0, allowedSizesToUse.Count);
        
        return allowedSizesToUse[randomIndex];
    }

    float GetAllowedMaxSize(Vector3 position)
    {
        float xp = FindMaxHoleSizeToDirection(new Vector2(1, 0), position);
        float xm = FindMaxHoleSizeToDirection(new Vector2(-1, 0), position);
        float yp = FindMaxHoleSizeToDirection(new Vector2(0, 1), position);
        float ym = FindMaxHoleSizeToDirection(new Vector2(0, -1), position);
        float xpyp= FindMaxHoleSizeToDirection(new Vector2(1, 1), position);
        float xmym = FindMaxHoleSizeToDirection(new Vector2(-1, -1), position);
        float xmyp = FindMaxHoleSizeToDirection(new Vector2(-1, 1), position);
        float xpym = FindMaxHoleSizeToDirection(new Vector2(1, -1), position);
        float smallestValue=Mathf.Min(xp, xm, yp, ym,xpym,xmym,xpyp,xmyp);
     //   Debug.Log("MAX sizes in x "+xp.ToString() + " " + xm.ToString());
        return smallestValue;
    }

    float FindMaxHoleSizeToDirection(Vector2 direction,Vector3 startPosition)
    {
        bool foundHoleInDirectionOrEnd = false;
        float distanceMoved = 0;
        Vector3 positionGoer = startPosition;
        while (!foundHoleInDirectionOrEnd)
        {
            distanceMoved += increment;
            if (direction.x != 0)
            {
                positionGoer.x = positionGoer.x + increment*direction.x;
            }
            if (direction.y != 0)
            {
                positionGoer.y = positionGoer.y + increment*direction.y;
            }
            foundHoleInDirectionOrEnd = RayHitsTophole(positionGoer);
            if (distanceMoved >= Common.gameSettings.maxHoleSize)
            {
                foundHoleInDirectionOrEnd = true;
            }
        }
        return distanceMoved;
    }
    
    bool RayHitsTophole(Vector3 position)
    {
       // Debug.Log("Starting to check if we hit TopHole layer 8");
        //return Common.usefulFunctions.RayCastAndCheckIfLayerHit(position, 8);
        return Common.usefulFunctions.RayCast2DCheckLayer(position, 8);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateHoleSizes();
        }
    }


}
