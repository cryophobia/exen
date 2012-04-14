using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CatGirls.Tests;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CatGirls
{
	class TestMenu
	{

		#region Menu Items (Tests)

		delegate void DrawIcon(SpriteBatch sb, Texture2D icon, Vector2 position, int frame);

		struct TestMenuItem
		{
			public string name;
			public DrawIcon drawIcon;
			public Type type;

			public static int CompareByName(TestMenuItem a, TestMenuItem b)
			{
				return a.name.CompareTo(b.name);
			}
		}

		List<TestMenuItem> items = new List<TestMenuItem>();

		static string NiceClassName(string s)
		{
			StringBuilder r = new StringBuilder(s.Length + 3);
			for(int i = 0; i < s.Length; i++)
			{
				if(char.IsUpper(s[i]) && i != 0)
					r.Append(' ');
				r.Append(s[i]);
			}
			return r.ToString();
		}

		void FillItemList()
		{
			var types = Assembly.GetExecutingAssembly().GetTypes();
			foreach(var type in types)
			{
				if(type.IsSubclassOf(typeof(Tests.Test)))
				{
					TestMenuItem menuItem = new TestMenuItem();
					menuItem.type = type;
					menuItem.name = NiceClassName(type.Name);

					var iconFunction = type.GetMethod("DrawIcon", BindingFlags.Static | BindingFlags.Public);
					if(iconFunction != null)
					{
						try
						{
							menuItem.drawIcon = Delegate.CreateDelegate(typeof(DrawIcon), iconFunction) as DrawIcon;
						} catch { }
					}

					items.Add(menuItem);
				}
			}

			items.Sort(TestMenuItem.CompareByName);
		}

		#endregion

				

		Texture2D icon;
		SpriteFont font;
				
		Vector2 startPosition;
		Vector2 textOffset; 
		Vector2 itemAdvance;


		public TestMenu(GraphicsDevice graphics, ContentManager content)
		{
			FillItemList();

			icon = content.Load<Texture2D>("CatGirlIcon");
			font = content.Load<SpriteFont>("UIFont");
			
			startPosition = new Vector2(5f, 5f);
			textOffset = new Vector2(icon.Width + 8f, 0f);
			itemAdvance = new Vector2(0, icon.Height + 5f);
		}

		Vector2 ItemTopLeft(int i)
		{
			return startPosition + i * itemAdvance;
		}

		Vector2 ItemBottomRight(int i)
		{
			Vector2 textSize = font.MeasureString(items[i].name);
			return ItemTopLeft(i) + textOffset + new Vector2(textSize.X, icon.Height);
		}

		public void Draw(SpriteBatch sb)
		{
			sb.Begin();

			for(int i = 0; i < items.Count; i++)
			{
				var item = items[i];
				Vector2 position = ItemTopLeft(i);

				if(item.drawIcon != null)
					item.drawIcon(sb, icon, position, frame);
				sb.DrawString(font, item.name, position + textOffset, Color.Black);
			}

			sb.End();
		}


		int frame = 0;

		public void Update(float seconds)
		{
			frame++;
		}

		public event Action<Type> TestSelected;


		public void Tap(Point point)
		{
			for(int i = 0; i < items.Count; i++)
			{
				Vector2 topLeft = ItemTopLeft(i);
				Vector2 bottomRight = ItemBottomRight(i);
				if(point.X > topLeft.X && point.X < bottomRight.X && point.Y > topLeft.Y && point.Y < bottomRight.Y)
				{
					if(TestSelected != null)
						TestSelected(items[i].type);
				}
			}
		}

		public void MouseMove(Point point) { }
		public void MouseUp(Point point) { }
		public void MouseDown(Point point) { }
	}
}
