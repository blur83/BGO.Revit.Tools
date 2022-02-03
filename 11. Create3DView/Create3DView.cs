#region Copyright
//
// Copyright (C) 2010-2011 by Autodesk, Inc.
//
// Permission to use, copy, modify, and distribute this software in
// object code form for any purpose and without fee is hereby granted,
// provided that the above copyright notice appears in all copies and
// that both that copyright notice and the limited warranty and
// restricted rights notice below appear in all supporting
// documentation.
//
// AUTODESK PROVIDES THIS PROGRAM "AS IS" AND WITH ALL FAULTS.
// AUTODESK SPECIFICALLY DISCLAIMS ANY IMPLIED WARRANTY OF
// MERCHANTABILITY OR FITNESS FOR A PARTICULAR USE.  AUTODESK, INC.
// DOES NOT WARRANT THAT THE OPERATION OF THE PROGRAM WILL BE
// UNINTERRUPTED OR ERROR FREE.
//
// Use, duplication, or disclosure by the U.S. Government is subject to
// restrictions set forth in FAR 52.227-19 (Commercial Computer
// Software - Restricted Rights) and DFAR 252.227-7013(c)(1)(ii)
// (Rights in Technical Data and Computer Software), as applicable.
//
// Migrated to C# by Saikat Bhattacharya
// 
#endregion // Copyright

#region Namespaces
using System;
using System.Linq;
using System.Collections.Generic;
// Autodesk
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;


#endregion

