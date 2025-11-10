using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class BookshelfInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt = "";
    [SerializeField] private GameObject _promptUI;

    [Header("Dialogue")]
    [SerializeField] private Dialogue dialogue;
    [SerializeField, TextArea]
    private string[] dialogueLines =
    {
        " This area seems to be blocked..."
    };

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

        Debug.Log($"Interacted with Bookshelf: {gameObject.name}");

       StartDialogue();
        
        return true;
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
        Debug.Log($"{gameObject.name} interacted!");
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
