using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

// Autodesk Revit
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;


namespace BGO.Revit.Tools
{
    class SheetUtils
    {
        private delegate void ProcessSheet(ViewSheet vs);

        private static void ProcessSelectedSheets(UIDocument UIdoc, ProcessSheet SheetCallBack, string ProcessName)
        {
            Document doc = UIdoc.Document;
            
            ICollection<ElementId> ViewSheetIds = null;
            ICollection<ElementId> SelectionIds = UIdoc.Selection.GetElementIds();

            if (SelectionIds.Count > 0)
            {
                var col = new FilteredElementCollector(doc, SelectionIds).OfClass(typeof(ViewSheet));
                ViewSheetIds = col.ToElementIds();
                if (ViewSheetIds.Count > 0)
                {
                    using (Transaction tr = new Transaction(doc))
                    {
                        tr.Start(ProcessName);
                        try
                        {
                            foreach (ElementId ViewSheetId in ViewSheetIds)
                            {
                                ViewSheet vs = doc.GetElement(ViewSheetId) as ViewSheet;
                                SheetCallBack(vs);
                            }
                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.RollBack();
                            throw;
                        }
                    }
                }
            }
        }

        private static void ProcessAllSheets(UIDocument UIdoc, ProcessSheet SheetCallBack, string ProcessName)
        {
            Document doc = UIdoc.Document;

            ICollection<ElementId> ViewSheetIds = null;

            var col = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));
            ViewSheetIds = col.ToElementIds();
            if (ViewSheetIds.Count > 0)
            {
                using (Transaction tr = new Transaction(doc))
                {
                    tr.Start(ProcessName);
                    try
                    {
                        foreach (ElementId ViewSheetId in ViewSheetIds)
                        {
                            ViewSheet vs = doc.GetElement(ViewSheetId) as ViewSheet;
                            SheetCallBack(vs);
                        }
                        tr.Commit();
                    }
                    catch (Exception)
                    {
                        tr.RollBack();
                        throw;
                    }
                }
            }
        }

        public static void SynchronizeSheetNames(UIDocument UIdoc)
        {
            ProcessSelectedSheets(UIdoc, SynchronizeSheetName, "Synchronize sheet names");
        }

        private static void SynchronizeSheetName(ViewSheet vs)
        {
            Parameter p = vs.get_Parameter(BuiltInParameter.SHEET_SCHEDULED);
            if (p != null && p.AsInteger() != 0)
            {
                string Title2, Title3, Title4;

                Title2 = ParameterUtils.GetParameterValueAsString(vs, "@G Title 2");
                Title3 = ParameterUtils.GetParameterValueAsString(vs, "@G Title 3");
                Title4 = ParameterUtils.GetParameterValueAsString(vs, "@G Title 4");

                // Sheet Name = Title3 + Title 4
                // string SheetName = Title2 + (Title3 == "" ? "" : " - " + Title3) + (Title4 == "" ? "" : " - " + Title4);
                string SheetName = Title3 + (Title4 == "" ? "" : " - " + Title4);
                vs.get_Parameter(BuiltInParameter.SHEET_NAME).Set(SheetName == "" ? "Unnamed" : SheetName);
            }
        }


        #region Mise à jour des données des feuilles

        /// <summary>
        /// Mise à jour des informations de feuilles en fonction de la liste de documents
        /// </summary>
        public static void UpdateSheetsFromLOD(UIDocument UIdoc)
        {
            // Lecture de la liste de documents
            var SheetParamInfos = ParameterUtils.ReadParametersTableFromTextFile("Séléctionner la liste de plans");

            // Si au moins un document dans la liste :)
            if (SheetParamInfos.Count != 0)
            {
                Document doc = UIdoc.Document;
                
                string ActiveFileName = Path.GetFileName(doc.PathName);
                FilteredElementCollector col = new FilteredElementCollector(doc).OfClass(typeof(ViewSheet));

                var ExistingSheets = new Dictionary<string, ElementId>();
                foreach (ElementId Id in col.ToElementIds())
                {
                    var vs = doc.GetElement(Id) as ViewSheet;
                    ExistingSheets.Add(vs.SheetNumber, Id);
                }

                //Initialisation nombre de feuilles traitées
                int UpdatedCount = 0;
                int CreatedCount = 0;
                int TotalCount = 0;


                //Initialisation liste des erreurs
                var ErrorList = new List<string>();

                // Initialisation nom de la transaction
                string TransactionName = "Update of sheets information";

                using (Transaction tr = new Transaction(doc))
                {
                    try
                    {
                        tr.Start(TransactionName);

                        var SheetParameterNames = SheetParamInfos.ElementAt(0).Value;
                        var TitleBlockFieldName = "Cartouche";
                        var TitleBlockFieldIndex = SheetParameterNames.IndexOf(TitleBlockFieldName);
                        string RevitFileFieldName = "NomDuFichierRevit";
                        var RevitFileFieldIndex = SheetParameterNames.IndexOf(RevitFileFieldName);
                        if (RevitFileFieldIndex < 0)
                        {
                            ErrorList.Add(string.Format("Field <{0}> does not exist in the document list?!", RevitFileFieldName));
                        }
                        else
                        {

                            var SheetEnumerator = SheetParamInfos.GetEnumerator();

                            while (SheetEnumerator.MoveNext())
                            {
                                var kvp = SheetEnumerator.Current;
                                var SheetNumber = kvp.Key;

                                var RevitFileName = kvp.Value.ElementAt(RevitFileFieldIndex);

                                if (SheetNumber != "ParamNames" && ActiveFileName.Contains(RevitFileName))
                                {
                                    TotalCount++;
                                    if (!ExistingSheets.Keys.Contains(SheetNumber))
                                    {
                                        FamilySymbol TitleBlock = null;

                                        if (TitleBlockFieldIndex >= 0)
                                        {
                                            var TitleBlockName = kvp.Value.ElementAt(TitleBlockFieldIndex);
                                            var TitleBlockSymbols = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).OfCategory(BuiltInCategory.OST_TitleBlocks);
                                            foreach (FamilySymbol ttlb in TitleBlockSymbols)
                                            {
                                                if (ttlb.Name == TitleBlockName)
                                                {
                                                    TitleBlock = ttlb;
                                                    break;
                                                }
                                            }
                                        }

                                        if (TitleBlock == null)
                                        {
                                            ErrorList.Add(string.Format("Missing title block. Unable to create sheet {0}", SheetNumber));
                                            continue;
                                        }
                                        var NewSheet = ViewSheet.Create(doc,TitleBlock.Id);
                                        if (NewSheet == null)
                                        {
                                            ErrorList.Add(string.Format("Error trying to create sheet {0}", SheetNumber));
                                            continue;
                                        }
                                        NewSheet.SheetNumber = SheetNumber;
                                        ExistingSheets.Add(SheetNumber, NewSheet.Id);
                                        CreatedCount++;
                                    }

                                    ElementId ViewSheetId;
                                    if (ExistingSheets.TryGetValue(SheetNumber, out ViewSheetId))
                                    {
                                        var vs = doc.GetElement(ViewSheetId) as ViewSheet;


                                        //Vérification que la feuille n'est pas en cours d'édition par un autre utilisateur
                                        Parameter p = vs.get_Parameter(BuiltInParameter.EDITED_BY);
                                        if (p != null)
                                        {
                                            string EditedBy = p.AsString();
                                            //if (!string.IsNullOrEmpty(EditedBy) && EditedBy != UIdoc.Application.Application.Username)
                                            if (!string.IsNullOrEmpty(EditedBy) && EditedBy != doc.Application.Username)
                                            {
                                                ErrorList.Add(string.Format("Sheet number {0} and Id {1}, is currently bein edited by {2}",
                                                                            vs.SheetNumber, vs.Id.IntegerValue, EditedBy));
                                                continue;
                                            }
                                        }

                                        for (int index = 1; index < SheetParameterNames.Count; index++)
                                        {
                                            var ParameterName = SheetParameterNames.ElementAt(index);
                                            var ParameterValue = kvp.Value.ElementAt(index);
                                            var SheetParam = vs.LookupParameter(ParameterName);
                                            if (SheetParam != null && !SheetParam.IsReadOnly)
                                            {
                                                SheetParam.Set(ParameterValue);
                                            }
                                        }
                                        //Incrémentation nombre de feuilles traitées
                                        UpdatedCount++;
                                    }
                                    else
                                    {
                                        ErrorList.Add(string.Format("Error retreiving sheet Id for sheet number {0}?! ", SheetNumber));
                                    }
                                }

                                //							if (UpdatedCount >3) throw new ApplicationException("Test exception durant traitement");
                            }

                        }

                        //Validation transaction
                        tr.Commit();

                    }
                    catch (Exception ex)
                    {
                        //En cas d'erreur non gérée, affichage du message et annulation de toutes les modifications
                        tr.RollBack();
                        //Ré-initialisation nombre de feuilles traitées
                        UpdatedCount = 0;
                        CreatedCount = 0;
                        TotalCount = SheetParamInfos.Count - 1;
                        ErrorList.Add(ex.ToString());
                    }
                }

                //Message récapitulatif du traitement 
                string msg = string.Format("{0} sheet(s) over {1} have been successfully updated\n" +
                                                   "including {2} newly created sheet(s)\n",
                                                   UpdatedCount, TotalCount, CreatedCount);

                if (ErrorList.Count == 0)
                {

                    TaskDialog.Show(TransactionName, msg);
                    //TaskDialog.Show(TransactionName, string.Format("{0} sheet(s) over {1} have been successfully updated\n" +
                    //                               "including {2} newly created sheet(s)\n",
                    //                               UpdatedCount, TotalCount, CreatedCount));
                }
                else
                {
                    msg = "ONLY " + msg + "ERRORS WERE ENCOUNTERED.\n";
                    TaskDialog.Show(TransactionName, msg);
                    //TaskDialog.Show(TransactionName, string.Format("ONLY {0} sheet(s) over {1} have been successfully updated \n" +
                    //                               "including {2} newly created sheet(s)\n"+
                    //                               "ERRORS WERE ENCOUNTERED.\n",
                    //                               UpdatedCount, TotalCount, CreatedCount));
                    StringUtility.ShowList(ErrorList);
                }
            }
        }

         #endregion

    }
}
