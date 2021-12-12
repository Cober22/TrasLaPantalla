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
    public GameObject[] chatPanel1;
    [SerializeField]
    public GameObject[] chatPanel2;


    public List<List<GameObject>> chatPanel = new List<List<GameObject>>();
    //public List<GameObject>[] chatPanel = new List<GameObject>[2];

    //public GameObject[] chatPanel;
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
    public static string sceneName;
    public static string nextChat;

    public static int character;
    public float waitForANewMessage = 1f;

    public GameObject respuesta1;
    public GameObject respuesta2;

    private void Awake()
    {
        int i = 0;
        while (i < 4)
        {
            nextMessage.Add("");
            i += 1;
        }

        List<GameObject> aux1 = new List<GameObject>();
        aux1.Add(chatPanel1[0]);
        aux1.Add(chatPanel1[1]);

        List<GameObject> aux2 = new List<GameObject>();
        aux2.Add(chatPanel2[0]);
        aux2.Add(chatPanel2[1]);

        chatPanel.Add(aux1);
        chatPanel.Add(aux2);

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
            if (chatBox.name == ChatContactPanels[i].name)
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

                if(playerNextMessage == "" && respuesta1.transform.childCount > 0)
                {
                    respuesta1.transform.GetChild(0).gameObject.SetActive(true);
                    respuesta2.transform.GetChild(0).gameObject.SetActive(true);
                } else if(respuesta1.transform.childCount > 0)
                {
                    respuesta1.transform.GetChild(0).gameObject.SetActive(false);
                    respuesta2.transform.GetChild(0).gameObject.SetActive(false);
                }
                //anterior = indice;
            }
            else
            {
                ChatContactPanels[i].SetActive(false);
            }
        }
    }

    void Update()
    {
        playerNextMessage = nextMessage[indice];
        Debug.Log(playerMessage);
        if (playerNextMessage != null && character < playerNextMessage.Length && Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
           || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            playerNextMessage = nextMessage[indice];

            if(nextMessage[indice] == "" || nextMessage[indice] == null)
                playerNextMessage = "";

            chatBoxInputs[indice].text += playerNextMessage[character];
            chatBoxInputs[indice].transform.FindChild("Text").GetComponent<Text>().text = chatBoxInputs[indice].text;
            character += 1;
            if (character >= 79) {
                chatBoxInputs[indice].transform.FindChild("Text").GetComponent<Text>().alignment = TextAnchor.MiddleRight;
            }
        }
        if (chatBoxInputs[indice].text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return) && character >= playerNextMessage.Length)
            {
                character = 0;
                SendMessageToChat(/*username + ": " + */playerNextMessage, Message.MessageType.playerMessage);
                chatBoxInputs[indice].transform.FindChild("Text").GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
                if (sceneName != "" && sceneName != null)
                {
                    Debug.Log(sceneName);
                    FindObjectOfType<DontDestroy>().GetComponent<Transform>().gameObject.SetActive(false);
                    SceneManager.LoadScene(sceneName);
                }
                if (nextChat != "")
                {
                    //GameObject.Find(nextChat).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
                    GameObject.Find(nextChat).GetComponent<Button>().interactable = true;

                    int aux = indice == 0 ? 1 : 0;
                    GameObject.Find("Contactos").transform.GetChild(aux).GetComponent<Button>().interactable = false;

                    nextChat = "";
                }
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
        Message newMessage2 = new Message();
        newMessage2.text = text;

        GameObject newText = Instantiate(textObjectPlayer, chatPanel[indice][0].transform);
        GameObject newText2 = Instantiate(textObjectPlayer, chatPanel[indice][1].transform);

        newMessage.textObject = newText.GetComponentInChildren<Text>();
        newMessage2.textObject = newText2.GetComponentInChildren<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage2.textObject.text = newMessage2.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        newMessage.textObject.color = new Color(0f, 0f, 0f, 0f);
        newText.GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 0f);
        character = 0;

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
