using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GamePush;

namespace Agava.GamePush.Samples
{
    public class ProductCatalogPanel : MonoBehaviour
    {
        [SerializeField] private ProductPanel _productPanelTemplate;
        [SerializeField] private LayoutGroup _productCatalogLayoutGroup;

        private readonly List<ProductPanel> _productPanels = new List<ProductPanel>();

        private void Awake()
        {
            _productPanelTemplate.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GP_Payments.OnFetchProducts += OnFetchProducts;
            GP_Payments.Fetch();

            void OnFetchProducts(List<FetchProducts> fetchProducts)
            {
                GP_Payments.OnFetchProducts -= OnFetchProducts;
                UpdateProductCatalog(fetchProducts);
            }
        }
        
        private void UpdateProductCatalog(List<FetchProducts> products)
        {
            foreach (ProductPanel productPanel in _productPanels)
                Destroy(productPanel.gameObject);

            _productPanels.Clear();

            foreach (FetchProducts product in products)
            {
                ProductPanel productPanel = Instantiate(_productPanelTemplate, _productCatalogLayoutGroup.transform);
                _productPanels.Add(productPanel);

                productPanel.gameObject.SetActive(true);
                productPanel.Product = product;
            }
        }
    }
}
