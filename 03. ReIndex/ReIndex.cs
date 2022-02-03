using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// Autodesk Revit
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace BGO.Revit.Tools
{
    [Transaction(TransactionMode.Manual)]
    public class ReIndex : IExternalCommand
    {
        private ExternalCommandData _CommandData;
        private ElementSet _NotIndexedElements;

        public string LastIndexParameterName
        { get; set; }

        public int LastIndex
        { get; set; }

        public int LastOrder
        { get; set; }

        public string LastFormat
        { get; set; }
        
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            _CommandData = commandData;
            _NotIndexedElements = elements;
            LastIndexParameterName = MainApp.LastIndexParameterName;
            LastIndex = MainApp.LastIndex;
            LastFormat = MainApp.LastFormat;
            LastOrder = MainApp.LastOrder;

            var SelectedElements = _CommandData.Application.ActiveUIDocument.Selection.GetElementIds();
            int SelectionCount = SelectedElements.Count();

            switch (SelectionCount)
            {
                case 0 :
                    {
                        break;
                    }
                default:
                    {
                        var frm = new FormReIndex(this);
                        frm.ShowDialog();
                        if (_NotIndexedElements.Size > 0)
                        {
                            message = "Unable to index selected elements";
                            return Result.Failed;
                        }
                        break;
                    }
            }
            return Result.Succeeded;
        }

        public bool Renumber(string indexParameterName, int startIndex, string displayFormat, int renumOrder)
        {
            Document doc = _CommandData.Application.ActiveUIDocument.Document;

            var SelectedIds = _CommandData.Application.ActiveUIDocument.Selection.GetElementIds();
            var SortedList = new SortedList<LocationPoint, Parameter>(new LocationComparer(renumOrder));
            
            foreach (ElementId Id in SelectedIds)
            {
                var e = doc.GetElement(Id);
                LocationPoint lp = e.Location as LocationPoint;
                if (lp == null)
                {
                    _NotIndexedElements.Insert(e);
                    continue;
                }

                Parameter p = e.LookupParameter(indexParameterName);
                if (p == null)
                {
                    _NotIndexedElements.Insert(e);
                    continue;
                }
                    
                SortedList.Add(lp, p);
            }
            if (_NotIndexedElements.Size > 0) return false;

            if (SortedList.Count > 0)
            {
                using (Transaction tr = new Transaction(doc))
                {
                    tr.Start("Re-Index elements");
                    int Index = startIndex;
                    foreach (Parameter p in SortedList.Values)
                    {
                        p.Set(Index.ToString(displayFormat));
                        Index++;
                    }
                    MainApp.LastIndexParameterName = indexParameterName;
                    MainApp.LastIndex = Index - 1;
                    MainApp.LastOrder = renumOrder;
                    MainApp.LastFormat = displayFormat;
                    tr.Commit();
                    TaskDialog.Show("Elements re-indexation", string.Format("{0} element(s) successfully reindexed", SortedList.Count));
                }
            }
            else
            {
                TaskDialog.Show("Elements re-indexation", "No element selected");
            }
            return true;
        }

 
        public class LocationComparer : IComparer<LocationPoint>    
        {
            private int _compareOrder;

            private LocationComparer()
            {
            }

            public LocationComparer(int compareOrder)
            {
                _compareOrder = compareOrder;
            }

            public int Compare(LocationPoint lp1, LocationPoint lp2)
            {
                double precision = 0.00001;
                switch (_compareOrder)
                {
                    case 0: // tri horizontal (Y décroissant puis X croissant)
                        {
                            if (lp1.Point.Y > lp2.Point.Y) return -1;
                            if (Math.Abs(lp1.Point.Y - lp2.Point.Y) < precision)
                            {
                                if (lp1.Point.X < lp2.Point.X) return -1; else return 1;
                            }
                            else return 1;
                        }
                    case 1: // tri horizontal (Y décroissant puis X croissant)
                        {
                            if (lp1.Point.X < lp2.Point.X) return -1;
                            if (Math.Abs(lp1.Point.X - lp2.Point.X) < precision)
                            {
                                if (lp1.Point.Y > lp2.Point.Y) return -1; else return 1;
                            }
                            else return 1;
                        }
                    default:
                        throw new ApplicationException("Not supported compare order");
                }
            }
        }
    }
}
