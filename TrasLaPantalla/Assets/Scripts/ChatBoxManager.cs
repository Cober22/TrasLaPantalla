using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBoxManager : MonoBehaviour
{
    public string username;

    public int maxMessages = 15;

    [SerializeField]
    public GameObject[] chatPanel;
    public GameObject textObject;
    public InputField[] chatBoxInputs;

    public Color playerMessage, info;

    [SerializeField]
    public GameObject[] ChatContactPanels;

    [SerializeField]
    public static int indice = 0;

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
            }
            else
                ChatContactPanels[i].SetActive(false);
        }
    }

    void Update()
    {
        if (chatBoxInputs[indice].text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + chatBoxInputs[indice].text, Message.MessageType.playerMessage);
                chatBoxInputs[indice].text = "";
            }
        }
        else
        {
            if (!chatBoxInputs[indice].isFocused && Input.GetKeyDown(KeyCode.Return))
                chatBoxInputs[indice].ActivateInputField();
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        Message newMessage = new Message();  
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel[indice].transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        ChatContactPanels[indice].transform.GetComponent<UIDialogueTextBoxController>().NextSimpleNode();
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
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
        info
    }
}
