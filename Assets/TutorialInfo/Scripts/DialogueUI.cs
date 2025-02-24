using UnityEngine;
using TMPro;

public class DialogueUi : MonoBehaviour
{

    [SerializeField] private TMP_Text textLabel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textLabel.text = "first line\nsecond line";
    }


}