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
            string hashpass;
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(a.PassWord);
                byte[] hash = sha.ComputeHash(textData);
                hashpass = BitConverter.ToString(hash).Replace("-", String.Empty);
            }
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                a.Email = a.Email.ToLower();
                a.PassWord = hashpass;
                _user = a;
                u.StartTransaction();
                u.UsersRepository.Add(a);
                u.Save();
                u.EndTransaction();
            }
        }
        
        public bool IsEmailInDB(string userName)
        {
            //User _user;
            List<User> x = new List<User>();
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                {
                    x = u.UsersRepository.Find(p => p.Email == userName.ToLower()).ToList();
                }
            }
            return x.Count() != 0;
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void ChangePass(string login,string newpass)
        {
            User _user;
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                {
                    _user = u.UsersRepository.Find(p => p.Email == login.ToLower()).ToList().First();
                }
            }

            string hashpass;
            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(newpass);
                byte[] hash = sha.ComputeHash(textData);
                hashpass = BitConverter.ToString(hash).Replace("-", String.Empty);
            }

            User newuser = new User()
            {
                Email = _user.Email,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                PhoneNo = _user.PhoneNo,
                PassWord = hashpass
            };
            using (var scope = Bootstrap.Container.BeginLifetimeScope())
            {
                IUnitOfWork u = scope.Resolve<IUnitOfWork>();
                u.StartTransaction();
                u.UsersRepository.Attach(_user);
                _user.PassWord = hashpass;
                u.Save();
                u.EndTransaction();
            }
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        public void Login()
        {

        }

    }
}

