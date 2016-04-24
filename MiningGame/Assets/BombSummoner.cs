using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public enum TypeOfTrajectory : int { straight,parabolicGravity,parabolicCalculated, linearIncrease, exponent };

[System.Serializable]
public enum SpawnLocationType : int { spawn_on_sides,spawn_around_circle,spawn_around_constricted_circle};


public class BombSummoner : MonoBehaviour
{

    void Awake()
    {
        Common.bonusBombSummoner= this;
    }


    // Use this for initialization
    void Start()
    {
        Vector3 p1 = new Vector3(1, 1, 1);
        Vector3 p2 = new Vector3(-1, 0, 0);
        Vector3 p3 = new Vector3(-2f, -2f, -1f);
        Vector3 eq=CalculateParabelEquation(p1, p2, p3);
        Debug.Log(eq);
        initialize();
        lastSpawn = Time.time;
    }

    void initialize()
    {
        Random.seed = System.DateTime.Now.Millisecond;
        mapleftBottom = Common.mapleftBottom;
        mapRightTop = Common.mapRightTop;
        mapMIddle = Common.mapMIddle;
        
    }

        // Update is called once per frame
        void Update()
    {

        if (lastSpawn == 0f || lastSpawn + spawnHz < Time.time)
        {

            if (summoning)
            {
                if (IsThereTimeLeft())
                {
                    lastSpawn = Time.time;

                    adjustHz();
                  //  Debug.Log("Spawning bomb"+lastSpawn);
                    Vector3 spawnLoc = getSpawnLocation();
                    float random = Random.Range(0f, 1f);
                    if (random > bigBombChance)
                    {
                        SpawnBomb(spawnLoc);
                    }
                    else {
                        SpawnBigOne(spawnLoc);
                    }
                    
                    objectsSpawned++;
                }
                else
                {
                    Common.gameMaster.BonusRoundEndsShowResults();
                    summoning = false;
                }

            }
        }
    }

    bool IsThereTimeLeft()
    {
        if (objectsSpawned > maxAmountOfObjectsToSpawn)
        {
            return false;
        }
        return true;
    }

    void adjustHz()
    {
        float objectSuhde = objectsSpawned/maxAmountOfObjectsToSpawn;
        spawnHz = Mathf.Lerp(startSpawnHz, endSpawnHz, objectSuhde);
        //		Debug.Log ("current spawnHz"+spawnHz + " end spawnhz on " + endSpawnHz + " masterin ajat ovat " + master.timePlayed + " ja " + master.timeLimit + " ja suhde on " + timeSuhde);
    }

    void SpawnBomb(Vector3 spawnLoc)
    {
        GameObject enemySpawned = (GameObject)Instantiate(normalPrefab, spawnLoc, Quaternion.identity);
        HandleBombInitialization(enemySpawned,false);
        forceAdding(enemySpawned);
    }

    void SpawnBigOne(Vector3 spawnLoc)
    {
        GameObject enemySpawned = (GameObject)Instantiate(bigOnePrefab, spawnLoc, Quaternion.identity);
        HandleBombInitialization(enemySpawned,true);
        forceAdding(enemySpawned);
    }
    
    void HandleBombInitialization(GameObject created,bool isBig)
    {
        BonusRoundBomb bomb= created.transform.GetComponentInChildren<BonusRoundBomb>();
        //Determining if it is a big one
        bool whoStarts = Common.usefulFunctions.GetRandomBool();
        bomb.Initialize(2, whoStarts,isBig);
    }



    Vector3 mapleftBottom;
    Vector3 mapRightTop;
    Vector3 mapMIddle = Vector3.zero;


    public GameObject normalPrefab;
    public GameObject bigOnePrefab;

    public float radius;
    public float spawnHz = 3f;
    public float endSpawnHz = 1f;
    public float startSpawnHz = 1f;
    public float yIncrement = 0f;
    public TypeOfTrajectory typeOfTrajectory;
    public SpawnLocationType spawnLocationType;
    int objectsSpawned = 0;
    public int maxAmountOfObjectsToSpawn;

    public Vector3 torgue;
    public float aimForceMagnitude = 100f;
    float lastSpawn = 0f;

    public float bigBombChance = 0.1f;

    bool summoning = false;

    public void StartBonusRound()
    {
        objectsSpawned = 0;
        summoning = true;
        
    }



   

    public Vector3 randomInsideCircle(float radius, Vector3 center)
    {
        Vector2 newPosition = Random.insideUnitCircle * radius;
        return new Vector3(newPosition.x + center.x, center.y, newPosition.y + center.z);
    }

    public Vector3 randomAroundCircle(float radius, Vector3 center)
    {
        Vector2 cirleLoc = circle(Random.Range(0, 360), radius);
        center.x += cirleLoc.x;
        center.y += cirleLoc.y;
        return center;
    }

