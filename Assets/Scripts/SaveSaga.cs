using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveSaga : MonoBehaviour 
{
    // saga content
    public List<Sprite> borderDecoration = new List<Sprite>();
    public RectTransform rectT; // Assign the UI element which you wanna capture
    public GameObject contentObject;
    public GameObject startOfSagaPrefab;
    public GameObject sagaContentPrefab;
    public GameObject endOfSagaPrefab;
    private int m_sagaContentCount = 0;
    float sagaContentStartY = 275.0f; // needs to be always fed in negative
    float sagaContentHight = 150.0f;

    private int width; // width of the object to capture
    private int height; // height of the object to capture
    private Transform rectTransformParent;  //RectTransform parent to reasign after usage
    private GameObject screenShotGO;    //Tempory GameObject to copy recttransform
                                        //RectTransform values to reasign after usage
    private Vector2 offsetMinValues;
    private Vector2 offsetMaxValues;
    private Vector3 localScaleValues;

    void Start()
    {
        InitialiseSage();
    }

    public void InitialiseSage()
    {
        GameObject start = Instantiate(startOfSagaPrefab, new Vector3(rectT.rect.x,rectT.rect.y,0.0f), Quaternion.identity) as GameObject;
        start.transform.SetParent(contentObject.transform);
        start.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        start.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectT.rect.width/2,-100.0f);
    }

    public void AddSagaContent(string content, GameTileTypes tileType = GameTileTypes.WaterTile)
    {
        GameObject sagaContent = Instantiate(sagaContentPrefab, new Vector3(rectT.rect.x,rectT.rect.y,0.0f), Quaternion.identity) as GameObject;
        sagaContent.name = "Saga Content "+m_sagaContentCount;
        sagaContent.transform.SetParent(contentObject.transform);
        sagaContent.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        sagaContent.GetComponent<RectTransform>().anchoredPosition = new Vector2(rectT.rect.width/2,-sagaContentStartY-(sagaContentHight*m_sagaContentCount));
        
        sagaContent.GetComponent<SagaContentBehaviour>().SetText(content);
        m_sagaContentCount += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (rectT != null)
            {
                if (rectT.root.GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize)
                {
                    if (rectT.root.GetComponent<CanvasScaler>().screenMatchMode != CanvasScaler.ScreenMatchMode.MatchWidthOrHeight || 
                        rectT.root.GetComponent<CanvasScaler>().matchWidthOrHeight != 0.5f)
                    { 
                            Debug.LogWarning("UI may not look the same due to Canvas Scaler either screenMatchMode was not set MatchWidthOrHeight or MatchWidthOrHeight is not set to 0.5f");
                    }
                    createCanvasWithRectTransform();
                }
                else if (rectT.root.GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
                {
                    StartCoroutine(takeScreenShot());
                }
                else
                {
                    Debug.LogWarning("Canvas Scaler mode not supported.");
                }
            }
            else
            {
                Debug.Log("Rect transform is null to capture the screenshot, hence fullscreen has been taken.");
                ScreenCapture.CaptureScreenshot("FullPageScreenShot.png");
            }
        }
    }

    private void createCanvasWithRectTransform()
    {
        rectTransformParent = rectT.parent; //Assigning Parent transform to reasign after usage

        //Copying RectTransform values to reasign after switching parent
        offsetMinValues = rectT.offsetMin;
        offsetMaxValues = rectT.offsetMax;
        localScaleValues = rectT.localScale;

        //Creating secondary CANVAS with required fields
        screenShotGO = new GameObject("ScreenShotGO");
        screenShotGO.transform.parent = null;
        Canvas canvas = screenShotGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler canvasScalar = screenShotGO.AddComponent<CanvasScaler>();
        canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
        screenShotGO.AddComponent<GraphicRaycaster>();

        rectT.SetParent(screenShotGO.transform);    //Assigning capture recttransform to temporary parent gameobject

        //Reasigning recttransform values
        rectT.offsetMin = offsetMinValues;
        rectT.offsetMax = offsetMaxValues;
        rectT.localScale = localScaleValues;

        Canvas.ForceUpdateCanvases();   // Forcing all canvas to update the UI

        StartCoroutine(takeScreenShot());   //Once everything was set ready, Capture the screenshot
    }

    private IEnumerator takeScreenShot()
    {
        yield return new WaitForEndOfFrame(); // it must be a coroutine 

        //Calcualtion for the width and height of the screenshot from recttransform
        width = System.Convert.ToInt32(rectT.rect.width);
        height = System.Convert.ToInt32(rectT.rect.height);

        //Calcualtion for the starting position of the recttransform to be captured
        Vector2 temp = rectT.transform.position;
        var startX = temp.x - width / 2;
        var startY = temp.y - height / 2;

        // Read the pixels from the texture
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
        tex.Apply();

        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;

        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        //Writing bytes to a file
        File.WriteAllBytes(Application.dataPath + "/Saga of CHARACTERNAME"+Time.timeSinceLevelLoad+".png", bytes);

        //In case of ScaleMode was not ScaleWithScreenSize, parent will not be assigned then no need to revert the changes
        if (rectTransformParent != null)
        {
            //Reasigning gameobject to its original parent group
            rectT.SetParent(rectTransformParent);

            //Reasigning recttransform values
            rectT.offsetMin = offsetMinValues;
            rectT.offsetMax = offsetMaxValues;
            rectT.localScale = localScaleValues;

            //Destorying temporary created gameobject after usage
            Destroy(screenShotGO);
        }

        Debug.Log("Picture taken");
    }
}
