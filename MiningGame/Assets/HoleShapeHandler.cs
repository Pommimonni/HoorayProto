using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HoleShapeHandler : MonoBehaviour {
    public Material materialPrefab;
    public List<Texture> shapeTextures;
    public List<Material> createdShapeMaterials;
    public List<Sprite> spriteShapes;
	// Use this for initialization
	void Start () {
        Common.holeShapeHandler = this;
        createdShapeMaterials = new List<Material>();
        foreach (Texture text in shapeTextures) {
            Material newMaterial = (Material)Instantiate(materialPrefab);
            newMaterial.SetTexture(Common.cullingMaskNameInTexture, text);
            
            createdShapeMaterials.Add(newMaterial);
                }
	}
	
	// Update is called once per frame
    public Material GetRandomShapeMaterial()
    {
        int randomIndex = Random.Range(0, createdShapeMaterials.Count);
        return createdShapeMaterials[randomIndex];
    }
    public Texture GetRandomShapeTexture()
    {
        int randomIndex = Random.Range(0, shapeTextures.Count);
        return shapeTextures[randomIndex];
    }
    public Sprite GetRandomSprite()
    {
        int randomIndex = Random.Range(0, spriteShapes.Count);
        return spriteShapes[randomIndex];
    }
}
