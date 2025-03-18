using UnityEngine;

public class ShowOnMobile : MonoBehaviour
{
    [SerializeField] private bool forceMobileInEditor = false; // Opzione per testare su PC

    void Start()
    {
        bool isMobile = Application.isMobilePlatform || forceMobileInEditor;
        gameObject.SetActive(isMobile);
    }
}
