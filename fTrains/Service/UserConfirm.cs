using Autofac;
using DomainModel;
using DomainModel.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.Linq;
using System.ServiceModel;

namespace Service
{
    public class UserConfirm:UserNamePasswordValidator
    {
       
        public override void Validate(string userName, string password)
        {
            List<User> s;
            string hashpass;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                s = u.UsersRepository.Find(p => p.Email.ToLower() == userName.ToString()).ToList();

            }
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                hashpass = BitConverter.ToString(hash).Replace("-", String.Empty);
            }
            if (s.Count == 0)
                throw new FaultException(" Incorrect Name");
            if (s.First().PassWord != hashpass)
                throw new FaultException(" Incorrect Password");
        }
    }
}