    public Vector2 circle(float angle, float radius)
    {
        angle = Mathf.Deg2Rad * angle;
        float z = Mathf.Sin(angle) * radius;
        float x = Mathf.Cos(angle) * radius;
        return new Vector2(x, z);
    }

    public Vector3 getSpawnLocation()
    { 

        if(spawnLocationType==SpawnLocationType.spawn_around_circle)
            return randomAroundCircle(radius, mapMIddle);
        if (spawnLocationType == SpawnLocationType.spawn_on_sides)
        {
            float rand=Random.Range(0f, 1f);
            float sign = 1f;
            if (rand > 0.5f)
            {
                sign = -1f;
            }
         //   Debug.Log("sign is " + sign.ToString());
            return SpawnSide(sign);

        }
        return Vector3.zero;

    }

    Vector3 CalculateParabelEquation(Vector3 p1,Vector3 p2,Vector3 p3)
    {
        float A1 = -p1.x * p1.x+ p2.x * p2.x;
        float B1 = -p1.x + p2.x;
        float D1 = -p1.y + p2.y;
        float A2 = -p2.x * p2.x + p3.x * p3.x;
        float B2 = -p2.x + p3.x;
        float D2 = -p2.y + p3.y;
        float Bmult = -(B2 / B1);
        float A3 = Bmult * A1 + A2;
        float D3 = Bmult * D1 + D2;
        float a = D3 / A3;
        float b = (D1 - A1 * a) / B1;
        float c = p1.y - a * p1.x * p1.x - b * p1.x;
        return new Vector3(a, b, c);
    }

    Vector3 SpawnSide(float sign)
    {
        float xPos = 0f;
        if (sign > 0)
        {
            xPos = mapRightTop.x;
        }
        else
        {
            xPos = mapleftBottom.x;
        }
        float yPos = Random.Range(mapleftBottom.y, mapRightTop.y);
        Debug.Log("New x in spawn is " + xPos);
        return new Vector3(xPos, yPos, mapleftBottom.z);
    }

    
    Vector3 GetRandomTarget(Vector3 spawnLoc)
    {
        float sign = -1f;
        Debug.Log("getting target " + spawnLoc);
        Debug.Log("Map middle is " + mapMIddle);
        if (spawnLoc.x < mapMIddle.x)
        {
            sign = 1f;
        }
        Debug.Log("sing is " + sign);
        Vector3 targetLoc = SpawnSide(sign);
        return targetLoc;
    }

    public float flyDuration = 0f;
    public float linearIncreaseMagnity = 5f;
    public Vector3 targetTest;
    public Vector3 normalizedTest;
    void forceAdding(GameObject spawnedObject)
    {
        Vector3 force = Vector3.zero;
        Rigidbody spawnedRigid = spawnedObject.GetComponent<Rigidbody>();
        Vector3 startPos = spawnedObject.transform.position;
        Vector3 target = GetRandomTarget(spawnedObject.transform.position);
        targetTest = target;
        if (TypeOfTrajectory.linearIncrease == typeOfTrajectory)
        {
            spawnedRigid.useGravity = false;

            Vector3 normalizedVector = (target - spawnedObject.transform.position).normalized;
            //   force = normalizedVector * aimForceMagnitude;
            // spawnedRigid.AddForce(force);
            spawnedRigid.velocity = CalculateVelocityBasedOnLinearSpeed(flyDuration, normalizedVector);
            StartCoroutine(lerpNumerator(spawnedRigid, new Vector3(0, linearIncreaseMagnity, 0), flyDuration));
        }

        if (typeOfTrajectory == TypeOfTrajectory.parabolicGravity)
        {
            spawnedRigid.useGravity = true;
            Vector3 throwSpeed = calculateBestThrowSpeed(spawnedRigid.position,target, flyDuration);
            spawnedRigid.velocity = throwSpeed;
        }
        if (typeOfTrajectory == TypeOfTrajectory.straight)
        {
            spawnedRigid.useGravity = false;

            Vector3 normalizedVector = (target - spawnedObject.transform.position).normalized;
            normalizedTest = normalizedVector;
            spawnedRigid.velocity = CalculateVelocityBasedOnLinearSpeed(flyDuration, normalizedVector);
           /// force = normalizedVector * aimForceMagnitude;
           // spawnedRigid.AddForce(force);
           // spawnedObject.GetComponent<Rigidbody>().AddForce(force);
        }
        if (typeOfTrajectory == TypeOfTrajectory.parabolicCalculated)
        {
            spawnedRigid.useGravity = false;

            Vector3 normalizedVector = (target - spawnedObject.transform.position).normalized;
            spawnedRigid.velocity = CalculateVelocityBasedOnLinearSpeed(flyDuration, normalizedVector);
            Vector3[] points= new Vector3[] {startPos ,mapMIddle,target };
            StartCoroutine(numericParabel(spawnedObject, points, flyDuration));
            /// force = normalizedVector * aimForceMagnitude;
            // spawnedRigid.AddForce(force);
            // spawnedObject.GetComponent<Rigidbody>().AddForce(force);
        }
        /*
        float xLenght = loc.x - spawnedObject.transform.position.x;
        float yStart = xLenght * yIncrement / 2;
        spawnedObject.GetComponent<Rigidbody>().velocity = new Vector3(force.x, yStart, 0);
        spawnedObject.GetComponent<IncreaseSpeedOverTime>().yStartSpeed = yStart;
        spawnedObject.GetComponent<IncreaseSpeedOverTime>().increment = yIncrement;
        */

        // spawnedObject.GetComponent<Rigidbody>().AddRelativeForce(force);
        //  spawnedObject.GetComponent<Rigidbody>().AddTorque(torgue);
        //   spawnedObject.GetComponent<Rigidbody>().AddForce(force);

    }

