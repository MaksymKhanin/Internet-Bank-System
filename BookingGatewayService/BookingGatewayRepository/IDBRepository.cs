using BookingGatewayService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingGatewayService
{
    internal interface IDBRepository
    {
        /// <summary>
        /// Save transaction data in repository
        /// </summary>
        /// <param name="transaction"></param>
        Task SaveBooking(TransactionData transaction);

        /// <summary>
        /// Return transaction statuses by transaction references
        /// </summary>
        /// <param name="uniqueTransactionRefs"></param>
        /// <returns></returns>
        Task<TransactionStatus[]> GetStatuses(string[] uniqueTransactionRefs);
    }
}
