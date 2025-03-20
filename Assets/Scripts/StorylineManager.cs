using UnityEngine;
using System.Collections.Generic;

public class StorylineManager : MonoBehaviour
{
    public static StorylineManager Instance;

    private Dictionary<string, bool> interactionStates = new Dictionary<string, bool>();
    private const string initialInteractionKey = "Computer"; // Key for the initial interaction

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this GameObject is not destroyed on load
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool CanInteract(string interactionKey)
    {
        // Allow interaction only if the initial interaction is completed or if it's the initial interaction itself
        if (interactionKey == initialInteractionKey || (interactionStates.ContainsKey(initialInteractionKey) && interactionStates[initialInteractionKey]))
        {
            return !interactionStates.ContainsKey(interactionKey) || !interactionStates[interactionKey];
        }
        return false;
    }

    public bool HasInteracted(string interactionKey)
    {
        return interactionStates.ContainsKey(interactionKey) && interactionStates[interactionKey];
    }

    public void SetInteractionState(string interactionKey, bool state)
    {
        if (interactionStates.ContainsKey(interactionKey))
        {
            interactionStates[interactionKey] = state;
        }
        else
        {
            interactionStates.Add(interactionKey, state);
        }
    }

    public bool IsInitialInteractionCompleted()
    {
        return interactionStates.ContainsKey(initialInteractionKey) && interactionStates[initialInteractionKey];
    }
}
