using UnityEngine;

public class ResetSaveData : MonoBehaviour
{
    private void Awake()
    {
        // Reset all save data for a fresh demo run
        SaveData.Reset();
        PlayerPrefs.Save();
        Debug.Log("Save data reset — starting demo from scratch");
    }
}
