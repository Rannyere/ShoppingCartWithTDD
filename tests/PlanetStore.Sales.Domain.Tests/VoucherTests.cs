using System;
using System.Linq;
using Xunit;

namespace PlanetStore.Sales.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validate Voucher Type Value Valid")]
        [Trait("Category", " Voucher")]
        public void Voucher_ValidateVoucherTypeValueValid_MustBeValid()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-TEST", null, 15, DiscountTypeVoucher.Value, 1, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.IsValidForUse();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Type Value Invalid")]
        [Trait("Category", " Voucher")]
        public void Voucher_ValidateVoucherTypeValueInvalid_MustBeInvalid()
        {
            // Arrange
            var voucher = new Voucher("", null, null, DiscountTypeVoucher.Value, 0, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.IsValidForUse();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherValidation.ActivErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.CodeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.DateValidityErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.QuantityErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.UsageErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.DiscountValueErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Validate Voucher Type Percentage Valid")]
        [Trait("Category", " Voucher")]
        public void Voucher_ValidateVoucherTypePercentageValid_MustBeValid()
        {
            // Arrange
            var voucher = new Voucher("PROMO-15-TEST", 15, null, DiscountTypeVoucher.Percentage, 1, DateTime.Now.AddDays(15), true, false);

            // Act
            var result = voucher.IsValidForUse();

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate Voucher Type Percentage Invalid")]
        [Trait("Category", " Voucher")]
        public void Voucher_ValidateVoucherTypePercentageInvalid_MustBeInvalid()
        {
            // Arrange
            var voucher = new Voucher("", null, null, DiscountTypeVoucher.Percentage, 0, DateTime.Now.AddDays(-1), false, true);

            // Act
            var result = voucher.IsValidForUse();

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherValidation.ActivErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.CodeErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.DateValidityErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.QuantityErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.UsageErroMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.DiscountPercentageErroMsg, result.Errors.Select(c => c.ErrorMessage));
        }
    }
}
