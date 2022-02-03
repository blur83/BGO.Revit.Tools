using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace BGO.Revit.Tools
{
    /// <summary>
    /// Utility for manipulating REVIT elements.
    /// </summary>
    class ElementUtils
    {
        public delegate string ElementCallBack(Document doc, ElementId Id, object Params);
        
        // Itère une fonction sur une liste d'éléments avec transaction (=> avec modification du document)
        public static void ProcessElements(Document doc, string TransactionTitle, List<ElementId> Ids, ElementCallBack CallBack, object CallBackParams)
        {
            if (Ids != null && Ids.Count > 0)
            {
                //Document doc = ActiveUIDocument.Document;
                var StatusList = new List<string>();
                using (Transaction tr = new Transaction(doc))
                {
                    tr.Start(TransactionTitle);
                    try
                    {
                        foreach (ElementId Id in Ids)
                        {
                            StatusList.Add(CallBack(doc, Id, CallBackParams));
                        }
                        tr.Commit();
                        StatusList.Add("Transaction <" + TransactionTitle + "> successfully committed");
                    }
                    catch (Exception ex)
                    {
                        StatusList.Add(ex.ToString());
                        tr.RollBack();
                        StatusList.Add("Transaction <" + TransactionTitle + "> rolled back");
                    }
                }
                StringUtility.ShowList(StatusList);
            }
        }

        // Itère une fonction sur une liste d'éléments sans transaction (=> sans aucune modification du document)
        public static void ProcessElements(Document doc, List<ElementId> Ids, ElementCallBack CallBack, object CallBackParams)
        {
            if (Ids != null && Ids.Count > 0)
            {
                //Document doc = ActiveUIDocument.Document;
                var StatusList = new List<string>();
                try
                {
                    foreach (ElementId Id in Ids)
                    {
                        StatusList.Add(CallBack(doc, Id, CallBackParams));
                    }
                }
                catch (Exception ex)
                {
                    StatusList.Add(ex.ToString());
                }
                StringUtility.ShowList(StatusList);
            }
        }
    }
}
