using System;
using System.Collections.Generic;

// This file is useful when targeting .NET 2.0 with your XNA game


// http://www.codethinked.com/post/2008/02/Using-Extension-Methods-in-net-20.aspx
namespace System.Runtime.CompilerServices
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method)]
	public sealed class ExtensionAttribute : Attribute { }
}

namespace System
{
	public delegate void Action();
}
