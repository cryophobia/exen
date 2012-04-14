using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

using Microsoft.Xna.Framework;

namespace Orientation
{
	[Activity(Label = "Orientation", MainLauncher = true,
		Theme = ExEnAndroidActivity.DefaultTheme,
		ConfigurationChanges = ExEnAndroidActivity.DefaultConfigChanges)]
	public class OrientationActivity : ExEnAndroidActivity
	{
		OrientationGame game;
		
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			game = new OrientationGame();
			game.Start(this);
		}
	}
}


