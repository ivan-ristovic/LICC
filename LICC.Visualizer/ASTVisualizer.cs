using System;
using System.Linq;
using System.Windows.Forms;
using LICC.AST;
using LICC.AST.Nodes;
using Serilog;

namespace LICC.Visualizer
{
    public sealed class ASTVisualizer
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static int Main(string[] args)
        {
            string? path = args?.FirstOrDefault();
            if (string.IsNullOrWhiteSpace(path)) {
                Log.Fatal("Missing source to visualize");
                return 1;
            }

            ASTNode ast = ASTFactory.BuildFromFile(path);
            return new ASTVisualizer().Visualize(ast);
        }


        public int Visualize(ASTNode? ast)
        {
            if (ast is null)
                return 1;

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VisualizeForm(ast));
            return 0;
        }
    }
}
