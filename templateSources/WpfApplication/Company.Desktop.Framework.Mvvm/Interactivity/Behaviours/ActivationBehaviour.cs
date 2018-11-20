using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ActivationBehaviour : IActivationBehaviour
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ActivationBehaviour));

		/// <inheritdoc />
		public int ExecutionOrder { get; }

		/// <inheritdoc />
		public event EventHandler Executed;

		/// <inheritdoc />
		public IActivationBehaviourContext Context { get; set; }

		/// <inheritdoc />
		public async Task ExecuteAsync()
		{
			try
			{
				if (Context.ViewModel is IActivateable activateable)
				{
					if (Context.ViewModel is IBusyStateHolder holder)
					{
						using (holder.LoadingState.Session())
						{
							await activateable.ActivateAsync(new ActivationContext(Context.ServiceProvider));
						}
					}
					else
					{
						await activateable.ActivateAsync(new ActivationContext(Context.ServiceProvider));
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
				Context.Cancel();
			}
			finally
			{
				Executed?.Invoke(this, EventArgs.Empty);
			}
		}
	}
}