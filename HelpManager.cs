using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace face_builder
{
    public static class HelpManager
    {
        private static readonly string helpURL = "https://richardson902.github.io/face-builder-help/";


        public static void DisplayKeyBindings()
        {
            Help.ShowHelp(null, helpURL + "ShortcutKeys.html");
        }

        public static void DisplayAbout()
        {
            Help.ShowHelp(null, helpURL + "Overview.html");
        }

        public static void DisplayAddImages()
        {
            Help.ShowHelp(null, helpURL + "AddingImages.html");
        }
    }
}
