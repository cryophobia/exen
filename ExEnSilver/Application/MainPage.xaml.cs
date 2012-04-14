using System.Windows;
using System.Windows.Controls;
using Microsoft.Xna.Framework;

namespace ExEnSilver
{
	public partial class MainPage : Grid
	{
		public MainPage()
		{
			InitializeComponent();

			var settings = Application.Current.Host.Settings;
			settings.MaxFrameRate = 60; // Default setting for game loop
		}
	}
}
