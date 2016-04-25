using UnityEngine;
using System.Collections;

public static class RoundSettings {

    public static float bet=1f;
    public static float ratio;
    public static int hits=3;
    public static float moneyInsertedPLayer1 = 1f;
    public static float moneyInsertedPlayer2 = 22f;
    public static float moneyInserted = 14;
    /*
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
    */
}
