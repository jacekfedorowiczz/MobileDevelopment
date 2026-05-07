using Microsoft.AspNetCore.Mvc;

namespace MobileDevelopment.API.Controllers
{
    [ApiController]
    public sealed class UserController
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }


    }
}
