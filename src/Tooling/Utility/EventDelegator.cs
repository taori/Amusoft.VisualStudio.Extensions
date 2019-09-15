using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using EnvDTE;

namespace Tooling.Utility
{
	public static class EventDelegator
	{
		private static SolutionEvents _solutionEvents;
		private static CompositeDisposable _disposables = new CompositeDisposable();
		
		public static void Initialize()
		{
			_solutionEvents = ToolingPackage.DTE.Events.SolutionEvents;
			_solutionEvents.Opened += SolutionEventsOnOpened;
			_solutionEvents.AfterClosing += SolutionEventsOnAfterClosing;
			_solutionEvents.ProjectAdded += SolutionEventsOnProjectAdded;
			_solutionEvents.ProjectRemoved += SolutionEventsOnProjectRemoved;
		}

		private static void SolutionEventsOnProjectRemoved(Project project)
		{
			_whenProjectRemoved.OnNext(project);
		}

		private static void SolutionEventsOnProjectAdded(Project project)
		{
			_whenProjectAdded.OnNext(project);
		}

		private static void SolutionEventsOnAfterClosing()
		{
			_whenSolutionClosed.OnNext(null);
		}

		private static void SolutionEventsOnOpened()
		{
			_whenSolutionOpened.OnNext(null);
		}

		public static void Unload()
		{
			_solutionEvents = null;
			_disposables?.Dispose();
			_disposables = null;
		}

		private static Subject<Project> _whenProjectAdded = new Subject<Project>();
		public static IObservable<Project> WhenProjectAdded => _whenProjectAdded;

		private static Subject<Project> _whenProjectRemoved = new Subject<Project>();
		public static IObservable<Project> WhenProjectRemoved => _whenProjectRemoved;

		private static Subject<object> _whenSolutionOpened = new Subject<object>();
		public static IObservable<object> WhenSolutionOpened => _whenSolutionOpened;

		private static Subject<object> _whenSolutionClosed = new Subject<object>();
		public static IObservable<object> WhenSolutionClosed => _whenSolutionClosed;
	}
}