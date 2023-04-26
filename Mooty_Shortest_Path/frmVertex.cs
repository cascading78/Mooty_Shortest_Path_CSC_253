using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mooty_Shortest_Path
{

    public partial class frmAddVertex : Form
    {

        private DirectedVertex currentVertex = null;

        public frmAddVertex()
        {
            InitializeComponent();
        }

        public void LoadVertex(DirectedVertex v, bool editMode = false)
        {
            btnApply.Text = editMode ? "Update" : "Add";

            currentVertex = v;

            txtLabel.Text = v.Label;
            txtX.Text = v.X.ToString();
            txtY.Text = v.Y.ToString();

            foreach (DirectedEdge e in v.Edges)
            {
                ListViewItem item = new ListViewItem(e.To.Label);
                item.SubItems.Add(e.Weight.ToString());
                lvwEdges.Items.Add(item);
            }

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            currentVertex.Label = txtLabel.Text;
            this.Close();
        }
    }


}
