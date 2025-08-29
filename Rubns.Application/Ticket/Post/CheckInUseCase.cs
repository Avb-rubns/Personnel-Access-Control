namespace Rubns.Application.Ticket.Post
{
    internal sealed class CheckInUseCase : ICheckPort<Response>
    {
        IUserRepositoryDapper LogInRepository { get; }
        ILogger Logger { get; }
        ITicketRepository TicketRepository { get; }

        public CheckInUseCase(IUserRepositoryDapper logInRepository, ITicketRepository ticketRepository, ILogger logger)
        {
            LogInRepository = logInRepository;
            TicketRepository = ticketRepository;
            Logger = logger;
        }


        public async Task<Response> CheckAsync(CheckDTO checkTicket, string op)
        {
            Response response = new();

            try
            {


                var userEmail = await LogInRepository.GetUserByEmailAsync(checkTicket.Email);

                var userPhone = await LogInRepository.GetUserByPhoneAsync(checkTicket.Phone);

                if (userEmail is { UserId: <= 0 } && userPhone is { UserId: <= 0 })
                {
                    response.StatusCode = System.Net.HttpStatusCode.NotFound;
                    response.Message = "El usuario no existe.";

                    return response;
                }
                int userID = userEmail.UserId > 0 ? userEmail.UserId : userPhone.UserId;
                string name = userEmail.UserId > 0 ? userEmail.UserName : userPhone.UserName;

                switch (op)
                {
                    case "in":
                        var checkInResult = await TicketRepository.InsertCheckInAsync(checkTicket, userID);

                        if (checkInResult > 0)
                        {
                            response.StatusCode = System.Net.HttpStatusCode.Created;
                            response.Message = $"{name}";
                        }
                        else
                        {
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            response.Message = "Check-in failed.";
                        }
                        break;
                    case "out":
                        var checkOutResult = await TicketRepository.InsertCheckOutAsync(checkTicket, userID);

                        if (checkOutResult > 0)
                        {
                            response.StatusCode = System.Net.HttpStatusCode.Created;
                            response.Message = $"{name}";
                        }
                        else
                        {
                            response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                            response.Message = "Check-Out failed.";
                        }
                        break;
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
