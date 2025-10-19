using UnityEngine;
using TMPro;

public class GhostInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [SerializeField] private Dialogue dialogue;
    [SerializeField] private string[] dialogueLines;

    private TMP_Text _promptText;

    public string InteractionPrompt => _prompt;

    private void Awake()
    {
        if (_promptUI != null)
        {
            _promptUI.SetActive(false);
            _promptText = _promptUI.GetComponentInChildren<TMP_Text>();
        }
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log($"Interacted with Ghost: {gameObject.name}");

        if (dialogue != null && dialogueLines.Length > 0)
        {
            dialogue.StartDialogue(dialogueLines);
        }

        return true;
    }

    public void ShowPrompt()
    {
        if (_promptUI != null)
        {
            if (_promptText != null)
                _promptText.text = _prompt;

            _promptUI.SetActive(true);
        }
    }

    public void HidePrompt()
    {
        if (_promptUI != null)
            _promptUI.SetActive(false);
    }
}
