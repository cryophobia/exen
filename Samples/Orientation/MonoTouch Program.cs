using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace Orientation
{
	[Register("AppDelegate")]
	class Program : ExEnEmTouchApplication
	{
		public override void FinishedLaunching(UIApplication application)
		{
			game = new OrientationGame();
			game.Run();
		}

		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}

