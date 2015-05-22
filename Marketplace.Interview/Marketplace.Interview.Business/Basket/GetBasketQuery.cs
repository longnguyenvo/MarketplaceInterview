using System.Web;
using System.Xml.Linq;

namespace Marketplace.Interview.Business.Basket
{
    public class GetBasketQuery : BasketOperationBase, IGetBasketQuery
    {
        private readonly IShippingCalculator _shippingCalculator;

        public GetBasketQuery()
        {
            _shippingCalculator = new ShippingCalculator();
        }

        public Basket Invoke(BasketRequest request)
        {
            var basket = GetBasket();
            basket.Shipping = _shippingCalculator.CalculateShipping(basket);
            _shippingCalculator.CalculateShippingWithDeduct(basket, 0.5m);
            return basket;
        }
    }

    public class BasketRequest { }
}