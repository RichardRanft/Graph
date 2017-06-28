using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace Graph.Items
{
    public sealed class ItemTextBoxPart : ItemPart
    {
		public event EventHandler<AcceptNodeTextChangedEventArgs> TextChanged;

        public bool Multiline { get; set; }

        public ItemTextBoxPart(string text, bool multi = false) :
            base()
        {
            this.Multiline = multi;
            this.Text = text;
        }

		#region Text
		string internalText = string.Empty;
		public string Text
		{
			get { return internalText; }
			set
			{
				if (internalText == value)
					return;
				if (TextChanged != null)
				{
					var eventArgs = new AcceptNodeTextChangedEventArgs(internalText, value);
					TextChanged(this, eventArgs);
					if (eventArgs.Cancel)
						return;
					internalText = eventArgs.Text;
				} else
					internalText = value;
				TextSize = Size.Empty;
			}
		}
		#endregion

		internal SizeF TextSize;

		public override bool OnDoubleClick()
		{
			base.OnDoubleClick();
			var form = new TextEditForm();
            form.Multiline = Multiline;
            form.Text = Name ?? "Edit text";
			form.InputText = Text;
			var result = form.ShowDialog();
			if (result == DialogResult.OK)
				Text = form.InputText;
			return true;
		}

        public override SizeF Measure(Graphics graphics)
		{
			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				if (this.TextSize.IsEmpty)
				{
					var size = new Size(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight);

					this.TextSize = graphics.MeasureString(this.Text, SystemFonts.MenuFont, size, GraphConstants.LeftMeasureTextStringFormat);
					
					this.TextSize.Width  = Math.Max(size.Width, this.TextSize.Width + 8);
					this.TextSize.Height = Math.Max(size.Height, this.TextSize.Height + 2);
				}
				return this.TextSize;
			} else
			{
				return new SizeF(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight);
			}
		}

        public override void Render(Graphics graphics, SizeF minimumSize, PointF location)
		{
			var size = Measure(graphics);
			size.Width  = Math.Max(minimumSize.Width, size.Width);
			size.Height = Math.Max(minimumSize.Height, size.Height);

			var path = GraphRenderer.CreateRoundedRectangle(size, location);

			location.Y += 1;
			location.X += 1;

			if ((state & RenderState.Hover) == RenderState.Hover)
			{
				graphics.DrawPath(Pens.White, path);
				graphics.DrawString(this.Text, SystemFonts.MenuFont, Brushes.Black, new RectangleF(location, size), GraphConstants.LeftTextStringFormat);
			} else
			{
				graphics.DrawPath(Pens.Black, path);
				graphics.DrawString(this.Text, SystemFonts.MenuFont, Brushes.Black, new RectangleF(location, size), GraphConstants.LeftTextStringFormat);
			}
		}
    }
}
