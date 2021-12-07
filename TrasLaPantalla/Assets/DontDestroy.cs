using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour
{
    public static bool hiddenChat = false; 
    public static bool lettergram = false;
    public static bool maiwer = false;
    public static bool whosapp = false;

    private void Awake()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "LETTERGRAM":
                if (!lettergram)
                {
                    DontDestroyOnLoad(transform.gameObject);
                    ManageAppScenes.lettergramScene = transform.gameObject;
                    lettergram = true;
                }
                break;
            case "MAIWER":
                if (!maiwer)
                {
                    DontDestroyOnLoad(transform.gameObject);
                    ManageAppScenes.maiwerScene = transform.gameObject;
                    maiwer = true;
                }
                break;
            case "WHOSAPP":
                if (!whosapp)
                {
                    DontDestroyOnLoad(transform.gameObject);
                    ManageAppScenes.whosappScene = transform.gameObject;
                    whosapp = true;
                }
                break;
        }
    }
}
