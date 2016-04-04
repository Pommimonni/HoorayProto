using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToogleGroupHandler : MonoBehaviour {
    public GameObject toInformAboutChange;
    public string whatFunctionToCallOnChange="GemChooseToggleChanged";
    public int whatChildIsToggledInt = -1;
    public List<Sprite> picturesToChoose;
    public bool wantToAddPicturesAtStartUp = true;
	// Use this for initialization
	void Start () {
        GetMyAllActiveChildren();
        if (wantToAddPicturesAtStartUp)
        {
            SetspritesToChoices(picturesToChoose);
        }
        SetToggleGroupFunctionToMyChildren();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SetspritesToChoices(Common.gemSkins.GiveGemSkinsAsSprites());
        }
	}
    public List<GameObject> myActiveChildren;
    void GetMyAllActiveChildren()
    {
        myActiveChildren = new List<GameObject>();
        foreach(Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                myActiveChildren.Add(child.gameObject);
            }
        }
        
        
    }

    void SetToggleGroupFunctionToMyChildren()
    {
        foreach(GameObject child in myActiveChildren)
        {
            Toggle childToggle=child.GetComponent<Toggle>();
            childToggle.onValueChanged.AddListener(ChildBoolToggle);
        }
    }

    public void SetspritesToChoices(List<Sprite> sprites)
    {
        int counter = 0;
        foreach(Sprite sprite in sprites)
        {
            Transform child=this.transform.GetChild(counter);
            child = child.GetChild(0);
            child.GetComponent<Image>().sprite = sprite;
            child.gameObject.SetActive(true);
            counter++;
            if (counter > this.transform.childCount)
            {
                Debug.LogError("Please add more children to ToogleGroup handler in gameobject "+this.gameObject.name+" Or change amount of pictures to add ");
            }
        }
        for(int n=counter; n < this.transform.childCount; n++)
        {
            this.transform.GetChild(n).gameObject.SetActive(false);
        }
        GetMyAllActiveChildren();
    }

    void ChildToggleChanged(int newChildToggled)
    {
        whatChildIsToggledInt = newChildToggled;
        toInformAboutChange.SendMessage(whatFunctionToCallOnChange, newChildToggled);

    }

    public void ChildBoolToggle(bool onOff)
    {
        if (onOff == true)
        {
            int counter = 0;
            foreach(GameObject child in myActiveChildren)
            {

                if (child.GetComponent<Toggle>().isOn)
                {
                    ChildToggleChanged(counter);
                    return;
                }
                    counter++;
            }
        }
    }
}
