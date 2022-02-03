using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;


// Autodesk Revit
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Architecture;
using AppRvt = Autodesk.Revit.ApplicationServices.Application; // to avoid ambiguities
using DocRvt = Autodesk.Revit.DB.Document; // to avoid ambiguities
using BindingRvt = Autodesk.Revit.DB.Binding; // to avoid ambiguities

namespace BGO.Revit.Tools
{
    static class ParameterUtils
    {
        static public bool SetParameterValue(Element e, string ParameterName, string ParameterValue)
        {
            Parameter p = e.LookupParameter(ParameterName);
            if (p != null)
            {
                try
                {
                    p.Set(ParameterValue);
                }
                catch (Exception)
                {
                    p = null;
                }
            }
            return p != null;
        }

        static public string GetParameterValueAsString(Element e, string ParameterName)
        {
            string s = "<Unknown parameter>";
            Parameter p = e.LookupParameter(ParameterName);
            if (p != null)
            {
                s = GetParameterValueAsString(p);
            }
            return s;
        }

        static public string GetParameterValueAsString(Element e, BuiltInParameter bip)
        {
            string s = "<Unknown parameter>";
            Parameter p = e.get_Parameter(bip);
            if (p != null)
            {
                s = GetParameterValueAsString(p);
            }
            return s;
        }

        static public string GetParameterValueAsString(Parameter p)
        {
            switch (p.StorageType)
            {
                case StorageType.Double:
                    return p.AsDouble().ToString();
                case StorageType.ElementId:
                    return "Id:" + p.AsElementId().IntegerValue.ToString();
                case StorageType.Integer:
                    return p.AsInteger().ToString();
                case StorageType.None:
                    return "Storage type: none";
                case StorageType.String:
                    string s = p.AsString();
                    return s == null ? "" : s;
            }
            return "Invalid parameter storage type";
        }

        /// <summary>
        /// Lecture d'une table de valeurs de paramètres
        /// première ligne = nom des paramètres
        /// ligne suivantes = valeurs
        /// première colonne = nom et valeurs du paramètre index (à liste de valeurs uniques)
        /// </summary>
        /// <returns>Dictionnaire des noms et valeurs des paramètres</returns>
        static public Dictionary<string, List<string>> ReadParametersTableFromTextFile(string DialogTitle)
        {
            //Création d'un dictionnaire de données 
            var ParamDictionary = new Dictionary<string, List<string>>();

            //Ouverture d'une boite de dialogue de sélection de fichier texte
            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Title = DialogTitle;
                fd.Filter = "Text file (*.txt)|*.txt";
                fd.RestoreDirectory = true;
                fd.Multiselect = false;
                //Si fichier sélectionné
                if (DialogResult.OK == fd.ShowDialog())
                {
                    //Ouverture d'un flux de lecture du fichier 
                    StreamReader sw;
                    try
                    {
                        sw = File.OpenText(fd.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur ouverture fichier :\n" + fd.FileName + "\n" + ex.ToString());
                        return ParamDictionary;
                    }

                    // Lecture première ligne (titres)
                    string Line;
                    Line = sw.ReadLine();
                    if (Line == string.Empty)
                    {
                        MessageBox.Show("Le fichier:\n" + fd.FileName + "\n" + "ne contient aucune ligne!");
                        return ParamDictionary;
                    }
                    // Décomposition ligne suivant champs séparés par des tabulations 
                    var ParamNames = Line.Split('\t').ToList();


                    //Initialistaion message d'erreur
                    string ErrorMsg = string.Empty;

                    try
                    {
                        int i = 0;
                        //Pour chaque ligne du fichier texte, mémorisation dans le dictionnaire des données associées
                        //à chaque feuille
                        while (!sw.EndOfStream)
                        {
                            i++;
                            Line = sw.ReadLine();
                            if (Line == string.Empty) break;
                            var ParamValues = Line.Split('\t').ToList();
                            if (ParamValues.Count != ParamNames.Count) break;
                            //Valeur Index = Première colonne
                            string IndexValue = ParamValues.ElementAt(0);
                            if (IndexValue == string.Empty) break;
                            if (i == 1)
                            {
                                ParamDictionary.Add("ParamNames", ParamNames);
                            }
                            ParamDictionary.Add(IndexValue, ParamValues);
                        }

                    }
                    catch (Exception ex)
                    {
                        //En cas d'erreur non gérée, affichage du message et vidage du dictionnaire
                        MessageBox.Show(ex.ToString());
                        ParamDictionary.Clear();
                    }
                    // Fermeture du fichier programme
                    sw.Close();
                }
            }
            return ParamDictionary;
        }

