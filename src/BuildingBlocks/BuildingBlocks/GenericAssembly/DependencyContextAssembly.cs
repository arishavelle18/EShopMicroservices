using Carter;
using System.Reflection;

namespace BuildingBlocks.GenericAssembly;

public class DependencyContextAssemblyCustom<T> : DependencyContextAssemblyCatalog
{
    public override IReadOnlyCollection<Assembly> GetAssemblies()
    {
        return new List<Assembly> { typeof(T).Assembly };
    }
}