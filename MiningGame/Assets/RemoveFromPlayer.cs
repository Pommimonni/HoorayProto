using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class RemoveFromPlayer : MonoBehaviour {


    public List<GameObject> objectsToRemove;
    public int playerNumber = 0;
	// Use this for initialization

    public void RemoveBasedOnTags(int player)
    {
        string tagToRemove = "RemoveFromPlayer"+player.ToString();
        RemoveFromTag(tagToRemove);
    }


    void RemoveFromTag(string tagToFind)
    {
        Transform[] allChilds = this.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChilds)
        {
            if (child.tag == tagToFind)
            {
                Destroy(child.gameObject);

            }
        }
    }


}