    void RandomTarget()
    {

    }
    private Vector3 CalculateVelocityBasedOnLinearSpeed(float duration,Vector3 speedToScale)
    {
        float width = mapRightTop.x - mapleftBottom.x;
        float newSpeed = width / duration;
        float suhde = Mathf.Abs(newSpeed / speedToScale.x);
        return suhde * speedToScale;
    }

    private Vector3 calculateBestThrowSpeed(Vector3 origin, Vector3 target, float timeToTarget)
    {
        // calculate vectors
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = toTarget;
        toTargetXZ.y = 0;

        // calculate xz and y
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        // calculate starting speeds for xz and y. Physics forumulase deltaX = v0 * t + 1/2 * a * t * t
        // where a is "-gravity" but only on the y plane, and a is 0 in xz plane.
        // so xz = v0xz * t => v0xz = xz / t
        // and y = v0y * t - 1/2 * gravity * t * t => v0y * t = y + 1/2 * gravity * t * t => v0y = y / t + 1/2 * gravity * t
        float t = timeToTarget;
        float v0y = y / t + 0.5f * Physics.gravity.magnitude * t;
        float v0xz = xz / t;

        // create result vector for calculated starting speeds
        Vector3 result = toTargetXZ.normalized;        // get direction of xz but with magnitude 1
        result *= v0xz;                                // set magnitude of xz to v0xz (starting speed in xz plane)
        result.y = v0y;                                // set y to v0y (starting speed of y plane)

        return result;
    }

    IEnumerator numericParabel(GameObject toControl,Vector3[] positions,float duration)
    {
        Vector3 parabParams = CalculateParabelEquation(positions[0], positions[1], positions[2]);
        Debug.Log("parab params are " + parabParams);
        yield return new WaitForFixedUpdate();
        float hz = Time.fixedDeltaTime;
        float runningTime = 0;
        int changePer = 5;
        int changeCounter = 0;
        while (runningTime / duration < 1)
        {
            yield return new WaitForSeconds(hz);
            runningTime += hz;
            float newY = ParabolicMovement(toControl.transform.position.x,parabParams);
            toControl.transform.position = new Vector3(toControl.transform.position.x, newY, toControl.transform.position.z);
                changeCounter = 0;
                //Watching to future
                Vector3 futurePoint = toControl.transform.position;
                futurePoint.x = futurePoint.x + toControl.GetComponent<Rigidbody>().velocity.x * hz;
                float futureY = ParabolicMovement(futurePoint.x, parabParams);
                futurePoint.y = futureY;
                toControl.transform.LookAt(futurePoint);

        }

    }

 

    float ParabolicMovement(float x,Vector3 parabParams)
    {
        float a = parabParams.x;
        float b = parabParams.y;
        float c = parabParams.z;
        float newYPosition = a * x * x + b * x + c;
        return newYPosition;
    }


    IEnumerator lerpNumerator(Rigidbody toChange,Vector3 endVelocity, float duration)
    {
        //startVelocity = this.rigidbody.velocity;
        yield return new WaitForFixedUpdate();
        Vector3 startVelocity = toChange.velocity;
        float hz = Time.fixedDeltaTime;
        float runningTime = 0;
        while (runningTime / duration < 1)
        {
            yield return new WaitForSeconds(hz);
            runningTime += hz;
            Vector3 newVec = Vector3.Lerp(startVelocity, endVelocity, (runningTime / duration));
            if (toChange)
            {
                toChange.velocity = new Vector3(toChange.velocity.x, newVec.y, toChange.velocity.z);
            }
        }



    }
}