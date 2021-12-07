using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueChoiceController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Choice;
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueNode m_ChoiceNextNode;
    private ChatBoxManager chatBoxManager;

    public DialogueChoice Choice
    {
        set
        {
            m_Choice.text = value.ChoicePreview;
            m_ChoiceNextNode = value.ChoiceNode;
        }
    }

    private void Start()
    {
        chatBoxManager = GameObject.FindObjectOfType<ChatBoxManager>();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        string aux = "ChatPanel";
        string username = "";
        int c = 0;

        while (c < chatBoxManager.ChatContactPanels[ChatBoxManager.indice].name.Length)
        {
            if (c > aux.Length - 1)
                username += chatBoxManager.ChatContactPanels[ChatBoxManager.indice].name[c];
            c++;
        }
        username = chatBoxManager.username;

        if(m_Choice.text != "")
        {
            m_Choice.text = FindObjectOfType<UIDialogueTextBoxController>().TextFormat(m_Choice.text);
            chatBoxManager.SendMessageToChat(username + ": " + m_Choice.text, Message.MessageType.playerMessage);
            m_DialogueChannel.RaiseRequestDialogueNode(m_ChoiceNextNode);
        }
    }
}