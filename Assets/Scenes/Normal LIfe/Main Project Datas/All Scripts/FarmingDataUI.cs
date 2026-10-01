// using TMPro;
// using UnityEngine;
//
// public class FarmingDataUI : MonoBehaviour
// {
//     [Header("UI References")]
//     [SerializeField] private TMP_Text fruitCountText;
//     [SerializeField] private TMP_Text seedCountText;
//
//     private void OnEnable()
//     {
//         if (Inventory.Instance == null)
//             return;
//
//         Inventory.Instance.OnItemChanged += OnItemChanged;
//
//         Refresh();
//     }
//
//     private void OnDisable()
//     {
//         if (Inventory.Instance == null)
//             return;
//
//         Inventory.Instance.OnItemChanged -= OnItemChanged;
//     }
//
//     private void OnItemChanged(ItemTypes itemType)
//     {
//         Refresh();
//     }
//
//     private void Refresh()
//     {
//         if (Inventory.Instance == null)
//             return;
//
//         int tomatoCount =
//             Inventory.Instance.GetItemCount(ItemTypes.Tomato);
//
//         int tomatoSeedCount =
//             Inventory.Instance.GetItemCount(ItemTypes.TomatoSeed);
//
//         if (fruitCountText != null)
//             fruitCountText.text = tomatoCount.ToString("D3");
//
//         if (seedCountText != null)
//             seedCountText.text = tomatoSeedCount.ToString("D3");
//     }
// }