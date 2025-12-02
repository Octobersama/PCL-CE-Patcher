using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using AsmResolver;
using AsmResolver.DotNet;
using AsmResolver.DotNet.Bundles;
using AsmResolver.IO;
using AsmResolver.PE.File;
using AsmResolver.PE.File.Headers;
using PCL_CE_Patcher.Core.Patches;

namespace PCL_CE_Patcher.Core
{
    public class PatcherService
    {
        private readonly List<IPatch> _patches = new()
        {
            new RemovePrecheckPatch(),
            new UnlockProfilePatch()
        };

        public void Execute(string inputExe, string outputExe)
        {
            ConfigService.Log($"[Service] Starting patch process for: {Path.GetFileName(inputExe)}");

            string tempDir = Path.Combine(Path.GetTempPath(), "PCL_Patch_" + Guid.NewGuid().ToString().Substring(0, 8));
            Directory.CreateDirectory(tempDir);

            string preparedHostPath = Path.Combine(tempDir, "PreparedHost.exe");
            string mainDllName = "Plain Craft Launcher 2.dll";

            try
            {
                // 1. Load Host
                ConfigService.Log("[Service] Loading CleanHost resource...");
                var cleanPe = LoadCleanHost();
                if (cleanPe == null) throw new Exception("内部错误：无法加载 Assets/CleanHost.exe");

                // 2. Extract
                ConfigService.Log("[Service] Extracting bundle...");
                var bundle = BundleManifest.FromFile(inputExe);
                int fileCount = 0;
                foreach (var file in bundle.Files)
                {
                    string safePath = file.RelativePath.Replace('/', Path.DirectorySeparatorChar);
                    string fullPath = Path.Combine(tempDir, safePath);
                    string? dir = Path.GetDirectoryName(fullPath);
                    if (dir != null && !Directory.Exists(dir)) Directory.CreateDirectory(dir);

                    if (file.Contents != null)
                    {
                        using var fs = File.Create(fullPath);
                        var writer = new BinaryStreamWriter(fs);
                        file.Contents.Write(writer);
                        fileCount++;
                    }
                }
                ConfigService.Log($"[Service] Extracted {fileCount} files.");

                // 3. Patch DLL
                string dllPath = Path.Combine(tempDir, mainDllName);
                if (!File.Exists(dllPath)) throw new FileNotFoundException($"解包数据异常，未找到 {mainDllName}");

                ConfigService.Log("[Service] Applying patches...");
                var module = ModuleDefinition.FromFile(dllPath);
                bool anyPatched = false;

                foreach (var patch in _patches)
                {
                    if (patch.Apply(module))
                    {
                        ConfigService.Log($"[Patch] Applied: {patch.Name}");
                        anyPatched = true;
                    }
                    else
                    {
                        ConfigService.Log($"[Patch] Skipped/Failed: {patch.Name}");
                    }
                }

                if (anyPatched)
                {
                    module.Write(dllPath);
                    ConfigService.Log("[Service] DLL patched and saved.");
                }

                // 4. Prepare Host
                ConfigService.Log("[Service] Transplanting resources & settings GUI...");
                var originalPe = PEFile.FromFile(inputExe);
                var rsrcSection = originalPe.Sections.FirstOrDefault(s => s.Name == ".rsrc");

                if (rsrcSection != null)
                {
                    uint align = cleanPe.OptionalHeader.SectionAlignment;
                    var last = cleanPe.Sections.Last();
                    uint newRva = PatcherUtils.Align(last.Rva + last.GetVirtualSize(), align);

                    var newRsrc = new PESection(".rsrc_n", rsrcSection.Characteristics, rsrcSection.Contents);
                    cleanPe.Sections.Add(newRsrc);

                    var dataDirs = cleanPe.OptionalHeader.DataDirectories;
                    dataDirs[(int)DataDirectoryIndex.ResourceDirectory] = new DataDirectory(newRva, newRsrc.GetVirtualSize());
                }

                cleanPe.OptionalHeader.SubSystem = SubSystem.WindowsGui;
                cleanPe.Write(preparedHostPath);

                // 5. Repack
                ConfigService.Log("[Service] Repacking bundle...");
                var newManifest = new BundleManifest(6);
                var tempFiles = Directory.GetFiles(tempDir, "*.*", SearchOption.AllDirectories);

                foreach (var f in tempFiles)
                {
                    if (Path.GetFileName(f).Equals("PreparedHost.exe", StringComparison.OrdinalIgnoreCase)) continue;

                    string relPath = Path.GetRelativePath(tempDir, f);
                    byte[] data = File.ReadAllBytes(f);
                    var type = PatcherUtils.DetectFileType(relPath);
                    newManifest.Files.Add(new BundleFile(relPath, type, new DataSegment(data)));
                }

                var parameters = BundlerParameters.FromTemplate(preparedHostPath, mainDllName);
                newManifest.WriteUsingTemplate(outputExe, parameters);

                // 6. Post Process
                var finalPe = PEFile.FromFile(outputExe);
                if (finalPe.OptionalHeader.SubSystem != SubSystem.WindowsGui)
                {
                    finalPe.OptionalHeader.SubSystem = SubSystem.WindowsGui;
                    finalPe.Write(outputExe);
                }

                ConfigService.Log($"[Service] Success! Output: {outputExe}");
            }
            finally
            {
                try { Directory.Delete(tempDir, true); } catch { }
            }
        }

        private PEFile? LoadCleanHost()
        {
            var assembly = Assembly.GetExecutingAssembly();
            // 确保这里的资源名称和你的项目结构一致
            string resourceName = "PCL_CE_Patcher.Assets.CleanHost.exe";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                ConfigService.Log($"[Error] Embedded resource not found: {resourceName}");
                return null;
            }

            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return PEFile.FromBytes(ms.ToArray());
        }
    }
}