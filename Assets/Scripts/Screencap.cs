using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screencap : MonoBehaviour
{
    //Declaration for file type. Modify with ,[file type for additional types you wish to add]
    public enum fileType { png, jpg, tif}
    public fileType targetType = fileType.png;

    //name of folder to ad screenshots to
    [SerializeField]
    string User = "";
    string folderPath = "Screenshots/";

    //Specific key to press to take screenshot
    public KeyCode targetKey = KeyCode.LeftControl;
    
    //Int value for uprezzing captions where 1 is screensize, 2 is double screen size etc.
    [SerializeField]
    int size = 1;

    void Awake()
    {
        folderPath = ($"Screenshots/{User}/");
    }

    void Start()
    {
        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);
    }


    void Update()
    {
        if (Input.GetKeyDown(targetKey))
        {
            TakeCaption();
        }
    }

    void TakeCaption()
    {
        var screenShotName = ($"Screenshot_{System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}.{targetType}");
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenShotName), size);
        Debug.Log(folderPath + screenShotName);
    }
}
