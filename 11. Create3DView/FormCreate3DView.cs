using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;


namespace BGO.Revit.Tools
{
    public partial class FormCreate3DView : System.Windows.Forms.Form
    {
        #region properties

        private UIDocument _UIdoc;
        private Document _doc;
        private Dictionary<string, ElementId> _Grids;
        private Dictionary<string, ElementId> _Levels;
        private Dictionary<string, ElementId> _ScopeBoxes;

        private string _ViewName;
        private BoundingBoxXYZ _SectionBox;

        public string ViewName
        {
            get { return _ViewName; }
        }

        public BoundingBoxXYZ SectionBox
        {
            get { return _SectionBox; }
        }

        #endregion

        #region Initialization

        private FormCreate3DView()
        {
            InitializeComponent();
        }

        public FormCreate3DView(UIDocument UIdoc)
        {
            InitializeComponent();

            _UIdoc = UIdoc;
            _doc = UIdoc.Document;
            _Grids = new Dictionary<string,ElementId>();
            _Levels = new Dictionary<string,ElementId>();
            _ScopeBoxes = new Dictionary<string, ElementId>();
            _SectionBox = null;
            _ViewName = null;

            InitializeForm();
        }

        private void InitializeForm()
        {
            var XAxis = new XYZ(1, 0, 0);
            var YAxis = new XYZ(0, 1, 0);
            var colGrids = new FilteredElementCollector(_doc).OfClass(typeof(Grid));
            IEnumerable<Grid> Grids = colGrids.Cast<Grid>();
            
            // Traiteement des grilles multisegment : On va ignorer toutes les grilles composant les grilles multisegments
            var msGrids = new FilteredElementCollector(_doc).OfClass(typeof(MultiSegmentGrid));
            var msgIds = new List<ElementId>(); //Ids de toutes les grilles élémentaires contenues dans les grilles multisegments
            foreach (MultiSegmentGrid msg in msGrids)
            {
                msgIds.AddRange(msg.GetGridIds());
            }

            foreach (Grid g in Grids)
            {
                // Si la grille est un arc, on ignore
                if (g.IsCurved) continue;
                
                // Si la grille est un segment de grille multisegments, on ignore
                if (msgIds.Contains(g.Id)) continue;

                Line GridLine = g.Curve as Line;
                string gName = NormalizeString(g.Name, 2, 2);
                _Grids.Add(gName, g.Id);
                XYZ Direction = GridLine.Direction;
                if (Direction.CrossProduct(XAxis).IsAlmostEqualTo(XYZ.Zero))
                {
                    comboBoxYGridStart.Items.Add(gName);
                    comboBoxYGridEnd.Items.Add(gName);
                }
                else if (Direction.CrossProduct(YAxis).IsAlmostEqualTo(XYZ.Zero))
                {
                    comboBoxXGridStart.Items.Add(gName);
                    comboBoxXGridEnd.Items.Add(gName);
                }
            }
            
            var colLevels = new FilteredElementCollector(_doc).OfClass(typeof(Level));
            IEnumerable<Level> Levels = colLevels.Cast<Level>();
            foreach (Level l in Levels)
            {
                string lName = NormalizeString(l.Name, 2, 0);
                _Levels.Add(lName, l.Id);
                comboBoxLevelStart.Items.Add(lName);
                comboBoxLevelEnd.Items.Add(lName);
            }

            var colScopeBoxes = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_VolumeOfInterest);
            IEnumerable<Element> ScopeBoxes = colScopeBoxes.Cast<Element>();
            foreach(Element e in ScopeBoxes)
            {
                _ScopeBoxes.Add(e.Name, e.Id);
                comboBoxScopeBoxes.Items.Add(e.Name);
            }

        }

        #endregion

