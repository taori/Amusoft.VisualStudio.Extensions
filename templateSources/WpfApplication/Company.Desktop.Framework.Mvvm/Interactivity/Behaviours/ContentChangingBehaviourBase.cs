
using System;
using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;
using NLog;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class ContentChangingBehaviourBase : BehaviourBase, IContentChangingBehaviour
	{
		private static readonly ILogger Log = LogManager.GetLogger(nameof(ContentChangingBehaviourBase));
		
		/// <inheritdoc />
		public IContentChangingBehaviourContext Context { get; private set; }

		/// <inheritdoc />
		public async Task ExecuteAsync()
		{
			try
			{
				await OnExecuteAsync();
			}
			catch (Exception e)
			{
				Log.Error(e);
				Context.Cancel();
			}
			finally
			{
				RaiseExecuted();
			}
		}

		protected abstract Task OnExecuteAsync();

		/// <inheritdoc />
		public void SetContext(IContentChangingBehaviourContext context)
		{
			Context = context;
		}
	}
}