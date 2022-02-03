// Autodesk Revit
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BGO.Revit.Tools
{
    [Transaction(TransactionMode.Manual)]
    public class SynchronizeSheetNames : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument UIdoc = commandData.Application.ActiveUIDocument;
            SheetUtils.SynchronizeSheetNames(UIdoc);
            return Result.Succeeded;
        }
    }
}
