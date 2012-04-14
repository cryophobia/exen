using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class TimingTest : Test
	{
		DateTime start;
		DateTime calculatedTime;
		DateTime actualTime;

		int updatesSinceLastDraw;
		int maxUpdatesSinceLastDraw;
		int drawSkipCount;

		float catGirlTime;
		const float catGirlTotalTime = 2; // seconds

		void Reset()
		{
			start = calculatedTime = actualTime = DateTime.Now;
			maxUpdatesSinceLastDraw = updatesSinceLastDraw = drawSkipCount = 0;
			catGirlTime = 0;
		}

		public override void BeginRun()
		{
			base.BeginRun();
			Reset();
		}


		public override void Update(float seconds)
		{
			base.Update(seconds);

			actualTime = DateTime.Now;
			calculatedTime += TimeSpan.FromSeconds(seconds);

			updatesSinceLastDraw++;

			catGirlTime += (seconds / catGirlTotalTime);
			if(catGirlTime > 1f)
				catGirlTime -= 1f;
		}


		const string format = "Target FPS = {0}\n\n"
				+ "Start = {1}\nReal = {2}\nGame = {3}\nDiff = {4}\n"
				+ "Diff/sec = {5}\n\n"
				+ "Updates = {6} (Max = {7})\nDraw Skips = {8}";

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.White);
			base.Draw(sb);
			
			Viewport vp = GraphicsDevice.Viewport;
			Vector2 catGirlStart = new Vector2(-vp.Width / 2, vp.Height * 0.75f).Floor();
			Vector2 catGirlEnd = catGirlStart + new Vector2(vp.Width, 0);

			string text = string.Format(format,
					(double)TimeSpan.TicksPerSecond / (double)Game.TargetElapsedTime.Ticks,
					start.ToLongTimeString(), actualTime.ToLongTimeString(), calculatedTime.ToLongTimeString(),
					calculatedTime - actualTime,
					(calculatedTime - actualTime).TotalSeconds / (actualTime - start).TotalSeconds,
					updatesSinceLastDraw, maxUpdatesSinceLastDraw, drawSkipCount);

			sb.Begin();
			sb.Draw(CatGirl, Vector2.Lerp(catGirlStart, catGirlEnd, catGirlTime), Color.White);
			sb.Draw(CatGirl, Vector2.Lerp(catGirlStart, catGirlEnd, catGirlTime+1), Color.White);
			sb.DrawString(UIFont, text, Vector2.Zero, Color.Black);
			sb.End();

			drawSkipCount += (updatesSinceLastDraw - 1);
			maxUpdatesSinceLastDraw = Math.Max(maxUpdatesSinceLastDraw, updatesSinceLastDraw);
			updatesSinceLastDraw = 0;
		}
	}
}
