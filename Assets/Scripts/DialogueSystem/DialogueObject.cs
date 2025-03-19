using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] private string[] dialogueKeys;
    public string[] DialogueKeys => dialogueKeys;
}