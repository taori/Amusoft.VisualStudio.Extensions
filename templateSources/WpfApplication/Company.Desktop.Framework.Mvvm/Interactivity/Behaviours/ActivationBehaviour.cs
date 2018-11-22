using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public class ActivationBehaviour : AsyncBehaviourBase<IActivationBehaviourContext>
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ActivationBehaviour));
		
		/// <inheritdoc />
		protected override async Task OnExecuteAsync(IActivationBehaviourContext context)
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