        # region Commands

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            this.Tag = "";
            if (radioButtonByGrid.Checked)
            {
                // Grids
                Grid gridXStart = _doc.GetElement(_Grids[comboBoxXGridStart.SelectedItem.ToString()]) as Grid;
                Grid gridXEnd = _doc.GetElement(_Grids[comboBoxXGridEnd.SelectedItem.ToString()]) as Grid;
                Grid gridYStart = _doc.GetElement(_Grids[comboBoxYGridStart.SelectedItem.ToString()]) as Grid;
                Grid gridYEnd = _doc.GetElement(_Grids[comboBoxYGridEnd.SelectedItem.ToString()]) as Grid;
#if RELEASE2013
                double XStart = gridXStart.Curve.get_EndPoint(0).X;
                double XEnd = gridXEnd.Curve.get_EndPoint(0).X;
                double YStart = gridYStart.Curve.get_EndPoint(0).Y;
                double YEnd = gridYEnd.Curve.get_EndPoint(0).Y;
#else
                double XStart = gridXStart.Curve.GetEndPoint(0).X;
                double XEnd = gridXEnd.Curve.GetEndPoint(0).X;
                double YStart = gridYStart.Curve.GetEndPoint(0).Y;
                double YEnd = gridYEnd.Curve.GetEndPoint(0).Y;
#endif
                double minX = Math.Min(XStart, XEnd);
                double maxX = Math.Max(XStart, XEnd);
                double minY = Math.Min(YStart, YEnd);
                double maxY = Math.Max(YStart, YEnd);

                // Levels
                Level levelStart = _doc.GetElement(_Levels[comboBoxLevelStart.SelectedItem.ToString()]) as Level;
                Level levelEnd = _doc.GetElement(_Levels[comboBoxLevelEnd.SelectedItem.ToString()]) as Level;
                double LStart = levelStart.Elevation;
                double LEnd = levelEnd.Elevation;
                double minZ = Math.Min(LStart, LEnd);
                double maxZ = Math.Max(LStart, LEnd);

                //Create the bounding box
                _SectionBox = new BoundingBoxXYZ();
                _SectionBox.set_Bounds(0, new XYZ(minX, minY, minZ));
                _SectionBox.set_Bounds(1, new XYZ(maxX, maxY, maxZ));
                _ViewName = string.Format("3D - {0}-{1}-{2}-{3}-{4}-{5}",
                        gridXStart.Name, gridXEnd.Name, gridYStart.Name, gridYEnd.Name, levelStart.Name, levelEnd.Name);

                this.Tag = "BY GRID";
            }
            else if (radioButtonByScopeBox.Checked)
            {
                // ScopeBox
                Element ScopeBox = _doc.GetElement(_ScopeBoxes[comboBoxScopeBoxes.SelectedItem.ToString()]);
                BoundingBoxXYZ bbXYZ = ScopeBox.get_BoundingBox(null);
                double minX = bbXYZ.Min.X;
                double maxX = bbXYZ.Max.X;
                double minY = bbXYZ.Min.Y;
                double maxY = bbXYZ.Max.Y;

                // Levels
                Level levelStart = _doc.GetElement(_Levels[comboBoxLevelStart.SelectedItem.ToString()]) as Level;
                Level levelEnd = _doc.GetElement(_Levels[comboBoxLevelEnd.SelectedItem.ToString()]) as Level;
                double LStart = levelStart.Elevation;
                double LEnd = levelEnd.Elevation;
                double minZ = Math.Min(LStart, LEnd);
                double maxZ = Math.Max(LStart, LEnd);

                //Create the bounding box
                _SectionBox = new BoundingBoxXYZ();
                _SectionBox.set_Bounds(0, new XYZ(minX, minY, minZ));
                _SectionBox.set_Bounds(1, new XYZ(maxX, maxY, maxZ));
                _ViewName = string.Format("3D - {0}-{1}-{2}",ScopeBox.Name, levelStart.Name, levelEnd.Name);
                
                this.Tag = "BY SCOPE BOX";
            }
            else if (radioButtonByElement.Checked)
            {
                try
                {
                    this.Tag = "INVALID ID";
                    string IdString = textBoxElementId.Text;
                    int i = Convert.ToInt32(IdString);
                    if (i != 0)
                    {
                        BoundingBoxXYZ bbXYZ;
                        ElementId id = new ElementId(i);
                        Element el = _doc.GetElement(id);
                        if (el != null && (bbXYZ = el.get_BoundingBox(null)) != null)
                        {
                            double ExtraRange = 10;
                            double minX = bbXYZ.Min.X - ExtraRange;
                            double maxX = bbXYZ.Max.X + ExtraRange;
                            double minY = bbXYZ.Min.Y - ExtraRange;
                            double maxY = bbXYZ.Max.Y + ExtraRange;
                            double minZ = bbXYZ.Min.Z - ExtraRange;
                            double maxZ = bbXYZ.Max.Z + ExtraRange;

                            //Create the bounding box
                            _SectionBox = new BoundingBoxXYZ();
                            _SectionBox.set_Bounds(0, new XYZ(minX, minY, minZ));
                            _SectionBox.set_Bounds(1, new XYZ(maxX, maxY, maxZ));
                            _ViewName = "3D - ID " + IdString;
                            this.Tag = "BY ELEMENT";
                        }
                    }
                }
                catch (Exception)
                {
                 
                }
             }
        }

        #endregion

        #region Events

        private void comboBoxXGridEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void comboBoxXGridStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void comboBoxYGridStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void comboBoxYGridEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void comboBoxScopeBoxes_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void comboBoxLevelStart_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void comboBoxLevelEnd_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void textBoxElementId_TextChanged(object sender, EventArgs e)
        {
            CheckSectionBoxValid();
        }

        private void radioButtonByGrid_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                this.groupBoxGridLimits.Enabled = true;
                this.groupBox_LevelLimits.Enabled = true;
            }
            else
            {
                this.groupBoxGridLimits.Enabled = false;
            }
            CheckSectionBoxValid();
        }

        private void radioButtonByScopeBox_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked)
            {
                this.comboBoxScopeBoxes.Enabled = true;
                this.groupBox_LevelLimits.Enabled = true;
            }
            else
            {
                this.comboBoxScopeBoxes.Enabled = false;
            }

            CheckSectionBoxValid();
        }

        private void radioButtonByElement_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                this.groupBoxGridLimits.Enabled = false;
                this.groupBox_LevelLimits.Enabled = false;
                this.textBoxElementId.Enabled = true;
            }
            else
            {
                this.textBoxElementId.Enabled = false;
            }
            CheckSectionBoxValid();
        }    

        private void CheckSectionBoxValid()
        {
            buttonCreate.Enabled =
                (radioButtonByGrid.Checked &&
                    (comboBoxXGridStart.SelectedIndex >= 0) &&
                    (comboBoxXGridEnd.SelectedIndex >= 0) &&
                    (comboBoxYGridStart.SelectedIndex >= 0) &&
                    (comboBoxYGridEnd.SelectedIndex >= 0) &&
                    (comboBoxLevelStart.SelectedIndex >= 0) &&
                    (comboBoxLevelEnd.SelectedIndex >= 0) &&
                    (comboBoxXGridStart.SelectedIndex != comboBoxXGridEnd.SelectedIndex) &&
                    (comboBoxYGridStart.SelectedIndex != comboBoxYGridEnd.SelectedIndex) &&
                    (comboBoxLevelStart.SelectedIndex != comboBoxLevelEnd.SelectedIndex)
                )
                ||
                (radioButtonByScopeBox.Checked &&
                    (comboBoxScopeBoxes.SelectedIndex >= 0) &&
                    (comboBoxLevelStart.SelectedIndex >= 0) &&
                    (comboBoxLevelEnd.SelectedIndex >= 0) &&
                    (comboBoxLevelStart.SelectedIndex != comboBoxLevelEnd.SelectedIndex)
                )
                ||
                (radioButtonByElement.Checked &&
                    Is3DElement(textBoxElementId.Text)
                );
        }

        #endregion

        #region Helpers

        private bool Is3DElement(string IdString)
        {
            bool isValid = false; 
            try 
	        {
                int i = Convert.ToInt32(IdString);
                if (i != 0)
                {
                    ElementId id = new ElementId(i);
                    Element el = _doc.GetElement(id);
                    isValid = (el != null && el.get_BoundingBox(null) != null);
                }
	        }
	        catch (Exception)
	        {
  	        }
            return isValid;
        }

        private string NormalizeString(string s, int digitCount, int leftLength)
        {
            int n = s.Length;
            char[] Chars = s.ToCharArray();
            int nDigits = 0;
            int index;
            //Comptage des digits de droite
            for (index = n - 1; index >= 0; index--)
            {
                if (Chars[index].ToString() == ".")
                {
                    nDigits = 0;
                    continue;
                }
                if (!char.IsDigit(Chars[index])) break;
                nDigits++;
            }
            if (index != -1)
            {
                //Cadrage à droite de la partie gauche
                while (index < leftLength - 1)
                {
                    s = " " + s;
                    index++;
                }
            }
            // Cadrage à droite des éventuels digits de droite
            if (nDigits > 0)
            {
                while (nDigits < digitCount)
                {
                    s = s.Substring(0, index+1) + " " + s.Substring(index+1);
                    nDigits++; index++;
                }
            }
        return s;
        }

        #endregion

        private void groupBoxXYLimits_Enter(object sender, EventArgs e)
        {

        }


     }
}

