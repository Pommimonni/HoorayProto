using UnityEngine;
using System.Collections;

public class PopUp  {

    GameObject target;
    Vector2 offsets = Vector2.zero;
    string text;
    float duration = 5.0f;
    Vector2 direction = Vector2.zero;
    float speed = 10.0f;

    Color fillColor = Color.white;
    Color outlineColor = Color.black;
    Font font = null;
    int fontSize = 15;


    public PopUp(GameObject target, Vector2 offsets, string text) {

        this.target = target;
        this.offsets = offsets;
        this.text = text;
    
    }

    public PopUp(GameObject target, Vector2 offsets, string text, float duration) {

        this.target = target;
        this.offsets = offsets;
        this.text = text;
        this.duration = duration;

    }

    public PopUp(GameObject target, Vector2 offsets, string text, float duration, Vector2 direction) {

        this.target = target;
        this.offsets = offsets;
        this.text = text;
        this.duration = duration;
        this.direction = direction;
    
    }

    public PopUp(GameObject target, Vector2 offsets, string text, float duration, Vector2 direction, float speed) {

        this.target = target;
        this.offsets = offsets;
        this.text = text;
        this.duration = duration;
        this.direction = direction;
        this.speed = speed;

    }

    public GameObject Target {

        get {

            return target;

        }

    }

    public Vector2 Offsets {

        get {

            return offsets;

        }

        set {

            offsets = value;
            
        }

    }

    public string Text {

        get {

            return text;

        }

    }

    public float Duration {

        get {

            return duration;

        }
    
    }

    public Vector2 Direction {

        get {

            return direction;

        }

    }

    public float Speed {

        get {

            return speed;

        }
    
    }

    public Color FillColor {

        get {

            return fillColor;

        }

        set {
        
            fillColor = value;

        }

    }

    public Color OutlineColor {

        get {

            return outlineColor;

        }

        set {

            outlineColor = value;
        
        }
    
    }

    public Font Font {

        get {

            return font;

        }

        set {

            font = value;

        }

    }

    public int FontSize {

        get {

            return fontSize;
        
        }

        set {

            fontSize = value;
        
        }
    
    }

}
