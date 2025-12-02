using System.Windows;
using System.Windows.Media;

namespace PCL_CE_Patcher
{
    public partial class ModernMsgBox : Window
    {
        public enum MsgResult { Cancel, OK, RestartAdmin }
        public MsgResult Result { get; private set; } = MsgResult.Cancel;

        private ModernMsgBox()
        {
            InitializeComponent();
        }

        // 显示普通错误
        public static void ShowError(string message)
        {
            var win = new ModernMsgBox();
            win.Title = "错误";
            win.TxtMessage.Text = message;
            // 红色警告图标
            win.IconText.Text = "\uEA39";
            win.IconText.Foreground = Brushes.Red;
            win.ShowDialog();
        }

        // 显示成功
        public static void ShowSuccess(string message)
        {
            var win = new ModernMsgBox();
            win.Title = "完成";
            win.TxtMessage.Text = message;
            // 绿色完成图标
            win.IconText.Text = "\uE930";
            win.IconText.Foreground = Brushes.Green;
            win.ShowDialog();
        }

        // 显示提权请求
        public static bool ShowAdminRequest(string message)
        {
            var win = new ModernMsgBox();
            win.Title = "权限不足";
            win.TxtMessage.Text = message;

            // 蓝色盾牌图标
            win.IconText.Text = "\uE7EF";
            win.IconText.Foreground = SystemParameters.WindowGlassBrush; // 使用系统主题色

            win.BtnCancel.Visibility = Visibility.Visible;
            win.BtnAction.Content = "以管理员重启";
            win.BtnAction.Tag = MsgResult.RestartAdmin;

            win.ShowDialog();
            return win.Result == MsgResult.RestartAdmin;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Result = MsgResult.Cancel;
            Close();
        }

        private void BtnAction_Click(object sender, RoutedEventArgs e)
        {
            if (BtnAction.Tag is MsgResult res)
            {
                Result = res;
            }
            else
            {
                Result = MsgResult.OK;
            }
            Close();
        }
    }
}