using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class UIDialogueChoiceController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_Choice;
    [SerializeField]
    private DialogueChannel m_DialogueChannel;

    private DialogueNode m_ChoiceNextNode;
    private ChatBoxManager chatBoxManager;

    private float waitForANewMessage = 1f;

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

        // Corta el nombre del chat panel en el que hemos clickado
        while (c < chatBoxManager.ChatContactPanels[ChatBoxManager.indice].name.Length)
        {
            if (c > aux.Length - 1)
                username += chatBoxManager.ChatContactPanels[ChatBoxManager.indice].name[c];
            c++;
        }
        List<GameObject> choices = new List<GameObject>();

        choices.Add(GameObject.Find("Respuesta1"));
        choices.Add(GameObject.Find("Respuesta2"));

        if(m_Choice.text != "")
        {
            Debug.Log(m_Choice.text);
            m_Choice.text = FindObjectOfType<UIDialogueTextBoxController>().TextFormat(m_Choice.text);
            chatBoxManager.SendMessageToChat(/*username + ": " + */ m_Choice.text, Message.MessageType.playerMessage);

            StartCoroutine(Coroutine2(choices));
        }
    }

    IEnumerator Coroutine2(List<GameObject> choices)
    {
        foreach (GameObject choice in choices)
            if (choice.transform.childCount > 0)
                choice.transform.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = "";

        yield return new WaitForSeconds(waitForANewMessage);
        foreach (GameObject choice in choices)
            if (choice.transform.childCount > 0)
                Destroy(choice.transform.GetChild(0).gameObject);
        m_DialogueChannel.RaiseRequestDialogueNode(m_ChoiceNextNode);
    }

}