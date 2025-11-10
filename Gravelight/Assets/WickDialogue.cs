using UnityEngine;
using TMPro;

public class WickDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;
    [SerializeField] public Dialogue dialogue;
    [SerializeField] private string[] dialogueLines;

    [Header("Audio")]
    [SerializeField] private AudioSource interactionAudioSource;
    [SerializeField] private AudioClip interactionSFX;

    private TMP_Text _promptText;
    private bool canInteract = false;
    private bool hasTalked = false;
    private bool introDone = false;


    public string InteractionPrompt => _prompt;

    private GameFlowManager gameFlowManager;
    private LibraryGameFlowManager libraryGameFlowManager;

    private void Awake()
    {
        if (_promptUI != null)
        {
            _promptUI.SetActive(false);
            _promptText = _promptUI.GetComponentInChildren<TMP_Text>();
        }

        gameFlowManager = FindFirstObjectByType<GameFlowManager>();
        libraryGameFlowManager = FindFirstObjectByType<LibraryGameFlowManager>();

        if (interactionAudioSource == null)
        {
            Debug.LogWarning("Interaction AudioSource not assigned on Wick!");
        }
    }

    public void EnableInteraction(bool enable)
    {
        canInteract = enable;
    }

    public bool Interact(Interactor interactor)
    {
        if (!canInteract )
            return false;

        Debug.Log("Player is talking to Wick.");

        if (interactionAudioSource != null && interactionSFX != null)
        {
            interactionAudioSource.PlayOneShot(interactionSFX);
        }

        if (dialogue != null )
        {
            dialogue.StartDialogue("Wick", dialogueLines, OnWickDialogueComplete);
            hasTalked = true;
        } 

            return true;
    } 


    private void OnWickDialogueComplete()
    {
        if (hasTalked && !introDone)
        {
            Debug.Log("Wick finished talking — now he will follow the player.");
            canInteract = false;
            gameFlowManager?.OnWickIntroComplete();
            introDone = true;
        }
        else if (hasTalked && introDone) { 
            Debug.Log("Wick will now teleport you"); 
            EnableInteraction(false);
            gameFlowManager?.StartTeleportSequence();
        }

        Debug.Log("Wick finished talking in library.");
        LibraryGameFlowManager.Instance?.OnWickInteractionComplete();
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
