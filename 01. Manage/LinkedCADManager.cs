using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using BGO.Revit.Tools;

namespace BGO.Revit.Manage
{
    class LinkedCadManager : IExternalCommand
    {
        public string Name { get; set; }
        public bool pined { get; set; }
        public bool imported { get; set; }
        public bool linked { get; set; }

        public LinkedCadManager()
            {
            }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            return Result.Succeeded;
        }
    }
}
