using UnityEngine;

public static class SaveData
{
    public static bool grimIntroDone = false;
    public static bool wickIntroDone = false;
    public static bool firstGhostDone = false;

    public static bool ghostHelped = false;

    // Player position/rotation
    public static Vector3 playerPosition;
    public static Quaternion playerRotation;

    // Camera position/rotation (if needed)
    public static Vector3 cameraPosition;
    public static Quaternion cameraRotation;

    public static bool hasSavedPosition = false;
}

