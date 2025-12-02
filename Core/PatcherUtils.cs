using AsmResolver.DotNet.Bundles;

namespace PCL_CE_Patcher.Core
{
    public static class PatcherUtils
    {
        public static uint Align(uint value, uint alignment)
            => (alignment == 0) ? value : (value + alignment - 1) / alignment * alignment;

        public static BundleFileType DetectFileType(string fileName)
        {
            string lower = fileName.ToLowerInvariant();
            if (lower.EndsWith(".deps.json")) return BundleFileType.DepsJson;
            if (lower.EndsWith(".runtimeconfig.json")) return BundleFileType.RuntimeConfigJson;
            if (lower.EndsWith(".dll")) return BundleFileType.Assembly;
            return BundleFileType.Unknown;
        }
    }
}