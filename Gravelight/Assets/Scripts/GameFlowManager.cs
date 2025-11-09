using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    public static GameFlowManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GrimDialogue grimNPC;   

    [SerializeField] public WickDialogue wickNPC; 
    [SerializeField] private WickFollower wickFollower;
    [SerializeField] private WickTeleportFlash wickFlash;


    [SerializeField] private LibrarianGhostDialogue firstGhost;   // Librarian ghost NPC

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
        // Allow the player to talk to Wick
        if (wickNPC != null)
        {
            wickNPC.EnableInteraction(true);
        }

        // Optional: Unsubscribe to prevent memory leaks
        if (grimNPC != null && grimNPC.GetComponentInChildren<Dialogue>() != null)
        {
            grimNPC.GetComponentInChildren<Dialogue>().onDialogueComplete -= OnGrimFirstDialogueEnd;
        }
    }

    public void OnWickIntroComplete()
    {
        Debug.Log("Wick intro finished — enabling Wick to follow player.");

        firstGhost.EnableInteraction(true);

        if (wickFollower != null)
        {
            wickFollower.enabled = true; // start following behavior
        }
    }

    // GHOST PHASE MANAGEMENT    

    private bool firstGhostDone = false;

    public void OnGhostDialogueStarted(LibrarianGhostDialogue ghost)
    {
        if (firstGhostDone) return;

        Dialogue d = ghost.dialogue;
        if (d != null)
            d.onDialogueComplete += OnGhostDialogueFinished;
    }

    private void OnGhostDialogueFinished()
    {
        Debug.Log("First ghost finished dialogue — preparing scene teleport.");

        firstGhostDone = true;

        // ENABLE WICK INTERACTION AGAIN
        wickNPC.EnableInteraction(true);
        
    }

    public void StartTeleportSequence()
    {
        StartCoroutine(TeleportRoutine());
    }

    private IEnumerator TeleportRoutine()
    {
        // Play flash
        yield return StartCoroutine(wickFlash.PlayFlash());

        yield return StartCoroutine(ScreenFader.Instance.FadeToWhite(0.5f));
        Debug.Log("Teleporting to Library");


        var playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions.Disable();  // important!
        }

        SceneManager.LoadScene("Library_Realm");

    }

}
