using UnityEngine;
using System.Collections;

public class RoundSettings : MonoBehaviour {

    public float bet;
    public float ratio;
    public int hits;
    public float moneyInserted;
    void Awake()
    {
        if (Common.roundSettings)
        {
            Destroy(this.gameObject);
        }
        else {
            Common.roundSettings = this;
            DontDestroyOnLoad(this.gameObject);
        }


    }
}
