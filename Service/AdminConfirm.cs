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
    public class AdminConfirm:UserNamePasswordValidator
    {
       
        public AdminConfirm()
        {
            if(Bootstrap.Container==null)
            Bootstrap.BuildContainer();
        }
        public override void Validate(string userName, string password)
        {
            DomainModel.Models.Attribute login, pass;
            string hashpass;
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hash = sha.ComputeHash(textData);
                hashpass = BitConverter.ToString(hash).Replace("-", String.Empty);
            }
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                login = u.AttributesRepository.Find(p=>p.Key=="AdminLogin" &&p.Value==userName).FirstOrDefault();
                pass = u.AttributesRepository.Find(p => p.Key == "AdminPassword" && p.Value == hashpass).FirstOrDefault();

            if (login==null)
                throw new FaultException(" Incorrect Name");
            if (pass==null)
                throw new FaultException(" Incorrect Password");
            }
        
        }
    }
}
