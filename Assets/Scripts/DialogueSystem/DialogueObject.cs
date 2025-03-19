using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] private string[] dialogueKeys;
    [SerializeField] private bool hasChoices;
    [SerializeField] private string[] choiceKeys; // Keys for the choices
    [SerializeField] private string[] choiceResultKeys; // Keys for the choice results

    public string[] DialogueKeys => dialogueKeys;
    public bool HasChoices => hasChoices;
    public string[] ChoiceKeys => choiceKeys;

    public string GetChoiceResultKey(int choiceIndex)
    {
        if (choiceResultKeys != null && choiceResultKeys.Length > choiceIndex)
        {
            return choiceResultKeys[choiceIndex];
        }
        return null;
    }
}