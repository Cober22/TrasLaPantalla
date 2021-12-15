using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool lettergram = false, Maiwer = false, Whosapp = false;
    public GameObject LettergramGB;
    public GameObject MaiwerGB;
    public GameObject WhosappGB;

    void Start() {
        MaiwerGB.GetComponent<Button>().interactable = false;
        WhosappGB.GetComponent<Button>().interactable = false;

    }


    void Update() {

        if (Maiwer)
        {
            LettergramGB.GetComponent<Button>().interactable = false;
            WhosappGB.GetComponent<Button>().interactable = false;
            MaiwerGB.GetComponent<Button>().interactable = true;
        }
        if (Whosapp)
        {
            MaiwerGB.GetComponent<Button>().interactable = false;
            LettergramGB.GetComponent<Button>().interactable = false;
            WhosappGB.GetComponent<Button>().interactable = true;
        }


    } 

    
}
