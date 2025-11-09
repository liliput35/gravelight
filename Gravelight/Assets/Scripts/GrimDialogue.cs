using UnityEngine;
using TMPro;

public class GrimDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [SerializeField] public Dialogue dialogue;
    [SerializeField] private string[] firstDialogueLines;
    [SerializeField] private string[] altDialogueLines;

    private TMP_Text _promptText;
    private GameFlowManager gameFlowManager; //  reference to the central manager
    private bool hasTalkedAlready = false;

    public string InteractionPrompt => _prompt;

    private void Awake()
    {
        if (_promptUI != null)
        {
            _promptUI.SetActive(false);
            _promptText = _promptUI.GetComponentInChildren<TMP_Text>();
        }

        // Try to auto-find GameFlowManager in the scene
        gameFlowManager = FindFirstObjectByType<GameFlowManager>();
    }

    public bool Interact(Interactor interactor)
    {
        Debug.Log($"Talking with Grim: {gameObject.name}");

        if (dialogue == null) return false;

        if (!hasTalkedAlready)
        {
            // Start first-time dialogue
            dialogue.StartDialogue("Grim", firstDialogueLines);
            hasTalkedAlready = true;

            // Notify the GameFlowManager
            gameFlowManager?.OnGrimFirstDialogueStarted(this);
        }
        else
        {
            // Alternate dialogue for later interactions
            dialogue.StartDialogue("Grim", altDialogueLines);
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
