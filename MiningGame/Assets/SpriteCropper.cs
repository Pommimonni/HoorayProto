using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SpriteCropper : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Common.spriteCropper = this;
        renderer=gameObject.GetComponent<SpriteRenderer>();
        source = renderer.sprite;
        textRect = source.textureRect;
        ratioy = textRect.height / sizeInView.y;
        ratiox = textRect.width / sizeInView.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 worldPos = Common.usefulFunctions.GetMouseWorldPosition();
            mousePosition = worldPos;
            CreateHoleAndMirrorBackground(worldPos, testSize);
            //  CropAndCreateHole(worldPos,testSize);
            //GetAndSetNewCullingMaskPositions(mousePosition, testSize);
            //CropTest(testRectangle);
        }
    }
    public Rect testRectangle;
    public float testSize;
    Sprite source;
    SpriteRenderer renderer;
    public Rect textRect;
    public Vector2 sizeInView = new Vector2(9, 6);
    public float ratioy;
    public float ratiox;
    public GameObject holePrefb;
    public Rect cropRect;
    public Vector3 mousePos;

    public Sprite CropTest(Rect toCrop)
    {
        Sprite sprite = new Sprite();

        sprite = Sprite.Create(source.texture, toCrop, new Vector2(0, 0), 100.0f);
        // position.z = -0.2f;
        // Vector3 corner = position;
        //corner.x -= size / 2;
        //corner.y += size / 2;
        Vector3 pos = Vector3.zero;
        pos.z = -0.3f;
        GameObject createdHole = (GameObject)Instantiate(holePrefb, pos, Quaternion.identity);
        createdHole.GetComponentInChildren<SpriteRenderer>().sprite = sprite;

        // createdHole.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = this.gameObject.transform.localScale;
        // createdHole.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = new Vector3(-0.5f,-0.5f, 0f);
        // renderer.sprite = sprite;
        return sprite;

    }
    public Vector3 offsetPosition;
    public float middle;
    public Vector2 offset;
    public Vector3 mousePosition;
    public float tileAmount;
    public GameObject CreateHoleAndMirrorBackground(Vector3 position,float size)
    {
        position.z = -1;
        GameObject newHole = (GameObject)Instantiate(holePrefb, new Vector3(0,0,-0.1f), Quaternion.identity);
        SpriteRenderer newRenderer = newHole.GetComponentInChildren<SpriteRenderer>();
        newRenderer.material = Common.holeShapeHandler.GetRandomShapeMaterial();
        newRenderer.sprite = renderer.sprite;
       // SpriteRenderer newRenderer = DublicateMyRenderer();
        GetAndSetNewCullingMaskPositions(position, size, newRenderer);
        newHole.GetComponentInChildren<BoxCollider2D>().gameObject.transform.position = position;
        newHole.GetComponentInChildren<BoxCollider2D>().gameObject.transform.localScale=new Vector3(size,size,1);
        return newHole;

    }
    public void GetAndSetNewCullingMaskPositions(Vector3 position,float floatSize,SpriteRenderer rendererToChange)
    {
        offset = GetOffsetValue(position, floatSize);
        Material newMaterial =new Material(rendererToChange.material);
        newMaterial.SetTextureOffset(Common.cullingMaskNameInTexture,offset);
        newMaterial.SetTextureScale(Common.cullingMaskNameInTexture, GetTileValue(position,floatSize));
        rendererToChange.material = newMaterial;
       
    }

   SpriteRenderer DublicateMyRenderer()
    {
        SpriteRenderer newRenderer = Instantiate(this.gameObject.GetComponent<SpriteRenderer>());
        return newRenderer;
    }

    Vector2 GetOffsetValue(Vector3 position, float floatSize)
    {
        offsetPosition = position;
        Debug.Log(offsetPosition);
        offsetPosition.x += (float)sizeInView.x / 2;
        offsetPosition.y += (float)sizeInView.y / 2;    

        Debug.Log(offsetPosition);
        tileAmount = (int)(10/floatSize); //1=10, 2=5,3=3
        float tileSize = (float)sizeInView.x / tileAmount;
        middle = tileAmount / 2;
        //offsetPosition.x -= tileSize / 2;
        //offsetPosition.y -= tileSize / 2;
        if (tileAmount % 2 == 0)
        {
            //EVEN amount
            middle = ((float)tileAmount - 1) / 2;
        }
        Debug.Log(offsetPosition);
        float xValue = FindTileValue(offsetPosition.x, tileSize);
        float yValue = FindTileValue(offsetPosition.y, tileSize);
        offset = new Vector2(-(xValue - 0.5f), -(yValue - 0.5f));
        return offset;
    }
    Vector2 GetTileValue(Vector3 position,float floatSize)
    {
        tileAmount = (int)(10 / floatSize); //0.5=201=10, 2=5,3=3
        return new Vector2(tileAmount, tileAmount);
    }

    float FindTileValue(float position,float tileSize)
    {
        float firstDivision = position / tileSize;

        int firstValue = (int)firstDivision;
        Debug.Log(" first value is " + firstValue.ToString() + " value is " + position.ToString());
        float secondValue = (position - firstValue * tileSize)*10;
        List<int> values = new List<int>();
        values=FindNextValue(secondValue, tileSize, ref values);
        Debug.Log("values");
// Debug.Log(values);
        float result = firstValue;
        int counter = 1;
        foreach(int value in values)
        {
            Debug.Log("Value is " + value.ToString()+" result "+result.ToString());
            if (value != 0)
            {
                result += value*Mathf.Pow(10, -counter);
            }
            else
            {
                result += 0;
            }
            counter++;
        }
        return result;
    }

     List<int> FindNextValue(float value, float tileSize, ref List<int> values)
    {
        float firstDivision = value / tileSize;
        int firstValue = (int)firstDivision;
        values.Add(firstValue);
        Debug.Log(" first value is " + firstValue.ToString()+" value is "+value.ToString());
        float nextValue = (value- firstValue * tileSize)*10;
        if (values.Count > 4)
        {
            return values;
        }
        FindNextValue(nextValue,tileSize,ref values);
        
        return values;

    }
    public Texture2D backgroundTexture2d;
    public Texture2D createdNewDebugging;
    public GameObject CropAndCreateHole(Vector3 position,float size)
    {

        //  Vector2 pos = new Vector2(position.x, position.y);
        //  Rect cropRect=new Rect(pos,)
        Sprite sprite = new Sprite();
        mousePos = position;
        cropRect = GetPixelRect(position, size);
        Vector2 pivot = cropRect.position;
        pivot.x += cropRect.width / 2;
        pivot.y += cropRect.height / 2;
        //  sprite = Sprite.Create(source.texture, cropRect,new Vector2(0,0) , 100.0f);
        Texture2D newText = CreateNew2DTexture(cropRect, source);
        cropRect.x = 0;
        cropRect.y = 0;
        sprite = Sprite.Create(newText, cropRect, new Vector2(0, 0), 100.0f);
       // SaveToPNG(newText);
        position.z = -0.2f;
        // Vector3 corner = position;
        //corner.x -= size / 2;
        //corner.y += size / 2;
        Debug.Log("Hole sprite cropped lets Instantiate hole prefab");
        GameObject createdHole = (GameObject)Instantiate(holePrefb, position, Quaternion.identity);
        createdHole.GetComponentInChildren<SpriteRenderer>().sprite = sprite;   // Common.holeShapeHandler.GetRandomSprite();//sprite;
        createdHole.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localScale = this.gameObject.transform.localScale;
        createdHole.GetComponentInChildren<SpriteRenderer>().gameObject.transform.localPosition = new Vector3(-size/2,-size/2, 0f);
        SpriteRenderer createdRend = createdHole.GetComponentInChildren<SpriteRenderer>();
        Material newMaterial = new Material(createdRend.material);
        //  newMaterial.SetTexture("_MainTex", newText); //sprite.texture);
        MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetTexture("_MainTex", newText);
        createdRend.SetPropertyBlock(block);
      //  newMaterial.EnableKeyword("_METALLICGLOSSMAP");
       // newMaterial.SetTexture("_MetallicGlossMap", newText);
        createdNewDebugging = newText;
        // newMaterial.SetTexture("")
        createdRend.material = newMaterial;
        //sprite.enc
        
        // renderer.sprite = sprite;
        return createdHole;

    }

    Texture2D CreateNew2DTexture(Rect cropRect,Sprite sprite)
    {

        if (cropRect.width !=sprite.texture.width)
        {
            Texture2D newText = new Texture2D(Mathf.CeilToInt(cropRect.width), Mathf.CeilToInt(cropRect.height));
            Color[] newColors = sprite.texture.GetPixels((int)cropRect.x, (int)cropRect.y, Mathf.CeilToInt(cropRect.width), Mathf.CeilToInt(cropRect.height));
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }


void SaveToPNG(Texture2D tex)
    {
        byte[] bytes =tex.EncodeToPNG();
       // Object.Destroy(tex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }
    Vector2 GetPixelPosition(Vector3 position)
    {

        Vector2 pixPos = new Vector2(position.x * ratiox, position.y * ratioy);
        pixPos += new Vector2(ratiox * sizeInView.x / 2, ratioy * sizeInView.y / 2);
        return pixPos;
    }

    float GetWidthPixels(float size)
    {

        return ratiox * size;
    }
    float GetHeightPixels(float size)
    {
        return ratioy * size;
    }

    Rect GetPixelRect(Vector3 position,float size)
    {
        Vector3 corner = position;
        corner.x -= size / 2;
        corner.y -= size / 2;
        Vector2 cornerPixPos = GetPixelPosition(corner);
        float width = GetWidthPixels(size);
        float height = GetHeightPixels(size);
        Rect pixRect = new Rect(cornerPixPos, new Vector2(width, height));
        return pixRect;
    }


}
    /*
    public Vector3 startPoint;
    public Vector3 endPoint;
    public SpriteRenderer spriteToCrop;
    private void cropSprite()
    {
        // Calculate topLeftPoint and bottomRightPoint of drawn rectangle
        Vector3 topLeftPoint = startPoint, bottomRightPoint = endPoint;
        if ((startPoint.x > endPoint.x))
        {
            topLeftPoint = endPoint;
            bottomRightPoint = startPoint;
        }
        if (bottomRightPoint.y > topLeftPoint.y)
        {
            float y = topLeftPoint.y;
            topLeftPoint.y = bottomRightPoint.y;
            bottomRightPoint.y = y;
        }
        SpriteRenderer spriteRenderer = spriteToCrop;
        Sprite spriteToCropSprite = spriteRenderer.sprite;
        Texture2D spriteTexture = spriteToCropSprite.texture;
        Rect spriteRect = spriteToCrop.sprite.textureRect;
        int pixelsToUnits = 100; // It's PixelsToUnits of sprite which would be cropped
                                 // Crop sprite
        GameObject croppedSpriteObj = new GameObject("CroppedSprite");
        Rect croppedSpriteRect = spriteRect;
        croppedSpriteRect.width = (Mathf.Abs(bottomRightPoint.x - topLeftPoint.x) * pixelsToUnits) * (1 / spriteToCrop.transform.localScale.x);
        croppedSpriteRect.x = (Mathf.Abs(topLeftPoint.x - (spriteRenderer.bounds.center.x - spriteRenderer.bounds.size.x / 2)) * pixelsToUnits) * (1 / spriteToCrop.transform.localScale.x);
        croppedSpriteRect.height = (Mathf.Abs(bottomRightPoint.y - topLeftPoint.y) * pixelsToUnits) * (1 / spriteToCrop.transform.localScale.y);
        croppedSpriteRect.y = ((topLeftPoint.y - (spriteRenderer.bounds.center.y - spriteRenderer.bounds.size.y / 2)) * (1 / spriteToCrop.transform.localScale.y)) * pixelsToUnits - croppedSpriteRect.height;//*(spriteToCrop.transform.localScale.y);
        Sprite croppedSprite = Sprite.Create(spriteTexture, croppedSpriteRect, new Vector2(0, 1), pixelsToUnits);
        SpriteRenderer cropSpriteRenderer = croppedSpriteObj.AddComponent();
        cropSpriteRenderer.sprite = croppedSprite;
        topLeftPoint.z = -1;
        croppedSpriteObj.transform.position = topLeftPoint;
        croppedSpriteObj.transform.parent = spriteToCrop.transform.parent;
        croppedSpriteObj.transform.localScale = spriteToCrop.transform.localScale;
        Destroy(spriteToCrop);
    }

    */


