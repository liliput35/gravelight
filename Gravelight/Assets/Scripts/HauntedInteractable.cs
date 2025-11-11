using UnityEngine;
using TMPro;

public class HauntedInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [SerializeField] public Dialogue dialogue;
    [SerializeField] private string[] dialogueLines = { "This grave feels… wrong. Like something’s watching from under the dirt." };

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
        Debug.Log($"Interacted with Grave: {gameObject.name}");

        if (dialogue != null && dialogueLines.Length > 0)
        {
            dialogue.StartDialogue("", dialogueLines);
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
