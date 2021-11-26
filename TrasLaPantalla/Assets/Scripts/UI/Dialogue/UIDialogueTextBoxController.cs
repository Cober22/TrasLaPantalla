using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

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

    public bool chatInit = false;

    public void DoInteraction()
    {
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

        Message newMessage = new Message();

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        m_SpeakerText.name = node.DialogueLine.Speaker.CharacterName;
        newMessage.textObject.text = m_SpeakerText.name + ": " + node.DialogueLine.Text;

        string name = transform.name;

        switch (name)
        {
            case "ChatPanelJuan":
                newMessage.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact1);
                break;
            case "ChatPanelLaura":
                newMessage.textObject.color = GameObject.FindObjectOfType<ChatBoxManager>().MessageTypeColor(Message.MessageType.contact2);
                break;
            default:
                newMessage.textObject.color = Color.white;
                break;
        }

        node.Accept(this);
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
            newChoice.Add(aux);
            aux.transform.position = new Vector3(aux.transform.position.x, aux.transform.position.y + escala, aux.transform.position.z);
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