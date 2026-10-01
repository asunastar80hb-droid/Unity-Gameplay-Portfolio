// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// public class FarmingCropButton : MonoBehaviour
// {
//     [SerializeField] private Button button;
//     [SerializeField] private TMP_Text cropNameText;
//     [SerializeField] private TMP_Text seedCountText;
//     [SerializeField] private Image cropIcon;
//
//     private CropData _cropData;
//     private FarmingUIManager _uiManager;
//
//     public void Setup(CropData cropData, FarmingUIManager uiManager)
//     {
//         _cropData = cropData;
//         _uiManager = uiManager;
//
//         gameObject.SetActive(true);
//
//         cropNameText.text = cropData.CropName;
//
//         if (cropIcon != null)
//         {
//             ItemData seedData = Inventory.Instance.GetItemData(cropData.SeedItem);
//
//             if (seedData != null)
//                 cropIcon.sprite = seedData.Icon;
//         }
//
//         RefreshSeedCount();
//
//         button.onClick.RemoveAllListeners();
//         button.onClick.AddListener(OnClicked);
//     }
//     public void RefreshSeedCount()
//     {
//         if (_cropData == null)
//             return;
//
//         int count = Inventory.Instance.GetItemCount(_cropData.SeedItem);
//
//         seedCountText.text = count.ToString();
//
//         button.interactable = count > 0;
//     }
//
//     public void Hide()
//     {
//         gameObject.SetActive(false);
//
//         _cropData = null;
//         _uiManager = null;
//     }
//
//     private void OnClicked()
//     {
//         if (_cropData == null || _uiManager == null)
//             return;
//
//         _uiManager.SelectCrop(_cropData);
//     }
// }