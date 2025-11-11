using UnityEngine;

public static class SaveData
{
    public static bool grimIntroDone = false;
    public static bool wickIntroDone = false;
    public static bool firstGhostDone = false;

    public static bool ghostHelped = false;

    // Player and camera position/rotation
    public static Vector3 playerPosition;
    public static Quaternion playerRotation;

    

    public static bool hasSavedPosition = false;
}

