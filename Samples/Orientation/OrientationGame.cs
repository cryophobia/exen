using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Orientation
{
	public class OrientationGame : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		public bool padDevice = false;

		public OrientationGame()
		{
			graphics = new GraphicsDeviceManager(this);

			// Are we a pad-style device or a phone-style device?
#if MONOTOUCH
			padDevice = (MonoTouch.UIKit.UIDevice.CurrentDevice.UserInterfaceIdiom
					== MonoTouch.UIKit.UIUserInterfaceIdiom.Pad);
#else
			padDevice = false;
#endif

			// Set orientation support
			if(padDevice) 
			{
				// Support all orientations on the iPad:
				//
				// The iPad will automatically support the iOS-specific "PortraitUpsideDown" orientation
				// if you specify DisplayOrientation.Portrait. ExEn disables this behaviour on iPhone
				// to be consistent with how most portrait applications behave on the iPhone.
				graphics.SupportedOrientations = DisplayOrientation.Portrait
						| DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
			}
			else
			{
				// Just support landscape on phones in this app:
				//
				// Note that ExEn itself supports both Portrait and Landscape as you would expect.
				// I am supporting only Landscape in this application to demonstrate how to load
				// a game in Landscape.
				graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
			}


#if WINDOWS_PHONE
			// On Windows Phone use the standard device resolution
			graphics.PreferredBackBufferWidth = 800;
			graphics.PreferredBackBufferHeight = 480;
			graphics.IsFullScreen = true; // Hide the status bar
#else
			// On Windows, Silverlight use iPhone resolution; on pad device use iPad resolution
			if(padDevice)
			{
				graphics.PreferredBackBufferWidth = 1024;
				graphics.PreferredBackBufferHeight = 768;
			}
			else
			{
				graphics.PreferredBackBufferWidth = 480;
				graphics.PreferredBackBufferHeight = 320;
			}
#endif

			IsMouseVisible = true;
			Content.RootDirectory = ".";
		}


		Texture2D catGirl;

		// These texture names are based on device orientation
		Texture2D portrait, landscapeLeft, landscapeRight, portraitUpsideDown;

		// You can only have a single, portrait launch image on the iPhone
		// So a landscape application must simply have a rotated launch image.
		// For this application I have arbitrarily chosen "Landscape Right"
		// as the device orientation for this image (so: interface orientation = landscape left).
		// Because I am reusing the launch images as textures, this variable tracks where
		// the texture needs to be "un-rotated".
		bool landscapeImagesAreRotated;

		protected override void LoadContent()
		{
			base.LoadContent();
			spriteBatch = new SpriteBatch(GraphicsDevice);
			catGirl = Content.Load<Texture2D>("CatGirl");

			if(padDevice) // Pad device: supports all orientations
			{
				Console.WriteLine("Loading Pad Device Graphics");
				// Note that the filenames here are from the iPad launch images
				// These use the iOS *interface* orientation, which has landscape left/right
				// swapped, compred to *device* orientation (on iOS and WP7)
				portrait = Content.Load<Texture2D>("Default-Portrait");
				landscapeLeft = Content.Load<Texture2D>("Default-LandscapeRight");
				landscapeRight = Content.Load<Texture2D>("Default-LandscapeLeft");
				portraitUpsideDown = Content.Load<Texture2D>("Default-PortraitUpsideDown");
				landscapeImagesAreRotated = false;
			}
			else // Phone device: just supporting landscape in this application:
			{
				Console.WriteLine("Loading Phone Device Graphics");
				landscapeLeft = landscapeRight = Content.Load<Texture2D>("Default");
				landscapeImagesAreRotated = true;
			}
		}


		float fadeTime = 0;

		protected override void Update(GameTime gameTime)
		{
			fadeTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

			MouseState mouseState = Mouse.GetState();
			if(mouseState.LeftButton == ButtonState.Pressed)
				fadeTime = 0;

			base.Update(gameTime);
		}


		void DrawLaunchImage(Texture2D image, bool rotated, float alpha)
		{
			if(image == null)
			{
				// This can happen because the portrait images are null on Phone devices,
				// But iOS will always start up (and draw a frame or two) in Portrait orientation!
				return;
			}

			Viewport vp = GraphicsDevice.Viewport;

			if(rotated) // The image we are drawing was saved rotated 90 degrees counter-clockwise
			{
				Rectangle rect = new Rectangle(vp.Width, 0, vp.Height, vp.Width);
				spriteBatch.Draw(image, rect, null, Color.White * alpha, MathHelper.PiOver2, Vector2.Zero, SpriteEffects.None, 0);
			}
			else // Draw normally:
			{
				spriteBatch.Draw(image, vp.Bounds, Color.White * alpha);
			}
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.White);

			spriteBatch.Begin();

			// Draw the cat girl
			Vector2 catGirlPosition = new Vector2(GraphicsDevice.Viewport.Width/2, GraphicsDevice.Viewport.Height/2);
			Vector2 catGirlOrigin = new Vector2(catGirl.Width/2, catGirl.Height/2);
			spriteBatch.Draw(catGirl, catGirlPosition, null, Color.White, 0, catGirlOrigin, 1, SpriteEffects.None, 0);

			// Draw the launch image over the top, fading out
			float alpha = 1.0f - Math.Min(fadeTime, 2f)/2f;
			switch(GraphicsDevice.PresentationParameters.DisplayOrientation)
			{
				default: // default orientation, assume landscape right
				case DisplayOrientation.LandscapeRight:
					DrawLaunchImage(landscapeRight, landscapeImagesAreRotated, alpha);
					break;

				case DisplayOrientation.LandscapeLeft:
					DrawLaunchImage(landscapeLeft, landscapeImagesAreRotated, alpha);
					break;

				case DisplayOrientation.Portrait:
					bool upsidedown = false;
#if MONOTOUCH
					// iOS also supports a PortraitUpsideDown orientation, which ExEn observes on the iPad (but ignores on the iPhone)
					upsidedown = (GraphicsDevice.PresentationParameters.ExEnInterfaceOrientation
							== ExEnInterfaceOrientation.PortraitUpsideDown);
#endif
					if(upsidedown)
						DrawLaunchImage(portraitUpsideDown, false, alpha);
					else
						DrawLaunchImage(portrait, false, alpha);
					break;
			}

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
