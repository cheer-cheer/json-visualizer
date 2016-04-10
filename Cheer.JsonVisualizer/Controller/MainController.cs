using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cheer.JsonVisualizer.UI;

namespace Cheer.JsonVisualizer.Controller
{
    internal class MainController
    {
        public MainController()
        {
        }

        public JsonView JsonView
        {
            get;
            set;
        }

        public MainForm MainForm
        {
            get;
            set;
        }
    }
}
