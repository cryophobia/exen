using System;

namespace Microsoft.Xna.Framework.Input.Touch
{
	public static class TouchPanel
	{
		static TouchPanelCapabilities caps = default(TouchPanelCapabilities);
		public static TouchPanelCapabilities GetCapabilities()
		{
			return caps;
		}

		static TouchCollection touches = default(TouchCollection);
		public static TouchCollection GetState()
		{
			return touches;
		}

		public static int DisplayWidth { get; set; }
		public static int DisplayHeight { get; set; }
		public static DisplayOrientation DisplayOrientation { get; set; }
	}
}

