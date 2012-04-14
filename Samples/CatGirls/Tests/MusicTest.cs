using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class MusicTest : Test
	{
		Song song;
		SoundEffect soundEffect;
		SoundEffectInstance loop;

		public override void LoadContent()
		{
			song = Content.Load<Song>("Short Disco");
			song.HintBeginPreload();
			soundEffect = Content.Load<SoundEffect>("ding");
			loop = Content.Load<SoundEffect>("motor").CreateInstance();
			loop.IsLooped = true;
		}

		public override void UnloadContent()
		{
			loop.Dispose();
		}

		bool loopIsPlaying;
		bool musicIsPlaying;
		bool startMusicNext = true;

		public override void BeginRun()
		{
			base.BeginRun();
			MediaPlayer.IsRepeating = true;
		}

		public override void EndRun()
		{
			base.EndRun();
			StopLoopAndMusic();
		}

		void StopLoopAndMusic()
		{
			if(musicIsPlaying)
			{
				MediaPlayer.Stop();
				musicIsPlaying = false;
			}
			if(loopIsPlaying)
			{
				loop.Stop();
				loopIsPlaying = false;
			}
		}

		public override void MouseDown(Point point)
		{
			base.MouseDown(point);

			soundEffect.Play();
			StopLoopAndMusic();

			if(startMusicNext)
			{
				MediaPlayer.Play(song);
				musicIsPlaying = true;
				startMusicNext = false;
			}
			else
			{
				loop.Play();
				loopIsPlaying = true;
				startMusicNext = true;
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.White);
			base.Draw(sb);
		}
	}



}

