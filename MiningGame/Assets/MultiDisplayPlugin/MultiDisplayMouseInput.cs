using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Drawing;
using System.Collections.Generic;

public class MultiDisplayMouseInput : MonoBehaviour {


    public Camera cameraP1;
    public Camera cameraP2;

    public Canvas canvasP1;
    public Canvas canvasP2;

    public float screenWidth = 1920;
    public float screenHeight = 1080;

    public Text uidebug;
    public Text prevClick;
    
	void Update () {
        Vector2 systemMousePosition = SystemMousePosition();
        if (Input.GetMouseButtonDown(0))
        {
            int screenNumber = GetCurrentMousePositionScreenIndex();
            Vector3 relativePos = GetMousePositionRelativeToScreen();
            if (screenNumber == 1)
            {
                RayCastFromCamera(relativePos, cameraP1, canvasP1);
            } else
            {
                RayCastFromCamera(relativePos, cameraP2, canvasP2);
            }
            if(prevClick)
                prevClick.text = "Screen: " + screenNumber + ", at: " + relativePos;
        }
        if (uidebug)
            uidebug.text = systemMousePosition + "";
    }

    void RayCastFromCamera(Vector3 relativeMousePosition, Camera cam, Canvas can)
    {
        Debug.Log("Raycasting to: " + relativeMousePosition +"on camera: "+cam.gameObject.name);
        Ray ray = cam.ScreenPointToRay(relativeMousePosition);
        RaycastHit hit;
        Debug.Log("CAST: " + Physics.Raycast(ray, out hit));
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Physics ray hit: "+hit.collider.gameObject.name);
            hit.collider.SendMessage("OnMultiDisplayMouseDown", SendMessageOptions.DontRequireReceiver);
        }

        //Code to be place in a MonoBehaviour with a GraphicRaycaster component
        GraphicRaycaster gr = can.GetComponent<GraphicRaycaster>();
        //Create the PointerEventData with null for the EventSystem
        PointerEventData ped = new PointerEventData(null);
        //Set required parameters, in this case, mouse position
        ped.position = relativeMousePosition;
        //Create list to receive all results
        List<RaycastResult> results = new List<RaycastResult>();
        //Raycast it
        gr.Raycast(ped, results);
        foreach(RaycastResult r in results)
        {
            Debug.Log("UI ray hit: " + r.gameObject.name);
            r.gameObject.SendMessage("OnMultiDisplayMouseDown", SendMessageOptions.DontRequireReceiver);
        }
    }

    private int GetCurrentMousePositionScreenIndex()
    {
        return ConvertMousePointToScreenIndex(System.Windows.Forms.Cursor.Position);
    }

    private int ConvertMousePointToScreenIndex(Point mousePoint)
    {
        if (mousePoint.X < screenWidth)
        {
            return 1;
        }
        else return 2;
    }

    private Vector3 GetMousePositionRelativeToScreen()
    {
        Vector2 sysPos = SystemMousePosition();
        if (sysPos.x > screenWidth) sysPos.x -= screenWidth;
        return new Vector3(sysPos.x, screenHeight - sysPos.y, 0);
    }

    private Vector2 SystemMousePosition()
    {
        return new Vector2(System.Windows.Forms.Cursor.Position.X, System.Windows.Forms.Cursor.Position.Y);
    }
}
