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
using System.Windows.Shapes;

namespace Company.Desktop.Application.Views.Windows
{
	/// <summary>
	/// Interaktionslogik für Splash.xaml
	/// </summary>
	public partial class Splash : Window
	{
		public Splash()
		{
			this.Loaded += OnLoaded;
			InitializeComponent();
		}

		private async void OnLoaded(object sender, RoutedEventArgs e)
		{
			this.Loaded -= OnLoaded;
			await Task.Delay(5000);
			this.Close();
		}
	}
}
