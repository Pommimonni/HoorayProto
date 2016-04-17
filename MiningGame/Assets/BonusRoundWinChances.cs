using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;





public class BonusRoundWinChances : MonoBehaviour {

    void Awake()
    {
        Common.bonusRoundChances = this;
        CreateWinChances();
    }
    public float[] winChanceGem0=new float[5];
    public float[] winChanceGem1= new float[5];
    public float[] winChanceGem2= new float[5];
    public float[] winChanceGem3=new float[5];
    public float[] winChanceGem4= new float[5];
    public List<float[]> winChances;

    void CreateWinChances()
    {
        winChances = new List<float[]>();
        winChances.Add(winChanceGem0);
        winChances.Add(winChanceGem1);
        winChances.Add(winChanceGem2);
        winChances.Add(winChanceGem3);
        winChances.Add(winChanceGem4);
        //index 0 =>First gem, index 1=second gem so on so on

    }
    
    public Gem DetermineWin(int gemWeEntered)
    {
        float random = Random.Range(0f, 1f);
        float changeGoer = 0f;
        Debug.Log("Gem we entered bonus round " + gemWeEntered);
        float[] chances = winChances[gemWeEntered];
        int counter = 0;
        foreach (float chance in chances)
        {
            changeGoer = chance + changeGoer;
            if (random <= changeGoer)
            {
                Debug.Log("Gem we won is " + counter);
                return Common.gemSkins.allGems[counter];
            }
            counter++;
        }
        return Common.gemSkins.allGems[gemWeEntered];
    }
	

}
