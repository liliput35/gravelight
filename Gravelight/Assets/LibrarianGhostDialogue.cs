using UnityEngine;
using TMPro;

public class LibrarianGhostDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [SerializeField] public Dialogue dialogue;
    [SerializeField] private string[] firstDialogueLines;
    [SerializeField] private string[] altDialogueLines;

    private TMP_Text _promptText;
    private GameFlowManager gameFlowManager; 
    private bool hasTalkedAlready = false;

    private bool canInteract = false;


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

    public void EnableInteraction(bool enable)
    {
        canInteract = enable;
    }

    public bool Interact(Interactor interactor)
    {
        if (!canInteract || hasTalkedAlready)
            return false;

        Debug.Log($"Talking with Lucille The Librarian: {gameObject.name}");

        if (dialogue == null) return false;

        if (!hasTalkedAlready)
        {
            // Start first-time dialogue
            dialogue.StartDialogue("Lucille The Librarian", firstDialogueLines);
            hasTalkedAlready = true;

            // Notify the GameFlowManager
            gameFlowManager?.OnGhostDialogueStarted(this);
        }
        else
        {
            // Alternate dialogue for later interactions
            dialogue.StartDialogue("Lucille The Librarian", altDialogueLines);
        }

        return true;
    }

    public void ShowPrompt()
    {
        if (canInteract && _promptUI != null)
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
