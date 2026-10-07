using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ============================================================
//  RiwayatUIController.cs
//  Fun3D Learn - Mengatur tampilan scene Riwayat
//
//  Setup di Unity:
//  1. Attach script ini ke GameObject di scene "Riwayat"
//  2. Buat ScrollView dengan Content (Vertical Layout Group)
//  3. Buat Prefab "HistoryItemPrefab" lalu assign ke inspector
//  4. Hubungkan semua referensi UI di Inspector
// ============================================================

public class RiwayatUIController : MonoBehaviour
{
    [Header("Referensi UI")]
    [Tooltip("Transform Content dari ScrollView")]
    public Transform contentParent;

    [Tooltip("Prefab card untuk setiap item history")]
    public GameObject historyItemPrefab;

    [Tooltip("Panel / Text yang muncul saat history kosong")]
    public GameObject panelKosong;

    [Tooltip("Tombol hapus semua history")]
    public Button btnHapusSemua;

    [Tooltip("Teks jumlah scan (contoh: '5 objek dipindai')")]
    public TextMeshProUGUI txtJumlahScan;

    // ── Unity Lifecycle ────────────────────────────────────

    void Start()
    {
        // Setup tombol hapus semua
        if (btnHapusSemua != null)
            btnHapusSemua.onClick.AddListener(OnHapusSemuaClick);

       
        //// KODE TEST 
        //if (ScanHistoryManager.Instance.JumlahHistory() == 0)
        //{
        //    ScanHistoryManager.Instance.TambahHistory("Apel", "Buah berwarna merah yang manis.", "");
        //    ScanHistoryManager.Instance.TambahHistory("Pisang", "Buah berwarna kuning kaya kalium.", "");
        //    ScanHistoryManager.Instance.TambahHistory("Jeruk", "Buah dengan vitamin C tinggi.", "");
        //}

        RefreshTampilan();
    }

    // ── Public Methods ─────────────────────────────────────

    /// <summary>
    /// Hapus semua card lama lalu buat ulang dari data terbaru.
    /// Dipanggil saat scene dibuka atau setelah hapus item.
    /// </summary>
    public void RefreshTampilan()
    {
        // Bersihkan card yang ada
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        List<ScanHistoryItem> dataHistory = ScanHistoryManager.Instance.GetAllHistory();

        bool historyKosong = dataHistory == null || dataHistory.Count == 0;

        // Tampilkan/sembunyikan panel kosong
        if (panelKosong != null)
            panelKosong.SetActive(historyKosong);

        // Update teks jumlah scan
        if (txtJumlahScan != null)
        {
            txtJumlahScan.text = historyKosong
                ? "Belum ada objek dipindai"
                : $"{dataHistory.Count} objek dipindai";
        }

        if (historyKosong) return;

        // Buat card untuk setiap item history
        for (int i = 0; i < dataHistory.Count; i++)
        {
            GameObject cardObj = Instantiate(historyItemPrefab, contentParent);
            HistoryItemUI cardUI = cardObj.GetComponent<HistoryItemUI>();

            if (cardUI != null)
                cardUI.IsiData(dataHistory[i], i, this);
        }
    }

    // ── Private Methods ────────────────────────────────────

    private void OnHapusSemuaClick()
    {
        // Tampilkan konfirmasi sebelum hapus semua
        // Kalau kamu punya panel konfirmasi, tampilkan di sini
        // Saat ini langsung hapus
        ScanHistoryManager.Instance.HapusSemuaHistory();
        RefreshTampilan();
        Debug.Log("[Riwayat] Semua history dihapus dari UI.");
    }
}
