﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Graph
{
	public sealed partial class TextEditForm : Form
	{
        public bool Multiline { get; set; }

		public TextEditForm()
		{
            this.Multiline = false;
			InitializeComponent();
            this.TextTextBox.Multiline = this.Multiline;
		}

		public string InputText { get { return TextTextBox.Text; } set { TextTextBox.Text = value; } }
	}
}
