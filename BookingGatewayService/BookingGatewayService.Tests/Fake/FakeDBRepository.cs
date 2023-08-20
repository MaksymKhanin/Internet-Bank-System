using BookingGatewayService.Model;

namespace BookingGatewayService.Tests.Fake
{
    internal class FakeDBRepository : IDBRepository
    {
        public IList<TransactionData> Data = new List<TransactionData>();
        public Task<TransactionStatus[]> GetStatuses(string[] uniqueTransactionRefs)
        {
            var data = Data.Where(x => uniqueTransactionRefs != null
                                    && uniqueTransactionRefs.Contains(x.UniqueRef))
                                                            .Select(s => new TransactionStatus() { Status = s.UniqueRef + "Status", UniqueRef = s.UniqueRef }).ToArray();
            
            return Task.FromResult(data);
        }

        public Task SaveBooking(TransactionData transaction)
        {
            Data.Add(transaction);

            return Task.CompletedTask;
        }
    }
}
