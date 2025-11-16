using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LibraryGameFlowManager : MonoBehaviour
{
    public static LibraryGameFlowManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WickDialogue wickNPC;          // Wick in the library
    [SerializeField] private WickFollower wickFollower;     // Wick follow behavior
    [SerializeField] private WickTeleportFlash wickFlash;

    [SerializeField] private GoodbookInteractable goodBook;     // Good book monster
    [SerializeField] private GameObject removableBookshelf;     // 


    [Header("Player Equipment")]
    [SerializeField] private GameObject sword;
    [SerializeField] private PlayerCombatController combatController;

    [Header("UI")]
    [SerializeField] private Dialogue dialogueUI;
    [SerializeField] private TextMeshProUGUI gemCounter;

    [Header("Gem Tracking")]
    public int gemsCollected = 0;
    public int gemsRequired = 3;

    private bool hasSword = false;

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
        // Library scene just loaded, enable Wick for interaction
        if (wickNPC != null)
            wickNPC.EnableInteraction(true);

        // Ensure Wick is not following yet
        if (wickFollower != null)
            wickFollower.enabled = false;

        Debug.Log("LibraryGameFlowManager started — Wick interaction enabled.");
    }

    // Called when player talks to Wick in the library
    public void OnWickInteractionComplete()
    {
        if (!hasSword)
        {
            Debug.Log("Player interacted with Wick in the library — start next sequence.");

            wickNPC.EnableInteraction(false);

            if (wickFollower != null)
            {
                wickFollower.enabled = true;
            }


            // Activate sword
            if (sword != null)
                sword.SetActive(true);

            // Enable combat
            if (combatController != null)
                combatController.enabled = true;

            if (dialogueUI != null)
                dialogueUI.StartDialogue(new string[] { "Wick gave you the sword!", "Press Q to attack" });
            hasSword = true;
        } else
        {
            OnLibraryWickFinalDialogueComplete();
        }
    }

    public void OnGemCollected()
    {
        gemsCollected++;

        Debug.Log($"Gem collected! Total: {gemsCollected}/{gemsRequired}");
        gemCounter.text = "x   " + gemsCollected;

        if (gemsCollected >= gemsRequired)
        {
            Debug.Log("All gems collected! Level complete (continue gameflow here).");

            // Notify player
            if (dialogueUI != null)
            {
                dialogueUI.StartDialogue(new string[]
                {
            "All memory fragments have been collected...",
            "Time to go back to the main world..."
                }); 
            }

            OnLevelCompleted();
        }
    }

    public void OnGoodBookFinished()
    {
        Debug.Log("Good Book dialogue finished — removing bookshelf.");

        // Remove bookshelf
        if (removableBookshelf != null)
            removableBookshelf.SetActive(false);

        // Notify player
        if (dialogueUI != null)
        {
            dialogueUI.StartDialogue(new string[]
            {
            "Something shifted in the library...",
            "Maybe some blocked paths are now open."
            });
        }
    }

    public void OnLevelCompleted()
    {
        // Wick is now interactable again in the Library
        if (wickNPC != null)
            wickNPC.EnableInteraction(true);

        // When Wick finishes talking next time, StartTeleportSequence() should run.
        if (dialogueUI != null)
        {
            dialogueUI.onDialogueComplete += OnLibraryWickFinalDialogueComplete;
        }
    }

    private void OnLibraryWickFinalDialogueComplete()
    {
        dialogueUI.onDialogueComplete -= OnLibraryWickFinalDialogueComplete;

        Debug.Log("Library Wick dialogue complete — starting teleport to Graveyard.");
        StartTeleportSequence();
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
        Debug.Log("Teleporting to Graveyard");


        var playerInput = FindFirstObjectByType<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions.Disable();  // important!
        }

        SaveData.GhostHelped = true;
        SceneManager.LoadScene("Graveyard_Helped");

    }
}
