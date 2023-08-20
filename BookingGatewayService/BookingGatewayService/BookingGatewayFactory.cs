namespace BookingGatewayService
{
    internal class BookingGatewayFactory
    {
        /// <summary>
        /// The method should create instance of IBookingGateway
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        public static IBookingGateway CreateGateway(IDBRepository repository) => new BookingGateway(repository);
        
    }
}
