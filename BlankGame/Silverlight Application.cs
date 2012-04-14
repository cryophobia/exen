using System;
using ExEnSilver;

namespace BlankGame
{
	public class App : ExEnSilverApplication
	{
		protected override void SetupMainPage(MainPage mainPage)
		{
			var game = new BlankGameGame();
			mainPage.Children.Add(game);
			game.Play();
		}
	}
}
