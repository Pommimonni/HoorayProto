using UnityEngine;
using System.Collections;

public static class RoundSettings {

    public static float bet=1f;
    public static float ratio;
    public static int hits=3;
    public static float player1Money =5f;
    public static float player2Money = 5f;
    public static float StartMoney = 5f;
   
    public static float moneyInserted = 14;
    public static int maxHits = 3;

    public static bool cameFromBet = false;
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
