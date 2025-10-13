using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueBubble : MonoBehaviour
{
    public GameObject bubbleUI; // parent object of the panel
    public TMP_Text dialogueText; // the TMP text element

    private void Start()
    {
        HideDialogue(); // make sure it's hidden at first
    }

    public void ShowDialogue(string text)
    {
        dialogueText.text = text;
        bubbleUI.SetActive(true);
    }

    public void HideDialogue()
    {
        bubbleUI.SetActive(false);
    }
}
