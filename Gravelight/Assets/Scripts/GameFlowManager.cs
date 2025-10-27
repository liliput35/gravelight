using UnityEngine;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GrimDialogue grimNPC;   

    private bool grimIntroDone = false;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Debug.Log("GameFlowManager started — waiting for Grim intro to begin.");
    }

    /// Called by GrimDialogue when the player first talks to Grim.
    public void OnGrimFirstDialogueStarted(GrimDialogue grim)
    {
        if (grimIntroDone)
        {
            Debug.Log("Grim intro already completed.");
            return;
        }

        Debug.Log("Grim intro started — player is talking to Grim for the first time.");

        
        Dialogue dialogue = grimNPC.dialogue;
        if (dialogue != null)
        {
            dialogue.onDialogueComplete += OnGrimFirstDialogueEnd;
        }
    }

    /// Triggered when Grim finishes his first dialogue.
    private void OnGrimFirstDialogueEnd()
    {
        Debug.Log("Grim's first dialogue finished — marking Grim intro as done.");
        grimIntroDone = true;

        // (Later: trigger Wick’s intro sequence here)

        // Optional: Unsubscribe to prevent memory leaks
        if (grimNPC != null && grimNPC.GetComponentInChildren<Dialogue>() != null)
        {
            grimNPC.GetComponentInChildren<Dialogue>().onDialogueComplete -= OnGrimFirstDialogueEnd;
        }
    }
}
