using InternetBankingService.Model;

namespace InternetBankingService.Tests
{
    public class InternetBankingServiceTests
    {
        private static string CorrectClientAccount = "12345678901234567890000001";
        private static string CorrectClientAccount2 = "12345678901234567890000002";
        private static string CorrectBeneficiaryAccount = "12345678901234567890999999";
        private static decimal ClientAvailableAmount = 100000.00m;
        private static string CorrectTitle = "Transfer Title";

        private InternetBankingService CreateService()
        {
            return new InternetBankingService(
                new List<MoneyTransfer>(),
                new List<BankAccount>()
                {
                    new BankAccount()
                    {
                        AccountNo = CorrectClientAccount,
                        AvailableAmount= ClientAvailableAmount
                    },
                    new BankAccount()
                    {
                        AccountNo = CorrectClientAccount2,
                        AvailableAmount= ClientAvailableAmount
                    }
                });
        }

        [Fact]
        public void CreateTransfer_Add_CorrectMoneyTransfer()
        {
            CreateService().CreateTransfer(CorrectClientAccount, 0.01m, CorrectTitle, CorrectBeneficiaryAccount);
            CreateService().CreateTransfer(CorrectClientAccount, 99999.99m, CorrectTitle, CorrectBeneficiaryAccount);
        }

        [Fact]
        public void CreateTransfer_TryAdd_InCorrectAccountNumbert()
        {
            var action = () => CreateService().CreateTransfer("1234567890", 1, "T", CorrectBeneficiaryAccount);
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void CreateTransfer_TryAdd_InCorrectAmount()
        {
            var action = () => CreateService().CreateTransfer(CorrectClientAccount, 0, CorrectTitle, CorrectBeneficiaryAccount);
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void CreateTransfer_TryAdd_ClientHasNoMoney()
        {
            var action = () => CreateService().CreateTransfer(CorrectClientAccount, ClientAvailableAmount + 100m, CorrectTitle, CorrectBeneficiaryAccount);
            Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        public void CreateTransfer_TryAdd_InCorrectTitle()
        {
            var action = () => CreateService().CreateTransfer(CorrectClientAccount, 1, "T", CorrectBeneficiaryAccount);
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void CreateTransfer_TryAdd_InCorrectBeneficiaryAmount()
        {
            var action = () => CreateService().CreateTransfer(CorrectClientAccount, 1, CorrectTitle, "1234567890");
            Assert.Throws<ArgumentException>(action);
        }

        [Fact]
        public void SearchTransferByTitle()
        {
            var service = CreateService();
            service.CreateTransfer(CorrectClientAccount, 50m, "Transfer1", CorrectBeneficiaryAccount);
            service.CreateTransfer(CorrectClientAccount, 150m, "Transfer2", CorrectBeneficiaryAccount);
            service.CreateTransfer(CorrectClientAccount, 100m, "Other", CorrectBeneficiaryAccount);

            var result = service.SearchTransfer(CorrectClientAccount, null, "Transfer", String.Empty);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.True(result.Any(a => a.ClientAccountNo == CorrectClientAccount && a.Amount == 50m && a.Title == "Transfer1" && a.BeneficiaryAccountNo == CorrectBeneficiaryAccount));
            Assert.True(result.Any(a => a.ClientAccountNo == CorrectClientAccount && a.Amount == 150m && a.Title == "Transfer2" && a.BeneficiaryAccountNo == CorrectBeneficiaryAccount));

            var result2 = service.SearchTransfer(CorrectClientAccount, null, "Other", String.Empty);
            Assert.NotNull(result);
            Assert.Equal(1, result2.Count);

            Assert.True(result.Any(a => a.ClientAccountNo == CorrectClientAccount && a.Amount == 100m && a.Title == "Other" && a.BeneficiaryAccountNo == CorrectBeneficiaryAccount));

        }

        [Fact]
        public void SearchTransferByAmount()
        {
            var service = CreateService();
            service.CreateTransfer(CorrectClientAccount, 250m, "Transfer1", CorrectBeneficiaryAccount);
            service.CreateTransfer(CorrectClientAccount, 300m, "Other 2", CorrectBeneficiaryAccount);

            var result = service.SearchTransfer(CorrectClientAccount, 300m, String.Empty, null);
            Assert.NotNull(result);
            Assert.Equal(1, result.Count);

            Assert.True(result.Any(a => a.ClientAccountNo == CorrectClientAccount && a.Amount == 300m && a.Title == "Other 2" && a.BeneficiaryAccountNo == CorrectBeneficiaryAccount));

        }

        [Fact]
        public void SearchTransferByBeneficiaryAccount()
        {
            var service = CreateService();
            service.CreateTransfer(CorrectClientAccount, 250m, "Transfer1", "12345678901234567890000001");
            service.CreateTransfer(CorrectClientAccount, 300m, "Other 2", "12345678901234567890000002");
            service.CreateTransfer(CorrectClientAccount, 400m, "Other 3", "12345678901234567890000003");
            service.CreateTransfer(CorrectClientAccount, 500m, "Other 4", "12345678901234567890000003");


            var result = service.SearchTransfer(CorrectClientAccount, null, "", "12345678901234567890000003");
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.True(result.Any(a => a.ClientAccountNo == CorrectClientAccount && a.Amount == 400m && a.Title == "Other 3" && a.BeneficiaryAccountNo == "12345678901234567890000003"));
            Assert.True(result.Any(a => a.ClientAccountNo == CorrectClientAccount && a.Amount == 500m && a.Title == "Other 4" && a.BeneficiaryAccountNo == "12345678901234567890000003"));

        }
    }
}