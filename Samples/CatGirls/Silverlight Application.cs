using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ExEnSilver;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CatGirls
{
	public class App : ExEnSilverApplication
	{
		protected override void SetupMainPage(MainPage mainPage)
		{
			FontSource uiFontSource = new FontSource(Application.GetResourceStream(
					new Uri("/CatGirls;component/Content/NGO_____.TTF", UriKind.Relative)).Stream);
			FontFamily uiFontFamily = new FontFamily("News Gothic");

			SilverlightFontTranslations.Add("UIFont", new SpriteFontTTF(uiFontSource, uiFontFamily, 16));


			var game = new CatGirlsGame(NamedScreenSizes.Get("web"));
			mainPage.Children.Add(game);
			game.Play();
		}
	}
}
