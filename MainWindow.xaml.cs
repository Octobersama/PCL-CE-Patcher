using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;
using PCL_CE_Patcher.Core; // 确保引用了 Core

namespace PCL_CE_Patcher
{
    public partial class MainWindow : Window
    {
        // ==========================================================
        // 1. 字段定义 (之前缺失的部分)
        // ==========================================================
        private string? _selectedFilePath;
        private bool _isProcessing = false;
        private readonly PatcherService _patcherService = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        // ==========================================================
        // 2. UI 事件处理 (之前缺失的部分)
        // ==========================================================

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Filter = "PCL2 Executable (*.exe)|*.exe",
                Title = "选择 PCL2 CE 原版程序"
            };

            if (dialog.ShowDialog() == true)
            {
                SetFilePath(dialog.FileName);
            }
        }

        private void TxtPath_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Handled = true;
            e.Effects = DragDropEffects.Copy;
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files != null && files.Length > 0 && files[0].EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    SetFilePath(files[0]);
                }
            }
        }

        private void TxtPath_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtPath.Text.StartsWith("请选择"))
            {
                TxtPath.Text = "";
                TxtPath.Foreground = Brushes.Black;
            }
        }

        private void TxtPath_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtPath.Text))
            {
                TxtPath.Text = "请选择或直接拖入文件...";
                TxtPath.Foreground = Brushes.Gray;
            }
            else if (File.Exists(TxtPath.Text))
            {
                SetFilePath(TxtPath.Text);
            }
        }

        private void SetFilePath(string path)
        {
            _selectedFilePath = path;
            TxtPath.Text = path;
            TxtPath.Foreground = Brushes.Black;
            BtnPatch.IsEnabled = true;
            LblStatus.Text = "准备就绪";
            LblStatus.Foreground = Brushes.Black;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // ==========================================================
        // 3. 核心修补逻辑 (整合了新弹窗和提权逻辑)
        // ==========================================================

        private async void BtnPatch_Click(object sender, RoutedEventArgs e)
        {
            if (_isProcessing || string.IsNullOrEmpty(_selectedFilePath)) return;

            _isProcessing = true;
            SetUiState(false);

            LblStatus.Text = "正在处理中... (请勿关闭)";
            LblStatus.Foreground = Brushes.Blue;

            string inputPath = _selectedFilePath;
            string outputPath = Path.Combine(Path.GetDirectoryName(inputPath)!,
                Path.GetFileNameWithoutExtension(inputPath) + "_Patched.exe");

            try
            {
                // 调用 Service
                await Task.Run(() => _patcherService.Execute(inputPath, outputPath));

                LblStatus.Text = "处理完成！";
                LblStatus.Foreground = Brushes.Green;

                // 成功弹窗
                ModernMsgBox.ShowSuccess($"修补成功！\n\n文件已保存至：\n{outputPath}");
            }
            catch (Exception ex)
            {
                LblStatus.Text = "处理失败";
                LblStatus.Foreground = Brushes.Red;

                // 捕获权限不足异常
                if (ex is UnauthorizedAccessException)
                {
                    bool restart = ModernMsgBox.ShowAdminRequest(
                        $"写入文件被拒绝。\n\n这通常是因为原文件位于受保护的系统目录（如 Program Files）。\n\n是否尝试以【管理员身份】重启程序？"
                    );

                    if (restart)
                    {
                        RestartAsAdmin();
                    }
                }
                else
                {
                    // 普通错误
                    ModernMsgBox.ShowError($"发生错误：\n{ex.Message}");
                }
            }
            finally
            {
                _isProcessing = false;
                SetUiState(true);
            }
        }

        private void SetUiState(bool enabled)
        {
            BtnPatch.IsEnabled = enabled;
            BtnBrowse.IsEnabled = enabled;
            TxtPath.IsEnabled = enabled;
            BtnExit.IsEnabled = enabled;
        }

        // 以管理员重启
        private void RestartAsAdmin()
        {
            try
            {
                var exeName = Process.GetCurrentProcess().MainModule?.FileName;
                if (string.IsNullOrEmpty(exeName)) return;

                var startInfo = new ProcessStartInfo(exeName)
                {
                    UseShellExecute = true,
                    Verb = "runas", // 触发 UAC
                    Arguments = ""
                };

                Process.Start(startInfo);
                Application.Current.Shutdown();
            }
            catch { /* 用户取消了 UAC */ }
        }
    }
}