using System.Windows.Controls;
using Tooling.Utility;

namespace Tooling.Features.ProjectMover.Views
{
	/// <summary>
	/// Interaction logic for OpenProjectMoverCommandControl.
	/// </summary>
	public partial class ProjectMoverControl : UserControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ProjectMoverControl"/> class.
		/// </summary>
		public ProjectMoverControl()
		{
			this.InitializeComponent();
			this.ShouldBeThemed();
		}
	}
}