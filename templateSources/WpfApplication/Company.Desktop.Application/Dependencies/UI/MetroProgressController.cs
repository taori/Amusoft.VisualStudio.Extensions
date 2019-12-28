using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Media;
using Company.Desktop.Framework.Mvvm.UI;
using MahApps.Metro.Controls.Dialogs;

namespace Company.Desktop.Application.Dependencies.UI
{
	public class MetroProgressController : IProgressController
	{
		/// <inheritdoc />
		public MetroProgressController(ProgressDialogController controller)
		{
			_controller = controller;
			_closeSubscription = Observable.FromEventPattern<EventHandler, EventArgs>(
				add => _controller.Closed += add,
				remove => _controller.Closed -= remove
			).Select(s => s.EventArgs).Subscribe(arg => _whenClosed.OnNext(arg));

			_cancelSubscription = Observable.FromEventPattern<EventHandler, EventArgs>(
				add => _controller.Canceled += add,
				remove => _controller.Canceled -= remove
			).Select(s => s.EventArgs).Subscribe(arg => _whenCanceled.OnNext(arg));
			
		}

		public void SetIndeterminate()
		{
			_controller.SetIndeterminate();
		}

		public void SetCancelable(bool value)
		{
			_controller.SetCancelable(value);
		}

		public void SetProgress(double value)
		{
			_controller.SetProgress(value);
		}

		public void SetMessage(string message)
		{
			_controller.SetMessage(message);
		}

		public void SetTitle(string title)
		{
			_controller.SetTitle(title);
		}

		public void SetProgressBarForegroundBrush(Brush brush)
		{
			_controller.SetProgressBarForegroundBrush(brush);
		}

		public Task CloseAsync()
		{
			return _controller.CloseAsync();
		}

		public bool IsCanceled => _controller.IsCanceled;

		public bool IsOpen => _controller.IsOpen;

		public double Minimum
		{
			get => _controller.Minimum;
			set => _controller.Minimum = value;
		}

		public double Maximum
		{
			get => _controller.Maximum;
			set => _controller.Maximum = value;
		}

		private Subject<EventArgs> _whenClosed = new Subject<EventArgs>();
		public IObservable<EventArgs> WhenClosed => _whenClosed;

		private Subject<EventArgs> _whenCanceled = new Subject<EventArgs>();
		public IObservable<EventArgs> WhenCanceled => _whenCanceled;

		private readonly ProgressDialogController _controller;

		private IDisposable _closeSubscription;
		private IDisposable _cancelSubscription;

		/// <inheritdoc />
		public void Dispose()
		{
			_whenClosed?.Dispose();
			_whenCanceled?.Dispose();
			_closeSubscription?.Dispose();
			_cancelSubscription?.Dispose();
		}
	}
}