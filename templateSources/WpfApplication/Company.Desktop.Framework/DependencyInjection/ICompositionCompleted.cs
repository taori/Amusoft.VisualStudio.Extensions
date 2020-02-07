// Copyright  Andreas Müller
// This file is a part of Amusoft.CodeAnalysis.Analyzers and is licensed under Apache 2.0
// See https://github.com/taori/Amusoft.CodeAnalysis.Analyzers/blob/master/LICENSE for details

using System.Threading.Tasks;

namespace Company.Desktop.Framework.DependencyInjection
{
	public interface ICompositionCompleted
	{
		void Complete();
	}
}