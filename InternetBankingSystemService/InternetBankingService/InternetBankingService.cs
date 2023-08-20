using InternetBankingService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InternetBankingService
{
    internal class InternetBankingService
    {
        /// <summary>
        /// Contains all outgoing money transfers sent by client
        /// </summary>
        internal IList<MoneyTransfer> TransfersHistory { get; set; } = new List<MoneyTransfer>();

        /// <summary>
        /// Contains all account in the bank
        /// </summary>
        internal IList<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

        public InternetBankingService(IList<MoneyTransfer> transfersHistory, IList<BankAccount> bankAccounts)
        {
            TransfersHistory = transfersHistory;
            BankAccounts = bankAccounts;
        }

        internal void CreateTransfer(string clientAccountNo, decimal? amount, string title, string beneficisaryAccountNo)
        {
            if (!Regex.IsMatch(clientAccountNo, @"^\d{26}$") || !Regex.IsMatch(beneficisaryAccountNo, @"^\d{26}$"))
            {
                throw new ArgumentException();
            }

            _ = amount ?? throw new ArgumentException();

            if (amount.Value < 0.01M || amount.Value > 99999.99M)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (title.Length < 3)
            {
                throw new ArgumentException();
            }

            var bankAccount = BankAccounts.FirstOrDefault(x => x.AccountNo == clientAccountNo) ?? throw new ArgumentException();

            if (bankAccount.AvailableAmount < amount)
            {
                throw new ArgumentOutOfRangeException();
            }

            var moneyTransfer = new MoneyTransfer();
            moneyTransfer.Amount = amount.Value;
            moneyTransfer.Title = title;
            moneyTransfer.ClientAccountNo = clientAccountNo;
            moneyTransfer.BeneficiaryAccountNo = beneficisaryAccountNo;

            TransfersHistory.Add(moneyTransfer);

            bankAccount.AvailableAmount -= amount.Value;
        }
        internal IList<MoneyTransfer> SearchTransfer(string clientAccountNo, decimal? amount, string phrase, string beneficisaryAccountNo)
        {
            if (!amount.HasValue && String.IsNullOrEmpty(phrase) && String.IsNullOrEmpty(beneficisaryAccountNo))
            {
                return new List<MoneyTransfer>();
            }

            var transfers = TransfersHistory.Where(x => x.ClientAccountNo == clientAccountNo);
            if (amount.HasValue)
                transfers = transfers.Where(x => x.Amount == amount);
            if (!String.IsNullOrEmpty(phrase))
                transfers = transfers.Where(x => x.Title == phrase);
            if (!String.IsNullOrEmpty(beneficisaryAccountNo))
                transfers = transfers.Where(x => x.BeneficiaryAccountNo == beneficisaryAccountNo);

            return transfers.ToList();
        }

    }
}
