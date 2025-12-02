using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PCL_CE_Patcher.Core;

namespace PCL_CE_Patcher
{
    public partial class AuthWindow : Window
    {
        private const string TargetUrl = "https://github.com/Octobersama/PCL-CE-Patcher";

        public AuthWindow()
        {
            InitializeComponent();
            // 初始化时检查水印状态
            CheckPlaceholder();
        }

        private void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            string input = TxtInput.Text.Trim();

            // 移除末尾可能的斜杠以增加容错
            string compareUrl = TargetUrl.TrimEnd('/');
            string compareInput = input.TrimEnd('/');

            if (string.Equals(compareInput, compareUrl, StringComparison.OrdinalIgnoreCase))
            {
                ConfigService.Config.ActivatedDate = DateTime.Now;
                ConfigService.Save();

                ConfigService.Log("User authenticated successfully.");

                this.DialogResult = true;
                this.Close();
            }
            else
            {
                ConfigService.Log($"Auth failed. Input: {input}");

                LblError.Text = "地址错误，请核对后重试";
                TxtInput.BorderBrush = Brushes.Red;
                TxtInput.SelectAll();
                TxtInput.Focus();
            }
        }

        // 监听文本变化，控制水印显示
        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckPlaceholder();
            // 输入时重置红色边框
            if (TxtInput.BorderBrush == Brushes.Red)
            {
                TxtInput.ClearValue(Border.BorderBrushProperty);
                LblError.Text = "";
            }
        }

        private void CheckPlaceholder()
        {
            if (Placeholder != null)
            {
                Placeholder.Visibility = string.IsNullOrEmpty(TxtInput.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }
    }
}