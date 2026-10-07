using UnityEngine;
using Vuforia;

public class HideUIOnTarget : MonoBehaviour
{
    public GameObject uiObject;

    [Header("Data Riwayat (Isi di Inspector)")]
    public string namaBuah;
    [TextArea(3, 5)]
    public string penjelasanBuah;

    [Tooltip("Ketik nama file gambar tanpa .png (Contoh: ikon_pisang)")]
    public string namaFileIkon; 

    private ObserverBehaviour observer;
    private bool wasTracked = false;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
            observer.OnTargetStatusChanged += OnStatusChanged;

        if (uiObject != null)
            uiObject.SetActive(false);
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED ||
                         status.Status == Status.EXTENDED_TRACKED;

        if (uiObject != null)
            uiObject.SetActive(isTracked);

        if (isTracked && !wasTracked)
        {
            if (!string.IsNullOrEmpty(namaBuah) && namaBuah != "DeviceObserver")
            {
                if (ScanHistoryManager.Instance != null)
                {
                    ScanHistoryManager.Instance.SimpanRiwayatBaru(namaBuah, penjelasanBuah, namaFileIkon);
                }
            }
        }

        wasTracked = isTracked;
    }

    void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnStatusChanged;
    }
}