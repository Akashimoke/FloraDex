using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ============================================================
//  HistoryItemUI.cs
//  Fun3D Learn - Script untuk setiap card item di list Riwayat
//
//  Cara pakai:
//  1. Buat Prefab "HistoryItemPrefab" (sebuah UI Panel/Card)
//  2. Attach script ini ke Prefab tersebut
//  3. Hubungkan semua referensi UI di Inspector
// ============================================================

public class HistoryItemUI : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Text nama buah/objek")]
    public TextMeshProUGUI txtNamaObjek;

    [Tooltip("Text penjelasan singkat")]
    public TextMeshProUGUI txtPenjelasan;

    [Tooltip("Text tanggal dan waktu scan")]
    public TextMeshProUGUI txtTanggalWaktu;

    [Tooltip("Image ikon buah (opsional)")]
    public Image imgIkon;

    [Tooltip("Tombol hapus item ini")]
    public Button btnHapus;

    // Index item ini di dalam list history
    private int itemIndex;

    // Referensi ke RiwayatUIController untuk refresh setelah hapus
    private RiwayatUIController controller;

    // ── Public Method ──────────────────────────────────────

    /// <summary>
    /// Isi data card ini dengan data dari ScanHistoryItem.
    /// Dipanggil oleh RiwayatUIController saat membuat card.
    /// </summary>
    public void IsiData(ScanHistoryItem data, int index, RiwayatUIController ctrl)
    {
        itemIndex  = index;
        controller = ctrl;

        // Isi teks
        if (txtNamaObjek != null)
            txtNamaObjek.text = data.namaObjek;

        if (txtPenjelasan != null)
        {
            // Tampilkan maks 60 karakter + "..." agar card tidak terlalu panjang
            string preview = data.penjelasan.Length > 60
                ? data.penjelasan.Substring(0, 60) + "..."
                : data.penjelasan;
            txtPenjelasan.text = preview;
        }

        if (txtTanggalWaktu != null)
            txtTanggalWaktu.text = $"{data.tanggal}  {data.waktu}";

        // Load ikon dari Resources/IconFruit jika ada
        if (imgIkon != null)
        {
            if (!string.IsNullOrEmpty(data.ikonPath))
            {
                Sprite ikon = Resources.Load<Sprite>($"IconFruit/{data.ikonPath}");
                if (ikon != null)
                    imgIkon.sprite = ikon;
                else
                    imgIkon.gameObject.SetActive(false); // sembunyikan jika tidak ditemukan
            }
            else
            {
                imgIkon.gameObject.SetActive(false);
            }
        }

        // Setup tombol hapus
        if (btnHapus != null)
        {
            btnHapus.onClick.RemoveAllListeners();
            btnHapus.onClick.AddListener(OnHapusClick);
        }
    }

    // ── Private Methods ────────────────────────────────────

    private void OnHapusClick()
    {
        ScanHistoryManager.Instance.HapusHistory(itemIndex);

        // Refresh tampilan setelah hapus
        if (controller != null)
            controller.RefreshTampilan();
    }
}
