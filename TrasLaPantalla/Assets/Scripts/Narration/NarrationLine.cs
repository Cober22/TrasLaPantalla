using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Line")]
public class NarrationLine : ScriptableObject
{
    [SerializeField]
    private NarrationCharacter m_Speaker;
    [SerializeField]
    private string m_Text;
    [SerializeField]
    private string m_playerAnswer;

    public NarrationCharacter Speaker => m_Speaker;
    public string Text => m_Text;
    public string PlayerText => m_playerAnswer;
}