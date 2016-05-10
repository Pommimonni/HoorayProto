using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CurtainDrop : MonoBehaviour {

	// Use this for initialization
	void Start () {
      //  gemLocation = this.transform.parent.parent.GetComponent<InfoGUI>().gemImagesOnWonGems[0].GetComponent<RectTransform>().position.y;
        myRectTransform = this.GetComponent<RectTransform>();
        upperGoal = myRectTransform.position.y;
	}
    float gemLocation= 14.36215f;
    RectTransform myRectTransform;
    public bool moving = false;
    bool doneGemChange = false;
    public float moveDirection = 1f;
    public float increment_speed = 0.1f;
    public float lowerGoal = 0f;
    public float upperGoal = 0f;
    public float currenty = 0f;
    public float reachedGoal = 0f;
    // Update is called once per frame
    void Update() {
        Vector3 rtp = myRectTransform.position;
        rtp.y += increment_speed * moveDirection;
       
        float yPos = rtp.y;
        if (moving)
        {


            if (!doneGemChange && false)  //Change back if wanted to change games from 3D=>2D
            {
                if (moveDirection < 0)
                {
                    if (yPos < gemLocation)
                    {
                        DoTransformation(true);
                    }
                }
                else
                {
                    if (yPos > gemLocation)
                    {
                        DoTransformation(false);
                    }
                }
            }
            if (moveDirection < 0)
            {
                if (yPos < lowerGoal)
                {
                    moving = false;
                    reachedGoal = lowerGoal;
                    Debug.Log(rtp);
                    rtp.y = reachedGoal;
                }
            }
            else {
                if (yPos > upperGoal)
                {
                    moving = false;
                    reachedGoal = upperGoal;
                    rtp.y = reachedGoal;
                    Debug.Log(rtp);
                }
            }

            myRectTransform.position = rtp;
        }
        else
        {
            //Vector3 pos=myRectTransform.position.position;
            //myRectTransform.position = rtp;
        }
        currenty = myRectTransform.position.y;

    }

    public void Startmoving(float direction)
    {
        this.moveDirection = direction;
        moving = true;
    }

    void DoTransformation(bool to2D)
    {
        doneGemChange = true;
        if (to2D)
        {
            this.transform.parent.parent.GetComponent<InfoGUI>().TransformTo2D();
        }
        else
        {
            this.transform.parent.parent.GetComponent<InfoGUI>().TransformTo3D();
        }

    }
}
