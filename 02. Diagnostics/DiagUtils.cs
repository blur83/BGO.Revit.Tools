using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BGO.Revit.Tools
{
    class DiagUtils
    {
        public static void UpdateDiagnosticSharedParameters(Document doc, ICollection<ElementId> Ids)
        {
            using (Transaction tr = new Transaction(doc))
            {
                try
                {
                    tr.Start("Update Diagnostic Shared Parameters");

                    int instanceCount = 0;
                    int updatedCount = 0;
                    //ClearMarkParameter(doc, Ids);
                    foreach (ElementId id in Ids)
                    {
                        Element e = doc.GetElement(id);
                        var fi = e as FamilyInstance;
                        if (null != fi)
                        {
                            instanceCount++;
                            var EditedBy = fi.get_Parameter(BuiltInParameter.EDITED_BY).AsString();
                            if (EditedBy == "" || EditedBy == doc.Application.Username)
                            {
                                //var MarkParameter = fi.LookupParameter(BuiltInParameter.ALL_MODEL_MARK);
                                //MarkParameter.Set(string.Format("Id:{0}", id.IntegerValue));

                                var HostSharedParameter = fi.LookupParameter("@G Host");
                                if (null != HostSharedParameter)
                                {
                                    var HostParameter = fi.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_PARAM);
                                    string HostName = HostParameter.AsString();
                                    HostSharedParameter.Set(HostName == null ? "<null>" : HostName);
                                }

                                var ElevationSharedParameter = fi.LookupParameter("@G Elevation");
                                if (null != ElevationSharedParameter)
                                {
                                    var ElevationParameter = fi.get_Parameter(BuiltInParameter.INSTANCE_ELEVATION_PARAM);
                                    double Elevation = ElevationParameter.AsDouble();
                                    ElevationSharedParameter.Set(Elevation);
                                }

                                var OffsetSharedParameter = fi.LookupParameter("@G Offset");
                                if (null != OffsetSharedParameter)
                                {
                                    var OffsetParameter = fi.get_Parameter(BuiltInParameter.INSTANCE_FREE_HOST_OFFSET_PARAM);
                                    double Offset = OffsetParameter.AsDouble();
                                    OffsetSharedParameter.Set(Offset);
                                }

                                var WorksetSharedParameter = fi.LookupParameter("@G Workset");
                                if (null != WorksetSharedParameter)
                                {
                                    WorksetSharedParameter.Set(GetWorksetName(fi));
                                }
                                updatedCount++;
                            }
                        }
                    }
                    tr.Commit();
                    if (updatedCount == instanceCount)
                    {
                        TaskDialog.Show("SUCCESS",
                        string.Format("{0} family instances were successfully updated" +
                                      "\nPLEASE SYNCHRONIZE WITH CENTRAL TO RELINQUISH UPDATED ELEMENTS",
                                      updatedCount));
                    }
                    else
                    {
                        TaskDialog.Show("WARNING",
                        string.Format("Only {0} over {1} family instances were successfully updated" +
                                      "\nOther family instances were not currently editable." +
                                      "\nTry again later..." +
                                      "\nPLEASE SYNCHRONIZE WITH CENTRAL TO RELINQUISH UPDATED ELEMENTS",
                                      updatedCount, instanceCount));
                    }
                }
                catch (Exception)
                {
                    tr.RollBack();
                    throw;
                }
            }
        }

        public static string GetWorksetName(Element e)
        {
            WorksetTable WST = e.Document.GetWorksetTable();
            Workset Workset;
            string WorksetName = "";
            if ((WST != null) &&
                (e.WorksetId != WorksetId.InvalidWorksetId) &&
                ((Workset = WST.GetWorkset(e.WorksetId)) != null))
            {
                WorksetName = Workset.Name;
            }
            return WorksetName;
        }
    }
}