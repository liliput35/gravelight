using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class GemInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField, TextArea]
    private string[] dialogueLines =
    {
        " You picked up the gem."
    };

    [Header("Audio")]
    [SerializeField] private AudioClip collectSFX;
    private AudioSource audioSource;

    private TMP_Text _promptText;
    private bool hasBeenCollected = false;

    public string InteractionPrompt => _prompt;

    private void Awake()
    {
        if (_promptUI != null)
        {
            _promptUI.SetActive(false);
            _promptText = _promptUI.GetComponentInChildren<TMP_Text>();
        }

        audioSource = GetComponentInChildren<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogWarning($"{name} missing AudioSource child!");
        }
    }

    public bool Interact(Interactor interactor)
    {
        if (hasBeenCollected) return false;

        Debug.Log($"Interacted with Gem: {gameObject.name}");

        hasBeenCollected = true;

        // Play SFX first, then start dialogue after it finishes
        if (collectSFX != null && audioSource != null)
        {
            audioSource.PlayOneShot(collectSFX);
            StartCoroutine(WaitForSFXThenDialogue(0));
        }
        else
        {
            // No SFX? Start dialogue immediately
            StartDialogue();
        }

        return true;
    }

    private IEnumerator WaitForSFXThenDialogue(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartDialogue();
    }

    private void StartDialogue()
    {
        if (dialogue != null && dialogueLines.Length > 0)
        {
            dialogue.StartDialogue(dialogueLines, OnDialogueComplete);
        }
        else
        {
            OnDialogueComplete();
        }
    }

    private void OnDialogueComplete()
    {
        // Disable visuals and collider
        var mesh = GetComponentInChildren<MeshRenderer>();
        if (mesh != null) mesh.enabled = false;

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        // Detach AudioSource so it can finish playing if needed
        if (audioSource != null && collectSFX != null)
        {
            audioSource.transform.parent = null;
            Destroy(audioSource.gameObject, collectSFX.length);
        }

        LibraryGameFlowManager.Instance?.OnGemCollected();

        // Finally destroy the gem object
        Destroy(gameObject);

        Debug.Log($"{gameObject.name} collected!");
    }

    public void ShowPrompt()
    {
        if (_promptUI != null && !hasBeenCollected)
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
