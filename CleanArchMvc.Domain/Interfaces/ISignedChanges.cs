    using System;

namespace CleanArchMvc.Domain.Interfaces
{
    public interface ISignedChanges
    {
        DateTime ModifiedWhen { get; }

        string ModifiedBy { get; }

        void SignChanges(string username);
    }
}