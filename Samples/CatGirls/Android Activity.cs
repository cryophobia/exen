using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;

using Microsoft.Xna.Framework;

namespace CatGirls
{
	[Activity(Label = "Cat Girls!", MainLauncher = true,
		Theme = ExEnAndroidActivity.DefaultTheme,
		ConfigurationChanges = ExEnAndroidActivity.DefaultConfigChanges)]
	public class CatGirlsActivity : ExEnAndroidActivity
	{
		CatGirlsGame game;
		
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			
			game = new CatGirlsGame(new Point(480, 800));
			game.Start(this);
		}
	}
}


