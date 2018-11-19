using System;
using System.Threading.Tasks;

namespace Company.Desktop.Framework
{
	public delegate Task AsyncEventHandler(object sender, EventArgs args);

	public delegate Task AsyncEventHandler<TArgs>(object sender, TArgs args);
}