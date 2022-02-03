
using System;
using System.Collections.Generic;
using System.Linq;
// Autodesk Revit
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using System.Windows.Forms;

namespace BGO.Revit.Tools
{
    class RoomUtils
    {
        static public string RoomNameSharedParameter = "@G Room Name";
        static public string RoomNumberSharedParameter = "@G Room Number";

        static public string _RoomTypeCodeParameter = "@G Room Type Code";
        static public string _RoomEntityCodeParameter = "@G Entity Code";
        static public string _RoomLevelCode = "@G Level Code";
        static public string _RoomOrderNumber = "@G Order Number";
    

        static public Room GetRoomAtPoint(Document doc, XYZ point, bool SearchRoomInLinkedFiles)
        {
            //XYZ p = point.Add(new XYZ(0, 0, 0.001));
            XYZ p = point;
            Room r = doc.GetRoomAtPoint(p);

            if (r == null && SearchRoomInLinkedFiles)
            {
                var LinkedDocuments = LinkUtils.GetLinkedDocuments(doc);
                foreach (Document d in LinkedDocuments)
                {
                    r = d.GetRoomAtPoint(p);
                    if (r != null) break;
                }
            }
            return r;
        }

        static public void UpdateRoomSharedParameters(Document doc, ICollection<ElementId> ElementIds)
        {
            using (Transaction tr = new Transaction(doc))
            {
                int totalCount = 0;
                int updatedCount = 0;
                int NoRoomCount = 0;
                tr.Start("Update room shared parameters");
                foreach (ElementId Id in ElementIds)
                {
                    FamilyInstance fi = doc.GetElement(Id) as FamilyInstance;
                    if (fi != null)
                    {
                        XYZ loc = XYZ.Zero;

                        LocationPoint lp = fi.Location as LocationPoint;
                        if (lp == null)
                        {
                            LocationCurve lc = fi.Location as LocationCurve;
                            if (lc == null) continue;
#if RELEASE2013
                            loc = lc.Curve.get_EndPoint(0);
#else
                            loc = lc.Curve.GetEndPoint(0);
#endif
                        }
                        else
                        {
                            loc = lp.Point;
                        }

                        //var bbx = fi.get_BoundingBox(null);
                        //if (bbx == null) continue;

                        //XYZ loc = (bbx.Max + bbx.Min )/2;

                        totalCount++;

                        //can't update elements currently edited by other user.
                        string editedBy = fi.get_Parameter(BuiltInParameter.EDITED_BY).AsString();
                        if (editedBy == "" || editedBy == null || editedBy == doc.Application.Username)
                        {
                            Room r = GetRoomAtPoint(doc, loc, true);
                            if (r != null)
                            {
                                Parameter pRoomName = fi.LookupParameter("@G Room Name");
                                if (pRoomName != null) pRoomName.Set(r.get_Parameter(BuiltInParameter.ROOM_NAME).AsString());
                                Parameter pRoomNumber = fi.LookupParameter("@G Room Number");
                                if (pRoomNumber != null) pRoomNumber.Set(r.get_Parameter(BuiltInParameter.ROOM_NUMBER).AsString());
                                updatedCount++;
                            }
                            else NoRoomCount++;
                        }
                    }
                }
                tr.Commit();

                if (totalCount == updatedCount)
                {
                    TaskDialog.Show("SUCCESS",string.Format("{0} family instances succesfully updated", updatedCount)+
                        "\nPLEASE SYNCHRONIZE WITH CENTRAL TO RELINQUISH UPDATED ELEMENTS");
                }
                else
                {
                    TaskDialog.Show("WARNING",string.Format("Only {0} family instance(s) succesfully updated over {1}"+
                        "\n{2} family instance(s) are not located inside a defined room" ,
                        updatedCount, totalCount, NoRoomCount)+ 
                        "\nOther family instances were not currently editable."+
                        "\nTry again later..."+
                        "\nPLEASE SYNCHRONIZE WITH CENTRAL TO RELINQUISH UPDATED ELEMENTS");
                }
            }
        }


        		/// <summary>
		/// Mise à jour des paramètres de pièces en fonction de l'ID programme
		/// </summary>


        //
    }
}
