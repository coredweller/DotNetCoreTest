using Dapper;
using Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediatrTest.Handlers
{
    public class UserUpdateHandler : IRequestHandler<User, User>
    {
        private readonly IConfiguration _config;

        public UserUpdateHandler(IConfiguration config)
        {
            this._config = config;
        }

        public Task<User> Handle(User request, CancellationToken cancellationToken)
        {
            User newUser = null;
            using (var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                if (connection.QueryFirstOrDefault<long?>(@"SELECT Id 
                                                               FROM Users 
                                                               WHERE Email = @Email",
                                                         new { Email = request.Email }) == null)
                {
                    connection.Execute(@"INSERT INTO Users (Name, Email) 
                                             VALUES (@Name, @Email)",
                                       request);
                }

                newUser = connection.QueryFirstOrDefault<User>(@"SELECT Id, Name, Email, CreatedDate, UpdatedDate
                                                                            FROM Users 
                                                                            WHERE Email = @Email",
                                                                      new { Email = request.Email });
            }
            return Task.FromResult(newUser);
        }
    }
}
