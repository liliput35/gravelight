using UnityEngine;

public static class SaveData
{
    private const string GrimKey = "GrimIntroDone";
    private const string WickKey = "WickIntroDone";
    private const string GhostKey = "FirstGhostDone";
    private const string GhostHelpedKey = "GhostHelped";

    private const string SetupPosKey = "SetupPos";
    private const string SetupRotKey = "SetupRot";
    private const string AnimatedPosKey = "AnimatedPos";
    private const string AnimatedRotKey = "AnimatedRot";
    private const string CameraPosKey = "CameraPos";
    private const string CameraRotKey = "CameraRot";

    public static bool GrimIntroDone
    {
        get => PlayerPrefs.GetInt(GrimKey, 0) == 1;
        set => PlayerPrefs.SetInt(GrimKey, value ? 1 : 0);
    }

    public static bool WickIntroDone
    {
        get => PlayerPrefs.GetInt(WickKey, 0) == 1;
        set => PlayerPrefs.SetInt(WickKey, value ? 1 : 0);
    }

    public static bool FirstGhostDone
    {
        get => PlayerPrefs.GetInt(GhostKey, 0) == 1;
        set => PlayerPrefs.SetInt(GhostKey, value ? 1 : 0);
    }

    public static bool GhostHelped
    {
        get => PlayerPrefs.GetInt(GhostHelpedKey, 0) == 1;
        set => PlayerPrefs.SetInt(GhostHelpedKey, value ? 1 : 0);
    }

    // Vector3 / Quaternion helper using PlayerPrefs
    public static void SaveVector3(string key, Vector3 value)
    {
        PlayerPrefs.SetFloat(key + "_x", value.x);
        PlayerPrefs.SetFloat(key + "_y", value.y);
        PlayerPrefs.SetFloat(key + "_z", value.z);
    }

    public static Vector3 LoadVector3(string key, Vector3 defaultValue = default)
    {
        return new Vector3(
            PlayerPrefs.GetFloat(key + "_x", defaultValue.x),
            PlayerPrefs.GetFloat(key + "_y", defaultValue.y),
            PlayerPrefs.GetFloat(key + "_z", defaultValue.z)
        );
    }

    public static void SaveQuaternion(string key, Quaternion value)
    {
        PlayerPrefs.SetFloat(key + "_x", value.x);
        PlayerPrefs.SetFloat(key + "_y", value.y);
        PlayerPrefs.SetFloat(key + "_z", value.z);
        PlayerPrefs.SetFloat(key + "_w", value.w);
    }

    public static Quaternion LoadQuaternion(string key, Quaternion defaultValue = default)
    {
        return new Quaternion(
            PlayerPrefs.GetFloat(key + "_x", defaultValue.x),
            PlayerPrefs.GetFloat(key + "_y", defaultValue.y),
            PlayerPrefs.GetFloat(key + "_z", defaultValue.z),
            PlayerPrefs.GetFloat(key + "_w", defaultValue.w)
        );
    }

    public static void Reset()
    {
        PlayerPrefs.DeleteKey(GrimKey);
        PlayerPrefs.DeleteKey(WickKey);
        PlayerPrefs.DeleteKey(GhostKey);
        PlayerPrefs.DeleteKey(GhostHelpedKey);

        PlayerPrefs.DeleteKey(SetupPosKey + "_x");
        PlayerPrefs.DeleteKey(SetupPosKey + "_y");
        PlayerPrefs.DeleteKey(SetupPosKey + "_z");

        PlayerPrefs.DeleteKey(SetupRotKey + "_x");
        PlayerPrefs.DeleteKey(SetupRotKey + "_y");
        PlayerPrefs.DeleteKey(SetupRotKey + "_z");
        PlayerPrefs.DeleteKey(SetupRotKey + "_w");

        PlayerPrefs.DeleteKey(AnimatedPosKey + "_x");
        PlayerPrefs.DeleteKey(AnimatedPosKey + "_y");
        PlayerPrefs.DeleteKey(AnimatedPosKey + "_z");

        PlayerPrefs.DeleteKey(AnimatedRotKey + "_x");
        PlayerPrefs.DeleteKey(AnimatedRotKey + "_y");
        PlayerPrefs.DeleteKey(AnimatedRotKey + "_z");
        PlayerPrefs.DeleteKey(AnimatedRotKey + "_w");

        PlayerPrefs.DeleteKey(CameraPosKey + "_x");
        PlayerPrefs.DeleteKey(CameraPosKey + "_y");
        PlayerPrefs.DeleteKey(CameraPosKey + "_z");

        PlayerPrefs.DeleteKey(CameraRotKey + "_x");
        PlayerPrefs.DeleteKey(CameraRotKey + "_y");
        PlayerPrefs.DeleteKey(CameraRotKey + "_z");
        PlayerPrefs.DeleteKey(CameraRotKey + "_w");

        Debug.Log("Data reset in menu");
    }
}
