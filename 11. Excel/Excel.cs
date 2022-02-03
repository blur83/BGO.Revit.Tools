using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Odbc;

using Autodesk.Revit.UI;

namespace BGO.Revit.Tools
{
    class Excel
    {
        static public bool CreateExcelConnection()
        {
            string fileName = @"C:\Temp\Project1 - EXPORT TO AUTOMOD.xls";
            
            string connectionString = 
                "Driver={Microsoft Excel Driver (*.xls)};DriverId=790;" +
                "Dbq="+ fileName + ";" +
                "DefaultDir=" + Path.GetDirectoryName(fileName);

            
            //"Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};"+
            //                          "DBQ=" + fileName + ";";
            bool success;
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
               
                try
                {
                    connection.Open();
                    success = true;
                }
                catch (Exception ex)
                {
                    TaskDialog.Show("ERROR",ex.ToString());
                    success = false;
                }
            }
            return success;
        }
    }
}
