using UnityEngine;

public class LibraryGameFlowManager : MonoBehaviour
{
    public static LibraryGameFlowManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private WickDialogue wickNPC;          // Wick in the library
    [SerializeField] private WickFollower wickFollower;     // Wick follow behavior

    [Header("Player Equipment")]
    [SerializeField] private GameObject sword;
    [SerializeField] private PlayerCombatController combatController;

    [Header("UI")]
    [SerializeField] private Dialogue dialogueUI;

    [Header("Gem Tracking")]
    public int gemsCollected = 0;
    public int gemsRequired = 3;

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
    }

    public void OnGemCollected()
    {
        gemsCollected++;

        Debug.Log($"Gem collected! Total: {gemsCollected}/{gemsRequired}");

        if (gemsCollected >= gemsRequired)
        {
            Debug.Log("All gems collected! Level complete (continue gameflow here).");
        }
    }
}
