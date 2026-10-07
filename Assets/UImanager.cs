using UnityEngine;
using Vuforia;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public GameObject uiScript;
    private Dictionary<ObserverBehaviour, bool> targetStates = new Dictionary<ObserverBehaviour, bool>();

    void Start()
    {
        ObserverBehaviour[] allTargets = FindObjectsByType<ObserverBehaviour>(FindObjectsSortMode.None);

        foreach (ObserverBehaviour target in allTargets)
        {
            targetStates[target] = false; // Semua false di awal
            target.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED ||
                         status.Status == Status.EXTENDED_TRACKED;

        targetStates[behaviour] = isTracked;

        bool anyTracked = false;
        foreach (var state in targetStates.Values)
        {
            if (state) { anyTracked = true; break; }
        }

        // Simpan riwayat saat berhasil scan
        if (isTracked && ScanHistoryManager.Instance != null)
        {
            //ScanHistoryManager.Instance.TambahHistory(behaviour.TargetName, "", "");
        }

        uiScript.SetActive(!anyTracked);
    }
}