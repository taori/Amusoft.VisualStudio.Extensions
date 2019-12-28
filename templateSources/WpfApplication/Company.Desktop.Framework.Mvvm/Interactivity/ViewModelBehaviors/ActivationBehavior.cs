using System;
using System.Threading.Tasks;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.ViewModelBehaviors
{
	public class ActivationBehavior : AsyncBehaviorBase<IActivationBehaviorContext>
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ActivationBehavior));
		
		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IActivationBehaviorContext context)
		{
			try
			{
				if (context.ViewModel is IActivateable activateable)
				{
					if (context.ViewModel is IBusyStateHolder holder)
					{
						using (holder.LoadingState.Session())
						{
							await activateable.ActivateAsync(new ActivationContext(context.ServiceProvider));
						}
					}
					else
					{
						await activateable.ActivateAsync(new ActivationContext(context.ServiceProvider));
					}
				}
			}
			catch (Exception e)
			{
				Log.Error(e);
				context.Cancel();
			}
		}
	}
}