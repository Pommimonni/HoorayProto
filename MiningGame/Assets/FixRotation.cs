using UnityEngine;
using System.Collections;

public class FixRotation : MonoBehaviour {

    Quaternion rotation;
    Vector3 position;
    Transform toFollow;
    void Awake()
    {
        rotation = transform.rotation;
        position=this.transform.position;
        toFollow = this.transform.parent;
       // this.transform.parent = null;
    }
    void Update()
    {

        
        transform.rotation=rotation;
      //  Vector3 currPos = this.transform.position;
       // currPos.x = toFollow.position.x;
        //currPos.y = toFollow.position.y;
        
        //this.transform.localPosition = position;
        //this.transform.localPosition = position;
        
    }
}
