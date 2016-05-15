using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PopUpManager : MonoBehaviour {

    public GameObject popUpPrefab;

    static PopUpManager m_PopUpManager;

    // Holds all the pop up text clones of a game object as its children
    GameObject placeholder;

    public static PopUpManager Instance {
        get { return m_PopUpManager; }
    }

    void Awake() {
        m_PopUpManager = this;
    }
    
    #region ---------MAIN METHODS---------

    public void Pop(PopUp pu, bool fade) {

        if (!pu.Target.transform.Find("PopUpPlaceholder")) {

            placeholder = new GameObject("PopUpPlaceholder");
            placeholder.transform.SetParent(pu.Target.transform, false);

        }

        //Debug.Log("target: " + pu.Target.transform);

        GameObject popGO = Instantiate(popUpPrefab) as GameObject;
        GameObject placeholdero = pu.Target.transform.Find("PopUpPlaceholder").gameObject;
        popGO.transform.SetParent(placeholdero.transform, false);

        RectTransform rt_popGO = popGO.GetComponent<RectTransform>();
        rt_popGO.anchoredPosition = pu.Offsets;

        Text popText = popGO.GetComponentInChildren<Text>();
        popText.text = pu.Text;

        // Font
        if (pu.Font != null) {

            popText.font = pu.Font;

        }

        // FontSize
        popText.fontSize = pu.FontSize;

        // FillColor
        popText.color = pu.FillColor;

        // OutlineColor
        Outline ol = popText.gameObject.GetComponent<Outline>();
        ol.effectColor = pu.OutlineColor;

        if (!fade) {

            StartCoroutine(Pop_Constant(popText, pu.Duration, pu.Direction, pu.Speed));

        } else {

            StartCoroutine(Pop_Fade(popText, pu.Duration, pu.Direction, pu.Speed));
        
        }
    }

    #endregion END MAIN METHODS

    #region ------------COROUTINES--------------

    IEnumerator Pop_Constant(Text popText, float duration, Vector2 direction, float speed) {

        RectTransform rt = popText.gameObject.GetComponent<RectTransform>();

        Vector2 pos = rt.anchoredPosition;
        float t = 0;

        while (t * Time.fixedDeltaTime < duration && popText != null) {

            // Move according to speed
            float x = 0;
            float y = 0;
            x += speed * Time.deltaTime * direction.x;
            y += speed * Time.deltaTime * direction.y;
            rt.anchoredPosition += new Vector2(x, y);

            t++;

            //  Pop up ends
            if (t * Time.fixedDeltaTime > duration) {

                Destroy(popText.transform.parent.gameObject);

            }

            yield return null;

        }

        yield return new WaitForFixedUpdate();

    }

    IEnumerator Pop_Fade(Text popText, float duration, Vector2 direction, float speed) {

        RectTransform rt = popText.gameObject.GetComponent<RectTransform>();

        Vector2 pos = rt.anchoredPosition;
        float t = 0;

        while ((popText.color.a > 0 || t * Time.fixedDeltaTime < duration) && popText != null) {

            // Move according to speed
            float x = 0;
            float y = 0;
            x += speed * Time.deltaTime * direction.x;
            y += speed * Time.deltaTime * direction.y;
            rt.anchoredPosition += new Vector2(x, y);

            // r, g, b, a color components of this text scaled from 0 to 1
            float r = popText.color.r;
            float b = popText.color.b;
            float g = popText.color.g;
            float a = popText.color.a;

            // fadeRate is the gradient or rate of decrease in alpha value per unit time
            float fadeRate = 0;
            fadeRate = -1 / duration;

            // Decrease the alpha value
            a = fadeRate * (t * Time.fixedDeltaTime) + 1;
            t++;

            // Update color of pop up text
            popText.color = new Color(r, g, b, a);

            //  Pop up ends
            if (popText.color.a <= 0 || t * Time.fixedDeltaTime > duration) {

                Destroy(popText.transform.parent.gameObject);

            }

            yield return null;

        }

        yield return new WaitForFixedUpdate();

    }

    #endregion END COROUTINES

}
