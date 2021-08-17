using System;
using FluentValidation;
using FluentValidation.Results;

namespace PlanetStore.Sales.Domain
{
    public class Voucher
    {
        public string Code { get; private set; }
        public decimal? DiscountPercentage { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public DiscountTypeVoucher DiscountType { get; private set; }
        public int Quantity { get; private set; }
        public DateTime DateValidity { get; private set; }
        public bool Activ { get; private set; }
        public bool Usage { get; private set; }

        public Voucher(string code, decimal? discountPercentage, decimal? discountValue, DiscountTypeVoucher discountType, int quantity, DateTime dateValidity, bool activ, bool usage)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
            DiscountValue = discountValue;
            DiscountType = discountType;
            Quantity = quantity;
            DateValidity = dateValidity;
            Activ = activ;
            Usage = usage;
        }

        public ValidationResult IsValidForUse()
        {
            return new VoucherValidation().Validate(this);
        }
    }

    public class VoucherValidation : AbstractValidator<Voucher>
    {
        public static string CodeErroMsg => "Voucher without valid code.";
        public static string DateValidityErroMsg => "This voucher has expired.";
        public static string ActivErroMsg => "This voucher is no longer valid.";
        public static string UsageErroMsg => "This voucher has already been used.";
        public static string QuantityErroMsg => "This voucher is no longer available.";
        public static string DiscountValueErroMsg => "The discount amount must be greater than 0";
        public static string DiscountPercentageErroMsg => "The discount percentage value must be greater than 0";

        public VoucherValidation()
        {
            RuleFor(c => c.Code)
                .NotEmpty()
                .WithMessage(CodeErroMsg);

            RuleFor(c => c.DateValidity)
                .Must(DataVencimentoSuperiorAtual)
                .WithMessage(DateValidityErroMsg);

            RuleFor(c => c.Activ)
                .Equal(true)
                .WithMessage(ActivErroMsg);

            RuleFor(c => c.Usage)
                .Equal(false)
                .WithMessage(UsageErroMsg);

            RuleFor(c => c.Quantity)
                .GreaterThan(0)
                .WithMessage(QuantityErroMsg);

            When(f => f.DiscountType == DiscountTypeVoucher.Value, () =>
            {
                RuleFor(f => f.DiscountValue)
                    .NotNull()
                    .WithMessage(DiscountValueErroMsg)
                    .GreaterThan(0)
                    .WithMessage(DiscountValueErroMsg);
            });

            When(f => f.DiscountType == DiscountTypeVoucher.Percentage, () =>
            {
                RuleFor(f => f.DiscountPercentage)
                    .NotNull()
                    .WithMessage(DiscountPercentageErroMsg)
                    .GreaterThan(0)
                    .WithMessage(DiscountPercentageErroMsg);
            });
        }

        protected static bool DataVencimentoSuperiorAtual(DateTime dataValidade)
        {
            return dataValidade >= DateTime.Now;
        }
    }
}

