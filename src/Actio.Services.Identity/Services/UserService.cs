using Actio.Common.Auth;
using Actio.Common.Exceptions;
using Actio.Services.Identity.Domain.Repository;
using Actio.Services.Identity.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IEncryter encryter;
        private readonly IJwtHandler jwtHandler;

        public UserService(IUserRepository userRepository, IEncryter encryter, IJwtHandler jwtHandler)
        {
            this.userRepository = userRepository;
            this.encryter = encryter;
            this.jwtHandler = jwtHandler;
        }

        public async Task<JsonWebToken> LoginAsync(string email, string password)
        {
            var user = await this.userRepository.GetAsync(email);

            Console.WriteLine($"User got - {user.Email} , {user.Password}");

            if (user == null)
            {
                throw new ActioException("invalid_credentials", $"Invalid credentials");
            }

            if(!user.ValidatePassword(password, encryter))
            {
                throw new ActioException("invalid_credentials", $"Invalid credentials");
            }

            return this.jwtHandler.Create(user.Id);
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            var user = await this.userRepository.GetAsync(email);

            if(user != null)
            {
                throw new ActioException("email_in_use", $"Email: {email} is already in use");
            }

            user = new Domain.Models.User(email, name);
            user.SetPassword(password, this.encryter);

            await this.userRepository.AddAsync(user);
        }
    }
}
