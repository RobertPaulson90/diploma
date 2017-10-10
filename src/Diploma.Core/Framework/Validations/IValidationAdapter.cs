using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Diploma.Core.Framework.Validations
{
    public interface IValidationAdapter
    {
        void Initialize([NotNull] object subject);

        Task<IEnumerable<string>> ValidatePropertyAsync(string propertyName);

        Task<Dictionary<string, IEnumerable<string>>> ValidateAllPropertiesAsync();
    }

    public interface IValidationAdapter<in T> : IValidationAdapter
    {
    }
}