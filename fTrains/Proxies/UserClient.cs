using System;
using System.ServiceModel;
using DomainModel.Models;
using Proxies.Contracts;

namespace Proxies
{
    public class UserClient : ClientBase<IUserService> ,IUserService
    {
        public void CreateUser(User u)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User u)
        {
            throw new NotImplementedException();
        }
    }
}
