namespace InternetBankingService.Model
{
    /// <summary>
    /// Contains Info about Client Account
    /// </summary>
    internal class BankAccount
    {

        /// <summary>
        /// client account number in bank
        /// </summary>
        internal string AccountNo { get; set; }= String.Empty;

        /// <summary>
        /// available amount on account
        /// </summary>
        internal decimal AvailableAmount { get; set; }

    }
}
