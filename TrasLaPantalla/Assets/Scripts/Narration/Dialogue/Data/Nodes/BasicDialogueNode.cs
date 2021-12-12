using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Dialogue/Node/Basic")]
public class BasicDialogueNode : DialogueNode
{
    [SerializeField]
    private DialogueNode m_NextNode;
    [SerializeField]
    private bool activateMessage;
    [SerializeField]
    private DialogueNode messageToActivate;

    public DialogueNode NextNode => m_NextNode;
    public bool ActivateMessage => activateMessage;
    public DialogueNode MessageToActivate => messageToActivate;


    public override bool CanBeFollowedByNode(DialogueNode node)
    {
        return m_NextNode == node;
    }

    public override void Accept(DialogueNodeVisitor visitor)
    {
        visitor.Visit(this);
    }
}