using System;
using Diploma.BLL.Queries.Responses;
using JetBrains.Annotations;

namespace Diploma.Framework.Messages
{
    internal sealed class LoggedInMessage
    {
        public LoggedInMessage([NotNull] UserDataResponse user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        [NotNull]
        public UserDataResponse User { get; }
    }
}