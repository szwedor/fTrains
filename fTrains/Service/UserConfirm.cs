using System;
using System.IdentityModel.Selectors;
using System.ServiceModel;

namespace Service
{
    public class UserConfirm:UserNamePasswordValidator
    {
       
        public override void Validate(string userName, string password)
        {
          if(userName!="szwedor")
                throw new FaultException("Unknown Username or Incorrect Password");
        }
    }
}
