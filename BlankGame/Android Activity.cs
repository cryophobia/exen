using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

using Microsoft.Xna.Framework;

namespace BlankGame
{
	[Activity(Label = "Blank Game", MainLauncher = true,
		Theme = ExEnAndroidActivity.DefaultTheme,
		ConfigurationChanges = ExEnAndroidActivity.DefaultConfigChanges)]
	public class BlankGameActivity : ExEnAndroidActivity
	{
		BlankGameGame game;
		
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			game = new BlankGameGame();
			game.Start(this);
		}
	}
}


