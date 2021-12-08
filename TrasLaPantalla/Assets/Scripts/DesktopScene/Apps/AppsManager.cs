using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppsManager : MonoBehaviour
{
    public GameObject exitGB;
    private static GameObject exitGBStatic;
    private static List<GameObject> desktopIcons;
    private void Start()
    {
        desktopIcons = FindChildrenWithName("desktop_Icon");
        exitGBStatic = exitGB;
    }
    /// <summary>
    /// This function closes the current opened app and hides the escape button.
    /// Also it enables again all the desktop Icons
    /// </summary>
    public void ExitApp()
    {
        EnableIcons();

        for (int i = 0; i < desktopIcons.Count; i++)
        {
            Desktop_Icon iconScript = (Desktop_Icon)desktopIcons[i].GetComponent(typeof(Desktop_Icon));
            iconScript.closeApp();
        }        
        exitGB.SetActive(false);

    }

    /// <summary>
    /// This function activates the X botton in the canvas and disables desktop icons
    /// </summary>
    public static void ActiveEscape()
    {
        exitGBStatic.SetActive(true);
        DisableIcons();
    }

    /// <summary>
    /// Disables all the desktop Icons through the icons list
    /// </summary>
    public static void DisableIcons()
    {
        for (int i = 0; i < desktopIcons.Count; i++)
        {
            desktopIcons[i].SetActive(false);
        }
    }

    /// <summary>
    /// Enables all the desktop Icons through the icons list
    /// </summary>
    public static void EnableIcons()
    {
        for (int i = 0; i < desktopIcons.Count; i++)
        {
            desktopIcons[i].SetActive(true);
        }
    }

    /// <summary>
    /// This function allows us search through the childs of each app with the same name
    /// </summary>
    /// <param name="nam">The name of the child we are looking for</param>
    /// <returns>It returns a list of the gameobjects with the same name</returns>
    public List<GameObject> FindChildrenWithName(string nam) {

        List<GameObject> results = new List<GameObject>();
        for(int i = 0; i< transform.childCount; i++){
            for (int j = 0; j < transform.GetChild(i).transform.childCount; j++)
            {
                if (transform.GetChild(i).transform.GetChild(j).name == nam)
                {
                    results.Add(transform.GetChild(i).transform.GetChild(j).gameObject);
                }
                Debug.Log(transform.GetChild(i).transform.GetChild(j).name);
            }                    
        }
        return results;
    }
}
