using System;
using System.Collections.Generic;
using MonoTouch.Foundation;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CatGirls.Controls;

namespace CatGirls.Tests
{
	[Preserve(AllMembers=true)]
	class ColorTest : Test
	{
		public static void DrawIcon(SpriteBatch sb, Texture2D icon, Vector2 position, int frame)
		{
			Color color = Color.White;
			switch((frame/30)%6)
			{
				default:
				case 0: color = Color.Red; break;
				case 1: color = Color.Yellow; break;
				case 2: color = Color.Lime; break;
				case 3: color = Color.Cyan; break;
				case 4: color = Color.Blue; break;
				case 5: color = Color.Magenta; break;
			}
			sb.Draw(icon, position, color);
		}


		Slider redSlider;
		Slider greenSlider;
		Slider blueSlider;
		Slider alphaSlider;
		Slider multiplySlider;
		Button blendModeButton;
		Button lockToGreenButton;
		Button doHintingButton;

		public override void Initialize()
		{
			base.Initialize();

			AddControl(redSlider = new Slider(new Rectangle(32, 32, 32, 256), Color.Red, 1f));
			AddControl(greenSlider = new Slider(new Rectangle(64, 32, 32, 256), Color.Lime, 1f));
			AddControl(blueSlider = new Slider(new Rectangle(96, 32, 32, 256), Color.Blue, 1f));
			AddControl(alphaSlider = new Slider(new Rectangle(128, 32, 32, 256), Color.White, 1f));
			AddControl(multiplySlider = new Slider(new Rectangle(192, 32, 32, 256), Color.CornflowerBlue, 1f));

			AddControl(blendModeButton = new Button(new Point(0, 0), Color.White));
			blendModeButton.Clicked += () => { blendMode = (blendMode + 1)%blendModeCount; };

			AddControl(lockToGreenButton = new Button(new Point(0, 288), Color.Green));
			lockToGreenButton.Clicked += () =>
			{
				lockToGreen = !lockToGreen;
				redSlider.Color = lockToGreen ? Color.Black : Color.Red;
				greenSlider.Color = lockToGreen ? Color.White : Color.Lime;
				blueSlider.Color = lockToGreen ? Color.Black : Color.Blue;
			};

#if DEBUG
			AddControl(doHintingButton = new Button(new Point(444, 288), Color.White));
			doHintingButton.Clicked += () =>
			{
				doHinting = !doHinting;
				doHintingButton.Color = doHinting ? Color.White : Color.Red;
			};
#endif
		}

		bool doHinting = true;

		bool lockToGreen; // quick and dirty to get pure grayscale
		
		int blendMode = 0;
		const int blendModeCount = 4;

		Rectangle catGirlRectangle;

		Rectangle catGirlSecondRectangle;
		bool displaySecondCatGirl = false;

		public override void Tap(Point point)
		{
			if(catGirlRectangle.Contains(point))
				displaySecondCatGirl = !displaySecondCatGirl;
			base.Tap(point);
		}


		public override void LoadContent()
		{
			int areaLeft = multiplySlider.Rectangle.X + multiplySlider.Rectangle.Width + 32;
			int areaRight = GraphicsDevice.Viewport.Width;
			int areaHeight = GraphicsDevice.Viewport.Height;

			catGirlRectangle = new Rectangle(areaLeft + (areaRight-areaLeft)/2 - CatGirl.Width/2,
					areaHeight/2 - CatGirl.Height/2, CatGirl.Width, CatGirl.Height);
			catGirlSecondRectangle = catGirlRectangle;
			catGirlSecondRectangle.X -= 40;
			catGirlSecondRectangle.Y -= 20;
		}

		public override void Draw(SpriteBatch sb)
		{
			GraphicsDevice.Clear(Color.Gray);
			base.Draw(sb);

			Point c = GraphicsDevice.Viewport.Bounds.Center;
			Vector2 catGirlPosition = new Vector2(c.X + 100, c.Y);

			Color color;
			if(lockToGreen)
				color = new Color(greenSlider.Value, greenSlider.Value, greenSlider.Value, alphaSlider.Value); 
			else
				color = new Color(redSlider.Value, greenSlider.Value, blueSlider.Value, alphaSlider.Value);

			color = color * multiplySlider.Value;

			BlendState blendState;
			switch (blendMode)
			{
				default:
				case 0: blendState = BlendState.AlphaBlend; break;
				case 1: blendState = BlendState.NonPremultiplied; break;
				case 2: blendState = BlendState.Additive; break;
				case 3: blendState = BlendState.Opaque; break;
			}

			sb.Begin(SpriteSortMode.Deferred, blendState);

			if(displaySecondCatGirl)
			{
				sb.HintDynamicColor(doHinting && blendMode != 0);
				sb.Draw(CatGirl, catGirlSecondRectangle, Color.White * multiplySlider.Value);
			}
			
			sb.HintDynamicColor(doHinting);
			sb.Draw(CatGirl, catGirlRectangle, color);
			sb.HintDynamicColor(false);

			sb.End();

			sb.Begin();
			sb.DrawString(UIFont, blendState.Name,
					new Vector2(blendModeButton.Rectangle.X + blendModeButton.Rectangle.Width + 5,
							blendModeButton.Rectangle.Y), Color.Black);
			sb.End();
		}
	}
}
