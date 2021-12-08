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
    public static int anterior = 1234;
    private bool thisChat;
    
    public List<string> nextMessage = new List<string>();
    public string playerNextMessage;

    public int character;
    public float waitForANewMessage = 1f;

    public GameObject respuesta1;
    public GameObject respuesta2;

    private void Awake()
    {
        int i = 0;
        while (i < 4) {
            nextMessage.Add("");
            i += 1;
        }

        if (respuesta1 == null)
        {
            respuesta1 = GameObject.Find("Respuesta1");
            respuesta2 = GameObject.Find("Respuesta2");
        }

        character = 0;

        if(DontDestroy.hiddenChat)
        {
            DontDestroy.hiddenChat = false;
            FindObjectOfType<DontDestroy>().GetComponent<Transform>().gameObject.SetActive(true);
        }
    }

    public void chooseContactIndex(GameObject chatBox)
    {
        indice = 0;
        for (int i = 0; i < chatBoxInputs.Length; i++)
        {
            if (chatBox.name == ChatContactPanels[i].name && anterior != i)
            {
                chatBox.SetActive(true);

                //------ INICIO DE LA CONVERSACION EN EL CHAT ------ //
                if (!chatBox.GetComponent<UIDialogueTextBoxController>().chatInit)
                {
                    chatBox.GetComponent<UIDialogueTextBoxController>().DoInteraction();
                    chatBox.GetComponent<UIDialogueTextBoxController>().chatInit = true;
                }
                indice = i;
                playerNextMessage = nextMessage[indice];

                if(playerNextMessage == "")
                {
                    respuesta1.SetActive(true);
                    respuesta2.SetActive(true);
                } else
                {
                    respuesta1.SetActive(false);
                    respuesta2.SetActive(false);
                }
                anterior = indice;
            }
            else
            {
                ChatContactPanels[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        if (playerNextMessage != null && character < playerNextMessage.Length && Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
           || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            chatBoxInputs[indice].text += playerNextMessage[character];
            character += 1;
        }
        if (chatBoxInputs[indice].text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return) && character  >= playerNextMessage.Length)
            {
                character = 0;
                SendMessageToChat(/*username + ": " + */playerNextMessage, Message.MessageType.playerMessage);
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
