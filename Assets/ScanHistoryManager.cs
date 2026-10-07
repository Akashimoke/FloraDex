using System;
using System.Collections.Generic;
using UnityEngine;

// ============================================================
//  ScanHistoryManager.cs
//  Fun3D Learn - Menyimpan & mengambil riwayat scan objek AR
//  Attach script ini ke GameObject "DontDestroyOnLoad" atau
//  buat GameObject baru bernama "HistoryManager" di scene Main
// ============================================================

[System.Serializable]
public class ScanHistoryItem
{
    public string namaObjek;       // Nama buah / objek yang di-scan
    public string penjelasan;      // Penjelasan singkat objek
    public string tanggal;         // Tanggal scan (format: dd/MM/yyyy)
    public string waktu;           // Waktu scan (format: HH:mm)
    public string ikonPath;        // Nama file ikon di Resources/IconFruit (opsional)
}

[System.Serializable]
public class ScanHistoryList
{
    public List<ScanHistoryItem> items = new List<ScanHistoryItem>();
}

public class ScanHistoryManager : MonoBehaviour
{
    // ── Singleton ──────────────────────────────────────────
    public static ScanHistoryManager Instance { get; private set; }

    private const string SAVE_KEY = "ScanHistory";
    private ScanHistoryList historyList = new ScanHistoryList();

    // ── Unity Lifecycle ────────────────────────────────────
    void Awake()
    {
        // Singleton: hanya boleh ada 1 instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadHistory();
    }

    // ── Public Methods ─────────────────────────────────────

    /// <summary>
    /// Simpan hasil scan baru ke history.
    /// Panggil method ini dari script AR saat objek berhasil di-scan.
    /// </summary>
    /// <param name="namaObjek">Nama buah/objek yang di-scan</param>
    /// <param name="penjelasan">Deskripsi/penjelasan objek</param>
    /// <param name="ikonPath">Nama file ikon di Resources/IconFruit (boleh kosong)</param>
    public void SimpanRiwayatBaru(string namaObjek, string penjelasan, string ikonPath = "")
    {
        // Cek apakah objek yang sama sudah pernah di-scan hari ini
        // (opsional: hapus blok ini kalau mau tetap disimpan duplikat)
        string hariIni = DateTime.Now.ToString("dd/MM/yyyy");
        bool sudahAdaHariIni = historyList.items.Exists(item =>
            item.namaObjek == namaObjek && item.tanggal == hariIni
        );

        if (sudahAdaHariIni)
        {
            Debug.Log($"[History] {namaObjek} sudah pernah di-scan hari ini, tidak disimpan ulang.");
            return;
        }

        ScanHistoryItem item = new ScanHistoryItem
        {
            namaObjek   = namaObjek,
            penjelasan  = penjelasan,
            tanggal     = hariIni,
            waktu       = DateTime.Now.ToString("HH:mm"),
            ikonPath    = ikonPath
        };

        // Tambahkan di posisi paling atas (terbaru duluan)
        historyList.items.Insert(0, item);

        SaveHistory();
        Debug.Log($"[History] Tersimpan: {namaObjek} pada {item.tanggal} {item.waktu}");
    }

    /// <summary>
    /// Ambil semua data history untuk ditampilkan di UI.
    /// </summary>
    public List<ScanHistoryItem> GetAllHistory()
    {
        return historyList.items;
    }

    /// <summary>
    /// Hapus seluruh history.
    /// </summary>
    public void HapusSemuaHistory()
    {
        historyList.items.Clear();
        PlayerPrefs.DeleteKey(SAVE_KEY);
        PlayerPrefs.Save();
        Debug.Log("[History] Semua history dihapus.");
    }

    /// <summary>
    /// Hapus satu item history berdasarkan index.
    /// </summary>
    public void HapusHistory(int index)
    {
        if (index >= 0 && index < historyList.items.Count)
        {
            Debug.Log($"[History] Menghapus: {historyList.items[index].namaObjek}");
            historyList.items.RemoveAt(index);
            SaveHistory();
        }
    }

    /// <summary>
    /// Jumlah total history yang tersimpan.
    /// </summary>
    public int JumlahHistory()
    {
        return historyList.items.Count;
    }

    // ── Private Methods ────────────────────────────────────

    private void SaveHistory()
    {
        string json = JsonUtility.ToJson(historyList);
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadHistory()
    {
        if (PlayerPrefs.HasKey(SAVE_KEY))
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);
            historyList = JsonUtility.FromJson<ScanHistoryList>(json);
            Debug.Log($"[History] Loaded {historyList.items.Count} item(s).");
        }
        else
        {
            historyList = new ScanHistoryList();
            Debug.Log("[History] Tidak ada history tersimpan, mulai baru.");
        }
    }
}
