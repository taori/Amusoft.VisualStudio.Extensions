using System.Threading.Tasks;
using Company.Desktop.Framework.Mvvm.Abstraction.Interactivity.Behaviours;

namespace Company.Desktop.Framework.Mvvm.Interactivity.Behaviours
{
	public abstract class ClosedBehaviourBase : BehaviourBase, IWindowClosedBehaviour
	{
		/// <inheritdoc />
		public IWindowClosedBehaviourContext Context { get; private set; }

		/// <inheritdoc />
		public async Task ExecuteAsync()
		{
			await OnExecuteAsync();
			RaiseExecuted();
		}

		protected abstract Task OnExecuteAsync();

		/// <inheritdoc />
		public void SetContext(IWindowClosedBehaviourContext context)
		{
			Context = context;
		}
	}
}