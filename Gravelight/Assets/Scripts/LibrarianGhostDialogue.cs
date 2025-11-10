using UnityEngine;
using TMPro;

public class LibrarianGhostDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [SerializeField] public Dialogue dialogue;
    [SerializeField] private string[] firstDialogueLines;
    [SerializeField] private string[] altDialogueLines;
    [SerializeField] private string[] thankDialogueLines;


    [Header("Audio")]
    [SerializeField] private AudioSource interactionAudioSource;
    [SerializeField] private AudioClip interactionSFX;

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

        if (interactionAudioSource == null)
        {
            Debug.LogWarning("Interaction AudioSource not assigned on Librarian!");
        }
    }

    public void EnableInteraction(bool enable)
    {
        canInteract = enable;
    }

    public bool Interact(Interactor interactor)
    {
        if (!canInteract)
            return false;

        Debug.Log($"Talking with Lucille The Librarian: {gameObject.name}");

        if (interactionAudioSource != null && interactionSFX != null)
            interactionAudioSource.PlayOneShot(interactionSFX);

        if (dialogue == null)
            return false;

        // If ghost was helped in the library, always show thank you message
        if (SaveData.ghostHelped)
        {
            dialogue.StartDialogue("Lucille The Librarian", thankDialogueLines);
            Debug.Log("helped ghost ascend");
            return true;
        }

        //  First-time talk in Graveyard
        if (!hasTalkedAlready)
        {
            dialogue.StartDialogue("Lucille The Librarian", firstDialogueLines);
            hasTalkedAlready = true;

            // Notify GameFlowManager only the first time
            gameFlowManager?.OnGhostDialogueStarted(this);
            return true;
        }

        // If talked already but ghost hasn’t been helped yet (unlikely case)
        dialogue.StartDialogue("Lucille The Librarian", altDialogueLines);
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
