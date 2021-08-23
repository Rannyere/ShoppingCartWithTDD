using System;
using FluentValidation;
using FluentValidation.Results;
using PlanetStore.Core.Messages;
using PlanetStore.Sales.Application.Commands;
using PlanetStore.Sales.Domain;

namespace PlanetStore.Sales.Application.Commands
{
    public class AddItemOrderCommand : Command
    {
        public Guid CustomerId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitValue { get; set; }

        public AddItemOrderCommand(Guid customerId, Guid productId, string productName, int quantity, decimal unitValue)
        {
            CustomerId = customerId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddItemOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}

public class AddItemOrderCommandValidation : AbstractValidator<AddItemOrderCommand>
{
    public static string CustomerIdErrorMsg => "Invalid customer id";
    public static string ProductIdErrorMsg => "Invalid product id";
    public static string ProductNameErrorMsg => "The product name was not informed";
    public static string QuantityMaxErrorMsg => $"The maximum quantity of an item is {Order.MAX_UNITS_ITEM}";
    public static string QuantityMinErrorMsg => "The minimum quantity of an item is 1";
    public static string UnitValueErrorMsg => "Item value must be greater than 0";

    public AddItemOrderCommandValidation()
    {
        RuleFor(c => c.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage(CustomerIdErrorMsg);

        RuleFor(c => c.ProductId)
            .NotEqual(Guid.Empty)
            .WithMessage(ProductIdErrorMsg);

        RuleFor(c => c.ProductName)
            .NotEmpty()
            .WithMessage(ProductNameErrorMsg);

        RuleFor(c => c.Quantity)
            .GreaterThan(0)
            .WithMessage(QuantityMinErrorMsg)
            .LessThanOrEqualTo(Order.MAX_UNITS_ITEM)
            .WithMessage(QuantityMaxErrorMsg);

        RuleFor(c => c.UnitValue)
            .GreaterThan(0)
            .WithMessage(UnitValueErrorMsg);
    }
}

