using UnityEngine;
using System.Collections;

public class CoveredGem : MonoBehaviour {

    public Transform targetLocation;
    public PlayerInformation myPlayer;
    public float waitUntilMovingToTarget;
    public float approachSpeed;

    public float revealDuration = 2f;

    bool approaching = false;

    bool onDisplay = false;

    float totalDistance;
    Vector3 startPosition;
    float startTime;

    public ParticleSystem dustFX;
    public ParticleSystem starFX;
    public ParticleSystem rockFX;
    public GameObject coveredGem;

    public FMODUnity.StudioEventEmitter gemFound;
    public FMODUnity.StudioEventEmitter coveredGemOnDisplay;
    public FMODUnity.StudioEventEmitter gemRevealed;
    public FMODUnity.StudioEventEmitter gemFlyToUI;

    GameObject created3DGem;

    // Use this for initialization
    void Start () {
        //Invoke("StartApproachingTarget", waitUntilMovingToTarget);
        if (!targetLocation) enabled = false;
        gemFound.Play();
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
        coveredGemOnDisplay.Play();
    }

    void OnMultiDisplayMouseDown(int screenIndex)
    {
        if (onDisplay)
        {
            dustFX.Play();
            starFX.Play();
            rockFX.Play();
            Destroy(coveredGem);
            onDisplay = false;
            coveredGemOnDisplay.Stop();
            gemRevealed.Play();
            //  Common.gameMaster.GemRevealOver(this.transform.position,myPlayer);
            SpawnGem(myPlayer.nextGemToWin);
        }
    }

    void SpawnGem(Gem gem)
    {
        //Common.gameMaster.
        GameObject gemInstance = (GameObject)GameObject.Instantiate(gem.my3DGem, transform.position, Quaternion.identity);
        created3DGem = gemInstance;
        gemInstance.GetComponent<Rigidbody>().AddTorque(Vector3.up * 30);
        Invoke("RevealOver", revealDuration);
        //REMOVE THIS 
      //  Destroy(gemInstance, 5f);
       // Destroy(gameObject, 5f);
    }
    
    void RevealOver()
    {
        Common.gameMaster.GemRevealOver(this.transform.position, myPlayer);
        gemFlyToUI.Play();
        Destroy(gameObject);
        Destroy(created3DGem);
    }

    bool IsBelowScreen()
    {
        return transform.position.y < -10f;
    }
}
