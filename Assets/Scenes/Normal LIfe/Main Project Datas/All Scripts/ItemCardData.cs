// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
// public class ItemCardData : MonoBehaviour
// {
//     [SerializeField] private TextMeshProUGUI valueText;
//     [SerializeField] private Image icon;
//
//     [SerializeField] private ItemCollection itemCollectionType;
//     [SerializeField] private ItemTypes itemTypes;
//     [SerializeField] private int cost;
//     
//     private int _value;
//
//     public int Value
//     {
//         set
//         {
//             _value = value;
//             valueText.text = _value.ToString();
//         }
//         get => _value;
//     }
//     public Sprite CardIcon { set => icon.sprite = value; }
//
//     public (ItemCollection, ItemTypes , int) GetData() => (itemCollectionType, itemTypes ,cost);
// }
