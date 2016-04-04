using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
    public float maxHoleSize = 3f;
    public float minHoleSize = 0.2f;
    public int amountOfHoleSizes = 5;
	// Use this for initialization
	void Start () {
        Common.gameSettings = this;
	}
	

}
