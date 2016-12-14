using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Autofac;
using DomainModel;
using DomainModel.Models;
using Service.App_Data.Contracts;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                  ConcurrencyMode = ConcurrencyMode.Multiple,
                  ReleaseServiceInstanceOnTransactionComplete = false)]
    public class UserManagment : IUserManagment,IUserManagmentUnsecure

    {
        public UserManagment()
        {

            Bootstrap.BuildContainer();
        }

        public void AddUser(User a)
        {
            User _user;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                a.Email = a.Email.ToLower();
                _user = a;
                u.StartTransaction();
                u.UsersRepository.Add(a);
                u.Save();
                u.EndTransaction();
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void ChangePass(string login,string oldpass,string newpass)
        {
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();

            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void Login()
        {

        }

    }
}

