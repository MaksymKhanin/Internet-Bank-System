using BookingGatewayService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingGatewayService
{
    internal interface IBookingGateway
    {

        /// <summary>
        /// Start booking process by thread
        /// </summary>
        Task StartBooking();

        /// <summary>
        /// Finish booking process by thread
        /// </summary>
        Task EndBooking();

        /// <summary>
        /// Booking transaction (Save transaction in DB using IDBRepository).
        /// </summary>
        /// <param name="uniqueReference"></param>
        /// <param name="amount"></param>
        /// <param name="transactionTitle"></param>
        /// <param name="srcAccountNo"></param>
        /// <param name="destAccountNo"></param>
        Task Booking(string uniqueReference, decimal amount, string transactionTitle, string srcAccountNo, string destAccountNo);

        /// <summary>
        /// Return transaction statuses
        /// </summary>
        /// <param name="uniqueTransactionRefs"></param>
        /// <returns></returns>
        Task<IList<TransactionStatus>> GetBookingStatuses(IList<string> uniqueTransactionRefs);
    }
}
