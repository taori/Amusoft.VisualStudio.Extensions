using System.Threading.Tasks;
using System.Windows;

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
