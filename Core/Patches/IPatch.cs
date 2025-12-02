using AsmResolver.DotNet;

namespace PCL_CE_Patcher.Core.Patches
{
    public interface IPatch
    {
        string Name { get; }
        bool Apply(ModuleDefinition module);
    }
}