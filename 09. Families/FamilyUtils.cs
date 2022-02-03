using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.UI;

namespace BGO.Revit.Tools
{
    [Transaction(TransactionMode.Manual)]
    public class PurgeFamilyFiles : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var App = commandData.Application.Application;
            FamilyUtils.PurgeFamilyFiles(App);
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class ListAllParametersInFamilyFiles : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var App = commandData.Application.Application;
            FamilyUtils.ListAllParametersInFamilyFiles(App);
            return Result.Succeeded;
        }
    }

    //[Transaction(TransactionMode.Manual)]
    //public class SaveLoadedFamiliesToFolder : IExternalCommand
    //{
    //    Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    //    {
    //        var ActiveUIDocument = commandData.Application.ActiveUIDocument;
    //        FamilyUtils.SaveLoadedFamiliesToFolder(ActiveUIDocument.Document);
    //        return Result.Succeeded;
    //    }
    //}

    static class FamilyUtils
    {
        private delegate void ProcessFamilyDocument(Document doc, object Params);

        private static void ProcessFamilyFiles(Autodesk.Revit.ApplicationServices.Application App, 
            ProcessFamilyDocument CallBack, object CallBackParams, bool ReadOnly)
		{
			var fd = new FolderBrowserDialog();
			fd.Description = "Select Root Folder for families to process";
			if (fd.ShowDialog() == DialogResult.OK)
			{
				var RootFolderPath = fd.SelectedPath;
				if (Directory.Exists(RootFolderPath))
				{
				    var StatusList = new List<string> ();
					foreach(string Filepath in Directory.EnumerateFiles(RootFolderPath,"*.rfa",SearchOption.AllDirectories))
					{
						if (!Filepath.Contains(".00"))
					    {
							var doc =  App.OpenDocumentFile(Filepath);
							if (doc.IsFamilyDocument)
							{
								if (ReadOnly)
								{
									try {
										CallBack(doc, CallBackParams);
										StatusList.Add("SUCCESS\t"+ Filepath);
									} catch (Exception ex) {
										StatusList.Add("ERROR\t"+ Filepath + "\t"+ex.ToString());
									}
								}
								else
								{
									bool modified = false;
									using (Transaction tr = new Transaction(doc))
									{
										tr.Start("Processing family document");
										try {
											CallBack(doc, CallBackParams);
											tr.Commit();
											StatusList.Add("SUCCESS\t"+ Filepath);
											modified = true;
										} catch (Exception ex) {
											tr.RollBack();
											StatusList.Add("ERROR\t"+ Filepath + "\t"+ex.ToString());
											modified = false;
										}
									}
									if (modified) 
									{
									
										var sao = new SaveAsOptions();
										sao.OverwriteExistingFile=true;
#if RELEASE2013
										sao.Rename=true;
#endif
										try {
											doc.SaveAs(Filepath,sao);
										} catch (Exception ex) {
											StatusList.Add("ERROR SAVING\t"+ Filepath + "\t"+ex.ToString());
										}
									}
								}
							}
							doc.Close(false);
					    }
					}
		          	StringUtility.ShowList(StatusList);
				}
			}
		}

        public static void PurgeFamilyFiles(Autodesk.Revit.ApplicationServices.Application App)
		{
			ProcessFamilyFiles(App, PurgeFamilyDocument, null, false);
		}

        private static void PurgeFamilyDocument(Document doc, object Params)
        {
            if (doc.IsFamilyDocument)
            {
                var AllMaterialIds = new FilteredElementCollector(doc).OfClass(typeof(Material)).ToElementIds();
                doc.Delete(AllMaterialIds);
            }
        }

        public static void ListAllParametersInFamilyFiles(Autodesk.Revit.ApplicationServices.Application App)
		{
			var ParameterList = new List<string>();
			ParameterList.Add("FileName\tFilePath\tName\tId\tIsInstance\tIsReadOnly\tIsBuiltIn\tIsShared\tGUID");
			ProcessFamilyFiles(App, ListFamilyParameters, ParameterList, true);
			StringUtility.ShowList(ParameterList);
		}
        
        public static void ListFamilyParameters(Document doc, object ParameterList)
        {
            if (doc.IsFamilyDocument)
            {
                var pl = ParameterList as List<string>;
                string FilePath = Path.GetDirectoryName(doc.PathName);
                string FileName = Path.GetFileName(doc.PathName);
                foreach (FamilyParameter fp in doc.FamilyManager.Parameters)
                {
                    bool IsShared = false;
                    bool IsBuiltIn = fp.Id.IntegerValue < 0;
                    string guid = string.Empty;
                    try
                    {
                        guid = fp.GUID.ToString();
                        IsShared = true;
                    }
                    catch (Exception)
                    {
                    }
                    pl.Add(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}",
                                         FileName, FilePath, fp.Definition.Name, fp.Id.IntegerValue, fp.IsInstance, fp.IsReadOnly, IsBuiltIn, IsShared, guid));
                }
            }
        }

        public static List<FamilySymbol> GetLoadedFamilySymbols(
                                                Document aDocument,
                                                string aFamilyName = "",
                                                BuiltInCategory aBuiltInCategory = BuiltInCategory.INVALID)
        {
            List<FamilySymbol> ReturnValue;

            if (aDocument != null)
            {
                var aCollector = new FilteredElementCollector(aDocument);
                List<FamilySymbol> aList = null;
                if (aBuiltInCategory == BuiltInCategory.INVALID)
                {
                    aList = aCollector
                            .OfClass(typeof(FamilySymbol))
                            .WhereElementIsElementType()
                            .Cast<FamilySymbol>()
                            .ToList();
                }
                else
                {
                    aList = aCollector
                            .OfCategory(aBuiltInCategory)
                            .OfClass(typeof(FamilySymbol))
                            .WhereElementIsElementType()
                            .Cast<FamilySymbol>()
                            .ToList();
                }
                if (string.IsNullOrEmpty(aFamilyName))
                {
                    ReturnValue = aList;
                }
                else
                {
                    ReturnValue = (from aElement in aList 
                                           where aElement.Family.Name == aFamilyName
                                           select aElement)
                                           .ToList();
                }
            }
            else
            {
                //Return an empty list
                ReturnValue = new System.Collections.Generic.List<FamilySymbol>();
            }
            return ReturnValue;
        }

        public static List<Family> GetLoadedFamilies(
                                            Document aDocument,
                                            BuiltInCategory aBuiltInCategory = BuiltInCategory.INVALID)
        {
            List<Family> ReturnValue;

            if (aDocument != null)
            {
                var aCollector = new FilteredElementCollector(aDocument)
                                    .OfClass(typeof(Family))
                                    .WhereElementIsElementType();
                                    
                List<Family> aList = null;
                if (aBuiltInCategory == BuiltInCategory.INVALID)
                {
                    aList = (from Family f in aCollector
                             where f.IsEditable && !f.IsInPlace
                             select f)
                            .ToList();
                }
                else
                {
                    aList = (from Family f in aCollector.OfCategory(aBuiltInCategory)
                             where f.IsEditable && !f.IsInPlace
                             select f)
                            .ToList();
                }
                ReturnValue = aList;
            }
            else
            {
                //Return an empty list
                ReturnValue = new System.Collections.Generic.List<Family>();
            }
            return ReturnValue;
        }

        #region To Finalize
        //public static void SaveLoadedFamiliesToFolder(Document doc)
        //{
        //    var ofd = new FolderBrowserDialog();
        //    ofd.ShowNewFolderButton = true;
        //    ofd.Description = "Choose a folder to save loaded families to";
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        //var doc = ActiveUIDocument.Document;
        //        var LoadedFamilyIds =
        //            (from Element e in new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.Family))
        //             let f = e as Autodesk.Revit.DB.Family
        //             where f.IsEditable && !f.IsInPlace
        //             select f.Id).ToList();
        //        ElementUtils.ProcessElements(doc, LoadedFamilyIds, SaveFamilyToFolder, ofd.SelectedPath);
        //    }
        //}

        //public static string SaveFamilyToFolder(Document doc, ElementId FamilyId, object FolderPath)
        //{
        //    //var doc = ActiveUIDocument.Document;
        //    var f = doc.GetElement(FamilyId) as Family;

        //    if (f == null) return string.Format("ERROR : Id <{0}> is not a loaded family Id!!", FamilyId.IntegerValue);

        //    if (f.IsInPlace) return string.Format("ERROR : Family <{0}> is an in place family and then can't be saved to file!!", f.Name);

        //    if (!f.IsEditable) return string.Format("ERROR : Family <{0}> is not an editable family and then can't be saved to file!!", f.Name);

        //    try
        //    {
        //        var fdoc = doc.EditFamily(f);
        //        var sao = new SaveAsOptions();
        //        sao.OverwriteExistingFile = true;
        //        sao.Rename = true;
        //        fdoc.SaveAs(Path.Combine(FolderPath as string, f.Name + ".rfa"), sao);
        //        fdoc.Close(false);
        //        return (string.Format("SUCCESS : Family <{0}> successfully saved to folder {1}", f.Name, FolderPath as string));
        //    }
        //    catch (Exception ex)
        //    {
        //        return string.Format("ERROR : Family <{0}> NOT SAVED to selected folder {1}\n{2}", f.Name, FolderPath as string, ex.ToString());
        //    }
        //}

        //public static void ReloadFamiliesFromFolder(Document doc)
        //{
        //    var ofd = new FolderBrowserDialog();
        //    ofd.ShowNewFolderButton = true;
        //    ofd.Description = "Choose a folder to reload families from";
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        //var doc = ActiveUIDocument.Document;
        //        var LoadedFamilyIds =
        //            (from Element e in new FilteredElementCollector(doc).OfClass(typeof(Autodesk.Revit.DB.Family))
        //             let f = e as Autodesk.Revit.DB.Family
        //             where f.IsEditable && !f.IsInPlace
        //             select f.Id).ToList();
        //        ElementUtils.ProcessElements(doc, "Reload loaded Families", LoadedFamilyIds, ReloadFamilyFromFolder, ofd.SelectedPath);
        //    }
        //}

        //public static string ReloadFamilyFromFolder(Document doc, ElementId FamilyId, object FolderPath)
        //{
        //    string status = string.Empty;
        //    try
        //    {
        //        //var doc = ActiveUIDocument.Document;
        //        var f = doc.GetElement(FamilyId) as Family;

        //        if (f == null)
        //        {
        //            status = string.Format("ERROR : Id <{0}> does not match an existent family Id!!!", FamilyId.IntegerValue);
        //        }
        //        else if (f.IsInPlace)
        //        {
        //            status = string.Format("ERROR : Family : <{0}> is an in place family and then can't be reloaded!!!", f.Name);
        //        }
        //        else if (!f.IsEditable)
        //        {
        //            status = string.Format("ERROR : Family : <{0}> is not an editable family and then can't be reloaded!!!", f.Name);
        //        }
        //        else
        //        {
        //            var FileName = Path.Combine(FolderPath as string, f.Name + ".rfa");
        //            var flo = new FamilyLoadOptions(OverwriteParametervalues:false, 
        //                                                 SharedFamilySource: FamilySource.Family);
        //            Family ReloadedFamily;

        //            if (doc.LoadFamily(FileName, flo, out ReloadedFamily))
        //            {
        //                status = string.Format("SUCCESS : Family <{0}> successfully reloaded from {1}", ReloadedFamily.Name, FolderPath as string);
        //            }
        //            else
        //            {
        //                status = string.Format("ERROR : Family <{0}> NOT RELOADED from {1}", f.Name, FolderPath as string);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        status = string.Format("ERROR : Family Id <{0}> NOT RELOADED from {1} \n {2}",
        //                               FamilyId.IntegerValue,
        //                               FolderPath as string,
        //                               ex.ToString());
        //    }
        //    return status;
        //}

        //public class FamilyLoadOptions : IFamilyLoadOptions
        //{
        //    bool _OverwriteParametervalues;
        //    FamilySource _SharedFamilySource;

        //    // Prevent use of default constructor
        //    private FamilyLoadOptions()
        //    {

        //    }

        //    public FamilyLoadOptions(bool OverwriteParametervalues,
        //                                FamilySource SharedFamilySource)
        //    {
        //        _OverwriteParametervalues = OverwriteParametervalues;
        //        _SharedFamilySource = SharedFamilySource;
        //    }

        //    public bool OnFamilyFound(bool familyInUse, out bool overwriteParameterValues)
        //    {
        //        //Debug.Print("Family found");
        //        overwriteParameterValues = _OverwriteParametervalues;
        //        return true;
        //        //throw new NotImplementedException();
        //    }

        //    public bool OnSharedFamilyFound(Family sharedFamily, bool familyInUse, out FamilySource source, out bool overwriteParameterValues)
        //    {
        //        //Debug.Print("Shared family found");
        //        source = _SharedFamilySource;
        //        overwriteParameterValues = _OverwriteParametervalues;
        //        return true;
        //        // throw new NotImplementedException();
        //    }
        //}
        #endregion
    }
}
