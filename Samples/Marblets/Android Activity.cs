using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

using Microsoft.Xna.Framework;

namespace Marblets
{
	[Activity(Label = "Marblets", MainLauncher = true,
		Theme = ExEnAndroidActivity.DefaultTheme,
		ConfigurationChanges = ExEnAndroidActivity.DefaultConfigChanges)]
	public class OrientationActivity : ExEnAndroidActivity
	{
		MarbletsGame game;
		
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			game = new MarbletsGame();
			game.Start(this);
		}
	}
}