namespace BGO.Revit.Tools
{
    /// <summary>
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class Create3DView : IExternalCommand
    {
        // Member variables
        UIApplication _uiApp;
        UIDocument _uiDoc;
        Document _doc;
        string _ViewName;
        BoundingBoxXYZ _SectionBox;
        string ActiveViewDiscipline = "";
        string ActiveViewSystem = "";
        string ActiveViewClassification = "";
        
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            // Get access to the top most objects. (we may not use them all in this specific lab.) 
            _uiApp = commandData.Application;
            _uiDoc = _uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;

            try
            {
                ActiveViewDiscipline = _doc.ActiveView.GetParameters("@G Discipline").First().AsString();
                ActiveViewSystem = _doc.ActiveView.GetParameters("@G System").First().AsString();
                ActiveViewClassification = _doc.ActiveView.GetParameters("@G Classification").First().AsString();
            }
            catch (Exception)
            {
                ActiveViewDiscipline = "";
                ActiveViewSystem = "";
                ActiveViewClassification = "";
            }

            var SelectedElementsIds = _uiDoc.Selection.GetElementIds();
            List<Element> SelectedElements = new List<Element>();
            foreach(ElementId eid in SelectedElementsIds)
            {Element e = _doc.GetElement(eid);
            SelectedElements.Add(e);}

            int SelectionCount = SelectedElements.Count();

            switch (SelectionCount)
            {
                case 0 :
                    var frm = new FormCreate3DView(_uiDoc);
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        if (frm.Tag.ToString() == "INVALID ID")
                        {
                            TaskDialog.Show("Create from element id", "Invalid 3D Object Id. View not created");
                        }
                        else
                        {
                            _ViewName = frm.ViewName;
                            _SectionBox = frm.SectionBox;
                            CreateNewView3D();
                        }
                    }
                    break;
                //case 1:
                //    var SelectedElement = SelectedElements.ElementAt(0);
                //    BoundingBoxXYZ bbXYZ = SelectedElement.get_BoundingBox(null);
                //    if (bbXYZ == null)
                //    {
                //        TaskDialog.Show("Create from selected element", "Selected element is not a 3D element.");
                //    }
                //    else
                //    {
                //        double ExtraRange = 10;
                //        double minX = bbXYZ.Min.X - ExtraRange;
                //        double maxX = bbXYZ.Max.X + ExtraRange;
                //        double minY = bbXYZ.Min.Y - ExtraRange;
                //        double maxY = bbXYZ.Max.Y + ExtraRange;
                //        double minZ = bbXYZ.Min.Z - ExtraRange;
                //        double maxZ = bbXYZ.Max.Z + ExtraRange;

                //        //Create the bounding box
                //        _SectionBox = new BoundingBoxXYZ();
                //        _SectionBox.set_Bounds(0, new XYZ(minX, minY, minZ));
                //        _SectionBox.set_Bounds(1, new XYZ(maxX, maxY, maxZ));
                //        _ViewName = "3D - ID " + SelectedElement.Id.ToString();
                //        CreateNewView3D();
                //    }
                //    break;
                default:

                    double ExtraRange = 10;
                    double minX=0, maxX=0, minY=0, maxY=0, minZ=0, maxZ=0;
                    bool No3DElement = true;

                    _ViewName = "3D - ID";
 
                    foreach (Element SelectedElement in SelectedElements)
                    {
                        BoundingBoxXYZ bbXYZ = SelectedElement.get_BoundingBox(null);
                        if (bbXYZ != null)
                        {
                            if (No3DElement)
                            {
                                minX = bbXYZ.Min.X;
                                maxX = bbXYZ.Max.X;
                                minY = bbXYZ.Min.Y;
                                maxY = bbXYZ.Max.Y;
                                minZ = bbXYZ.Min.Z;
                                maxZ = bbXYZ.Max.Z;
                                No3DElement = false;
                            }
                            else
                            {
                                minX = Math.Min(bbXYZ.Min.X, minX);
                                maxX = Math.Max(bbXYZ.Max.X, maxX);
                                minY = Math.Min(bbXYZ.Min.Y,minY);
                                maxY = Math.Max(bbXYZ.Max.Y, maxY);
                                minZ = Math.Min(bbXYZ.Min.Z,minZ);
                                maxZ = Math.Max(bbXYZ.Max.Z,maxZ);
                            }
                            _ViewName = _ViewName + " " + SelectedElement.Id.ToString();
                        }
                    }
                    if (No3DElement)
                    {
                        TaskDialog.Show("Create 3D view",
                            "No 3D element is selected.\nPlease select at least one 3D element");
                        break;
                    }
                    //Create the bounding box
                    _SectionBox = new BoundingBoxXYZ();
                    _SectionBox.set_Bounds(0, new XYZ(minX-ExtraRange, minY-ExtraRange, minZ-ExtraRange));
                    _SectionBox.set_Bounds(1, new XYZ(maxX+ExtraRange, maxY+ExtraRange, maxZ+ExtraRange));
                    CreateNewView3D();

                    break;
            }
            return Result.Succeeded;
        }

        private void CreateNewView3D()
        {
            using (Transaction tr = new Transaction(_doc))
            {
                View3D view3D = null;
                try
                {
                    tr.Start("Create 3D View " + _ViewName);

                    //Create a new default 3D view
                    
                    //RVT2012
                    //view3D = _doc.Create.NewView3D(new XYZ(-1, 1, -1));

                    //RVT2013
                    ViewFamilyType viewFamilyType3D = new FilteredElementCollector(_doc)
                          .OfClass(typeof(ViewFamilyType))
                          .Cast<ViewFamilyType>()
                          .FirstOrDefault<ViewFamilyType>(x => ViewFamily.ThreeDimensional == x.ViewFamily);
                    view3D = View3D.CreateIsometric(_doc, viewFamilyType3D.Id);

                    // try to use the active view as view template for the newly created 3D view (will copy BIMgo24/ADP view shared parameters)
                    try
                    {
                        view3D.ApplyViewTemplateParameters(_doc.ActiveView);
                    }
                    catch (Exception)
                    {
                        // Ignore errors
                    }

                    // Eventually, remove associated view template
                    view3D.ViewTemplateId = ElementId.InvalidElementId;

                    //Set Detail level to <Fine> and Graphic Style to <Shaded>
                    view3D.get_Parameter(BuiltInParameter.VIEW_DETAIL_LEVEL).Set(3); // View Detail : Fine
                    view3D.get_Parameter(BuiltInParameter.MODEL_GRAPHICS_STYLE).Set(4);  // Graphic Style : Shaded

                    //Apply the bouding box to the section box


                    view3D.SetSectionBox(_SectionBox);
                    //Hide scope boxes
                    Category ScopeBoxesCategory = _doc.Settings.Categories.get_Item(BuiltInCategory.OST_VolumeOfInterest);
                    view3D.SetCategoryHidden(ScopeBoxesCategory.Id, false);

                    //Hide imported instances
                    var col = new FilteredElementCollector(_doc).OfClass(typeof(ImportInstance));
                    foreach (Element e in col)
                    {
                       
                        var cat = e.Category;
                        if (cat != null && cat.get_AllowsVisibilityControl(view3D))
                            view3D.SetCategoryHidden(e.Category.Id,false);
                    }

                   

                    //// try to set the same BIMgo24 parameters as the active view
                    //try
                    //{
                    //    view3D.LookupParameter("@G Discipline").Set(ActiveViewDiscipline);
                    //    view3D.LookupParameter("@G System").Set(ActiveViewSystem);
                    //    view3D.LookupParameter("@G Classification").Set(ActiveViewClassification);
                    //}
                    //catch (Exception)
                    //{
                    //    // Ignore non existing parameters (not an BIMgo24 standard project)
                    //}

                    ////Setting Detail level to <Fine> and Graphic Style to <Shaded>
                    //view3D.LookupParameter(BuiltInParameter.VIEW_DETAIL_LEVEL).Set(3); // View Detail : Fine
                    //view3D.LookupParameter(BuiltInParameter.MODEL_GRAPHICS_STYLE).Set(4);  // Graphic Style : Shaded
                    
                    //Name the view (if possible)
                    if (_ViewName != null && _ViewName != "")
                    {
                        try
                        {
                            if (ActiveViewDiscipline != "")
                            {
                                view3D.Name = string.Format("{0}-{1} - {2} - {3}",
                                    ActiveViewDiscipline, ActiveViewSystem, _ViewName, ActiveViewClassification);
                            }
                            else
                            {
                                view3D.Name = _ViewName;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    tr.Commit();

                    //Swith UI to new 3D view after transaction is committed
                    _uiDoc.ActiveView = view3D;
                }
                catch (Exception ex)
                {
                    tr.RollBack();
                    TaskDialog.Show("Error creating new 3D view", ex.ToString());
                }
            }
        }

    //    /// <summary>
    //    /// Show basic information about the given element. 
    //    /// </summary>
    //    public void ShowBasicElementInfo(Element e)
    //    {
    //        // Let's see what kind of element we got. 
    //        string s = "You picked: \n";

    //        s += ElementToString(e);

    //        // Show what we got. 

    //        TaskDialog.Show("Revit UI Lab", s);
    //    }

    //    /// <summary>
    //    /// Pick methods sampler. 
    //    /// Quickly try: PickObject, PickObjects, PickElementByRectangle, PickPoint. 
    //    /// Without specifics about objects we want to pick. 
    //    /// </summary>
    //    public void PickMethodsBasics()
    //    {
    //        // (1) Pick Object (we have done this already. But just for the sake of completeness.) 
    //        PickMethod_PickObject();

    //        // (2) Pick Objects 
    //        PickMethod_PickObjects();

    //        // (3) Pick Element By Rectangle 
    //        PickMethod_PickElementByRectangle();

    //        // (4) Pick Point 
    //        PickMethod_PickPoint();
    //    }

    //    /// <summary>
    //    /// Minimum PickObject 
    //    /// </summary>
    //    public void PickMethod_PickObject()
    //    {
    //        Reference r = _uiDoc.Selection.PickObject(ObjectType.Element, "Select one element");
    //        //Element e = r.Element; // 2011
    //        Element e = _uiDoc.Document.GetElement(r); // 2012

    //        ShowBasicElementInfo(e);
    //    }

    //    /// <summary>
    //    /// Minimum PickObjects 
    //    /// Note: when you run this code, you will see "Finish" and "Cancel" buttons in the dialog bar. 
    //    /// </summary>
    //    public void PickMethod_PickObjects()
    //    {
    //        IList<Reference> refs = _uiDoc.Selection.PickObjects(ObjectType.Element, "Select multiple elemens");

    //        // Put it in a List form. 
    //        IList<Element> elems = new List<Element>();
    //        foreach (Reference r in refs)
    //        {
    //            //elems.Add( r.Element ); // 2011 Warning: 'Autodesk.Revit.DB.Reference.Element' is obsolete: 
    //            // 'Property will be removed. Use Document.GetElement(Reference) instead'
    //            elems.Add(_uiDoc.Document.GetElement(r)); // 2012
    //        }

    //        ShowElementList(elems, "Pick Objects: ");
    //    }

    //    /// <summary>
    //    /// Minimum PickElementByRectangle 
    //    /// </summary>
    //    public void PickMethod_PickElementByRectangle()
    //    {
    //        // Note: PickElementByRectangle returns the list of element. not reference. 
    //        IList<Element> elems = _uiDoc.Selection.PickElementsByRectangle("Select by rectangle");

    //        // Show it. 

    //        ShowElementList(elems, "Pick By Rectangle: ");
    //    }

    //    /// <summary>
    //    /// Minimum PickPoint 
    //    /// </summary>
    //    public void PickMethod_PickPoint()
    //    {
    //        XYZ pt = _uiDoc.Selection.PickPoint("Pick a point");

    //        // Show it. 
    //        string msg = "Pick Point: ";
    //        msg += PointToString(pt);

    //        TaskDialog.Show("PickPoint", msg);
    //    }

    //    /// <summary>
    //    /// Pick face, edge, point on an element 
    //    /// objectType options is applicable to PickObject() and PickObjects() 
    //    /// </summary>
    //    public void PickFaceEdgePoint()
    //    {
    //        // (1) Face 
    //        PickFace();

    //        // (2) Edge 
    //        PickEdge();

    //        // (3) Point 
    //        PickPointOnElement();
    //    }

    //    public void PickFace()
    //    {
    //        Reference r = _uiDoc.Selection.PickObject(ObjectType.Face, "Select a face");
    //        Element e = _uiDoc.Document.GetElement(r);

    //        //Face oFace = r.GeometryObject as Face; // 2011
    //        Face oFace = e.GetGeometryObjectFromReference(r) as Face; // 2012

    //        string msg = "";
    //        if (oFace != null)
    //        {
    //            msg = "You picked the face of element " + e.Id.ToString() + "\r\n";
    //        }
    //        else
    //        {
    //            msg = "no Face picked \n";
    //        }

    //        TaskDialog.Show("PickFace", msg);
    //    }

    //    public void PickEdge()
    //    {
    //        Reference r = _uiDoc.Selection.PickObject(ObjectType.Edge, "Select an edge");
    //        Element e = _uiDoc.Document.GetElement(r);
    //        //Edge oEdge = r.GeometryObject as Edge; // 2011
    //        // BUG Face oEdge = e.GetGeometryObjectFromReference(r) as Face; // 2012
    //        Edge oEdge = e.GetGeometryObjectFromReference(r) as Edge; // 2012

    //        // Show it. 
    //        string msg = "";
    //        if (oEdge != null)
    //        {
    //            msg = "You picked an edge of element " + e.Id.ToString() + "\r\n";
    //        }
    //        else
    //        {
    //            msg = "no Edge picked \n";
    //        }

    //        TaskDialog.Show("PickEdge", msg);
    //    }

    //    public void PickPointOnElement()
    //    {
    //        Reference r = _uiDoc.Selection.PickObject(
    //          ObjectType.PointOnElement,
    //          "Select a point on element");

    //        Element e = _uiDoc.Document.GetElement(r);
    //        XYZ pt = r.GlobalPoint;

    //        string msg = "";
    //        if (pt != null)
    //        {
    //            msg = "You picked the point " + PointToString(pt) + " on an element " + e.Id.ToString() + "\r\n";
    //        }
    //        else
    //        {
    //            msg = "no Point picked \n";
    //        }

    //        TaskDialog.Show("PickPointOnElement", msg);
    //    }

    //    /// <summary>
    //    /// Pick with selection filter 
    //    /// Let's assume we only want to pick up a wall. 
    //    /// </summary>
    //    public void ApplySelectionFilter()
    //    {
    //        // Pick only a wall 
    //        PickWall();

    //        // Pick only a planar face. 
    //        PickPlanarFace();
    //    }

    //    /// <summary>
    //    /// Selection with wall filter. 
    //    /// See the bottom of the page to see the selection filter implementation. 
    //    /// </summary>
    //    public void PickWall()
    //    {
    //        SelectionFilterWall selFilterWall = new SelectionFilterWall();
    //        Reference r = _uiDoc.Selection.PickObject(ObjectType.Element, selFilterWall, "Select a wall");

    //        // Show it
    //        Element e = _uiDoc.Document.GetElement(r);

    //        ShowBasicElementInfo(e);
    //    }

    //    /// <summary>
    //    /// Selection with planar face. 
    //    /// See the bottom of the page to see the selection filter implementation. 
    //    /// </summary>
    //    public void PickPlanarFace()
    //    {
    //        // To call ISelectionFilter.AllowReference, use this. 
    //        // This will limit picked face to be planar. 
    //        Document doc = _uiDoc.Document;
    //        SelectionFilterPlanarFace selFilterPlanarFace = new SelectionFilterPlanarFace(doc);
    //        Reference r = _uiDoc.Selection.PickObject(ObjectType.Face, selFilterPlanarFace, "Select a planar face");
    //        Element e = doc.GetElement(r);
    //        //Face oFace = r.GeometryObject as Face; // 2011
    //        Face oFace = e.GetGeometryObjectFromReference(r) as Face; // 2012

    //        string msg = (null == oFace)
    //          ? "No face picked."
    //          : "You picked a face on element " + e.Id.ToString();

    //        TaskDialog.Show("PickPlanarFace", msg);
    //    }

    //    /// <summary>
    //    /// Canceling selection 
    //    /// When the user presses [Esc] key during the selection, OperationCanceledException will be thrown. 
    //    /// </summary>
    //    public void CancelSelection()
    //    {
    //        try
    //        {
    //            Reference r = _uiDoc.Selection.PickObject(ObjectType.Element, "Select an element, or press [Esc] to cancel");
    //            Element e = _uiDoc.Document.GetElement(r);

    //            ShowBasicElementInfo(e);
    //        }
    //        catch (Autodesk.Revit.Exceptions.OperationCanceledException)
    //        {
    //            TaskDialog.Show("CancelSelection", "You canceled the selection.");
    //        }
    //        catch (Exception ex)
    //        {
    //            TaskDialog.Show("CancelSelection", "Other exception caught in CancelSelection(): " + ex.Message);
    //        }
    //    }

    //    #region "Helper Function"
    //    //==================================================================== 
    //    // Helper Functions 
    //    //==================================================================== 

    //    /// <summary>
    //    /// Helper function to display info from a list of elements passed onto. 
    //    /// (Same as Revit Intro Lab3.) 
    //    /// </summary>
    //    public void ShowElementList(IEnumerable elems, string header)
    //    {
    //        string s = "\n\n - Class - Category - Name (or Family: Type Name) - Id - " + "\r\n";

    //        int count = 0;
    //        foreach (Element e in elems)
    //        {
    //            count++;
    //            s += ElementToString(e);
    //        }

    //        s = header + "(" + count + ")" + s;

    //        TaskDialog.Show("Revit UI Lab", s);
    //    }

    //    /// <summary>
    //    /// Helper function: summarize an element information as a line of text, 
    //    /// which is composed of: class, category, name and id. 
    //    /// Name will be "Family: Type" if a given element is ElementType. 
    //    /// Intended for quick viewing of list of element, for example. 
    //    /// (Same as Revit Intro Lab3.) 
    //    /// </summary>
    //    public string ElementToString(Element e)
    //    {
    //        if (e == null)
    //        {
    //            return "none";
    //        }

    //        string name = "";

    //        if (e is ElementType)
    //        {
    //            Parameter param = e.LookupParameter(BuiltInParameter.SYMBOL_FAMILY_AND_TYPE_NAMES_PARAM);
    //            if (param != null)
    //            {
    //                name = param.AsString();
    //            }
    //        }
    //        else
    //        {
    //            name = e.Name;
    //        }
    //        string ctyName = "none";
    //        if (null != e.Category)
    //        {
    //            ctyName = e.Category.Name;
    //        }

    //        return "Type:" + e.GetType().Name + "; Category:" + ctyName + "; Name:" + name + "; Id:" + e.Id.IntegerValue.ToString() + "\r\n";
    //    }

    //    /// <summary>
    //    /// Helper Function: returns XYZ in a string form. 
    //    /// (Same as Revit Intro Lab2) 
    //    /// </summary>
    //    public static string PointToString(XYZ pt)
    //    {
    //        if (pt == null)
    //        {
    //            return "";
    //        }

    //        return "(" + pt.X.ToString("F2") + ", " + pt.Y.ToString("F2") + ", " + pt.Z.ToString("F2") + ")";
    //    }
    //    #endregion
    //}

    ///// <summary>
    ///// Selection filter that limit the type of object being picked as wall. 
    ///// </summary>
    //class SelectionFilterWall : ISelectionFilter
    //{
    //    public bool AllowElement(Element e)
    //    {
    //        return e is Wall;
    //    }

    //    public bool AllowReference(Reference reference, XYZ position)
    //    {
    //        return true;
    //    }
    //}

    ///// <summary>
    ///// Selection filter that limit the reference type to be planar face 
    ///// </summary>
    //class SelectionFilterPlanarFace : ISelectionFilter
    //{
    //    Document _doc;

    //    public SelectionFilterPlanarFace(Document doc)
    //    {
    //        _doc = doc;
    //    }

    //    public bool AllowElement(Element e)
    //    {
    //        return true;
    //    }

    //    public bool AllowReference(Reference r, XYZ position)
    //    {
    //        // Example: if you want to allow only planar faces 
    //        // and do some more checking, add this:

    //        // Optimal geometry object access in ISelectionFilter.AllowReference:
    //        //
    //        // In 2012 we get the warning 'Property GeometryObject As Autodesk.Revit.DB.GeometryObject is obsolete: Property will be removed. Use Element.GetGeometryObjectFromReference(Reference) instead'.
    //        // C:\a\doc\revit\blog\draft\geometry_object_access_in_ISelectionFilter_AllowReference.htm

    //        //if( r.GeometryObject is PlanarFace ) // 2011

    //        ElementId id = r.ElementId; // 2012: added _doc and constructor to initialise it as well
    //        Element e = _doc.get_Element(id); // 2012

    //        if (e.GetGeometryObjectFromReference(r) is PlanarFace) // 2012 
    //        {
    //            // Do additional checking here if needed

    //            return true;
    //        }
    //        return false;
    //    }
    //}

    ///// <summary>
    ///// Create House with UI added 
    ///// 
    ///// Ask the user to pick two corner points of walls
    ///// then ask to choose a wall to add a front door. 
    ///// </summary>
    //[Transaction(TransactionMode.Automatic)]
    //public class UICreateHouse : IExternalCommand
    //{
    //    UIApplication _uiApp;
    //    UIDocument _uiDoc;
    //    Document _doc;

    //    public Result Execute(
    //      ExternalCommandData commandData,
    //      ref string message,
    //      ElementSet elements)
    //    {
    //        // Get the access to the top most objects. (we may not use them all in this specific lab.) 
    //        _uiApp = commandData.Application;
    //        _uiDoc = _uiApp.ActiveUIDocument;
    //        _doc = _uiDoc.Document;

    //        CreateHouseInteractive(_uiDoc);

    //        return Result.Succeeded;
    //    }

    //    /// <summary>
    //    /// Create a simple house with user interactions. 
    //    /// The user is asked to pick two corners of rectangluar footprint of a house, 
    //    /// then which wall to place a front door. 
    //    /// </summary>
    //    public static void CreateHouseInteractive(UIDocument uiDoc)
    //    {
    //        // (1) Walls 
    //        // Pick two corners to place a house with an orthogonal rectangular footprint 
    //        XYZ pt1 = uiDoc.Selection.PickPoint("Pick the first corner of walls");
    //        XYZ pt2 = uiDoc.Selection.PickPoint("Pick the second corner");

    //        // Simply create four walls with orthogonal rectangular profile from the two points picked. 
    //        List<Wall> walls = IntroCs.ModelCreationExport.CreateWalls(uiDoc.Document, pt1, pt2);

    //        // (2) Door 
    //        // Pick a wall to add a front door to
    //        SelectionFilterWall selFilterWall = new SelectionFilterWall();
    //        Reference r = uiDoc.Selection.PickObject(ObjectType.Element, selFilterWall, "Select a wall to place a front door");
    //        Wall wallFront = uiDoc.Document.GetElement(r) as Wall;

    //        // Add a door to the selected wall 
    //        IntroCs.ModelCreationExport.AddDoor(uiDoc.Document, wallFront);

    //        // (3) Windows 
    //        // Add windows to the rest of the walls. 
    //        for (int i = 0; i <= 3; i++)
    //        {
    //            if (!(walls[i].Id.IntegerValue == wallFront.Id.IntegerValue))
    //            {
    //                IntroCs.ModelCreationExport.AddWindow(uiDoc.Document, walls[i]);
    //            }
    //        }

    //        // (4) Roofs 
    //        // Add a roof over the walls' rectangular profile. 

    //        IntroCs.ModelCreationExport.AddRoof(uiDoc.Document, walls);
    //    }

    }

//    public class CreateSheetsFromScopeBoxes : IExternalCommand
//    {
//        // Member variables
//        UIApplication m_uiApp;
//        UIDocument m_uiDoc;
//        Document m_Doc;
//        ViewPlan m_SourceView;
//        string m_CommandName;

//        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//        {
//            //            throw new NotImplementedException();

//            // Get access to the top most objects. (we may not use them all in this specific lab.) 
//            m_uiApp = commandData.Application;
//            m_uiDoc = m_uiApp.ActiveUIDocument;
//            m_Doc = m_uiDoc.Document;
//            m_CommandName = "Create Sheets From Scope Boxes";
//            View v = m_uiDoc.ActiveView;
//            if (v.ViewType != ViewType.FloorPlan)
//            {
//                TaskDialog.Show(m_CommandName, "The active view is not a floor plan view");
//                return Result.Failed;
//            }
//            m_SourceView = v as ViewPlan;

//            string ViewDisciplineCode = GetViewDisciplineCode(m_SourceView);
//            string ViewSystemCode = GetViewSystemCode(m_SourceView);
//            string SystemLetter = GetSystemLetter(ViewSystemCode);
//            string ViewLevelName = m_SourceView.GenLevel.Name;
//            string SheetLevelName = GetSheetLevelName(ViewLevelName);

//            //string ScaleDigit = GetScaleDigit(m_SourceView.Scale);
//            //string SheetName1 = GetSheetName1(ViewSystemCode);
//            //string SheetName2 = "LEVEL " + m_SourceView.GenLevel.Name;
//            //string SheetName3;
//            //string SheetName4;
//            //string ZoneDigits;

//            ICollection<ElementId> ScopeBoxIds = null;
//            var col = new FilteredElementCollector(m_Doc).OfCategory(BuiltInCategory.OST_VolumeOfInterest);
//            ScopeBoxIds = col.ToElementIds();
//            using (Transaction tr = new Transaction(m_Doc))
//            {
//                tr.Start("Creating Sheets");
//                try
//                {
//                    foreach (ElementId ScopeBoxId in ScopeBoxIds)
//                    {
//                        Element ScopeBox = m_Doc.get_Element(ScopeBoxId);
//                        string scn = ScopeBox.Name;
//                        int i = scn.IndexOf("Zone");

//                        if (i >= 0)
//                        {
//                            string ZoneName = scn.Substring(i,7);
//                            string TargetViewName = string.Format("{0}-{1} - {2} - {3} - Dubai Coordination",ViewDisciplineCode,ViewSystemCode,ViewLevelName,ZoneName);
//                            ViewPlan vp = ViewPlanDuplicate(m_SourceView, TargetViewName, true);
//                            if (null != vp)
//                            {
//                                vp.ApplyTemplate(m_SourceView);
//                                Parameter p = vp.LookupParameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP);
//                                p.Set(ScopeBoxId);
//                                //ViewSheet vs = m_Doc.Create.NewViewSheet(TitleBlockSymbol);
//                                //vs.ViewName = SheetName1;
//                                //vs.SheetNumber = "EY" + SystemDigits + "-" + LevelDigit + ScaleDigit + GetZoneDigits(ScopeBox);
//                                //vs.AddView(vp, TitleBlockCenter);
//                            }
//                        }
//                    }
//                }
//                catch (Exception)
//                {
//                    return Result.Failed;
//                    //throw;
//                }
//            }
//            return Result.Succeeded;
//        }

//        private string GetViewDisciplineCode(View aView)
//        {
//            Parameter p = aView.LookupParameter("@G Discipline");
//            if (null == p) return "?";
//            return p.AsValueString();
//        }

//        private string GetViewSystemCode(View aView)
//        {
//            Parameter p = aView.LookupParameter("@G System");
//            if (null == p) return "????";
//            return p.AsValueString();
//        }

//        private string GetSystemLetter(string SystemName)
//        {
//            switch (SystemName)
//            {
//                case "SCS": return "t";
//                case "CCTV": return "c";
//                default : return "?";
//            }
//        }

//        private string GetSheetLevelName(string LevelName)
//        {
//            switch (LevelName)
//            {
//                case "AR-GF-FFL": return "APRON PARTIAL PLAN";
//                case "AR-01-FFL": return "DEPARTURE PARTIAL PLAN";
//                case "AR-02-FFL": return "ARRIVAL PARTIAL PLAN";
//                case "AR-03-FFL": return "THIRD FLOOR PARTIAL PLAN";
//                default: return LevelName;
//            }
//        }

//        private string GetScaleDigit(int scale)
//        {
//            switch (scale)
//            {
//                case 500: return "5";
//                case 100: return "1";
//                default: return "0";
//            }
//        }

//        private string GetSheetName1(string SystemCode)
//        {
//            switch (SystemCode)
//            {
//                case "SCN": return "ELV CABLING SYSTEM";
//                default: return "???";
//            }
//        }

//        private ViewPlan ViewPlanDuplicate(ViewPlan SourceView, string TargetViewName, bool DuplicateDetailing)
//        {
//            ViewPlan TargetView;
//            using (Transaction tr = new Transaction(SourceView.Document))
//            {
//                tr.Start("Duplicate " + SourceView.ViewName);
//                try
//                {
//                    TargetView = m_Doc.Create.NewViewPlan(TargetViewName, SourceView.Level, ViewPlanType.FloorPlan);
//                    if (null != TargetView)
//                    {
//                        if (DuplicateDetailing)
//                        {
//                            //var AnnotationCategoryIds = new List<ElementId>();
//                            //AnnotationCategoryIds.Add(new ElementId(BuiltInCategory.OST_Tags));
//                            //AnnotationCategoryIds.Add(new ElementId(BuiltInCategory.OST_Dimensions));
//                            //var filter = new ElementMulticategoryFilter(AnnotationCategoryIds);
//                            //var detailIds = col.WherePasses(filter).ToElementIds();
//                            var col = new FilteredElementCollector(SourceView.Document).OfClass(typeof(AnnotationSymbol)).WhereElementIsNotElementType();
//                            var AnnotationSymbolIds = col.ToElementIds();
//                            foreach (ElementId id in AnnotationSymbolIds)
//                            {
//                                var SourceSymbol = m_Doc.get_Element(id) as AnnotationSymbol;
//                                XYZ SourceSymbolOrigin = (SourceSymbol.Location as LocationPoint).Point;
//                                var SourceSymbolAnnotationType = SourceSymbol.Symbol as AnnotationSymbolType;
//                                m_Doc.Create.NewAnnotationSymbol(SourceSymbolOrigin, SourceSymbolAnnotationType, TargetView);
//                            }
//                        }
//                        TargetView.ApplyTemplate(SourceView);
//                    }
//                    tr.Commit();
//                }
//                catch (Exception)
//                {
//                    tr.RollBack();
//                    TargetView = null;
//                }
//            }
//            return TargetView;
//        }
//    }

//    public class CreateShaftSpaces : IExternalCommand
//    {
//        // Member variables
//        UIApplication _uiApp;
//        UIDocument _uiDoc;

//        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
//        {
////            throw new NotImplementedException();

//            // Get access to the top most objects. (we may not use them all in this specific lab.) 
//            _uiApp = commandData.Application;
//            _uiDoc = _uiApp.ActiveUIDocument;

//            ICollection<ElementId> ShaftOpenings = null;
//            ICollection<ElementId> SelectionIds = _uiDoc.Selection.GetElementIds();
//            Document doc = _uiDoc.Document;
//            if (SelectionIds.Count > 0)
//            {
//                // Get only selected shaft openings
//                var col = new FilteredElementCollector(doc, SelectionIds).OfCategory(BuiltInCategory.OST_ShaftOpening);
//                ShaftOpenings = col.ToElementIds();
//            }
//            else
//            {
//                var col = new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_ShaftOpening);
//                ShaftOpenings = col.ToElementIds();
//            }
//            //doc.Create.NewSpaceBoundaryLines(
//            //var frm = new FormCreate3DView(_uiDoc, ShaftOpenings);
//            //frm.ShowDialog();
//            return Result.Succeeded;
//        }
//    }

    //try
    // {
    //     // Pick an element which boundingbox to use as section box
    //     var filter = new Object3DSelectionFilter();
    //     Reference r = _uiDoc.Selection.PickObject(ObjectType.Element, 
    //         filter,"Select a 3D object");
    //     if (null != r)
    //     {
    //         double ExtraRange = 10;
    //         Element SelectedElement = _doc.GetElement(r);
    //         BoundingBoxXYZ bbXYZ = SelectedElement.get_BoundingBox(null);
    //         double minX = bbXYZ.Min.X - ExtraRange;
    //         double maxX = bbXYZ.Max.X + ExtraRange;
    //         double minY = bbXYZ.Min.Y - ExtraRange;
    //         double maxY = bbXYZ.Max.Y + ExtraRange;
    //         double minZ = bbXYZ.Min.Z - ExtraRange;
    //         double maxZ = bbXYZ.Max.Z + ExtraRange;

    //         //Create the bounding box
    //         _SectionBox = new BoundingBoxXYZ();
    //         _SectionBox.set_Bounds(0, new XYZ(minX, minY, minZ));
    //         _SectionBox.set_Bounds(1, new XYZ(maxX, maxY, maxZ));
    //         _ViewName = "3D - ID " + SelectedElement.Id.ToString();
    //         CreateNewView3D();
    //     }
    // }
    //private class Object3DSelectionFilter:ISelectionFilter      
    //{
    //    public bool AllowElement(Element elem)
    //    {
    //        return(elem.get_BoundingBox(null) != null);
    //        //throw new NotImplementedException();
    //    }

    //    public bool AllowReference(Reference reference, XYZ position)
    //    {
    //        return false;
    //        //throw new NotImplementedException();
    //    }
    //}


}
