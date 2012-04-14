using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoTouch.Foundation
{
	[AttributeUsage(AttributeTargets.Class
			| AttributeTargets.Struct
			| AttributeTargets.Enum
			| AttributeTargets.Constructor
			| AttributeTargets.Method
			| AttributeTargets.Property
			| AttributeTargets.Field
			| AttributeTargets.Event
			| AttributeTargets.Interface
			| AttributeTargets.Delegate
			| AttributeTargets.All)]
	public sealed class PreserveAttribute : Attribute
	{
		public bool AllMembers { get; set; }
		public bool Conditional { get; set; }
	}
}
