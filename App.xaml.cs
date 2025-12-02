using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using PCL_CE_Patcher.Core;

namespace PCL_CE_Patcher
{
    public partial class App : Application
    {
        [DllImport("kernel32.dll")]
        private static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern bool FreeConsole(); // 显式释放控制台

        // 后门参数
        private const string MagicArg = "--2iGnOBvq2bXSF3LR";

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // =========================================================
            // 分支 1：命令行模式 (CLI)
            // =========================================================
            if (e.Args.Length > 0)
            {
                // 1. 挂载控制台
                if (!AttachConsole(ATTACH_PARENT_PROCESS)) AllocConsole();

                // 2. 检查后门
                bool bypassAuth = e.Args.Contains(MagicArg);

                // 3. 身份验证
                if (!bypassAuth && !ConfigService.IsAuthValid())
                {
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Authentication required or expired.");
                    Console.WriteLine("    Please run the GUI version to verify.");
                    Console.ResetColor();
                    SafeExit(1); // 使用封装的退出方法
                    return;
                }

                // 4. 执行逻辑
                try
                {
                    HandleCommandLine(e.Args);
                    SafeExit(0);
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[!] Error: {ex.Message}");
                    Console.ResetColor();
                    SafeExit(1);
                }
                return;
            }

            // =========================================================
            // 分支 2：图形界面模式 (GUI)
            // =========================================================

            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            if (!ConfigService.IsAuthValid())
            {
                var authWindow = new AuthWindow();
                bool? result = authWindow.ShowDialog();

                if (result != true)
                {
                    Shutdown();
                    return;
                }
            }

            var mainWindow = new MainWindow();
            this.MainWindow = mainWindow;
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            mainWindow.Show();
        }

        private void HandleCommandLine(string[] args)
        {
            var realArgs = args.Where(a => a != MagicArg).ToArray();

            if (realArgs.Length == 0)
            {
                PrintHelp();
                return;
            }

            string firstArg = realArgs[0];

            if (firstArg == "-h" || firstArg == "--help" || firstArg == "/?")
            {
                PrintHelp();
                return;
            }

            string inputPath = firstArg;
            string outputPath;

            if (realArgs.Length >= 2)
            {
                outputPath = realArgs[1];
            }
            else
            {
                string dir = Path.GetDirectoryName(inputPath) ?? "";
                string name = Path.GetFileNameWithoutExtension(inputPath);
                outputPath = Path.Combine(dir, $"{name}_Patched.exe");
            }

            if (!File.Exists(inputPath))
            {
                throw new FileNotFoundException($"Input file not found: {inputPath}");
            }

            Console.WriteLine($"[*] Input:  {inputPath}");
            Console.WriteLine($"[*] Output: {outputPath}");

            if (args.Contains(MagicArg))
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[*] Magic Key detected. Auth bypassed.");
                Console.ResetColor();
            }

            Console.WriteLine("[*] Processing...");

            var patcher = new PatcherService();
            patcher.Execute(inputPath, outputPath);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] File patched successfully: {outputPath}");
            Console.ResetColor();
        }

        private void PrintHelp()
        {
            Console.WriteLine("PCE Patcher - Command Line Interface");
            Console.WriteLine("Usage:");
            Console.WriteLine("  PCL_CE_Patcher.exe <input_file> [output_file]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -h, --help    Show this help message");
        }

        // ==========================================================
        // 安全退出封装：处理暂停逻辑和释放控制台
        // ==========================================================
        private void SafeExit(int exitCode)
        {
            // 只有在不是重定向输入的情况下（即只有人肉操作时）才暂停
            // 这样如果你写脚本批量调用，它就不会卡住
            if (!Console.IsInputRedirected)
            {
                Console.WriteLine();
                Console.WriteLine("Press Enter to exit...");
                try
                {
                    Console.ReadLine();
                }
                catch { }
            }

            // 释放控制台挂钩，防止对父进程造成残留影响
            FreeConsole();

            // 强制杀进程
            Environment.Exit(exitCode);
        }
    }
}