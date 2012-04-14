using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ExEnSilver;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Orientation
{
	public class App : ExEnSilverApplication
	{
		protected override void SetupMainPage(MainPage mainPage)
		{
			var game = new OrientationGame();
			mainPage.Children.Add(game);
			game.Play();
		}
	}
}
