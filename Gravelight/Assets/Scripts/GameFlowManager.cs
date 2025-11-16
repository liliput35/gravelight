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
    private bool firstGhostDone = false;


    private bool grimIntroDone = false;

    [SerializeField] private GameObject hauntedGrave;
    [SerializeField] private GameObject ascendedGrave;



    private void Awake()
    {
        //comment/comment out until endif to keep data
       /*#if UNITY_EDITOR
                SaveData.Reset();
                Debug.Log("Reset on play start");
        #endif*/

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {

        if (SaveData.hasSavedPosition && SaveData.ghostHelped)
        {
            Debug.Log("has position saved");
            RestorePlayerTransform();

            grimIntroDone = SaveData.grimIntroDone;
            firstGhostDone = SaveData.ghostHelped;

            if (SaveData.wickIntroDone)
                wickFollower.enabled = true;

            if (SaveData.ghostHelped)
            {
                firstGhost.EnableInteraction(true);
            }
        }

        
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
        wickNPC.EnableInteraction(false);

        firstGhost.EnableInteraction(true);

        if (wickFollower != null)
        {
            wickFollower.enabled = true; // start following behavior
        }
    }



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

    public void OnGhostAlreadyAscended()
    {
        StartCoroutine(GhostAscendSequence());

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

        var setup = GameObject.Find("PlayerLiliSetupPrefab");
        var animated = GameObject.Find("PlayerLiliAnimated");
        var cameraNormal = GameObject.Find("CameraNormal");

        if (setup != null)
        {
            SaveData.setupPos = setup.transform.position;
            SaveData.setupRot = setup.transform.rotation;
        }

        if (animated != null)
        {
            SaveData.animatedPos = animated.transform.position;
            SaveData.animatedRot = animated.transform.rotation;
        }

        if (cameraNormal != null)
        {
            SaveData.cameraPos = cameraNormal.transform.position;
            SaveData.cameraRot = cameraNormal.transform.rotation;
        }


        SaveData.hasSavedPosition = true;

        SaveData.grimIntroDone = grimIntroDone;
        SaveData.wickIntroDone = wickFollower.enabled;
        SaveData.firstGhostDone = firstGhostDone;

        SceneManager.LoadScene("Library_Realm");

    }
    private void RestorePlayerTransform()
    {
        var setup = GameObject.Find("PlayerLiliSetupPrefab");
        var animated = GameObject.Find("PlayerLiliAnimated");
        var cameraNormal = GameObject.Find("CameraNormal");

        if (setup != null)
        {
            setup.transform.position = SaveData.setupPos;
            setup.transform.rotation = SaveData.setupRot;
        }

        if (animated != null)
        {
            animated.transform.position = SaveData.animatedPos;
            animated.transform.rotation = SaveData.animatedRot;
        }

        if (cameraNormal != null)
        {
            cameraNormal.transform.position = SaveData.cameraPos;
            cameraNormal.transform.rotation = SaveData.cameraRot;
        }
    }


    private IEnumerator GhostAscendSequence()
    {
        // Fade to white
        yield return StartCoroutine(ScreenFader.Instance.FadeToWhite(0.8f));

        // Wait while screen is fully white
        yield return new WaitForSeconds(0.8f);

        firstGhost.gameObject.SetActive(false);
        hauntedGrave.SetActive(false);
        ascendedGrave.SetActive(true);

        // Fade back from white
        yield return StartCoroutine(ScreenFader.Instance.FadeFromWhite(0.8f));

        Debug.Log("LIBRARIAN GHOST ASCENDED");
        #if UNITY_EDITOR
                SaveData.Reset();
        #endif
    }
}
