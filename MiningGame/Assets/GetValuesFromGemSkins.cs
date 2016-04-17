using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GetValuesFromGemSkins : MonoBehaviour {

    public GameObject cellPrefab;
	// Use this for initialization
	void Start () {
        CreateAllGemInfos();

    }

    void Awake()
    {

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void CreateAllGemInfos()
    {
        grid = this.gameObject.GetComponent<GridLayoutGroup>();
        foreach (Gem gem in Common.gemSkins.allGems)
        {
            CreateOneCell(gem);
        }
    }
    GridLayoutGroup grid;
    void CreateOneCell(Gem gem)
    {
       
        GameObject created = (GameObject)Instantiate(cellPrefab);
        created.GetComponentInChildren<Image>().sprite = gem.gemSprite;
        created.GetComponentInChildren<Text>().text = gem.priceMoney.ToString();
        created.transform.parent = this.gameObject.transform;
        created.transform.localScale = Vector3.one;
        created.transform.localPosition =new Vector3(created.transform.position.x, created.transform.position.y, 0);
    }
}
