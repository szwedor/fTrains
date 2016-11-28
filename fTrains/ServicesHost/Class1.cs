using System.IdentityModel.Selectors;
using System.Security.Authentication;

namespace ServicesHost
{
    class UsernameAuthentication : UserNamePasswordValidator
    {
        /// <summary>
        /// When overridden in a derived class, validates the specified username and password.
        /// </summary>
        /// <param name="userName">The username to validate.</param><param name="password">The password to validate.</param>
        public override void Validate(string userName, string password)
        {
            //TODOs: Lookup match in user storage (DB?).
            var ok = (userName == "Ole") && (password == "Pwd");
            if (ok == false)
                throw new AuthenticationException("u/p does not match");
        }
    }
}