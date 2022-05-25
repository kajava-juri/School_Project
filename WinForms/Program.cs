using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.View;

namespace WinForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var repository = new Model.ArtistClient();
            var view = new View.ArtistForm();

            // Poor Man's Dependency Injection/Pure Dependency Injection, Main() is the Composition Root.
            // See https://github.com/mrts/winforms-mvp/issues/2.
            var presenter = new Presenter.ArtistPresenter(view, repository);

            Application.Run(view);
        }
    }
}
