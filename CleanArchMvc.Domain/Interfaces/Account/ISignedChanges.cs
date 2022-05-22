    using System;

namespace CleanArchMvc.Domain.Interfaces.Account
{
    public interface ISignedChanges
    {
        DateTime ModifiedWhen { get; }

        string ModifiedBy { get; }

        void SignChanges(string username);
    }
}