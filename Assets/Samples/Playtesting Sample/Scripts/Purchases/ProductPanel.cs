using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GamePush;

namespace Agava.GamePush.Samples
{
    public class ProductPanel : MonoBehaviour
    {
        [SerializeField] private RawImage _productImage;
        [SerializeField] private Text _productIdText;

        private FetchProducts _product;

        public FetchProducts Product
        {
            set
            {
                _product = value;

                _productIdText.text = value.tag;
                
                if (Uri.IsWellFormedUriString(value.icon, UriKind.Absolute))
                    StartCoroutine(DownloadAndSetProductImage(value.icon));
            }
        }

        private IEnumerator DownloadAndSetProductImage(string imageUrl)
        {
            var remoteImage = new RemoteImage(imageUrl);
            remoteImage.Download();

            while (!remoteImage.IsDownloadFinished)
                yield return null;

            if (remoteImage.IsDownloadSuccessful)
                _productImage.texture = remoteImage.Texture;
        }

        public void OnPurchaseButtonClick()
        {
            GP_Payments.Purchase(_product.tag, onPurchaseSuccess: (productTag) =>
            {
                Debug.Log($"Purchased {productTag}");
            });
        }

        public void OnPurchaseAndConsumeButtonClick()
        {
            GP_Payments.Purchase(_product.tag, onPurchaseSuccess: (productTag) =>
            {
                Debug.Log($"Purchased {productTag}");

                GP_Payments.Consume(_product.tag, onConsumeSuccess: (consumeProductTag) =>
                {
                    Debug.Log($"Consumed {consumeProductTag}");
                });
            });
        }
    }
}
