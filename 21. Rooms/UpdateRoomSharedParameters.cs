using System.Collections.Generic;
// Autodesk Revit
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


namespace BGO.Revit.Tools
{
    [Transaction(TransactionMode.Manual)]
    public class UpdateRoomSharedParameters:IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            WorksetId workingWorksetId = WorksetId.InvalidWorksetId;

            WorksetTable WST = doc.GetWorksetTable();
            if (null != WST)
            {
                workingWorksetId = WST.GetActiveWorksetId();
                var frm = new formSelectWorkset();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                    return Result.Succeeded;

                // frm.Tag return true when "All worksets" selected
                if ((bool)(frm.Tag)) workingWorksetId = WorksetId.InvalidWorksetId;
            }

            ICollection<ElementId> ElementIds = null;
            var col = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance));

            if (workingWorksetId == WorksetId.InvalidWorksetId)
            {
                ElementIds = col.ToElementIds();
            }
            else
            {
                var Filter = new ElementWorksetFilter(workingWorksetId, false);
                ElementIds = col.WherePasses(Filter).ToElementIds();
            }
            
            RoomUtils.UpdateRoomSharedParameters(doc, ElementIds);
            return Result.Succeeded;
        }
    }
}
