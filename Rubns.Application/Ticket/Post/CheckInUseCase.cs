namespace Rubns.Application.Ticket.Post
{
    internal sealed class CheckInUseCase : ICheckInPort<Response>
    {
        ILogInRepository LogInRepository { get; }
        ILogger Logger { get; }
        ITicketRepository TicketRepository { get; }

        public CheckInUseCase(ILogInRepository logInRepository, ITicketRepository ticketRepository, ILogger logger)
        {
            LogInRepository = logInRepository;
            TicketRepository = ticketRepository;
            Logger = logger;
        }


        public async Task<Response> CheckIn(CheckDTO checkTicket)
        {
            Response response = new();

            try
            {

                var userEmail = await LogInRepository.GetUserByEmailAsync(checkTicket.Email);

                var userPhone = await LogInRepository.GetUserByPhoneAsync(checkTicket.Phone);

                if (userEmail is { UserID: <= 0 } && userPhone is { UserID: <= 0 })
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.Message = "El usuario no existe.";

                    return response;
                }
                int userID = userEmail.UserID > 0 ? userEmail.UserID : userPhone.UserID;
                var checkInResult = await TicketRepository.InsertCheckAsync(checkTicket, userID);

                if (checkInResult > 0)
                {
                    response.StatusCode = System.Net.HttpStatusCode.OK;
                    response.Message = "Check-in successful.";
                }
                else
                {
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    response.Message = "Check-in failed.";
                }

            }
            catch (Exception e)
            {
                Logger.Error(e, "LogIn an error occurred: {ErrorMessage}", e.Message);

            }

            return response;
        }
    }
}
