namespace Rubns.Application.UseCases.Ticket.Post
{
    internal sealed class CreateCheckUseCase : ICreateCheckUseCase
    {
        private readonly IUserRepositoryDapper _loginRepository;
        private readonly ILogger _logger;
        private readonly ITicketRepository _ticketRepository;

        public CreateCheckUseCase(IUserRepositoryDapper loginRepository, ITicketRepository ticketRepository, ILogger logger)
        {
            _loginRepository = loginRepository;
            _ticketRepository = ticketRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync(CheckDTO checkTicket, string op)
        {
            try
            {
                var userEmail = await _loginRepository.GetUserByEmailAsync(checkTicket.Email);

                var userPhone = await _loginRepository.GetUserByPhoneAsync(checkTicket.Phone);

                if (userEmail is { UserId: <= 0 } && userPhone is { UserId: <= 0 })
                {
                    throw new NotFoundException("No esta registrado en la plataforma.", "No se encontro el usuario");
                }
                int userID = userEmail.UserId > 0 ? userEmail.UserId : userPhone.UserId;
                string name = userEmail.UserId > 0 ? userEmail.UserName : userPhone.UserName;

                switch (op)
                {
                    case "in":
                        await _ticketRepository.InsertAsync(checkTicket, userID, "p_InsertCheckInPersonal");
                        break;
                    case "out":
                        await _ticketRepository.InsertAsync(checkTicket, userID, "p_InsertCheckOutPersonal");
                        break;
                }


            }
            catch (Exception e)
            {
                _logger.Error(e, "LogIn an error occurred: {ErrorMessage}", e.Message);
                throw;

            }
        }
    }
}
