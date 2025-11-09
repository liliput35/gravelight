using UnityEngine;
using TMPro;

public class GemInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField, TextArea]
    private string[] dialogueLines =
    {
        "You picked up the gem."
    };

    private TMP_Text _promptText;
    private bool hasBeenCollected = false;

    private GemCollection gemCollection; // reference to your separate script

    public string InteractionPrompt => _prompt;

    private void Awake()
    {
        if (_promptUI != null)
        {
            _promptUI.SetActive(false);
        }

        // find attached GemCollection
        gemCollection = GetComponent<GemCollection>();
    }

    public bool Interact(Interactor interactor)
    {
        if (hasBeenCollected) return false;

        Debug.Log($"Interacted with Gem: {gameObject.name}");

        if (dialogue != null && dialogueLines.Length > 0)
        {
            // Start dialogue and register callback
            dialogue.StartDialogue(dialogueLines, OnDialogueComplete);
        }
        else
        {
            // No dialogue? Just collect instantly
            OnDialogueComplete();
        }

        return true;
    }

    private void OnDialogueComplete()
    {
        hasBeenCollected = true;

        if (gemCollection != null)
        {
            gemCollection.Collect();
        } else
        {
            Debug.LogWarning($"{name} has no GemCollection script!");
        }
            gameObject.SetActive(false);  // Disappear (collected)
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
