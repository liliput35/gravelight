using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using System; // for Action delegate

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed = 0.05f;

    private int index;
    private Action onDialogueComplete; // callback for when dialogue finishes

    void Start()
    {
        textComponent.text = string.Empty;
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    // --- Overload 1: Start dialogue with just lines (no callback)
    public void StartDialogue(string[] newLines)
    {
        StartDialogue(newLines, null);
    }

    // --- Overload 2: Start dialogue with callback
    public void StartDialogue(string[] newLines, Action onComplete)
    {
        lines = newLines;
        onDialogueComplete = onComplete;

        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StopAllCoroutines();
            StartCoroutine(TypeLine());
        }
        else
        {
            // Finished all lines
            gameObject.SetActive(false);

            // Trigger callback if there is one
            onDialogueComplete?.Invoke();
            onDialogueComplete = null;
        }
    }
}