//    Module CharStructure

//    Public Sub Main()

//        Dim chA As Char
//        chA = "A"c
//        Dim ch1 As Char
//        ch1 = "1"c
//        Dim str As String
//        str = "test string"

//        Console.WriteLine(chA.CompareTo("B"c))          ' Output: "-1" (meaning 'A' is 1 less than 'B')
//        Console.WriteLine(chA.Equals("A"c))             ' Output: "True"
//        Console.WriteLine(Char.GetNumericValue(ch1))    ' Output: "1"
//        Console.WriteLine(Char.IsControl(Chr(9)))       ' Output: "True"
//        Console.WriteLine(Char.IsDigit(ch1))            ' Output: "True"
//        Console.WriteLine(Char.IsLetter(","c))          ' Output: "False"
//        Console.WriteLine(Char.IsLower("u"c))           ' Output: "True"
//        Console.WriteLine(Char.IsNumber(ch1))           ' Output: "True"
//        Console.WriteLine(Char.IsPunctuation("."c))     ' Output: "True"
//        Console.WriteLine(Char.IsSeparator(str, 4))     ' Output: "True"
//        Console.WriteLine(Char.IsSymbol("+"c))          ' Output: "True"
//        Console.WriteLine(Char.IsWhiteSpace(str, 4))    ' Output: "True"
//        Console.WriteLine(Char.Parse("S"))              ' Output: "S"
//        Console.WriteLine(Char.ToLower("M"c))           ' Output: "m"
//        Console.WriteLine("x"c.ToString())              ' Output: "x"

//    End Sub

//End Module


