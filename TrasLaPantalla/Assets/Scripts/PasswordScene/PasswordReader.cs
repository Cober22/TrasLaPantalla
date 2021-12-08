using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;

public class PasswordReader : MonoBehaviour
{
    public GameObject scoreText;
    private string newPassword;

    private bool hasWhiteSpace = false, hasMayus = false, hasMinus = false;

    /// <summary>
    /// Needed info for the final score
    /// </summary>

    private int totalNumbers, totalLetters, totalSpecialChars;
    private char[] specialChars;

    private void Start()
    {
        specialChars = "-*?!@#$/(){}=.,;:".ToCharArray();
    }
    public void PasswordReaderInput(string passwordInput)
    {
        newPassword = passwordInput;
        Reset();
        GetScore();
    }
    public void ChangeText(string newText)
    {
        scoreText.GetComponent<TextMeshProUGUI>().text = newText;
    }
    public void Reset()
    {
        totalNumbers = 0;
        totalLetters = 0;
        totalSpecialChars = 0;
        hasWhiteSpace = false; 
        hasMayus = false;
        hasMinus = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //DontDestroy.hiddenChat = true;

            if(DontDestroy.lettergram)
                ManageAppScenes.lettergramScene.SetActive(true);
            else if (DontDestroy.maiwer)
                ManageAppScenes.maiwerScene.SetActive(true);
            else if (DontDestroy.whosapp)
                ManageAppScenes.whosappScene.SetActive(true);

            ChatBoxManager.sceneName = "";

//            SceneManager.LoadScene("Escritorio");


        }
    }
    /*
        Debe incluir n�meros.

        Utilice una combinaci�n de letras may�sculas y min�sculas.

        Incluya caracteres especiales. �Cu�les son los caracteres especiales?

        Cualquiera de los siguientes caracteres:

                   - * ? ! @ # $ / () {} = . , ; :

        Tenga una longitud mayor o igual a 8 caracteres.
        No debe tener espacios en blanco.
        No utilice informaci�n personal en la contrase�a (como su nombre, fecha de nacimiento, etc.)

        No utilice patrones de teclado (qwerty) ni n�meros en secuencia (1234).

        No utilice �nicamente n�meros, may�sculas o min�sculas en su contrase�a.

        No repita caracteres (1111111).
     */

    private async void GetScore() {
        int totalChars = 0, totalScore = 0;

        char[] charsReader = new char[newPassword.Length];

        using (StringReader reader = new StringReader(newPassword))
        {
            await reader.ReadAsync(charsReader, 0, newPassword.Length);
        }

        foreach (char c in charsReader)
        {
            if (char.IsWhiteSpace(c))
            {
                ChangeText("La contrase�a es inv�lida, contiene espacios en blanco, c�mbiala");
                return;
            }
            else if(char.IsNumber(c)){
                if (totalNumbers == 0)//We only count once if it has numbs
                    totalScore += 10;
                totalNumbers++;
                
            }
            else if (char.IsLetter(c))
            {
                totalLetters++;

                if (char.IsLower(c))
                {
                    if (!hasMinus) //We only count once if it has Minus
                    {
                        hasMinus = true;
                        totalScore += 10;
                    }
                    

                }
                else
                {
                    if (!hasMayus) //We only count once if it has Minus
                    {
                        hasMayus = true;
                        totalScore += 10;
                    }
                    

                }
            }
            else
            {
                foreach (char symbol in specialChars)
                {
                    if (c.Equals(symbol))
                    {
                        if(totalSpecialChars == 0)
                            totalScore += 10;  //We only count once if it has special 
                        totalSpecialChars++;
                        

                    }
                }
            }
        }
       
        totalChars = totalLetters + totalNumbers + totalSpecialChars;

        if(totalChars >= 8)
        {
            totalScore += 10;

        }
        else if (totalChars == totalLetters)
        {
            totalScore -= 10;
            //ChangeText("La contrase�a introducida s�lo tiene letras");
        }
        else if (totalChars == totalNumbers)
        {
            totalScore -= 10;
            //ChangeText("La contrase�a introducida s�lo tiene n�meros");
        }
        else if (totalChars == totalSpecialChars)
        {
            totalScore -= 10;
            //ChangeText("La contrase�a introducida s�lo tiene s�mbolos");
        }
        

        ChangeText("Score: " + totalScore.ToString());

    }
}
