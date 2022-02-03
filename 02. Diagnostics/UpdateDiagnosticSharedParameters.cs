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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
// Autodesk
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

#endregion

namespace BGO.Revit.Tools
{
    /// <summary>
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class UpdateDiagnosticSharedParameters : IExternalCommand
    {
        // Member variables
        UIApplication _uiApp;
        UIDocument _uiDoc;
        Document _doc;

        public Result Execute(
              ExternalCommandData commandData,
              ref string message,
              ElementSet elements)
        {
            _uiApp = commandData.Application;
            _uiDoc = _uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
         
            WorksetId workingWorksetId = WorksetId.InvalidWorksetId;

            WorksetTable WST = _doc.GetWorksetTable();
            if (null != WST) 
            {
                workingWorksetId = WST.GetActiveWorksetId();
                //Workset ActiveWorkset = WST.GetWorkset(ActiveWorksetId);
                //ActiveWorksetName = ActiveWorkset.Name;
                
                var frm = new formSelectWorkset();
                if (frm.ShowDialog() != System.Windows.Forms.DialogResult.OK) 
                    return Result.Succeeded ;

                // frm.Tag return true when "All worksets" selected
                if ((bool)(frm.Tag)) workingWorksetId = WorksetId.InvalidWorksetId; 
            }
            
            ICollection<ElementId> ElementIds = null;
            var col = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance));
            
            if (workingWorksetId == WorksetId.InvalidWorksetId)
            {
                ElementIds = col.ToElementIds();
            }
            else
            {
                var Filter = new ElementWorksetFilter(workingWorksetId, false);
                ElementIds = col.WherePasses(Filter).ToElementIds();
            }
            
            DiagUtils.UpdateDiagnosticSharedParameters(_doc, ElementIds);
            ElementIds.Clear();

            return Result.Succeeded;
        }
     }
}
