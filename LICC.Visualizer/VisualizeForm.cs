using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LICC.AST.Nodes;

namespace LICC.Visualizer
{
    public partial class VisualizeForm : Form
    {
        private readonly NodeControlCreator cc;


        public VisualizeForm(ASTNode ast)
        {
            this.InitializeComponent();
            this.cc = new NodeControlCreator(this);

            this.Text = "AST Visualizer";
            this.Size = new Size(1200, 800);
            this.AutoScroll = true;

            this.Draw(ast, this.Width / 2, 10, this.Width);
        }


        private Panel Draw(ASTNode node, int x, int y, int w, int depth = 1)
        {
            Panel p = this.cc.CreateNodeControl(node, x, y);

            int cpx = x;
            int step = w / (node.Children.Count + 1);
            foreach (ASTNode child in node.Children) {
                this.Draw(child, cpx, y + 100, w / 2, depth + 1);
                cpx += step;
            }

            //using (Graphics g = this.CreateGraphics()) {
            //    g.TranslateTransform(100, 100);
            //    int sx = p.Location.X + p.Width / 2;
            //    int sy = p.Location.Y + p.Height;
            //    int dx = cp.Location.X + cp.Width / 2;
            //    int dy = p.Location.Y;
            //    g.DrawLine(new Pen(Color.Black), sx, sy, dx, dy);
            //}

            return p;
        }

        private int Depth(ASTNode node)
            => node.Children.Count == 0 ? 0 : node.Children.Max(c => this.Depth(c)) + 1;
    }
}
