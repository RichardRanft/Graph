using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace Graph.Items
{
    public sealed class NodePartEventArgs : EventArgs
    {
        public NodePartEventArgs(ItemPart part) { Part = part; }
        public ItemPart Part { get; private set; }
    }

    public abstract class ItemPart : IElement
    {
        public ItemPart()
        {
        }

        public NodeItem Item { get; internal set; }
        public String Name { get; set; }

        public NodeConnector Input { get; private set; }
        public NodeConnector Output { get; private set; }

        public RectangleF bounds;
        public RenderState state = RenderState.None;

        public virtual bool OnClick() { return false; }
        public virtual bool OnDoubleClick() { return false; }
        public virtual bool OnStartDrag(PointF location, out PointF original_location) { original_location = Point.Empty; return false; }
        public virtual bool OnDrag(PointF location) { return false; }
        public virtual bool OnEndDrag() { return false; }
        public abstract SizeF Measure(Graphics context);
        public abstract void Render(Graphics graphics, SizeF minimumSize, PointF position);

        public ElementType ElementType { get { return ElementType.ItemPart; } }
    }
}
