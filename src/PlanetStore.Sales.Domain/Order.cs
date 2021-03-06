using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using PlanetStore.Core.DomainObjects;

namespace PlanetStore.Sales.Domain
{
    public class Order : Entity, IAggregateRoot
    {
        public static int MAX_UNITS_ITEM => 15;
        public static int MIN_UNITS_ITEM => 1;

        public Guid CustomerId { get; private set; }
        public decimal TotalValue { get; private set; }
        public decimal Discount { get; private set; }
        public OrderStatus OrderStatus { get; private set; }

        public bool VoucherUtilizado { get; private set; }
        public Voucher Voucher { get; private set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var result = voucher.IsValidForUse();
            if (!result.IsValid) return result;

            Voucher = voucher;
            VoucherUtilizado = true;

            CalculateTotalValueWithDiscount();

            return result;
        }

        public void CalculateTotalValueWithDiscount()
        {
            if (!VoucherUtilizado) return;

            decimal discount = 0;

            if (Voucher.DiscountType == DiscountTypeVoucher.Value)
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                }
            }
            else
            {
                if (Voucher.DiscountPercentage.HasValue)
                {
                    discount = (TotalValue * Voucher.DiscountPercentage.Value) / 100;
                }
            }

            TotalValue -= discount;
            if (TotalValue < 0) TotalValue = 0;
            Discount = discount;
        }

        public void CalculateTotalValueOrder()
        {
            TotalValue = OrderItems.Sum(i => i.CalculateValue());
            CalculateTotalValueWithDiscount();
        }

        private bool OrderItemExisting(OrderItem orderItem)
        {
            return _orderItems.Any(p => p.ProductId == orderItem.ProductId);
        }

        private void ValidateItemNonExistent(OrderItem item)
        {
            if (!OrderItemExisting(item)) throw new DomainException($"Item nonexistent in Order");
        }

        private void ValidateQuantityItemAllowed(OrderItem item)
        {
            var quantityItems = item.Quantity;
            if (OrderItemExisting(item))
            {
                var itemExisting = _orderItems.FirstOrDefault(p => p.ProductId == item.ProductId);
                quantityItems += itemExisting.Quantity;
            }

            if (quantityItems > MAX_UNITS_ITEM) throw new DomainException($"Maximum units {MAX_UNITS_ITEM} per product above the allowed");
        }

        public void AddItem(OrderItem orderItem)
        {
            ValidateQuantityItemAllowed(orderItem);

            if (OrderItemExisting(orderItem))
            {
                var itemExisting = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);

                itemExisting.AddUnits(orderItem.Quantity);
                orderItem = itemExisting;
                _orderItems.Remove(itemExisting);
            }

            _orderItems.Add(orderItem);
            CalculateTotalValueOrder();
        }

        public void UpdateItem(OrderItem item)
        {
            ValidateItemNonExistent(item);
            if (item.Quantity > MAX_UNITS_ITEM) throw new DomainException($"Maximum units {MAX_UNITS_ITEM} per product above the allowed");

            var itemExisting = _orderItems.FirstOrDefault(p => p.ProductId == item.ProductId);

            _orderItems.Remove(itemExisting);
            _orderItems.Add(item);

            CalculateTotalValueOrder();
        }

        public void RemoveItem(OrderItem item)
        {
            ValidateItemNonExistent(item);

            _orderItems.Remove(item);

            CalculateTotalValueOrder();
        }


        public void CreateDraft()
        {
            OrderStatus = OrderStatus.draft;
        }

        public static class OrderFactory
        {
            public static Order NewOrderDraft(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                };

                order.CreateDraft();
                return order;
            }
        }
    }
}
