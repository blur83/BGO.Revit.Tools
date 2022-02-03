using System.Collections.Generic;
// Autodesk
using Autodesk.Revit.DB;


namespace BGO.Revit.Tools
{
    class LinkUtils
    {
        public static DocumentSet GetLinkedDocuments(Document doc)
        {
            var LinkedDocuments = new DocumentSet();
            ICollection<ElementId> RevitLinkIds = null;

//            var col = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_RvtLinks).WhereElementIsNotElementType();
            var col = new FilteredElementCollector(doc)
                            .OfCategory(BuiltInCategory.OST_RvtLinks)
                            .OfClass(typeof (Autodesk.Revit.DB.Instance));
            RevitLinkIds = col.ToElementIds();
            foreach (ElementId id in RevitLinkIds)
            {
                var RvtLinkInstance = doc.GetElement(id) as Autodesk.Revit.DB.Instance;
                var RvtLinkTypeId = RvtLinkInstance.GetTypeId();
                var RvtLinkType = doc.GetElement(RvtLinkTypeId) as RevitLinkType;
                foreach (Document d in doc.Application.Documents)
                {
                    if (RvtLinkType.Name.StartsWith(d.Title))
                    {
                        LinkedDocuments.Insert(d);
                        break;
                    }
                }
            }
            return LinkedDocuments;
        }
    }
}
