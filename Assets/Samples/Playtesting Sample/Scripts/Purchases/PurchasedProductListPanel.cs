using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamePush;

namespace Agava.GamePush.Samples
{
    public class PurchasedProductListPanel : MonoBehaviour
    {
        [SerializeField] private PurchasedProductPanel _purchasedProductPanelTemplate;
        [SerializeField] private LayoutGroup _purchasedProductsLayoutGroup;

        private readonly List<PurchasedProductPanel> _purchasedProductPanels = new List<PurchasedProductPanel>();

        private void Awake()
        {
            _purchasedProductPanelTemplate.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GP_Payments.OnFetchPlayerPurchases += OnFetchPlayerPurchases;
            GP_Payments.Fetch();
            
            void OnFetchPlayerPurchases(List<FetchPlayerPurcahses> fetchPlayerPurcahses)
            {
                GP_Payments.OnFetchPlayerPurchases -= OnFetchPlayerPurchases;
                UpdatePurchasedProducts(fetchPlayerPurcahses);
            }
        }

        private void UpdatePurchasedProducts(List<FetchPlayerPurcahses> purchasedProducts)
        {
            foreach (PurchasedProductPanel purchasedProductPanel in _purchasedProductPanels)
                Destroy(purchasedProductPanel.gameObject);

            _purchasedProductPanels.Clear();

            foreach (FetchPlayerPurcahses purchasedProduct in purchasedProducts)
            {
                PurchasedProductPanel purchasedProductPanel = Instantiate(_purchasedProductPanelTemplate, _purchasedProductsLayoutGroup.transform);
                _purchasedProductPanels.Add(purchasedProductPanel);

                purchasedProductPanel.gameObject.SetActive(true);
                purchasedProductPanel.PurchasedProduct = purchasedProduct;
            }
        }
        
        public void RemovePurchasedProductPanel(PurchasedProductPanel purchasedProductPanel)
        {
            _purchasedProductPanels.Remove(purchasedProductPanel);
        
            Destroy(purchasedProductPanel.gameObject);
        }
    }
}
