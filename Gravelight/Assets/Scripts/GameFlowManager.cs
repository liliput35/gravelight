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

    /*private void Start()
    {

        // Restore saved state if available
        if (SaveData.GhostHelped)
        {
            Debug.Log("Restoring saved player state...");

            RestorePlayerTransform();

            grimIntroDone = SaveData.GrimIntroDone;
            firstGhostDone = SaveData.FirstGhostDone;

            if (SaveData.WickIntroDone)
                wickFollower.enabled = true;

            if (firstGhostDone)
                firstGhost.EnableInteraction(true);
        }


    }*/

    private void Start()
    {
        StartCoroutine(RestoreStateAfterSceneLoad());
    }

    private IEnumerator RestoreStateAfterSceneLoad()
    {
        // Wait one frame to ensure all objects are loaded
        yield return null;

        if (SaveData.GhostHelped)
        {
            // Wait until all player objects exist
            GameObject setup = null;
            GameObject animated = null;
            GameObject cameraNormal = null;

            while (setup == null || animated == null || cameraNormal == null)
            {
                setup = GameObject.Find("PlayerLiliSetupPrefab");
                animated = GameObject.Find("PlayerLiliAnimated");
                cameraNormal = GameObject.Find("CameraNormal");
                yield return null;
            }

            RestorePlayerTransform();

            grimIntroDone = SaveData.GrimIntroDone;
            firstGhostDone = SaveData.FirstGhostDone;

            if (SaveData.WickIntroDone)
                wickFollower.enabled = true;

            if (firstGhostDone)
                firstGhost.EnableInteraction(true);

            Debug.Log("Saved state restored successfully.");
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
            wickFollower.enabled = true;
            SaveData.WickIntroDone = true;
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
        SaveData.FirstGhostDone = true;

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

        // Save transforms
        SaveTransforms();

        // Mark ghost helped
        SaveData.GhostHelped = true;

        SceneManager.LoadScene("Library_Realm");

    }

    private void SaveTransforms()
    {
        var setup = GameObject.Find("PlayerLiliSetupPrefab");
        var animated = GameObject.Find("PlayerLiliAnimated");
        var cameraNormal = GameObject.Find("CameraNormal");

        if (setup != null) SaveData.SaveVector3("SetupPos", setup.transform.position);
        if (setup != null) SaveData.SaveQuaternion("SetupRot", setup.transform.rotation);

        if (animated != null) SaveData.SaveVector3("AnimatedPos", animated.transform.position);
        if (animated != null) SaveData.SaveQuaternion("AnimatedRot", animated.transform.rotation);

        if (cameraNormal != null) SaveData.SaveVector3("CameraPos", cameraNormal.transform.position);
        if (cameraNormal != null) SaveData.SaveQuaternion("CameraRot", cameraNormal.transform.rotation);

        PlayerPrefs.Save(); // ensure persistent storage
    }

    private void RestorePlayerTransform()
    {
        var setup = GameObject.Find("PlayerLiliSetupPrefab");
        var animated = GameObject.Find("PlayerLiliAnimated");
        var cameraNormal = GameObject.Find("CameraNormal");

        if (setup != null)
        {
            setup.transform.position = SaveData.LoadVector3("SetupPos", setup.transform.position);
            setup.transform.rotation = SaveData.LoadQuaternion("SetupRot", setup.transform.rotation);
        }

        if (animated != null)
        {
            animated.transform.position = SaveData.LoadVector3("AnimatedPos", animated.transform.position);
            animated.transform.rotation = SaveData.LoadQuaternion("AnimatedRot", animated.transform.rotation);
        }

        if (cameraNormal != null)
        {
            cameraNormal.transform.position = SaveData.LoadVector3("CameraPos", cameraNormal.transform.position);
            cameraNormal.transform.rotation = SaveData.LoadQuaternion("CameraRot", cameraNormal.transform.rotation);
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
        
    }
}
