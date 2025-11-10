using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using System; // for Action delegate

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public TextMeshProUGUI nameComponent;

    public string[] lines;
    public float textSpeed = 0.05f;

    private int index;
    public Action onDialogueComplete; // callback for when dialogue finishes

    [Header("Speaker Colors")]
    [SerializeField] private Color defaultSpeakerColor = Color.white;

    // Optional: assign in Inspector or hardcode
    [SerializeField] private SpeakerColorEntry[] speakerColors;

    [System.Serializable]
    public struct SpeakerColorEntry
    {
        public string speakerName;
        public Color color;
    }


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
    public void StartDialogue(string speakerName, string[] newLines)
    {
        StartDialogue(speakerName, newLines, null);
    }

    // --- Overload 2: Start dialogue with callback
    public void StartDialogue(string speakerName, string[] newLines, Action onComplete)
    {
        lines = newLines;
        onDialogueComplete = onComplete;


        // Set the speaker name on UI (optional)
        if (nameComponent != null)
        {
            nameComponent.text = speakerName;
            nameComponent.gameObject.SetActive(!string.IsNullOrEmpty(speakerName));
        }

        // Set color
        Color c = defaultSpeakerColor;
        foreach (var entry in speakerColors)
        {
            if (entry.speakerName.Equals(speakerName, StringComparison.OrdinalIgnoreCase))
            {
                c = entry.color;
                break;
            }
        }
        nameComponent.color = c;


        index = 0;
        textComponent.text = string.Empty;
        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    // --- Overload 3: Start dialogue with no speaker name (for items, narration, etc.)
    public void StartDialogue(string[] newLines, Action onComplete)
    {
        StartDialogue("", newLines, onComplete);
    }

    // --- Overload 4: Start dialogue with no speaker name (for items, narration, etc.)
    public void StartDialogue(string[] newLines)
    {
        StartDialogue("", newLines, null);
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

            Debug.Log("Dialogue finished, calling onDialogueComplete!");

            // Trigger callback if there is one
            onDialogueComplete?.Invoke();
            onDialogueComplete = null;
        }
    }

    public void SetOnCompleteCallback(Action callback)
    {
        onDialogueComplete = callback;
    }
}
