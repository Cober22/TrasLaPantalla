using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Desktop_Icon : MonoBehaviour
{
    public float transitionSpeed = 0.1f;
    private Transform appGB;

    /// <summary>
    /// When the sprite is clicked it scales up the app gameobject
    /// </summary>
    public void OnMouseDown()
    {
        appGB = transform.parent.Find("app");      
        StartCoroutine(ScaleUP());
    }

    /// <summary>
    /// When the sprite is clicked it scales down the app gameobject
    /// </summary>
    public void closeApp()
    {
        
        appGB = transform.parent.Find("app");
        StartCoroutine(ScaleDown());
    }

    /// <summary>
    /// Coroutine that activates the gb and scales it up to do the background
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleUP()
    {
        appGB.gameObject.SetActive(true);
        SpriteRenderer sr = appGB.GetComponent<SpriteRenderer>();
        if (sr == null) yield return null;

        appGB.transform.localScale = new Vector3(1f, 1f, 1f);
        
        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;


        Vector3 scaleChange = new Vector3(transitionSpeed, transitionSpeed, 0f);
        Vector3 worldScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height, 0f);
        while (appGB.transform.localScale.x <= worldScale.x * 2)
        {
            appGB.transform.localScale += scaleChange;
            yield return new WaitForSeconds(0);
        }


        AppsManager.ActiveEscape();
        yield return null;
    }

    /// <summary>
    /// Coroutine that activates the gb and scales it down and disables it
    /// </summary>
    /// <returns></returns>
    IEnumerator ScaleDown()
    {
        SpriteRenderer sr = appGB.GetComponent<SpriteRenderer>();
        if (sr == null) yield return null;

        Vector3 scaleChange = new Vector3(transitionSpeed, transitionSpeed, 0f);
        while (appGB.transform.localScale.x >= 0.5f)
        {
            appGB.transform.localScale -= scaleChange;
            yield return new WaitForSeconds(0);
        }

        appGB.gameObject.SetActive(false);
        yield return null;
    }

}
