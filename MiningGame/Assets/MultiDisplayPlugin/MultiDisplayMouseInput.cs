using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Drawing;
using System.Collections.Generic;
using TouchScript;

public class MultiDisplayMouseInput : MonoBehaviour {


    public Camera cameraP1;
    public Camera cameraP2;

    public Canvas canvasP1;
    public Canvas canvasP2;

    public float screenWidth = 1920;
    public float screenHeight = 1080;

    public Text uidebug;
    public Text prevClick;

    public bool hackFromOneScreenInput = true;
    public float clickDelay = 0.05f;

    public bool alwaysHitScreenTwo = false;

    public bool tuioInput = true;
    public TouchManager tuioTouchManager;

    void OnEnable()
    {
        if (TouchManager.Instance != null)
            TouchManager.Instance.TouchesBegan += touchesBeganHandler;
    }

    void OnDisable()
    {
        if(TouchManager.Instance != null)
            TouchManager.Instance.TouchesBegan -= touchesBeganHandler;
    }

    private void touchesBeganHandler(object sender, TouchEventArgs e)
    {
        if (!tuioInput || inputBlocked) return;
        var count = e.Touches.Count;
        for(var i = 0; i < count; i++)
        {
            var touch = e.Touches[i];
            ApplyTuioTouch(touch);
        }
    }

    private void ApplyTuioTouch(TouchPoint tuioTouchPoint)
    {
        int screenNumber = ConvertTouchPointToScreenIndex(tuioTouchPoint);
        Vector3 relativePos = GetTouchPositionRelativeToScreen(tuioTouchPoint);
        HandleClickEvent(screenNumber, relativePos);
    }

    void HandleClickEvent(int screenNumber, Vector3 relativePosition)
    {
        Vector3 relativePos = relativePosition;
        if (Application.isEditor && !tuioInput)
        {
            relativePos = Input.mousePosition;
        }
        if (screenNumber == 1)
        {
            RayCastFromCamera(relativePos, cameraP1, canvasP1, screenNumber);
        }
        else
        {
            RayCastFromCamera(relativePos, cameraP2, canvasP2, screenNumber);
        }
        if (prevClick)
            prevClick.text = "Screen: " + screenNumber + ", at: " + relativePos;
    }

    bool inputBlocked;
    public float blockInputStart = 1f;

    void Start()
    {
        if (Application.isEditor && !tuioInput)
        {
            clickDelay = 0;
            hackFromOneScreenInput = false;
        }
        
        if (hackFromOneScreenInput)
        {
            screenWidth /= 2f;
        }

        inputBlocked = true;
        Invoke("AllowInput", blockInputStart);
    }

    void AllowInput()
    {
        inputBlocked = false;
    }

	void Update () {
        Vector2 systemMousePosition = SystemMousePosition();
        if (!tuioInput)
        {
            if (Input.GetMouseButtonDown(0))
            {
                alwaysHitScreenTwo = false;
                if (clickDelay > 0)
                {
                    DelayedClick(clickDelay);
                }
                else
                {
                    ApplyClick();
                }

            }
            if (Input.GetMouseButtonDown(1))
            {
                alwaysHitScreenTwo = true;
                if (clickDelay > 0)
                {
                    DelayedClick(clickDelay);
                }
                else
                {
                    ApplyClick();
                }

            }
        }
        if (uidebug)
            uidebug.text = systemMousePosition + "";
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            clickDelay += 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            clickDelay -= 0.05f;
            if(clickDelay <= 0.01f)
            {
                clickDelay = 0.05f;
            }
        }
        if (Input.GetKeyDown(KeyCode.F8))
        {
            //alwaysHitScreenTwo = !alwaysHitScreenTwo;
        }
    }

    void DelayedClick(float delay)
    {
        Invoke("ApplyClick", delay);
    }

    void ApplyClick()
    {
        int screenNumber = GetCurrentMousePositionScreenIndex();
        Vector3 relativePos = GetMousePositionRelativeToScreen();
        HandleClickEvent(screenNumber, relativePos);
    }

    void RayCastFromCamera(Vector3 relativeMousePosition, Camera cam, Canvas can, int screenIndex)
    {
        Debug.Log("Raycasting to: " + relativeMousePosition +"on camera: "+cam.gameObject.name + " screenindex: "+screenIndex);


        //RAYCAST UI FIRST
        bool uiWasHit = false;
        GraphicRaycaster gr = can.GetComponent<GraphicRaycaster>();
        PointerEventData ped = new PointerEventData(null);
        ped.position = relativeMousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(ped, results);
        foreach (RaycastResult r in results)
        {
            Debug.Log("UI ray hit: " + r.gameObject.name);
            uiWasHit = true;
            r.gameObject.SendMessage("OnMultiDisplayMouseDown", screenIndex, SendMessageOptions.DontRequireReceiver);
        }
        if (uiWasHit) return;
        //RAYCAST PHYSICS
        Ray ray = cam.ScreenPointToRay(relativeMousePosition);
        RaycastHit hit;
        Debug.Log("CAST: " + Physics.Raycast(ray, out hit));
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Physics ray hit: "+hit.collider.gameObject.name);
            hit.collider.SendMessage("OnMultiDisplayMouseDown", screenIndex, SendMessageOptions.DontRequireReceiver);
        }

    }

    private int GetCurrentMousePositionScreenIndex()
    {
        return ConvertMousePointToScreenIndex(System.Windows.Forms.Cursor.Position);
    }

    private int ConvertMousePointToScreenIndex(Point mousePoint)
    {
        if (alwaysHitScreenTwo) return 2;
        if (mousePoint.X < screenWidth)
        {
            return 1;
        }
        else return 2;
    }

    private int ConvertTouchPointToScreenIndex(TouchPoint touchPoint)
    {
        if (alwaysHitScreenTwo) return 2;
        if (touchPoint.Position.x < screenWidth)
        {
            return 1;
        }
        else return 2;
    }

    private Vector3 GetMousePositionRelativeToScreen()
    {
        Vector2 sysPos = SystemMousePosition();
        if (sysPos.x > screenWidth) sysPos.x -= screenWidth;
        if (hackFromOneScreenInput)
        {
            sysPos.x *= 2;
        }
        return new Vector3(sysPos.x, screenHeight - sysPos.y, 0);
    }
    private Vector3 GetTouchPositionRelativeToScreen(TouchPoint tuioTouchPoint)
    {
        Vector2 sysPos = new Vector2(tuioTouchPoint.Position.x, tuioTouchPoint.Position.y);
        if (sysPos.x > screenWidth) sysPos.x -= screenWidth;
        if (hackFromOneScreenInput)
        {
            sysPos.x *= 2;
        }
        return new Vector3(sysPos.x, sysPos.y, 0);
    }

    private Vector2 SystemMousePosition()
    {
        return new Vector2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
    }
}
