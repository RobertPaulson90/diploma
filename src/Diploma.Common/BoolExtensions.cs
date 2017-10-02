namespace Diploma.Common
{
    public static class BoolExtensions
    {
        public static bool Implies(this bool antecedent, bool consequent)
        {
            return !antecedent || consequent;
        }
    }
}
