using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingGatewayService.Model
{
    internal class TransactionData
    {
        /// <summary>
        /// Amount
        /// </summary>
        internal decimal Amount { get; set; }

        /// <summary>
        /// TransactionTitle
        /// </summary>
        internal string TransactionTitle { get; set; } = String.Empty;

        /// <summary>
        /// SourceAccountNo
        /// </summary>
        internal string SourceAccountNo { get; set; } = String.Empty;

        /// <summary>
        /// DestAccountNo
        /// </summary>
        internal string DestAccountNo { get; set; } = String.Empty;

        /// <summary>
        /// UniqueRef
        /// </summary>
        internal string UniqueRef { get; set; } = String.Empty;


    }
}
