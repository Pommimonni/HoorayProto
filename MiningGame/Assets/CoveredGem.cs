using UnityEngine;
using System.Collections;

public class CoveredGem : MonoBehaviour {

    public Transform targetLocation;
    public float waitUntilMovingToTarget;
    public float approachSpeed;

    bool approaching = false;

    bool onDisplay = false;

    float totalDistance;
    Vector3 startPosition;
    float startTime;

    public ParticleSystem dustFX;
    public ParticleSystem starFX;
    public ParticleSystem rockFX;
    public GameObject coveredGem;

    // Use this for initialization
    void Start () {
        //Invoke("StartApproachingTarget", waitUntilMovingToTarget);
        if (!targetLocation) enabled = false;
	}

    bool droppingDown = true;
	// Update is called once per frame
	void Update () {
        if (IsBelowScreen() && droppingDown)
        {
            StartApproachingTarget();
        }
        if (approaching)
        {
            float distCovered = (Time.time - startTime) * approachSpeed;
            float fracJourney = distCovered / totalDistance;
            //Debug.Log("fracJourney: " + fracJourney);
            transform.position = Vector3.Lerp(startPosition, targetLocation.position, fracJourney);
            if (fracJourney >= 1f)
            {
                EndInterpolation();
            }
        }
    }

    void StartApproachingTarget()
    {
        approaching = true;
        droppingDown = false;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        totalDistance = Vector3.Distance(transform.position, targetLocation.position);
        startPosition = transform.position;
        startTime = Time.time;
    }

    void EndInterpolation()
    {
        approaching = false;
        onDisplay = true;
    }

    void OnMouseDown()
    {
        if (onDisplay)
        {
            dustFX.Play();
            starFX.Play();
            rockFX.Play();
            Destroy(coveredGem);
            onDisplay = false;
            SpawnGem(Common.gemSkins.getGemSkin(0));
        }
    }

    void SpawnGem(Gem gem)
    {
        GameObject gemInstance = (GameObject)GameObject.Instantiate(gem.my3DGem, transform.position, Quaternion.identity);
        gemInstance.GetComponent<Rigidbody>().AddTorque(Vector3.up * 30);
        //REMOVE THIS 
        Destroy(gemInstance, 5f);
        Destroy(gameObject, 5f);
    }

    bool IsBelowScreen()
    {
        return transform.position.y < -10f;
    }
}
