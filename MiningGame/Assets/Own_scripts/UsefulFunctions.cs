using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UsefulFunctions : MonoBehaviour {

	// Use this for initialization
    public Vector3 GetMouseWorldPosition()
    {
        //print position of mouse
        float mousex = (Input.mousePosition.x);
        float mousey = (Input.mousePosition.y);
        Vector3 mouseposition = Camera.main.ScreenToWorldPoint(new Vector3(mousex, mousey, 0));
        return mouseposition;
    }

    public Vector3 GetWorldPositionFromPixelInCamera(Camera camera, Vector2 pixelPos)
    {
        return camera.ScreenToWorldPoint(new Vector3(pixelPos.x, pixelPos.y, 0));
    }

    public Vector3 GetMousePixelPosition()
    {
        return Input.mousePosition;
    }
    void Awake()
    {
        Common.usefulFunctions = this;
    }

    public bool RayCastAndCheckIfLayerHit(Vector3 startPosition, int layer)
    {
        Debug.Log("Checking raycast on " + startPosition);
        RaycastHit hit;
        startPosition.z = -2f;
        Ray ray = new Ray(startPosition,new Vector3(0,0,1)); //or whatever you're doing for your ray
        float distance = 123.123f; //however far your ray shoots
        int layerMask = 1 << layer;  // "7" here needing to be replaced by whatever layer it is you're wanting to use
        //layerMask = ~layerMask; //invert the mask so it targets all layers EXCEPT for this one
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Debug.Log("RAYCast hits");
            return true;
            //do stuff 
        }
        Debug.Log("RayCast doesnt hit " + ray);
        return false;
    }

    public void DelayDestroy(GameObject go,float delay)
    {
        StartCoroutine(DelayDestroyRoutine(delay, go));
    }

    IEnumerator DelayDestroyRoutine(float delay,GameObject go)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }



    public GameObject RayCastAlongCameraAndReturnhit(Camera camera,Vector3 position,int layer)
    {
        //Debug.Log("Checking raycast on " + startPosition);
        RaycastHit hit;
        position.z = -2f;
        Vector3 screenPoint = camera.WorldToScreenPoint(position);
        Ray ray = camera.ScreenPointToRay(screenPoint);
        float distance = 123.123f; //however far your ray shoots
        int layerMask = 1 << layer;  // "7" here needing to be replaced by whatever layer it is you're wanting to use
        //layerMask = ~layerMask; //invert the mask so it targets all layers EXCEPT for this one
        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            Debug.Log("RAYCast hits");
            return hit.transform.gameObject;
            //do stuff 
        }
        return null;
        
    }

    public bool RayCast2DCheckLayer(Vector3 startPosition, int layer)
    {
        int layerMask = 1 << layer;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(startPosition.x, startPosition.y), new Vector2(0, 0), 10f, layerMask);
        if (hit) { 
            Debug.Log("Ray hits at pos"+startPosition);
            Debug.Log(hit.collider.gameObject);
            Debug.DrawRay(transform.position, transform.right, Color.red);
            Debug.DrawRay(transform.position, transform.right, Color.red);
            return true;
        }
        return false;

    }

    public void  SetObjectToParent(Transform parentGO, Transform child,Vector3 newLocalPosition)
    {
        child.SetParent(parentGO.transform);
        child.localPosition = newLocalPosition;
    }

    public void SetObjectToParent(Transform parentGO, Transform child)
    {
        child.SetParent(parentGO.transform);
    }

    public bool CheckIfExists(GameObject go)
    {
        if (go == null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3> SortVector3ListaBasedOnx(List<Vector3> unsortedVectors)
    {

            return unsortedVectors.OrderBy(v => v.x).ToList<Vector3>();
        
    }

    public void scaleGOOverTime(GameObject go, Vector3 endScale,float duration)
    {
        StartCoroutine(scaleTo(go.transform, endScale, duration));
    }

    IEnumerator scaleTo(Transform objectToScale, Vector3 endScale, float duration)
    {
        float timeGoer = 0f;
        Vector3 startScale = objectToScale.localScale;
        float startTime = Time.time;
        while (timeGoer < duration)
        {
            if (objectToScale)
            {
                // Debug.Log("<color=red>We are still scaling:</color>");
                timeGoer = Time.time - startTime;
                objectToScale.localScale = Vector3.Lerp(startScale, endScale, timeGoer / duration);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                yield break;
            }
        }

        yield break;
    }

    public List<GameObject> OrderByTransformxPosition(List<GameObject> gos)
    {
        return gos.OrderBy(go => go.transform.position.x).ToList<GameObject>();
    }

    public Vector3 GetMeanPositionFromGameObjects(List<GameObject> GOS)
    {
        List<Vector3> allPos = new List<Vector3>();
        foreach(GameObject go in GOS)
        {
            allPos.Add(go.transform.position);
        }
        return GetMeanVector(allPos);
    }
    string formatting = "N2";

    public string FormatTOtaleAmountTOText(float amount,bool hasEur=true)
    {

        string str = "";
        if (amount > 1000)
        {
            string str1 = amount.ToString();
            str += "€";
            str += str1;
        }
        else
        {
            str += amount.ToString(formatting);
        }
        return str;
    }

    public Vector3 GetMeanVector(List<Vector3> positions)
    {
        if (positions.Count == 0)
            return Vector3.zero;
        float x = 0f;
        float y = 0f;
        float z = 0f;
        foreach (Vector3 pos in positions)
        {
            x += pos.x;
            y += pos.y;
            z += pos.z;
        }
        return new Vector3(x / positions.Count, y / positions.Count, z / positions.Count);
    }


    IEnumerator moveObjectToPlaceRoutine(Transform obj, Vector3 where, float duration)
    {
        float timeGoer = 0f;
        Vector3 startLocation = obj.position;
        float startTime = Time.time;
        while (timeGoer < duration)
        {
            if (obj)
            {
                //Debug.Log("<color=red>We are still scaling:</color>");
                timeGoer =Time.time- startTime;
                obj.position = Vector3.Lerp(startLocation, where, timeGoer / duration);

            }
            else {
                timeGoer = duration + 1;
            }
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    public void MoeObjectToPlaceOverTime(Transform toMove, Vector3 where, float duration)
    {
        StartCoroutine(moveObjectToPlaceRoutine(toMove, where, duration));
    }

    public void MoveObjectToPlaceOverTimeFixedZ(Transform toMove, Vector3 where, float duration, float fixedz)
    {
        where.z = fixedz;
        StartCoroutine(moveObjectToPlaceRoutine(toMove, where, duration));
    }
    public void MoveObjectToPlaceNonFixed(Transform toMove, Vector3 where, float duration)
    {
        StartCoroutine(moveObjectToPlaceRoutine(toMove, where, duration));
    }

    public IEnumerator ScaleSpritesColourOverTime(SpriteRenderer sprite, Color endColor,float duration)
    {
        float timeGoer = 0f;
        Color startColour = sprite.color;
        float startTime = Time.time;
        while (timeGoer < duration)
        {
            if (sprite)
            {
                //Debug.Log("<color=red>We are still scaling:</color>");
                timeGoer = Time.time - startTime;
                sprite.color = Color.Lerp(startColour, endColor, timeGoer / duration);
                

            }
            else {
                timeGoer = duration + 1;
            }
            yield return new WaitForEndOfFrame();
            
        }

        yield break;
    }

    public bool GetRandomBool(float trueChance=0.5f)
    {
        float random = Random.Range(0, 1f);
        if (random < trueChance)
        {
            return true;
        }
        return false;
    }

    public void ShowChildForxSeconds(Transform parent,float seconds)
    {
        StartCoroutine(ShowGameObjectChildForxSeconds(parent, seconds));
    }

    IEnumerator ShowGameObjectChildForxSeconds(Transform parent, float seconds)
    {
        GameObject objectToShow = parent.GetChild(0).gameObject;
        objectToShow.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        objectToShow.SetActive(false);
        yield break;
    }

}
