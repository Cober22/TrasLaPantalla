using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Text;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
    [SerializeField]
    private GameObject m_SpeakerText;
    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private List<GameObject> chatPanel = new List<GameObject>();
    [SerializeField]
    private InputField chatBox;


    [SerializeField]
    private RectTransform m_ChoicesBoxTransform;
    [SerializeField]
    public  UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    public DialogueChannel m_DialogueChannel;

    [SerializeField]
    public UnityEvent m_OnInteraction;

    [SerializeField]
    public int maxCharactersPerLine = 7;

    public bool chatInit = false;



    public void DoInteraction()
    {
        if(!DontDestroy.hiddenChat)
            m_OnInteraction.Invoke();
    }

    public DialogueNode m_NextNode = null;

    private void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        //gameObject.SetActive(false);
        //m_ChoicesBoxTransform.gameObject.SetActive(false);

        //DoInteraction();
    }

    public void NextSimpleNode()
    {
        m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    public void OnDialogueNodeStart(DialogueNode node)
    {
        //...... MENSAJES CONTACTO ......//
        Message newMessage = new Message();
        Message newMessage2 = new Message();

        GameObject newText = Instantiate(textObject, chatPanel[0].transform);
        GameObject newText2 = Instantiate(textObject, chatPanel[1].transform);
        //RectTransform pos = newText.GetComponent<RectTransform>();
        //newText.GetComponent<RectTransform>().rect.Set(-380f, pos.rect.y, pos.rect.width, pos.rect.height);
        
        newMessage.textObject = newText.GetComponentInChildren<Text>();
        newMessage2.textObject = newText2.GetComponentInChildren<Text>();

        newMessage.textObject.text = /*m_SpeakerText.name + ": " + */node.DialogueLine.Text;
        newMessage.textObject.text = TextFormat(newMessage.textObject.text);
        newMessage2.textObject.text = /*m_SpeakerText.name + ": " + */node.DialogueLine.Text;
        newMessage2.textObject.text = TextFormat(newMessage2.textObject.text);

        //Debug.Log(FindObjectOfType<ChatBoxManager>().GetComponent<ChatBoxManager>().nextMessage.Count);
        Debug.Log("Hola: " + node.DialogueLine.NextChat);
        switch (m_SpeakerText.name)
        {
            
            case "MadreChat (1)":
                if(node.DialogueLine.PlayerText != "")
                    FindObjectOfType<ChatBoxManager>().GetComponent<ChatBoxManager>().nextMessage[0] = TextFormat(node.DialogueLine.PlayerText);
                if (node.DialogueLine.Scene != "")
                    ChatBoxManager.sceneName = node.DialogueLine.Scene;
                if (node.DialogueLine.NextChat != "") {
                    Debug.Log("entra: " + node.DialogueLine.NextChat);
                    ChatBoxManager.nextChat = node.DialogueLine.NextChat;
                
                }
                break;
            case "PadreChat (1)":
                if (node.DialogueLine.PlayerText != "")
                    FindObjectOfType<ChatBoxManager>().GetComponent<ChatBoxManager>().nextMessage[1] = TextFormat(node.DialogueLine.PlayerText);
                if (node.DialogueLine.Scene != "")
                    ChatBoxManager.sceneName = node.DialogueLine.Scene;
                if (node.DialogueLine.NextChat != "")
                {
                    Debug.Log("entra: " + node.DialogueLine.NextChat);
                    ChatBoxManager.nextChat = node.DialogueLine.NextChat;

                }
                break;   
    
        }

        //...... ASIGNAR COLORES ......//
        string name = transform.name;
        switch (name)
        {
            case "ChatPanelPadre":
                newMessage.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact1);
                newMessage2.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact1);
                break;
            case "ChatPanelMadre":  
                newMessage.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact2);
                newMessage2.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact2);
                break;
            default:
                newMessage.textObject.color = Color.black;
                newMessage2.textObject.color = Color.black;
                break;
        }

        newMessage2.textObject.color = new Color(0f, 0f, 0f, 0f);
        newText2.GetComponentInChildren<Image>().color = new Color(0f, 0f, 0f, 0f);
        ChatBoxManager.character = 0;

        node.Accept(this);
    }

    public string TextFormat(string message)
    {
        string mensaje = "";

        // Programar saltos de linea con un límite de carácteres por linea
        int contador = 0;
        int mensajeLenght = 0;

        while ((message.Length - mensajeLenght) > maxCharactersPerLine)
        {
            while (contador < maxCharactersPerLine)
            {
                mensaje += message[mensajeLenght];
                contador += 1;
                mensajeLenght += 1;
            }

            while (message[mensajeLenght - 1] != ' ' && message.Length > mensajeLenght)
            {
                mensaje += message[mensajeLenght];
                mensajeLenght += 1;
            }

            contador = 0;
            mensaje += "\n";
        }

        while (mensajeLenght < message.Length)
        {
            mensaje += message[mensajeLenght];
            mensajeLenght += 1;
        }

        return mensaje;
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        m_NextNode = null;
    }

    public void Visit(BasicDialogueNode node)
    {
        m_NextNode = node.NextNode;
    }

    public void Visit(ChoiceDialogueNode node)
    {
        List<UIDialogueChoiceController> newChoice = new List<UIDialogueChoiceController>();
        int i = 1;
        foreach (DialogueChoice choice in node.Choices)
        {
            GameObject holder = GameObject.Find("Respuesta" + i.ToString());
            i += 1;
            UIDialogueChoiceController aux = Instantiate(m_ChoiceControllerPrefab, holder.transform);
            DontDestroyOnLoad(aux);
            newChoice.Add(aux);
            aux.Choice = choice;
        }
        //m_ChoiceControllerPrefab.gameObject.GetComponent<Image>().enabled = false;
    }
}   