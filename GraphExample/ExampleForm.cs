using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Graph;
using System.Drawing.Drawing2D;
using Graph.Compatibility;
using Graph.Items;

namespace GraphNodes
{
	public partial class ExampleForm : Form
	{
		public ExampleForm()
		{
			InitializeComponent();

			graphControl.CompatibilityStrategy = new TagTypeCompatibility();
		}

		void OnImgClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("IMAGE");
		}

		void OnColClicked(object sender, NodeItemEventArgs e)
		{
			MessageBox.Show("Color");
		}

		void OnConnectionRemoved(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		void OnShowElementMenu(object sender, AcceptElementLocationEventArgs e)
		{
			if (e.Element == null)
			{
				// Show a test menu for when you click on nothing
				testMenuItem.Text = "(clicked on nothing)";
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is Node)
			{
				// Show a test menu for a node
				testMenuItem.Text = ((Node)e.Element).Title;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			if (e.Element is NodeItem)
			{
				// Show a test menu for a nodeItem
				testMenuItem.Text = e.Element.GetType().Name;
				nodeMenu.Show(e.Position);
				e.Cancel = false;
			} else
			{
				// if you don't want to show a menu for this item (but perhaps show a menu for something more higher up) 
				// then you can cancel the event
				e.Cancel = true;
			}
		}

		void OnConnectionAdding(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
		}

		static int counter = 1;
		void OnConnectionAdded(object sender, AcceptNodeConnectionEventArgs e)
		{
			//e.Cancel = true;
			e.Connection.Name = "Connection " + counter ++;
			e.Connection.DoubleClick += new EventHandler<NodeConnectionEventArgs>(OnConnectionDoubleClick);
		}

		void OnConnectionDoubleClick(object sender, NodeConnectionEventArgs e)
		{
			e.Connection.Name = "Connection " + counter++;
		}

		private void SomeNode_MouseDown(object sender, MouseEventArgs e)
		{
			var node = new Node("Some node");
            node.AddItem(new NodeLabelItem("Entry 1", NodeIOMode.Input) { Tag = TagType.COLOR });
            node.AddItem(new NodeLabelItem("Entry 2", NodeIOMode.Input) { Tag = TagType.IMAGE });
            node.AddItem(new NodeLabelItem("Entry 3", NodeIOMode.Input) { Tag = TagType.TEXTBOX });
            node.AddItem(new NodeTextBoxItem("TEXTTEXT", NodeIOMode.Output) { Tag = TagType.TEXTBOX });
            node.AddItem(new NodeDropDownItem(new string[] { "1", "2", "3", "4" }, 0, NodeIOMode.Output) { Tag = TagType.TEXTBOX });
            var compItem = new NodeCompositeItem(NodeIOMode.InOut) { Tag = TagType.COMPOSITE };
            var txtPart = new ItemTextBoxPart("Test text");
            compItem.AddPart(txtPart);
            node.AddItem(compItem);
			this.DoDragDrop(node, DragDropEffects.Copy);
		}

		private void TextureNode_MouseDown(object sender, MouseEventArgs e)
		{
			var textureNode = new Node("Texture");
			textureNode.Location = new Point(300, 150);
            var imageItem = new NodeImageItem(Properties.Resources.example, 64, 64, NodeIOMode.Output) { Tag = TagType.IMAGE };
			imageItem.Clicked += new EventHandler<NodeItemEventArgs>(OnImgClicked);
			textureNode.AddItem(imageItem);
			this.DoDragDrop(textureNode, DragDropEffects.Copy);
		}

		private void ColorNode_MouseDown(object sender, MouseEventArgs e)
		{
			var colorNode = new Node("Color");
			colorNode.Location = new Point(200, 50);
			var redChannel = new NodeSliderItem("R", 64.0f, 16.0f, 0, 1.0f, 0.0f);
            var greenChannel = new NodeSliderItem("G", 64.0f, 16.0f, 0, 1.0f, 0.0f);
            var blueChannel = new NodeSliderItem("B", 64.0f, 16.0f, 0, 1.0f, 0.0f);
            var colorItem = new NodeColorItem("Color", Color.Black, NodeIOMode.Output) { Tag = TagType.COLOR };

			EventHandler<NodeItemEventArgs> channelChangedDelegate = delegate(object s, NodeItemEventArgs args)
			{
				var red = redChannel.Value;
				var green = greenChannel.Value;
				var blue = blueChannel.Value;
				colorItem.Color = Color.FromArgb((int)Math.Round(red * 255), (int)Math.Round(green * 255), (int)Math.Round(blue * 255));
			};
			redChannel.ValueChanged += channelChangedDelegate;
			greenChannel.ValueChanged += channelChangedDelegate;
			blueChannel.ValueChanged += channelChangedDelegate;


			colorNode.AddItem(redChannel);
			colorNode.AddItem(greenChannel);
			colorNode.AddItem(blueChannel);

			colorItem.Clicked += new EventHandler<NodeItemEventArgs>(OnColClicked);
			colorNode.AddItem(colorItem);

			this.DoDragDrop(colorNode, DragDropEffects.Copy);
		}

        private void CompNode_MouseDown(object sender, MouseEventArgs e)
        {
            var compNode = new Node("Composite Node");
            compNode.Location = new Point(300, 150);
            var compItem = new NodeCompositeItem(NodeIOMode.Output) { Tag = TagType.COMPOSITE };
            var compTxtPart = new ItemTextBoxPart("Test text");
            compItem.AddPart(compTxtPart);
            var compTxtPart2 = new ItemTextBoxPart("Test 2 text with really long text");
            compItem.AddPart(compTxtPart2);
            compNode.AddItem(compItem);
            this.DoDragDrop(compNode, DragDropEffects.Copy);
        }

        private void OnShowLabelsChanged(object sender, EventArgs e)
		{
			graphControl.ShowLabels = showLabelsCheckBox.Checked;
		}
	}

    public static class TagType
    {
        public static CheckboxClass CHECKBOX = new CheckboxClass();
        public static ColorClass COLOR = new ColorClass();
        public static DropDownClass DROPDOWN = new DropDownClass();
        public static ImageClass IMAGE = new ImageClass();
        public static LabelClass LABEL = new LabelClass();
        public static NumericSliderClass NUMERICSLIDER = new NumericSliderClass();
        public static SliderClass SLIDER = new SliderClass();
        public static TextBoxClass TEXTBOX = new TextBoxClass();
        public static NodeTitleClass NODETITLE = new NodeTitleClass();
        public static CompositeClass COMPOSITE = new CompositeClass();
    }

    public class CheckboxClass : object { }
    public class ColorClass : object { }
    public class DropDownClass : object { }
    public class ImageClass : object { }
    public class LabelClass : object { }
    public class NumericSliderClass : object { }
    public class SliderClass : object { }
    public class TextBoxClass : object { }
    public class NodeTitleClass : object { }
    public class CompositeClass : Object { }
}
