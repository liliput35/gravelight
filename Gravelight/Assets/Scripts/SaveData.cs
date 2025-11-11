using UnityEngine;

public static class SaveData
{
    public static bool grimIntroDone = false;
    public static bool wickIntroDone = false;
    public static bool firstGhostDone = false;

    public static bool ghostHelped = false;

    public static Vector3 setupPos;
    public static Quaternion setupRot;

    public static Vector3 animatedPos;
    public static Quaternion animatedRot;

    public static Vector3 cameraPos;
    public static Quaternion cameraRot;



    public static bool hasSavedPosition = false;

    public static void Reset()
    {
        grimIntroDone = false;
        wickIntroDone = false;
        firstGhostDone = false;
        ghostHelped = false;

        hasSavedPosition = false;

        setupPos = Vector3.zero;
        setupRot = Quaternion.identity;

        animatedPos = Vector3.zero;
        animatedRot = Quaternion.identity;

        cameraPos = Vector3.zero;
        cameraRot = Quaternion.identity;
    }

}

