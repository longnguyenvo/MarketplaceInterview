using System.Collections.Generic;
using NUnit.Framework;
using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Shipping;

namespace Marketplace.Interview.Tests
{
    [TestFixture]
    public class ShippingOptionTests
    {
        [Test]
        public void FlatRateShippingOptionTest()
        {
            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.5m};
            var shippingAmount = flatRateShippingOption.GetAmount(new LineItem(), new Basket());

            Assert.That(shippingAmount, Is.EqualTo(1.5m), "Flat rate shipping not correct.");
        }

        [Test]
        public void PerRegionShippingOptionTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
                                              {
                                                  PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
                                              };

            var shippingAmount = perRegionShippingOption.GetAmount(new LineItem() {DeliveryRegion = RegionShippingCost.Regions.Europe}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(1.5m));

            shippingAmount = perRegionShippingOption.GetAmount(new LineItem() { DeliveryRegion = RegionShippingCost.Regions.UK}, new Basket());
            Assert.That(shippingAmount, Is.EqualTo(.75m));
        }

        [Test]
        public void BasketShippingTotalTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping {FlatRate = 1.1m};

            var basket = new Basket()
                             {
                                 LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
                             };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(3.35m));
        }


        [Test]
        public void BasketShippingTotalWithDeductTest()
        {
            var perRegionShippingOption = new PerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var newperRegionShippingOption1 = new NewPerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var newperRegionShippingOption2 = new NewPerRegionShipping()
            {
                PerRegionCosts = new[]
                                                                       {
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.UK,
                                                                                   Amount = .75m
                                                                               },
                                                                           new RegionShippingCost()
                                                                               {
                                                                                   DestinationRegion =
                                                                                       RegionShippingCost.Regions.Europe,
                                                                                   Amount = 1.5m
                                                                               }
                                                                       },
            };

            var flatRateShippingOption = new FlatRateShipping { FlatRate = 1.1m };

            var basket = new Basket()
            {
                LineItems = new List<LineItem>
                                                 {
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.UK,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem()
                                                         {
                                                             DeliveryRegion = RegionShippingCost.Regions.Europe,
                                                             Shipping = perRegionShippingOption
                                                         },
                                                     new LineItem() {Shipping = flatRateShippingOption},
                                                 }
            };

            var calculator = new ShippingCalculator();

            decimal basketShipping = calculator.CalculateShipping(basket);

            Assert.That(basketShipping, Is.EqualTo(7.35m));
        }
    }
}