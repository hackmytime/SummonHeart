using System;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.ui.UIComponents;
using Terraria.DataStructures;
using Terraria.UI;

namespace Paradigm.Library
{
    public class UIBuilder<T> where T : UIElement
	{
		public UIBuilder(T element)
		{
			this.element = element;
		}

		public UIBuilder<T> Init(T init)
		{
			this.element = init;
			return this;
		}

		public UIBuilder<T> Width(float width)
		{
			this.element.Width.Set(width, 0f);
			return this;
		}

		public UIBuilder<T> Height(float height)
		{
			this.element.Height.Set(height, 0f);
			return this;
		}

		public UIBuilder<T> Size(Point16 size)
		{
			this.element.Width.Set((float)size.X, 0f);
			this.element.Height.Set((float)size.Y, 0f);
			return this;
		}

		public UIBuilder<T> Size(float size)
		{
			this.element.Width.Set(size, 0f);
			this.element.Height.Set(size, 0f);
			return this;
		}

		public UIBuilder<T> Size(Texture2D size)
		{
			this.element.Width.Set((float)size.Width, 0f);
			this.element.Height.Set((float)size.Height, 0f);
			return this;
		}

		public UIBuilder<T> Top(float top)
		{
			this.element.Top.Set(top, 0f);
			return this;
		}

		public UIBuilder<T> Left(float left)
		{
			this.element.Left.Set(left, 0f);
			return this;
		}

		public UIBuilder<T> Left(Func<T, float> left)
		{
			float amount = left(this.element);
			this.element.Left.Set(amount, 0f);
			return this;
		}

		internal object Extra(Action<UIHoverImageButton> p)
		{
			throw new NotImplementedException();
		}

		public UIBuilder<T> Top(Func<T, float> top)
		{
			float amount = top(this.element);
			this.element.Top.Set(amount, 0f);
			return this;
		}

		public UIBuilder<T> Align(float hAlign, float vAlign)
		{
			this.element.HAlign = hAlign;
			this.element.VAlign = vAlign;
			return this;
		}

		public UIBuilder<T> Align(float align)
		{
			this.element.HAlign = align;
			this.element.VAlign = align;
			return this;
		}

		public UIBuilder<T> HAlign(float hAlign)
		{
			this.element.HAlign = hAlign;
			return this;
		}

		public UIBuilder<T> VAlign(float vAlign)
		{
			this.element.VAlign = vAlign;
			return this;
		}

		public UIBuilder<T> OnClick(UIElement.MouseEvent method)
		{
			this.element.OnClick += method;
			return this;
		}

		public UIBuilder<T> Extra(Action<T> act)
		{
			act(this.element);
			return this;
		}

		public T Append(UIElement baseElement)
		{
			baseElement.Append(this.element);
			return this.element;
		}

		public T element;
	}
}