        public enum BindSharedParamResult
        {
            eAlreadyBound,
            eSuccessfullyBound,
            eWrongParamType,
            //eWrongCategory, //not exposed
            //eWrongVisibility, //not exposed
            eWrongBindingType,
            eFailed
        }

        /// <summary>
        /// Get Element Parameter *by name*. By defualt NOT case sensitive. Use overloaded one if case sensitive needed.
        /// </summary>
        /// <param name="elem"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Parameter GetElemParam(Element elem, string name)
        {
            return GetElemParam(elem, name, false);
        }
        public static Parameter GetElemParam(Element elem, string name, bool matchCase)
        {
            StringComparison comp = StringComparison.CurrentCultureIgnoreCase;
            if (matchCase) comp = StringComparison.CurrentCulture;

            foreach (Parameter p in elem.Parameters)
            {
                if (p.Definition.Name.Equals(name, comp)) return p;
            }
            // if here, not found
            return null;
        }




   /// <summary>
   /// Get shared Parematers txt file
   /// </summary>
   /// <param name="app"></param>
   /// <param name="filePath"></param>
   /// <returns></returns>
        public static DefinitionFile GetSharedParamsFile(AppRvt app)
        {
            string fileName = string.Empty;
            try // generic
            {
                // Get file
                fileName = app.SharedParametersFilename;
                
                // Create file if not set yet (ie after Revit installed and no Shared params used so far)
                if (string.Empty == fileName)
                {
                    MessageBox.Show("ERROR: No shared parameters file!");
                    return null;
                }
                return app.OpenSharedParameterFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: Failed to get the Shared Params File: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get or Create Shared Params File
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static DefinitionFile CreateSharedParamsFile(AppRvt app, string filePath)
        {
            string fileName = string.Empty;
            try // generic
            {
                // Get file
                fileName = app.SharedParametersFilename;

                // Create file if not set yet (ie after Revit installed and no Shared params used so far)
                if (string.Empty == fileName)
                {
                    fileName = filePath;
                    StreamWriter stream = new StreamWriter(fileName);
                    stream.Close();
                    app.SharedParametersFilename = fileName;
                }

                else {
                    MessageBox.Show("ERROR: Already a parameter file in the apps ");
                    return null;
                }
                return app.OpenSharedParameterFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR: Failed to create Shared Params File: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get or Create Shared Parameters Group
        /// </summary>
        /// <param name="defFile"></param>
        /// <param name="grpName"></param>
        /// <returns></returns>
        public static DefinitionGroup GetOrCreateSharedParamsGroup(DefinitionFile defFile, string grpName)
        {
            try // generic
            {
                DefinitionGroup defGrp = defFile.Groups.get_Item(grpName);
                if (null == defGrp) defGrp = defFile.Groups.Create(grpName);
                return defGrp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("ERROR: Failed to get or create Shared Params Group: {0}", ex.Message));
                return null;
            }
        }

        /// <summary> 
        /// Get or Create Shared Parameter Definition
        /// </summary>
        /// <param name="defGrp"></param>
        /// <param name="parType">used only if creating</param>
        /// <param name="parName">used only if creating</param>
        /// <param name="visible">used only if creating</param>
        /// <returns></returns>
        public static Definition GetOrCreateSharedParamDefinition(DefinitionGroup defGrp, ForgeTypeId parType, string parName, bool visible)
        {
            try // generic
            {
                Definition def = defGrp.Definitions.get_Item(parName);
                ExternalDefinitionCreationOptions defopt = new ExternalDefinitionCreationOptions(parName,parType);
                if (null == def)
                    def =  defGrp.Definitions.Create(defopt);
                return def;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("ERROR: Failed to get or create Shared Params Definition: {0}", ex.Message));
                return null;
            }
        }

        


    }
}
