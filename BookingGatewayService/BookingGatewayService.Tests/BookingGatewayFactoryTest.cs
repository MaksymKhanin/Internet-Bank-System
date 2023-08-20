using BookingGatewayService.Tests.Fake;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingGatewayService.Tests
{
    internal class BookingGatewayFactoryTest
    {
        public void CheckIsBookingGatewayImplemented()
        {
            Func<IBookingGateway> f = () => { return BookingGatewayFactory.CreateGateway(new FakeDBRepository()); };
            f.Should().NotBeNull();
        }
    }
}
