#region License
// Copyright (c) 2009 Sander van Rossen
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Graph.Items
{
	public sealed class NodeCompositeItem : NodeItem
	{
        public event EventHandler<NodeItemEventArgs> Clicked;

        public NodeCompositeItem(NodeIOMode mode) :
            base(mode)
        {
            PartSize = new SizeF(GraphConstants.MinimumItemWidth, GraphConstants.MinimumItemHeight);
            itemParts = new List<ItemPart>();
        }

		internal SizeF PartSize;
        internal List<ItemPart> itemParts;

        public override bool OnClick()
        {
            base.OnClick();
            if (Clicked != null)
                Clicked(this, new NodeItemEventArgs(this));
            return true;
        }

        public void AddPart(ItemPart part)
        {
            if (itemParts.Contains(part))
                return;
            itemParts.Add(part);
        }

        public void RemovePart(ItemPart part)
        {
            if (!itemParts.Contains(part))
                return;
            itemParts.Remove(part);
        }

        public override SizeF Measure(Graphics graphics)
		{
            int partsHeight = 1;
            int partsWidth = 0;
            foreach (ItemPart part in itemParts)
            {
                SizeF size = part.Measure(graphics);
                partsHeight += (int)size.Height + 1;
                partsWidth = (partsWidth >= size.Width ? partsWidth : (int)size.Width + 4);
            }
            partsHeight += 3;
            int height = partsHeight > GraphConstants.MinimumItemHeight ? partsHeight : GraphConstants.MinimumItemHeight;
			return new SizeF(partsWidth, height);
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
            }
            else
            {
                graphics.DrawPath(Pens.Black, path);
            }
            location.X += 1;
            location.Y += 1;
            foreach(ItemPart part in itemParts)
            {
                SizeF partSize = part.Measure(graphics);
                part.Render(graphics, partSize, new PointF(location.X, location.Y));
                location.Y += partSize.Height + 1;
            }
        }
	}
}
