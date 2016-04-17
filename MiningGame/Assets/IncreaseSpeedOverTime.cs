using UnityEngine;
using System.Collections;

public class IncreaseSpeedOverTime : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public float increment = 0f;
    public float yStartSpeed = 0f;





    // Update is called once per frame
    /*
	void Update () {

        float toAdd =yStartSpeed+this.transform.position.x*increment ;
        //    this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, -toAdd*increment, 0));
        Vector3 vel = this.gameObject.GetComponent<Rigidbody>().velocity;//new Vector3(0, -toAdd*increment, 0);
          vel.y = -toAdd * increment;
        this.gameObject.GetComponent<Rigidbody>().velocity = vel;
	}
    */
    public TypeOfTrajectory movementType;
    public Vector3 initialVelocityVector =new Vector3(100f,0f,0f);
    public Vector3 gravityVector=new Vector3(0f,-9.8f,0f);
    bool movementStarted = false;
    void Update()
    {
        if (movementStarted)
        {
            float newY = 0f;
            if (movementType == TypeOfTrajectory.parabolicGravity)
            {
                newY = ParabolicMovement();
            }
            this.transform.position = new Vector3(this.transform.position.x, newY, this.transform.position.z);
            //transform.position += (initialVelocityVector + 0.5f * gravityVector * Time.deltaTime) * Time.deltaTime;
        }
    }

    float a;
    float b;
    float c;

    public void StartMovement(Vector3 parameters)
    {
        this.parabParams = parameters;
        movementStarted = true;
    }

    float ParabolicMovement()
    {
        float a = parabParams.x;
        float b = parabParams.y;
        float c = parabParams.z;
        Vector3 position=this.gameObject.transform.position;
        float x = position.x;
        float newYPosition = a * x * x + b * x + c;
        return newYPosition;
    }

    private Vector3 parabParams;



}
