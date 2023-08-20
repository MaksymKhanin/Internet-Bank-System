using BookingGatewayService.Exceptions;
using BookingGatewayService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BookingGatewayService
{
    internal class BookingGateway : IBookingGateway
    {
        private readonly object _locker = new();
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        private static bool _isInProgress = false;
        private static bool _isBookingStatusInProgress = false;
        private int _threadId = new();

        private readonly IDBRepository _dBRepository;

        public BookingGateway(IDBRepository dBRepository) => _dBRepository = dBRepository;

        public Task Booking(string uniqueReference, decimal amount, string transactionTitle, string srcAccountNo, string destAccountNo)
        {
            if (!_isInProgress) throw new NoStartBookingException();

            var transaction = new TransactionData();
            transaction.UniqueRef = uniqueReference;
            transaction.Amount = amount;
            transaction.TransactionTitle = transactionTitle;
            transaction.SourceAccountNo = srcAccountNo;
            transaction.DestAccountNo = destAccountNo;

            return _dBRepository.SaveBooking(transaction);
        }

        public Task EndBooking()
        {
            lock (_locker)
            {
                if (!_isInProgress) throw new NoStartBookingException();

                return Task.CompletedTask;
            }
        }

        public async Task<IList<TransactionStatus>> GetBookingStatuses(IList<string> uniqueTransactionRefs)
        {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;

            await _semaphoreSlim.WaitAsync();

            try
            {
                if (_isInProgress && currentThreadId != _threadId) throw new BookingInProgressException();

                _isBookingStatusInProgress = true;

                var statuses = await _dBRepository.GetStatuses(uniqueTransactionRefs.ToArray());

                if (uniqueTransactionRefs == null || uniqueTransactionRefs.Count() == 0)
                {
                    return new List<TransactionStatus>();
                }

                _isBookingStatusInProgress = false;

                return statuses;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        public Task StartBooking()
        {
            lock (_locker)
            {
                if (_isInProgress) throw new BookingInProgressException();
                if (_isBookingStatusInProgress) throw new ReadOperationInProgressException();

                _threadId = Thread.CurrentThread.ManagedThreadId;

                _isInProgress = true;

                return Task.CompletedTask;
            }
        }
    }
}
