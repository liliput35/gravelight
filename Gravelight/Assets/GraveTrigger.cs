using UnityEngine;

public class GraveTrigger : MonoBehaviour
{
    public string playerTag = "Player";
    public DialogueBubble dialogueBubble; // assign in Inspector
    public string dialogueText;

    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag(playerTag))
        {
            hasActivated = true;
            dialogueBubble.ShowDialogue(dialogueText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            dialogueBubble.HideDialogue();
            hasActivated = false;
        }
    }
}
