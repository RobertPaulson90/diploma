using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Diploma.Core.Framework.Validations
{
    public interface IValidationAdapter
    {
        void Initialize([NotNull] object subject);

        [NotNull]
        Task<string[]> ValidatePropertyAsync(string propertyName);

        [NotNull]
        Task<Dictionary<string, string[]>> ValidateAllPropertiesAsync();
    }

    public interface IValidationAdapter<in T> : IValidationAdapter
    {
    }
}