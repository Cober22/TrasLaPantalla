using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ManageAppScenes : MonoBehaviour
{

    public static GameObject lettergramScene;
    public static GameObject maiwerScene;
    public static GameObject whosappScene;

    public void GoToTheApp()
    {
        switch (EventSystem.current.currentSelectedGameObject.name)
        {
            case "LETTERGRAM":
                if (!DontDestroy.lettergram)
                    SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
                else
                {
                    GameObject.Find("Escritorio").transform.gameObject.SetActive(false);
                    lettergramScene.SetActive(true);
                }
                    break;
            case "MAIWER":
                if (!DontDestroy.maiwer)
                    SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
                else
                {
                    GameObject.Find("Escritorio").transform.gameObject.SetActive(false);
                    maiwerScene.SetActive(true);
                }
                break;
            case "WHOSAPP":
                if (!DontDestroy.whosapp)
                    SceneManager.LoadScene(EventSystem.current.currentSelectedGameObject.name);
                else
                {
                    GameObject.Find("Escritorio").transform.gameObject.SetActive(false);
                    whosappScene.SetActive(true);
                }
                break;

        }
    }
}
