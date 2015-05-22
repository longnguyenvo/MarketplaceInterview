using System.Linq;

namespace Marketplace.Interview.Business.Basket
{
    public interface IShippingCalculator
    {
        decimal CalculateShipping(Basket basket);
        void CalculateShippingWithDeduct(Basket basket, decimal deductAmount);
    }

    public class ShippingCalculator : IShippingCalculator
    {
        public decimal CalculateShipping(Basket basket)
        {
            foreach (var lineItem in basket.LineItems)
            {
                lineItem.ShippingAmount = lineItem.Shipping.GetAmount(lineItem, basket);
                lineItem.ShippingDescription = lineItem.Shipping.GetDescription(lineItem, basket);
            }

            return basket.LineItems.Sum(li => li.ShippingAmount);
        }

        public void CalculateShippingWithDeduct(Basket basket, decimal deductAmount)
        {
            if (basket.LineItems.Any(a1 => a1.Shipping.GetType().Equals(typeof(Shipping.NewPerRegionShipping))))
            {
                if (basket.LineItems.Any(a2 => basket.LineItems.Any(a3 => a3.Id != a2.Id && (a2.SupplierId.Equals(a3.SupplierId) && a2.Shipping.GetType().Name.Equals(a3.Shipping.GetType().Name) && a2.DeliveryRegion.Equals(a3.DeliveryRegion)))))
                {
                    basket.Shipping -= deductAmount;
                }
            }
        }
    }
}