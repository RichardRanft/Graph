using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Graph;
using Graph.Items;
using Graph.Compatibility;

namespace GraphNodes
{
    public class NodeConversationItem : NodeItem
    {
        public event EventHandler<AcceptNodeTextChangedEventArgs> TextChanged;

        public NodeConversationItem(string text, NodeIOMode mode) :
            base(mode)
        {
            this.Text = text;
        }

		#region Text
		string publicText = string.Empty;
		public string Text
		{
			get { return publicText; }
			set
			{
				if (publicText == value)
					return;
				publicText = value;
				TextSize = Size.Empty;
			}
		}
		#endregion

		public SizeF TextSize;

        public override bool OnDoubleClick()
        {
            base.OnDoubleClick();
            var form = new TextEditForm();
            form.Text = Name ?? "Edit text";
            form.InputText = Text;
            var result = form.ShowDialog();
            if (result == DialogResult.OK)
                Text = form.InputText;
            return true;
        }
		
		const int ColorBoxSize = 16;
		const int Spacing = 2;

        public override SizeF Measure(Graphics graphics)
		{
			if (!string.IsNullOrWhiteSpace(this.Text))
			{
				if (this.TextSize.IsEmpty)
				{
					var size = new Size(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight);

					if (this.Input.Enabled != this.Output.Enabled)
					{
						if (this.Input.Enabled)
							this.TextSize = graphics.MeasureString(this.Text, SystemFonts.MenuFont, size, GraphConstants.LeftMeasureTextStringFormat);
						else
							this.TextSize = graphics.MeasureString(this.Text, SystemFonts.MenuFont, size, GraphConstants.RightMeasureTextStringFormat);
					} else
						this.TextSize = graphics.MeasureString(this.Text, SystemFonts.MenuFont, size, GraphConstants.CenterMeasureTextStringFormat);

					this.TextSize.Width  = Math.Max(size.Width, this.TextSize.Width + ColorBoxSize + Spacing);
					this.TextSize.Height = Math.Max(size.Height, this.TextSize.Height);
				}
				return this.TextSize;
			} else
			{
				return new SizeF(GraphConstants.MinimumItemWidth, GraphConstants.TitleHeight + GraphConstants.TopHeight);
			}
		}

        public override void Render(Graphics graphics, SizeF minimumSize, PointF location)
		{
			var size = Measure(graphics);
			size.Width  = Math.Max(minimumSize.Width, size.Width);
			size.Height = Math.Max(minimumSize.Height, size.Height);

			var alignment	= HorizontalAlignment.Center;
			var format		= GraphConstants.CenterTextStringFormat;
			if (this.Input.Enabled != this.Output.Enabled)
			{
				if (this.Input.Enabled)
				{
					alignment	= HorizontalAlignment.Left;
					format		= GraphConstants.LeftTextStringFormat;
				} else
				{
					alignment	= HorizontalAlignment.Right;
					format		= GraphConstants.RightTextStringFormat;
				}
			}

			var rect		= new RectangleF(location, size);
			var colorBox	= new RectangleF(location, size);
			colorBox.Width	= ColorBoxSize;
			switch (alignment)
			{
				case HorizontalAlignment.Left:
					rect.Width	-= ColorBoxSize + Spacing;
					rect.X		+= ColorBoxSize + Spacing;
					break;
				case HorizontalAlignment.Right:
					colorBox.X	= rect.Right - colorBox.Width;
					rect.Width	-= ColorBoxSize + Spacing;
					break;
				case HorizontalAlignment.Center:
					rect.Width	-= ColorBoxSize + Spacing;
					rect.X		+= ColorBoxSize + Spacing;
					break;
			}

			graphics.DrawString(this.Text, SystemFonts.MenuFont, Brushes.Black, rect, format);

			using (var path = GraphRenderer.CreateRoundedRectangle(colorBox.Size, colorBox.Location))
			{
				using (var brush = new SolidBrush(Color.Black))
				{
					graphics.FillPath(brush, path);
				}
				if ((state & RenderState.Hover) != 0)
					graphics.DrawPath(Pens.White, path);
				else
					graphics.DrawPath(Pens.Black, path);
			}
		}
    }
}
