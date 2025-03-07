﻿using Payment.Core.DTOs.BankDtos;
using System.ComponentModel.DataAnnotations;

namespace Payment.Core.DTOs.BeneficiaryDto
{
    public class BeneficiaryRequestDto
    {
        public string RecipientType { get; set; } = "nuban";

        [Required]
        public string AccountNumber { get; set; } = null!;

        [Required]
        public string AccountName { get; set; } = null!;

        [Required]
        public BeneficiaryBankDto BeneficiaryBank { get; set; } = null!;
    }
}
