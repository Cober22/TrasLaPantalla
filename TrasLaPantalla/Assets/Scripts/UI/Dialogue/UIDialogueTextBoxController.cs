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
    private GameObject chatPanel;
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

    private DialogueNode m_NextNode = null;

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
        //gameObject.SetActive(true);


        //...... MENSAJES CONTACTO ......//
        Message newMessage = new Message();

        //Transform trans = chatPanel.transform;
        //trans.position = new Vector3(-380f, 0, 0f);
        
        GameObject newText = Instantiate(textObject, chatPanel.transform);
        //newText.GetComponent<RectTransform>().rect.position = new Vector3(-380f, 0f, 0f);
        
        //newText.transform.position = new Vector3(-380f,
                                                                            //newText.transform.position.y,
                                                                            //newText.transform.position.z);
        //Debug.Log(newText.GetComponent<RectTransform>().localPosition.x);
        //Debug.Log(newText.name);
        
        newMessage.textObject = newText.GetComponentInChildren<Text>();

        newMessage.textObject.text = m_SpeakerText.name + ": " + node.DialogueLine.Text;
        newMessage.textObject.text = TextFormat(newMessage.textObject.text);

        if(node.DialogueLine.PlayerText != "")
            FindObjectOfType<ChatBoxManager>().GetComponent<ChatBoxManager>().nextMessage = TextFormat(node.DialogueLine.PlayerText);


        //...... ASIGNAR COLORES ......//
        string name = transform.name;
        switch (name)
        {
            case "ChatPanelPadre":
                newMessage.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact1);
                break;
            case "ChatPanelMadre":
                newMessage.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact2);
                break;
            default:
                newMessage.textObject.color = Color.black;
                break;
        }

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

            while (message[mensajeLenght - 1] != ' ' && message.Length - 1 > mensajeLenght)
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
        //m_ChoicesBoxTransform.GetComponent<Image>().enabled = false;
        float escala = 0;

        List<UIDialogueChoiceController> newChoice = new List<UIDialogueChoiceController>();
        foreach (DialogueChoice choice in node.Choices)
        {
            UIDialogueChoiceController aux = Instantiate(m_ChoiceControllerPrefab);
            DontDestroyOnLoad(aux);
            newChoice.Add(aux);
            aux.transform.position = new Vector3(aux.transform.position.x, aux.transform.position.y + escala, aux.transform.position.z);

            aux.transform.SetParent(GameObject.Find("DialogueChoiceButton").transform);
            escala += aux.GetComponentInParent<RectTransform>().rect.height;
            aux.Choice = choice;
        }

        float y = 0;
        foreach(UIDialogueChoiceController choice in newChoice)
        {
            choice.transform.position = new Vector3(m_ChoiceControllerPrefab.transform.position.x,
                                                    m_ChoiceControllerPrefab.transform.position.y + y,
                                                    m_ChoiceControllerPrefab.transform.position.z);
            choice.transform.SetParent(m_ChoiceControllerPrefab.transform);
            y -= 50f;
        }
        m_ChoiceControllerPrefab.gameObject.GetComponent<Image>().enabled = false;
    }
}   