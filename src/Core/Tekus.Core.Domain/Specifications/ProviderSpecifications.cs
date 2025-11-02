using System.Linq.Expressions;
using Tekus.Core.Domain.Aggregates.ProviderAggregate;

namespace Tekus.Core.Domain.Specifications;

/// <summary>
/// Concrete specifications for Provider aggregate
/// </summary>
public static class ProviderSpecifications
{
    /// <summary>
    /// Specification for active providers
    /// </summary>
    public static Specification<Provider> ActiveProviders()
    {
        return new ActiveProviderSpecification();
    }

    /// <summary>
    /// Specification for provider by NIT
    /// </summary>
    public static Specification<Provider> ByNit(string nit)
    {
        return new ProviderByNitSpecification(nit);
    }

    /// <summary>
    /// Specification for provider by email
    /// </summary>
    public static Specification<Provider> ByEmail(string email)
    {
        return new ProviderByEmailSpecification(email);
    }

    /// <summary>
    /// Specification for searching providers by name
    /// </summary>
    public static Specification<Provider> SearchByName(string searchTerm)
    {
        return new ProviderSearchByNameSpecification(searchTerm);
    }

    // Concrete specification implementations

    private class ActiveProviderSpecification : Specification<Provider>
    {
        public override Expression<Func<Provider, bool>> ToExpression()
        {
            return provider => provider.IsActive;
        }
    }

    private class ProviderByNitSpecification : Specification<Provider>
    {
        private readonly string _nit;

        public ProviderByNitSpecification(string nit)
        {
            _nit = nit;
        }

        public override Expression<Func<Provider, bool>> ToExpression()
        {
            return provider => provider.Nit.Value == _nit;
        }
    }

    private class ProviderByEmailSpecification : Specification<Provider>
    {
        private readonly string _email;

        public ProviderByEmailSpecification(string email)
        {
            _email = email.ToLowerInvariant();
        }

        public override Expression<Func<Provider, bool>> ToExpression()
        {
            return provider => provider.Email.Value == _email;
        }
    }

    private class ProviderSearchByNameSpecification : Specification<Provider>
    {
        private readonly string _searchTerm;

        public ProviderSearchByNameSpecification(string searchTerm)
        {
            _searchTerm = searchTerm.ToLowerInvariant();
        }

        public override Expression<Func<Provider, bool>> ToExpression()
        {
            return provider => provider.Name.ToLower().Contains(_searchTerm);
        }
    }
}