using BookingGatewayService.Exceptions;
using BookingGatewayService.Tests.Fake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingGatewayService.Tests
{
    public class BookingGatewayTest
    {
        /// <summary>
        /// Check is StartBooking Implemented
        /// </summary>
        /// <returns></returns>
        public async Task StartBookingImplementationTest()
        {
            var fakeDb = new FakeDBRepository();
            var gateway = BookingGatewayFactory.CreateGateway(fakeDb);

            Assert.NotNull(gateway);
            await gateway.StartBooking();
            await gateway.EndBooking();
        }

        /// <summary>
        /// Check is EndBooking Implemented
        /// </summary>
        /// <returns></returns>
        
        public async Task EndBookingImplementationTest()
        {
            var fakeDb = new FakeDBRepository();
            var gateway = BookingGatewayFactory.CreateGateway(fakeDb);

            Assert.NotNull(gateway);
            await gateway.StartBooking();
            await gateway.EndBooking();

            await Assert.ThrowsAsync<NoStartBookingException>(async () => await gateway.EndBooking());//call endBooking again
        }

        /// <summary>
        /// Check is EndBooking Implemented
        /// </summary>
        /// <returns></returns>

        public async Task EndBookingBeforeStartBookingImplementationTest()
        {
            var fakeDb = new FakeDBRepository();
            var gateway = BookingGatewayFactory.CreateGateway(fakeDb);

            Assert.NotNull(gateway);

            await Assert.ThrowsAsync<NoStartBookingException>(async () => await gateway.EndBooking()); //call end before start
        }

        public async Task CheckBookingTest()
        {
            var fakeDb = new FakeDBRepository();
            var gateway = BookingGatewayFactory.CreateGateway(fakeDb);
            Assert.NotNull(gateway);
            await gateway.StartBooking();
            try
            {
                await gateway.Booking("ref1", 100, "title", "123", "456");
                await gateway.Booking("ref2", 200, "title2", "123", "456");
                await gateway.GetBookingStatuses(new List<string>() { "noneRef"});
            }
            finally
            {
                await gateway.EndBooking();
            }

            Assert.Equal(2, fakeDb.Data.Count());

        }

        public async Task CheckBookingDataTest()
        {
            var fakeDb = new FakeDBRepository();
            var gateway = BookingGatewayFactory.CreateGateway(fakeDb);

            Assert.NotNull(gateway);
            Assert.Empty(fakeDb.Data);

            await gateway.StartBooking();
            try
            {
                await gateway.Booking("ref1", 100m, "title", "123", "456");
                await gateway.Booking("ref2", 200m, "title2", "123", "456");
            }
            finally
            {
                await gateway.EndBooking();
            }

            Assert.Equal(2, fakeDb.Data.Count());
            Assert.Equal(100m, fakeDb.Data[0].Amount);
            Assert.Equal("title", fakeDb.Data[0].TransactionTitle);
            Assert.Equal("123", fakeDb.Data[0].SourceAccountNo);
            Assert.Equal("456", fakeDb.Data[0].DestAccountNo);
            Assert.Equal("ref1", fakeDb.Data[0].UniqueRef);

        }

    }
}
