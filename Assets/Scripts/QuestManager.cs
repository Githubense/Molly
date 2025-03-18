using UnityEngine;
using System.Collections.Generic;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    private Dictionary<string, bool> questStatus = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartQuest(string questName)
    {
        if (!questStatus.ContainsKey(questName))
        {
            questStatus.Add(questName, false);
        }
    }

    public void CompleteQuest(string questName)
    {
        if (questStatus.ContainsKey(questName))
        {
            questStatus[questName] = true;
        }
    }

    public bool IsQuestComplete(string questName)
    {
        return questStatus.ContainsKey(questName) && questStatus[questName];
    }
}