using Spectre.Console.Cli;
using Microsoft.Extensions.DependencyInjection;

public class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceProvider _serviceProvider;

    public TypeRegistrar(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public ITypeResolver Build()
    {
        return new TypeResolver(_serviceProvider);
    }

    public void Register(Type service, Type implementation)
    {
        // Not needed as services are registered in ServiceCollection
    }

    public void RegisterInstance(Type service, object implementation)
    {
        // Not needed as services are registered in ServiceCollection
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        // Not needed as services are registered in ServiceCollection
    }
}

public class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _serviceProvider;

    public TypeResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public object? Resolve(Type? type)
    {
        if (type is null) return null;
        return _serviceProvider.GetService(type);
    }
}
