using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MultiDisplayToggle : MonoBehaviour {



    Button b;
    public int order = 0;
    public Text textToColourChange;
    
    // Use this for initialization
    void Start()
    {
        b = GetComponent<Button>();
        int money = Mathf.FloorToInt(RoundSettings.player1Money);
        if (money < order + 1)
        {
            MakeNotAllowedToPressMe();
        }
    }

    void Enable()
    {

    }

    void MakeNotAllowedToPressMe()
    {
        allowedToHit = false;
        Color def = textToColourChange.transform.GetComponent<ChangeColorBasedOnToggle>().falseColor;
        def.a = 0.5f;
        textToColourChange.color = def;// new Color(textToColourChange.color.r, textToColourChange.color.g, textToColourChange.color.b, 0.5f);
        Color ic = GetComponent<Image>().color;
        ic.a = 0.5f;
        this.GetComponent<Image>().color = ic; //new Color(textToColourChange.color.r, textToColourChange.color.g, textToColourChange.color.b, 0.5f);
        this.textToColourChange.transform.GetComponent<ChangeColorBasedOnToggle>().enabled = false;
    }
    bool allowedToHit = true;
    // Update is called once per frame
    void Update()
    {

    }

    void OnMultiDisplayMouseDown()
    {
        if (allowedToHit)
        {
            Debug.Log("MultiDisplayButton pressed");
            // PopUp pu = new PopUp(this.gameObject, Vector2.zero, this.gameObject.name, 5f, Vector2.up, 60f);//60f);
            //pu.FontSize = 45;
            b.onClick.Invoke();
            //b.transform.position += Vector3.up * 5;
            //Toggle toggle = myActiveChildren[toggled].GetComponent<Toggle>();
            //toggle.isOn = true;
            //b.isOn = true;
            //b.onValueChanged.Invoke(true);
            //b.transform.position += Vector3.up * 5;
            // Destroy(this.gameObject);
        }
    }


}
