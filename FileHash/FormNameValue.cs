using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sha512
{
    public partial class FormNameValue : Form
    {
        public FormNameValue()
        {
            InitializeComponent();
        }

        public FormNameValue(string caption, string value) : this()
        {
            Text = caption;
            textBoxValue.Text = value;
        }
    }
}
