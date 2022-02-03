using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Autodesk.Revit;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;




namespace BGO.Revit.Tools
{
    [Transaction(TransactionMode.Manual)]
    public class InfoFlac : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument UIdoc = commandData.Application.ActiveUIDocument;
            Document doc = UIdoc.Document;

            InfoFlacForm showInfoFlacForm = new InfoFlacForm();
            showInfoFlacForm.ShowDialog();

            return Result.Succeeded;
        }


    }
}
