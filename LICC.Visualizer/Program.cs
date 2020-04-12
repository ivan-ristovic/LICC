using System;
using System.Windows.Forms;
using CommandLine;
using LICC.AST;
using LICC.AST.Nodes;

namespace LICC.Visualizer
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Options>(args)
                .MapResult(
                    (Options o) => Visualize(o),
                    errs => 1
                );
        }

        private static int Visualize(Options o)
        {
            ASTNode ast = ASTFactory.BuildFromFile(o.Source);
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new VisualizeForm(ast));
            return 0;
        }
    }
}
