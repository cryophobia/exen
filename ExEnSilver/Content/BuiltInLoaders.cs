using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Microsoft.Xna.Framework.Content
{
	public class BuiltInLoaders
	{
		static readonly string[] texture2DExtensions = { ".png", ".jpg", ".jpeg" };
		static readonly string[] encodedAudioExtensions = { ".mp3", ".wma" };

		static GraphicsDevice GetGraphicsDevice(ContentManager contentManager)
		{
			var gds = contentManager.ServiceProvider.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
			if(gds == null)
				throw new InvalidOperationException("No graphics device service");
			var gd = gds.GraphicsDevice;
			if(gd == null)
				throw new InvalidOperationException("No graphics device");
			return gd;
		}


		static Texture2D LoadTexture(string assetName, ContentManager contentManager)
		{
			Stream stream = ContentHelpers.GetAssetStream(assetName, contentManager.RootDirectory, texture2DExtensions);
			return new Texture2D(stream, GetGraphicsDevice(contentManager));
		}

		static SpriteFont LoadSpriteFont(string assetName, ContentManager contentManager)
		{
			SpriteFont font = null;
			SilverlightFontTranslations.list.TryGetValue(assetName, out font);

			if(font == null)
			{
				Texture2D texture = LoadTexture(assetName + "-exenfont", contentManager);
				using(Stream metricsDataStream = ContentHelpers.GetAssetStream(assetName, contentManager.RootDirectory, "-exenfont.exenfont"))
				{
					font = new SpriteFontBitmap(texture, metricsDataStream, 1f);
				}
			}

			return font;
		}


		static SoundEffect LoadSoundEffect(string assetName, ContentManager contentManager)
		{
			bool isWav = false;
			Stream stream = ContentHelpers.GetAssetStream(assetName, contentManager.RootDirectory, ".wav");
			if(stream != null)
				isWav = true;
			else
				stream = ContentHelpers.GetAssetStream(assetName, contentManager.RootDirectory, encodedAudioExtensions);
			return new SoundEffect(stream, isWav, GetGraphicsDevice(contentManager));
		}

		static Song LoadSong(string assetName, ContentManager contentManager)
		{
			return new Song(ContentHelpers.GetAssetUri(assetName, contentManager.RootDirectory, ".mp3"),
					GetGraphicsDevice(contentManager));
		}


		static bool hasRegistered = false;
		internal static void Register()
		{
			if(!hasRegistered)
			{
				ContentManager.RegisterLoader<Texture2D>(LoadTexture);
				ContentManager.RegisterLoader<SpriteFont>(LoadSpriteFont);
				ContentManager.RegisterLoader<SoundEffect>(LoadSoundEffect);
				ContentManager.RegisterLoader<Song>(LoadSong);

				hasRegistered = true;
			}
		}
	}
}
