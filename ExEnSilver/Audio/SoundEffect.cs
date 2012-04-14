using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Collections.Generic;
using System.Windows.Resources;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WaveMSS;

namespace Microsoft.Xna.Framework.Audio
{
	public partial class SoundEffect : IDisposable
	{
		#region Constructor and Data

		GraphicsDevice device;

		bool isWav = false;
		byte[] soundBuffer;

		internal SoundEffect(Stream stream, bool isWav, GraphicsDevice graphicsDevice)
		{
			this.isWav = isWav;

			soundBuffer = new byte[stream.Length];
			stream.Read(soundBuffer, 0, soundBuffer.Length);
			stream.Close();

			this.device = graphicsDevice;
		}

		public void Dispose()
		{
			DisposeFireAndForgetQueue();
		}

		#endregion


		public SoundEffectInstance CreateInstance()
		{
			SoundEffectInstance instance;
			MemoryStream stream = new MemoryStream(soundBuffer);
			if(isWav)
				instance = new SoundEffectInstance(device.Root, new WaveMediaStreamSource(stream));
			else
				instance = new SoundEffectInstance(device.Root, stream);

			return instance;
		}

	}
}
