using Client.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Client.Views
{

    public sealed partial class MainPage : Page
    {
        public MainViewModel viewModel { get; set; }
        public MainPage()
        {
            InitializeComponent();
            viewModel = App.Services.GetService<MainViewModel>();
        }

        private void TextBox_SendMessage(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                var textBox = sender as TextBox;
                viewModel.CurrentMessage = textBox.Text;

                if (viewModel.SendMessageCommand.CanExecute(null))
                {
                    viewModel.SendMessageCommand.Execute(null);
                }

                e.Handled = true;
            }
        }


    }
}
