namespace InternetBankingService.Model
{
    internal class MoneyTransfer
    {
        /// <summary>
        /// Client Account number in the bank
        /// </summary>
        internal string ClientAccountNo { get; set; } = string.Empty;

        /// <summary>
        /// Transfer title
        /// </summary>
        internal string Title { get; set; } = string.Empty;

        /// <summary>
        /// Destination Account(where the funds are transfered)
        /// </summary>
        internal string BeneficiaryAccountNo { get; set; } = string.Empty;
        /// <summary>
        /// Transfer Amount
        /// </summary>
        internal decimal Amount { get; set; }


    }
}
