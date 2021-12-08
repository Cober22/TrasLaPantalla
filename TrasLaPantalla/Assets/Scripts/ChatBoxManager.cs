using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class ChatBoxManager : MonoBehaviour
{
    public string username;

    public int maxMessages = 15;

    [SerializeField]
    public GameObject[] chatPanel;
    public GameObject textObjectContacts;
    public GameObject textObjectPlayer;
    public InputField[] chatBoxInputs;

    public Color playerMessage, contact1, contact2;

    [SerializeField]
    public GameObject[] ChatContactPanels;

    [SerializeField]
    public static int indice = 0;

    private bool thisChat;
    
    public string nextMessage;

    public int character;
    public float waitForANewMessage = 1f;

    private void Awake()
    {
        //switch(SceneManager.GetActiveScene().name)
        //{
        //    case "LETTERGRAM": thisChat = DontDestroy.lettergram;
        //            break;
        //    case "MAIWER":
        //        thisChat = DontDestroy.maiwer;
        //            break;
        //    case "WHOSAPP":
        //        thisChat = DontDestroy.whosapp;
        //            break;
        //    default:
        //        thisChat = false;
        //            break;
        //}

        character = 0;

        if(DontDestroy.hiddenChat)
        {
            DontDestroy.hiddenChat = false;
            FindObjectOfType<DontDestroy>().GetComponent<Transform>().gameObject.SetActive(true);
            //chatBox[indice].GetComponent<UIDialogueTextBoxController>().chatInit = true;
        }
    }

    public void chooseContactIndex(GameObject chatBox)
    {

        GameObject hijos = GameObject.Find("DialogueChoiceButton");
        if(chatBox.name != ChatContactPanels[indice].name && hijos.transform.childCount > 1)
        {
            for (int i = 0; i < hijos.transform.childCount; i++)
                for (int e = 0; e < hijos.transform.GetChild(i).childCount; e++)
                    hijos.transform.GetChild(i).GetChild(e).gameObject.SetActive(false);
        } else if(hijos.transform.childCount > 1)
        {
            for (int i = 1; i < hijos.transform.childCount; i++)
            {
                for (int e = 0; e < hijos.transform.GetChild(i).childCount; e++)
                {
                    hijos.transform.GetChild(i).GetChild(e).gameObject.SetActive(true);

                }
                hijos.transform.GetChild(i).gameObject.SetActive(true);
                hijos.transform.GetChild(i).GetComponent<Image>().enabled = true;
            }

            hijos.transform.GetChild(0).gameObject.SetActive(false);
            hijos.transform.GetChild(hijos.transform.childCount-1).GetChild(1).gameObject.SetActive(false);
        }
        
        indice = 0;
        for (int i = 0; i < chatBoxInputs.Length; i++)
        {
            if (chatBox.name == ChatContactPanels[i].name)
            {
                //if (chatBox.GetComponent<UIDialogueTextBoxController>().m_DialogueChannel.name == "DialogueChannel 1")
                //    chatBox.GetComponent<UIDialogueTextBoxController>().m_ChoiceControllerPrefab.gameObject.SetActive(true);
                //else
                //    chatBox.GetComponent<UIDialogueTextBoxController>().m_ChoiceControllerPrefab.gameObject.SetActive(false);
                chatBox.SetActive(true);

                //------ INICIO DE LA CONVERSACION EN EL CHAT ------ //
                if (!chatBox.GetComponent<UIDialogueTextBoxController>().chatInit)
                {
                    chatBox.GetComponent<UIDialogueTextBoxController>().DoInteraction();
                    chatBox.GetComponent<UIDialogueTextBoxController>().chatInit = true;
                }
                indice = i;
            }
            else
            {
                ChatContactPanels[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (character < nextMessage.Length && Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
           || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            chatBoxInputs[indice].text += nextMessage[character];
            character += 1;
        }
        if (chatBoxInputs[indice].text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return) && character  >= nextMessage.Length)
            {
                character = 0;
                SendMessageToChat(/*username + ": " + */nextMessage, Message.MessageType.playerMessage);
                chatBoxInputs[indice].text = "";
            }
        }
        else
        {
            if (!chatBoxInputs[indice].isFocused && Input.GetKeyDown(KeyCode.Return))
                chatBoxInputs[indice].ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            DontDestroy.hiddenChat = true;
            FindObjectOfType<DontDestroy>().GetComponent<Transform>().gameObject.SetActive(false);
            SceneManager.LoadScene("Escritorio");
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        Message newMessage = new Message();  
        newMessage.text = text;

        GameObject newText = Instantiate(textObjectPlayer, chatPanel[indice].transform);

        newMessage.textObject = newText.GetComponentInChildren<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        StartCoroutine(CoroutineNextMessage());

    }

    IEnumerator CoroutineNextMessage() 
    { 
        //yield return new WaitForSecondsRealtime(waitForANewMessage);
        ChatContactPanels[indice].transform.GetComponent<UIDialogueTextBoxController>().NextSimpleNode();
        yield return null;
    }


    public Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = contact1;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
            case Message.MessageType.contact1:
                color = contact1;
                break;
            case Message.MessageType.contact2:
                color = contact2;
                break;
            default:
                color = Color.black;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage, 
        contact1, 
        contact2
    }
}
