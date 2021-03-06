﻿using UnityEngine;
using System.Collections;

public class DisplayScript : MonoBehaviour
{
    void Awake()
    {

        if (!Common.displayScript)
        {
            DontDestroyOnLoad(transform.gameObject);
            Common.displayScript = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("displays connected: " + Display.displays.Length);
        // Display.displays[0] is the primary, default display and is always ON.
        // Check if additional displays are available and activate each.
        if (Display.displays.Length > 1)
        {
           // Display.displays[1].h

            Display.displays[1].Activate();//800,800,30); //public void Activate(int width, int height, int refreshRate)
        }
        if (Display.displays.Length > 2)
            Display.displays[2].Activate();
        
    }
    // Update is called once per frame
    void Update()
    {

    }


}