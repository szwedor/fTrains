﻿using System;
using System.Collections.Generic;
using System.ServiceModel;
using DomainModel.Models;

namespace Service.App_Data.Contracts
{
    
    [ServiceContract]
    public interface IUserManagmentUnsecure
    {
        [OperationContract]
        void AddUser(User a);
    }
    [ServiceContract]
    public interface IUserManagment : IUserManagmentUnsecure
    {
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void Login();
        [OperationContract]
        [TransactionFlow(TransactionFlowOption.Allowed)]
        void ChangePass(string login, string oldpass, string newpass);
    }
}
