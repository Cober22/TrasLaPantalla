using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    [SerializeField]
    UnityEvent m_OnInteraction;

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

        DoInteraction();
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
        //newMessage.textObject.color = GameObject.FindGameObjectsWithTag("Contacto")[0].GetComponent<Image>().color;

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
        foreach (DialogueChoice choice in node.Choices)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, transform);
            newChoice.transform.SetParent(m_ChoiceControllerPrefab.transform);
            newChoice.transform.position = new Vector3(newChoice.transform.position.x, newChoice.transform.position.y + escala, newChoice.transform.position.z);
            escala += newChoice.GetComponentInParent<RectTransform>().rect.height;
            newChoice.Choice = choice;
        }
    }
}   