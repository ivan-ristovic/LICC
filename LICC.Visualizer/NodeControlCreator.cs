using System;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using LICC.AST.Nodes;
using Newtonsoft.Json;

namespace LICC.Visualizer
{
    internal sealed class NodeControlCreator
    {
        public Form Parent { get; }


        public NodeControlCreator(VisualizeForm parent)
        {
            this.Parent = parent;
        }


        public Panel CreateNodeControl(ASTNode node, int w, int h)
        {
            Panel frame = this.CreateFrame(node, w, h);

            PropertyInfo[] props = node.GetType()
                .GetProperties()
                .Where(p => !p.CustomAttributes.Any(attr => attr.AttributeType == typeof(JsonIgnoreAttribute)))
                .ToArray()
                ;

            int currh = 20;
            int maxw = frame.Width;
            foreach (PropertyInfo prop in props.OrderBy(p => p.Name)) {
                if (prop.Name == "Children" || prop.Name == "NodeType")
                    continue;
                string value = prop.GetValue(node)?.ToString() ?? "null";
                var l = new Label {
                    Text = $"{prop.Name}: {(string.IsNullOrWhiteSpace(value) ? "N/A" : value)}",
                    Parent = frame,
                    AutoSize = true,
                    Location = new Point(0, currh),
                    Font = new Font("Consolas", 9)
                };
                currh += l.Height;
                maxw = Math.Max(l.Width, maxw);
            }

            frame.Size = new Size(maxw, currh);
            return frame;
        }


        private Panel CreateFrame(ASTNode node, int w, int h)
        {
            var panel = new Panel() {
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.LightBlue,
                Location = new Point(w, h),
                Parent = this.Parent,
            };

            var title = new Label {
                Text = node.NodeType,
                Parent = panel,
                AutoSize = true,
                ForeColor = Color.Black,
                Font = new Font("Consolas", 11, FontStyle.Bold)
            };

            panel.Width = title.Width;

            return panel;
        }
    }
}
