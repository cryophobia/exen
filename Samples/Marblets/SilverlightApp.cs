using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Marblets;
using ExEnSilver;

namespace Marblets
{
	public class App : ExEnSilverApplication
	{
		protected override void SetupMainPage(MainPage mainPage)
		{
			Game game = new MarbletsGame();
			mainPage.Children.Add(game);
			game.Play();
		}
	}

}
