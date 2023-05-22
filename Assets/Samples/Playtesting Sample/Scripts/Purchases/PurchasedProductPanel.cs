using UnityEngine;
using UnityEngine.UI;
using GamePush;

namespace Agava.GamePush.Samples
{
    public class PurchasedProductPanel : MonoBehaviour
    {
        [SerializeField] private Text _purchasedProductIdText;
        [SerializeField] private PurchasedProductListPanel _purchasedProductListPanel;

        private FetchPlayerPurcahses _purchasedProduct;

        public FetchPlayerPurcahses PurchasedProduct
        {
            set
            {
                _purchasedProduct = value;

                _purchasedProductIdText.text = value.productId.ToString();
            }
        }

        public void OnConsumeButtonClick()
        {
            GP_Payments.Consume(_purchasedProduct.productId.ToString(), onConsumeSuccess: (productId) =>
            {
                Debug.Log($"Consumed {productId}");
                _purchasedProductListPanel.RemovePurchasedProductPanel(this);
            });
        }
    }
}
