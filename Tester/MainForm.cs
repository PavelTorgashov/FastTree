using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tester
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new FastListSimplestSample().Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new FastListExpandedSample().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new FastListDragItemSample().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            new FastListDropItemSample().Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            new FastListReadonlyAndDisabledItemsSample().Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            new FastTreeSimplestSample().Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            new FastTreeFileExplorerSample().Show();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            new FastTreeFileExplorerSample2().Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            new FastTreeDragAndDropSample().Show();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            new FastListVirtualCheckboxesSample().Show();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            new FastListStressTest().Show();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            new FastTreeStressTest().Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            new FastListCustomize().Show();
        }
    }
}
