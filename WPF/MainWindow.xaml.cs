using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.ViewModels;

namespace WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowViewModel();
            
            DataContext = viewModel;

            viewModel.LoadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (MainWindowViewModel)DataContext;
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete Artist\n"+ 
                $"Id: {viewModel.SelectedItem.ArtistId}\nName: {viewModel.SelectedItem.Name}", 
                "Confirmation", 
                MessageBoxButton.YesNo
            );
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (viewModel.DeleteCommand.CanExecute(null)) { viewModel.DeleteCommand.Execute(null); }
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

    }
}
