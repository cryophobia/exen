using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework;

namespace Marblets
{
	[Register("AppDelegate")]
	class Program : ExEnEmTouchApplication
	{
		public override void FinishedLaunching(UIApplication application)
		{
			var bounds = UIScreen.MainScreen.Bounds;
			var scale = UIScreen.MainScreen.Scale;

			Point size = new Point((int)bounds.Width, (int)bounds.Height);

			// This seems to be required, despite what is in the info.plist...

			// Landscape Mode:
			//application.StatusBarOrientation = UIInterfaceOrientation.LandscapeRight;
			//if(size.Y > size.X)
			//	size = new Point(size.Y, size.X);

			// Portrait Mode:
			application.StatusBarOrientation = UIInterfaceOrientation.Portrait;
			if(size.X > size.Y)
				size = new Point(size.Y, size.X);

			game = new MarbletsGame();
			game.Run();
		}

		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}

