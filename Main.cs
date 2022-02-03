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
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Imaging;

using Autodesk.Revit.Attributes;
//using Autodesk.Revit.ApplicationServices:
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

#endregion

namespace BGO.Revit.Tools
{
    /// <summary>
    /// Ribbon UI. 
    /// we'll be using commands we defined in Revit Intro labs. alternatively, 
    /// you can use your own. Any command will do for this ribbon exercise. 
    /// cf. Developer Guide, Section 3.8: Ribbon Panels and Controls. (pp 46). 
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class MainApp : IExternalApplication
    {
        private UIControlledApplication _UICApp;
        private string _AssemblyPath;
        private string _NameSpace;

        /// <summary>
        /// This is both the assembly name and the namespace 
        /// of the external command provider.
        /// </summary>
        /// 
        
        /// <summary>
        /// Name of subdirectory containing images.
        /// </summary>
        //const string _imageFolderName = "Images";
        /// <summary>
        /// Location of images for icons.
        /// </summary>
        //string _imageFolder = "";

        /// <summary>
        /// global parameters for elements reindexation
        /// </summary>
        public static string LastIndexParameterName = "@G Index";
        public static int LastIndex = 0;
        public static string LastFormat = "#";
        public static int LastOrder = 0; // 0: Horizontal, 1: Vertical

        ///// <summary>
        ///// Load a new icon bitmap from our image folder.
        ///// </summary>
        //BitmapImage NewBitmapImage(string imageName)
        //{
        //    return new BitmapImage(new Uri(
        //      Path.Combine(_imageFolder, imageName)));

        //}

        /// <summary>
        /// Create a bitmap image from an icon.
        /// </summary>
        BitmapSource IconToBitMap(System.Drawing.Icon ico)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon
            (
                ico.Handle,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            ); 
        }

        /// <summary>
        /// OnStartup() - called when Revit starts. 
        /// </summary>
        public Result OnStartup(UIControlledApplication app)
        {
            _UICApp = app;
            _AssemblyPath = GetType().Assembly.Location;
            _NameSpace = GetType().Namespace;
            
            // Add "Document Changed" Eventhandler
            //app.ControlledApplication.DocumentChanged +=
            //    new EventHandler<Autodesk.Revit.DB.Events.DocumentChangedEventArgs>(ControlledApplication_DocumentChanged);

            // Add BIMgo24 Ribbon to Revit Ribbon
            AddFlacRibbon();

             
            return Result.Succeeded;
        }

        /// <summary>
        /// OnShutdown() - called when Revit ends. 
        /// </summary>
        public Result OnShutdown(UIControlledApplication app)
        {
            //// Remove "Document Changed" eventhandler
            //app.ControlledApplication.DocumentChanged -= ControlledApplication_DocumentChanged;
            
            return Result.Succeeded;
        }

        void ControlledApplication_DocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Create our own ribbon panel with verious buttons 
        /// for our exercise. We re-use commands defined in the
        /// Revit Intro Labs here. Cf. Section 3.8 (pp 46) of 
        /// the Developers Guide. 
        /// </summary>
        public void AddFlacRibbon()
        {
            // Create a ribbon tab for BIMgo24     
            _UICApp.CreateRibbonTab("FLAC");

            // Create a panel for BIMgo24 info
            AddInfoPanel();

            // Create a panel for Utilities
            AddUtilityPanel();


            // Below are samplers of ribbon items. Uncomment 
            // functions of your interest to see how it looks like 

            // (2.1) add a simple push button for Hello World 

            //AddPushButtons(BIMgo24UtilitiesPanel);

            // (2.2) add split buttons for "Command Data", "DB Element" and "Element Filtering" 

            //AddSplitButton(BIMgo24SACSPanel);

            // (2.3) add pulldown buttons for "Command Data", "DB Element" and "Element Filtering" 

            //AddPulldownButton(panel);

            // (2.4) add radio/toggle buttons for "Command Data", "DB Element" and "Element Filtering" 
            // we put it on the slide-out below. 
            //AddRadioButton(panel);
            //panel.AddSeparator();

            // (2.5) add text box - TBD: this is used with the conjunction with event. Probably too complex for day one training. 
            //  for now, without event. 
            // we put it on the slide-out below. 
            //AddTextBox(panel);
            //panel.AddSeparator();

            // (2.6) combo box - TBD: this is used with the conjunction with event. Probably too complex for day one training. 
            // For now, without event. show two groups: Element Bascis (3 push buttons) and Modification/Creation (2 push button)  

            //AddComboBox(panel);

            // (2.7) stacked items - 1. hello world push button, 2. pulldown element bscis (command data, DB element, element filtering) 
            // 3. pulldown modification/creation(element modification, model creation). 

            //AddStackedButtons_Complex(panel);

            // (2.8) slide out - if you don't have enough space, you can add additional space below the panel. 
            // anything which comes after this will be on the slide out. 

            //panel.AddSlideOut();

            // (2.4) radio button - what it is 

            //AddRadioButton(panel);

            // (2.5) text box - what it is 

            //AddTextBox(panel);
        }

        public void AddInfoPanel()
        {//Create a ribbon panel for info
            RibbonPanel panel = _UICApp.CreateRibbonPanel("FLAC", "FLAC");

            // Create Info FLAC
            // Set the information about the command we will be assigning to the button 
            PushButtonData pushButtonDataInfoFlac = new PushButtonData("FlacPushButtonInfoFlac",
                "Info BIM", _AssemblyPath, _NameSpace+".InfoFlac");
            // Add the button to the panel 
            PushButton pushButtonInfoFlac = panel.AddItem(pushButtonDataInfoFlac) as PushButton;
            // Add an icon 
            // Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            //pushButtonCreate3DView.LargeImage = NewBitmapImage("House.ico");
            pushButtonInfoFlac.LargeImage = IconToBitMap(Properties.Resources.Flac);
            // Add a tooltip 
            pushButtonInfoFlac.ToolTip = "";



        }

        public void AddUtilityPanel()
        {
            //Create a ribbon panel for utilities
            RibbonPanel panel = _UICApp.CreateRibbonPanel("FLAC", "Utilities");

            // Create 3D View
            // Set the information about the command we will be assigning to the button 
            PushButtonData pushButtonDataCreate3DView = new PushButtonData("PushButtonCreate3DView",
                "Create 3D View", _AssemblyPath, _NameSpace + ".Create3DView");
            // Add the button to the panel 
            PushButton pushButtonCreate3DView = panel.AddItem(pushButtonDataCreate3DView) as PushButton;
            // Add an icon 
            // Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            //pushButtonCreate3DView.LargeImage = NewBitmapImage("House.ico");
            pushButtonCreate3DView.LargeImage = IconToBitMap(Properties.Resources.House);
            // Add a tooltip 
            pushButtonCreate3DView.ToolTip = "Create a 3D View using grids, scope boxes, levels or previously selected element(s)";

            // UpdateSheetsFromLOD
            // Set the information about the command we will be assigning to the button 
            PushButtonData pushButtonDataUpdateSheetsFromLOD = new PushButtonData("PushButtonUpdateSheetsFromLOD",
                "Sheets from LOD", _AssemblyPath, _NameSpace + ".UpdateSheetsFromLOD");
            // Add the button to the panel 
            PushButton pushButtonUpdateSheetsFromLOD = panel.AddItem(pushButtonDataUpdateSheetsFromLOD) as PushButton;
            // Add an icon 
            // Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            //pushButtonCreate3DView.LargeImage = NewBitmapImage("House.ico");
            pushButtonUpdateSheetsFromLOD.LargeImage = IconToBitMap(Properties.Resources.Drawing);
            // Add a tooltip 
            pushButtonUpdateSheetsFromLOD.ToolTip = "Update Sheets parameters with Excel file";

            //// Save loaded families to folder
            //// Set the information about the command we will be assigning to the button 
            //PushButtonData pushButtonDataSaveLoadedFamiliesToFolder = new PushButtonData("SaveLoadedFamiliesToFolder",
            //    "Save loaded families to folder", _AssemblyPath, _NameSpace + ".SaveLoadedFamiliesToFolder");
            //// Add the button to the panel 
            //PushButton pushButtonSaveLoadedFamiliesToFolder = panel.AddItem(pushButtonDataSaveLoadedFamiliesToFolder) as PushButton;
            //// Add an icon 
            //// Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            //// pushButtonSaveLoadedFamiliesToFolder.LargeImage = NewBitmapImage("Basics.ico");
            //pushButtonSaveLoadedFamiliesToFolder.LargeImage = IconToBitMap(Properties.Resources.Basics);
            //// Add a tooltip 
            //pushButtonSaveLoadedFamiliesToFolder.ToolTip = "Save all loaded families into a specified folder";

        }

        public void AddSACSPanel()
        {
            RibbonPanel BIMgo24SACSPanel = _UICApp.CreateRibbonPanel("ADP", "SACS");
            // Create push buttons for CreateDoorControls 
            PushButtonData pushButtonData1 = new PushButtonData(
                "SACS_CreateDoorControls", 
                "Create Door Controls",
                _AssemblyPath,
                _NameSpace + ".SACS_CreateDoorControls");
            //pushButtonData1.LargeImage = NewBitmapImage("Basics.ico");
            pushButtonData1.LargeImage = IconToBitMap(Properties.Resources.Basics);

            // Create push buttons for SynchronizeDoorControls
            PushButtonData pushButtonData2 = new PushButtonData(
                "SACS_SynchronizeDoorControls",
                "Synchronize Door Controls",
                _AssemblyPath,
                _NameSpace + ".SACS_DoorControlsSynchronization");
            //pushButtonData2.LargeImage = NewBitmapImage("Basics.ico");
            pushButtonData2.LargeImage = IconToBitMap(Properties.Resources.Basics);

            // Make a split button now 
            SplitButtonData splitBtnData = new SplitButtonData("SplitButton", "Split Button");
            SplitButton splitBtn = BIMgo24SACSPanel.AddItem(splitBtnData) as SplitButton;
            splitBtn.AddPushButton(pushButtonData1);
            splitBtn.AddPushButton(pushButtonData2);
        }

        public void AddBHSPanel()
        {
            //Create a ribbon panel for BHS
            RibbonPanel BIMgo24BHSPanel = _UICApp.CreateRibbonPanel("ADP", "BHS");

            // Show BHS Form
            // Set the information about the command we will be assigning to the button 
            PushButtonData pushButtonDataShowBHSForm = new PushButtonData("BHS_ShowToolBar",
                "Show BHS Toolbar", _AssemblyPath, _NameSpace + ".BHS_ShowToolBar");
            // Add the button to the panel 
            PushButton pushButtonShowBHSForm = BIMgo24BHSPanel.AddItem(pushButtonDataShowBHSForm) as PushButton;
            // Add an icon 
            // Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            //pushButtonShowBHSForm.LargeImage = NewBitmapImage("Basics.ico");
            pushButtonShowBHSForm.LargeImage = IconToBitMap(Properties.Resources.Basics);
            // Add a tooltip 
            pushButtonShowBHSForm.ToolTip = "Show BHS Toolbar";
        }


        public void AddTestPanel()
        {
            //Create a ribbon panel for Test
            RibbonPanel BIMgo24TestPanel = _UICApp.CreateRibbonPanel("ADP", "TEST");

            // Show BHS Form
            // Set the information about the command we will be assigning to the button 
            PushButtonData pushButtonDataTest = new PushButtonData("Test",
                "test", _AssemblyPath, _NameSpace + ".Test");
            // Add the button to the panel 
            PushButton pushButtonTest = BIMgo24TestPanel.AddItem(pushButtonDataTest) as PushButton;
            // Add an icon 
            // Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
            //pushButtonTest.LargeImage = NewBitmapImage("Basics.ico");
            pushButtonTest.LargeImage = IconToBitMap(Properties.Resources.Basics);
            // Add a tooltip 
            pushButtonTest.ToolTip = "Test";
        }

        //public void AddPushButtons(RibbonPanel panel)
        //{

        //    // Get the location of this dll. 
        //    string AssemblyPath = GetType().Assembly.Location;
        //    string NameSpace = GetType().Namespace;

        //    // Create 3D View
        //    // Set the information about the command we will be assigning to the button 
        //    PushButtonData pushButtonDataCreate3DView = new PushButtonData("BIMgo24PushButtonCreate3DView",
        //        "Create 3D View", AssemblyPath, NameSpace + ".Create3DView");
        //    // Add the button to the panel 
        //    PushButton pushButtonCreate3DView = panel.AddItem(pushButtonDataCreate3DView) as PushButton;
        //    // Add an icon 
        //    // Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
        //    pushButtonCreate3DView.LargeImage = NewBitmapImage("House.ico");
        //    // Add a tooltip 
        //    pushButtonCreate3DView.ToolTip = "Create a 3D View using grids, scope boxes, levels or previously selected element(s)";

        //    // 

        //    //// Update Data Devices Information
        //    //// Set the information about the command we will be assigning to the button 
        //    //PushButtonData pushButtonDataUpdateDataDevicesSharedInformation = new PushButtonData("BIMgo24PushButtonUpdateDataDevicesSharedInformation",
        //    //    "Update Data Devices Information", AssemblyPath, NameSpace + ".UpdateDataDevicesSharedInformation");
        //    //// Add the button to the panel 
        //    //PushButton pushButtonUpdateDataDevicesSharedInformation = panel.AddItem(pushButtonDataUpdateDataDevicesSharedInformation) as PushButton;
        //    //// Add an icon 
        //    //// Make sure you reference WindowsBase and PresentationCore, and import System.Windows.Media.Imaging namespace. 
        //    //pushButtonUpdateDataDevicesSharedInformation.LargeImage = NewBitmapImage("Basics.ico");
        //    //// Add a tooltip 
        //    //pushButtonUpdateDataDevicesSharedInformation.ToolTip = "Update Data Devices Shared Information (Mark, @Workset, @Host, @Elevation) ";
        //}

        /////// <summary>
        /////// Split button for "Command Data", "DB Element" and "Element Filtering" 
        /////// </summary>
        //public void AddSplitButton(RibbonPanel panel)
        //{
        //    // Get the location of this dll. 
        //    string AssemblyPath = GetType().Assembly.Location;
        //    string NameSpace = GetType().Namespace;

        //    // Create three push buttons for split button drop down 
        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("SACS_CreateDoorControls", "Create Door Controls", 
        //        AssemblyPath, NameSpace + ".SACS_CreateDoorControls");
        //    pushButtonData1.LargeImage = NewBitmapImage("Basics.ico");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("SACS_SynchronizeDoorControls", "Synchronize Door Controls",
        //        AssemblyPath, NameSpace + ".SACS_DoorControlsSynchronization");
        //    pushButtonData2.LargeImage = NewBitmapImage("Basics.ico");

        //    //// #3 
        //    //PushButtonData pushButtonData3 = new PushButtonData("SplitElementFiltering", "ElementFiltering",
        //    //    AssemblyPath, NameSpace + ".SACS_DoorControlsSynchronization");
        //    //pushButtonData3.LargeImage = NewBitmapImage("ImgHelloWorld.png");

        //    // Make a split button now 
        //    SplitButtonData splitBtnData = new SplitButtonData("SplitButton", "Split Button");
        //    SplitButton splitBtn = panel.AddItem(splitBtnData) as SplitButton;
        //    splitBtn.AddPushButton(pushButtonData1);
        //    splitBtn.AddPushButton(pushButtonData2);
        //    //splitBtn.AddPushButton(pushButtonData3);
        //}

        ///// <summary>
        ///// Pulldown button for "Command Data", "DB Element" and "Element Filtering"
        ///// </summary>
        //public void AddPulldownButton(RibbonPanel panel)
        //{
        //    // Create three push buttons for pulldown button drop down 

        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("PulldownCommandData", "Command Data", _externalAssemblyPath, _introLabName + ".CommandData");
        //    pushButtonData1.LargeImage = NewBitmapImage("Basics.ico");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("PulldownDbElement", "DB Element", _externalAssemblyPath, _introLabName + ".DbElement");
        //    pushButtonData2.LargeImage = NewBitmapImage("Basics.ico");

        //    // #3 
        //    PushButtonData pushButtonData3 = new PushButtonData("PulldownElementFiltering", "Filtering", _externalAssemblyPath, _introLabName + ".ElementFiltering");
        //    pushButtonData3.LargeImage = NewBitmapImage("Basics.ico");

        //    // Make a pulldown button now 
        //    PulldownButtonData pulldownBtnData = new PulldownButtonData("PulldownButton", "Pulldown");
        //    PulldownButton pulldownBtn = panel.AddItem(pulldownBtnData) as PulldownButton;
        //    pulldownBtn.AddPushButton(pushButtonData1);
        //    pulldownBtn.AddPushButton(pushButtonData2);
        //    pulldownBtn.AddPushButton(pushButtonData3);
        //}

        ///// <summary>
        ///// Radio/toggle button for "Command Data", "DB Element" and "Element Filtering"
        ///// </summary>
        //public void AddRadioButton(RibbonPanel panel)
        //{
        //    // Create three toggle buttons for radio button group 

        //    // #1 
        //    ToggleButtonData toggleButtonData1 = new ToggleButtonData("RadioCommandData", "Command" + "\n Data", _externalAssemblyPath, _introLabName + ".CommandData");
        //    toggleButtonData1.LargeImage = NewBitmapImage("Basics.ico");

        //    // #2 
        //    ToggleButtonData toggleButtonData2 = new ToggleButtonData("RadioDbElement", "DB" + "\n Element", _externalAssemblyPath, _introLabName + ".DbElement");
        //    toggleButtonData2.LargeImage = NewBitmapImage("Basics.ico");

        //    // #3 
        //    ToggleButtonData toggleButtonData3 = new ToggleButtonData("RadioElementFiltering", "Filtering", _externalAssemblyPath, _introLabName + ".ElementFiltering");
        //    toggleButtonData3.LargeImage = NewBitmapImage("Basics.ico");

        //    // Make a radio button group now 
        //    RadioButtonGroupData radioBtnGroupData = new RadioButtonGroupData("RadioButton");
        //    RadioButtonGroup radioBtnGroup = panel.AddItem(radioBtnGroupData) as RadioButtonGroup;
        //    radioBtnGroup.AddItem(toggleButtonData1);
        //    radioBtnGroup.AddItem(toggleButtonData2);
        //    radioBtnGroup.AddItem(toggleButtonData3);
        //}

        ///// <summary>
        ///// Text box 
        ///// Text box used in conjunction with event. We'll come to this later. 
        ///// For now, just shows how to make a text box. 
        ///// </summary>
        //public void AddTextBox(RibbonPanel panel)
        //{
        //    // Fill the text box information 
        //    TextBoxData txtBoxData = new TextBoxData("TextBox");
        //    txtBoxData.Image = NewBitmapImage("Basics.ico");
        //    txtBoxData.Name = "Text Box";
        //    txtBoxData.ToolTip = "Enter text here";
        //    txtBoxData.LongDescription = "<p>This is Revit UI Labs.</p><p>Ribbon Lab</p>";
        //    txtBoxData.ToolTipImage = NewBitmapImage("ImgHelloWorld.png");

        //    // Create the text box item on the panel 
        //    TextBox txtBox = panel.AddItem(txtBoxData) as TextBox;
        //    txtBox.PromptText = "Enter a comment";
        //    txtBox.ShowImageAsButton = true;

        //    txtBox.EnterPressed += new EventHandler<Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs>(txtBox_EnterPressed);
        //    txtBox.Width = 180;
        //}

        ///// <summary>
        ///// Event handler for the above text box 
        ///// </summary>
        //void txtBox_EnterPressed(object sender, Autodesk.Revit.UI.Events.TextBoxEnterPressedEventArgs e)
        //{
        //    // Cast sender to TextBox to retrieve text value
        //    TextBox textBox = sender as TextBox;
        //    TaskDialog.Show("TextBox Input", "This is what you typed in: " + textBox.Value.ToString());
        //}

        ///// <summary>
        ///// Combo box - 5 items in 2 groups. 
        ///// Combo box is used in conjunction with event. We'll come back later. 
        ///// For now, just demonstrates how to make a combo box. 
        ///// </summary>
        //public void AddComboBox(RibbonPanel panel)
        //{
        //    // Create five combo box members with two groups 

        //    // #1 
        //    ComboBoxMemberData comboBoxMemberData1 = new ComboBoxMemberData("ComboCommandData", "Command Data");
        //    comboBoxMemberData1.Image = NewBitmapImage("Basics.ico");
        //    comboBoxMemberData1.GroupName = "DB Basics";

        //    // #2 
        //    ComboBoxMemberData comboBoxMemberData2 = new ComboBoxMemberData("ComboDbElement", "DB Element");
        //    comboBoxMemberData2.Image = NewBitmapImage("Basics.ico");
        //    comboBoxMemberData2.GroupName = "DB Basics";

        //    // #3 
        //    ComboBoxMemberData comboBoxMemberData3 = new ComboBoxMemberData("ComboElementFiltering", "Filtering");
        //    comboBoxMemberData3.Image = NewBitmapImage("Basics.ico");
        //    comboBoxMemberData3.GroupName = "DB Basics";

        //    // #4 
        //    ComboBoxMemberData comboBoxMemberData4 = new ComboBoxMemberData("ComboElementModification", "Modify");
        //    comboBoxMemberData4.Image = NewBitmapImage("Basics.ico");
        //    comboBoxMemberData4.GroupName = "Modeling";

        //    // #5 
        //    ComboBoxMemberData comboBoxMemberData5 = new ComboBoxMemberData("ComboModelCreation", "Create");
        //    comboBoxMemberData5.Image = NewBitmapImage("Basics.ico");
        //    comboBoxMemberData5.GroupName = "Modeling";

        //    // Make a combo box now 
        //    ComboBoxData comboBxData = new ComboBoxData("ComboBox");
        //    ComboBox comboBx = panel.AddItem(comboBxData) as ComboBox;
        //    comboBx.ToolTip = "Select an Option";
        //    comboBx.LongDescription = "select a command you want to run";
        //    comboBx.AddItem(comboBoxMemberData1);
        //    comboBx.AddItem(comboBoxMemberData2);
        //    comboBx.AddItem(comboBoxMemberData3);
        //    comboBx.AddItem(comboBoxMemberData4);
        //    comboBx.AddItem(comboBoxMemberData5);

        //    comboBx.CurrentChanged += new EventHandler<Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs>(comboBx_CurrentChanged);
        //}

        ///// <summary>
        ///// Event handler for the above combo box 
        ///// </summary>    
        //void comboBx_CurrentChanged(object sender, Autodesk.Revit.UI.Events.ComboBoxCurrentChangedEventArgs e)
        //{
        //    // Cast sender as TextBox to retrieve text value
        //    ComboBox combodata = sender as ComboBox;
        //    ComboBoxMember member = combodata.Current;
        //    TaskDialog.Show("Combobox Selection", "Your new selection: " + member.ItemText);
        //}

        ///// <summary>
        ///// Stacked Buttons - combination of: push button, dropdown button, combo box and text box. 
        ///// (no radio button group, split buttons). 
        ///// Here we stack three push buttons for "Command Data", "DB Element" and "Element Filtering". 
        ///// </summary>
        //public void AddStackedButtons_Simple(RibbonPanel panel)
        //{
        //    // Create three push buttons to stack up 
        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("StackSimpleCommandData", "Command Data", _externalAssemblyPath, _introLabName + ".CommandData");
        //    pushButtonData1.Image = NewBitmapImage("ImgHelloWorldSmall.png");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("StackSimpleDbElement", "DB Element", _externalAssemblyPath, _introLabName + ".DbElement");
        //    pushButtonData2.Image = NewBitmapImage("ImgHelloWorldSmall.png");

        //    // #3 
        //    PushButtonData pushButtonData3 = new PushButtonData("StackSimpleElementFiltering", "Element Filtering", _externalAssemblyPath, _introLabName + ".ElementFiltering");
        //    pushButtonData3.Image = NewBitmapImage("ImgHelloWorldSmall.png");

        //    // Put them on stack 
        //    IList<RibbonItem> stackedButtons = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3);
        //}

        ///// <summary>
        ///// Stacked Buttons - combination of: push button, dropdown button, combo box and text box. 
        ///// (no radio button group, split buttons). 
        ///// Here we define 6 buttons, make grouping of 1, 3, 2 items, and stack them in three layer: 
        ///// (1) simple push button with "Hello World" 
        ///// (2) pull down with 3 items: "Command Data", "DB Element" and "Element Filtering". 
        ///// (3) pull down with 2 items: "Element Modification" and "Model Creation" 
        ///// </summary>
        //public void AddStackedButtons_Complex(RibbonPanel panel)
        //{
        //    // Create six push buttons to group for pull down and stack up 

        //    // #0 
        //    PushButtonData pushButtonData0 = new PushButtonData("StackComplexHelloWorld", "Hello World", _externalAssemblyPath, _introLabName + ".HelloWorld");
        //    pushButtonData0.Image = NewBitmapImage("Basics.ico");

        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("StackComplexCommandData", "Command Data", _externalAssemblyPath, _introLabName + ".CommandData");
        //    pushButtonData1.Image = NewBitmapImage("Basics.ico");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("StackComplexDbElement", "DB Element", _externalAssemblyPath, _introLabName + ".DbElement");

        //    // #3 
        //    PushButtonData pushButtonData3 = new PushButtonData("StackComplexElementFiltering", "Filtering", _externalAssemblyPath, _introLabName + ".ElementFiltering");

        //    // #4 
        //    PushButtonData pushButtonData4 = new PushButtonData("StackComplexElementModification", "Modify", _externalAssemblyPath, _introLabName + ".ElementModification");

        //    // #5 
        //    PushButtonData pushButtonData5 = new PushButtonData("StackComplexModelCreation", "Create", _externalAssemblyPath, _introLabName + ".ModelCreation");

        //    // Make two sets of pull down 

        //    PulldownButtonData pulldownBtnData1 = new PulldownButtonData("StackComplePulldownButton1", "DB Basics");
        //    PulldownButtonData pulldownBtnData2 = new PulldownButtonData("StackComplePulldownButton2", "Modeling");

        //    // Create three item stack. 
        //    IList<RibbonItem> stackedItems = panel.AddStackedItems(pushButtonData0, pulldownBtnData1, pulldownBtnData2);
        //    PulldownButton pulldownBtn2 = stackedItems[1] as PulldownButton;
        //    PulldownButton pulldownBtn3 = stackedItems[2] as PulldownButton;

        //    pulldownBtn2.Image = NewBitmapImage("Basics.ico");
        //    pulldownBtn3.Image = NewBitmapImage("House.ico");

        //    // Add each sub items 
        //    PushButton button1 = pulldownBtn2.AddPushButton(pushButtonData1);
        //    PushButton button2 = pulldownBtn2.AddPushButton(pushButtonData2);
        //    PushButton button3 = pulldownBtn2.AddPushButton(pushButtonData3);
        //    PushButton button4 = pulldownBtn3.AddPushButton(pushButtonData4);
        //    PushButton button5 = pulldownBtn3.AddPushButton(pushButtonData5);

        //    // Note: we need to set the image later. if we do in button data, it won't show in the Ribbon. 
        //    button1.Image = NewBitmapImage("Basics.ico");
        //    button2.Image = NewBitmapImage("Basics.ico");
        //    button3.Image = NewBitmapImage("Basics.ico");
        //    button4.Image = NewBitmapImage("Basics.ico");

        //    button5.Image = NewBitmapImage("Basics.ico");
        //}

        ///// <summary>
        ///// Add buttons for the commands we define in this labs. 
        ///// Here we stack three push buttons and repeat it as we get more. 
        ///// This is a template to use during the Ribbon lab exercise prior to going to following labs. 
        ///// </summary>
        //public void AddUILabsCommandButtons_Template(RibbonPanel panel)
        //{
        //    // Get the location of this dll. 
        //    string assembly = GetType().Assembly.Location;

        //    // Create three push buttons to stack up 
        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("UILabsCommand1", "Command1", assembly, _AssemblyName + ".Command1");
        //    pushButtonData1.Image = NewBitmapImage("ImgHelloWorldSmall.png");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("UILabsCommand2", "Command2", assembly, _AssemblyName + ".Command2");
        //    pushButtonData2.Image = NewBitmapImage("ImgHelloWorldSmall.png");

        //    // #3 
        //    PushButtonData pushButtonData3 = new PushButtonData("UILabsCommand3", "Command3", assembly, _AssemblyName + ".Command3");
        //    pushButtonData3.Image = NewBitmapImage("ImgHelloWorldSmall.png");

        //    // Put them on stack 

        //    IList<RibbonItem> stackedButtons = panel.AddStackedItems(pushButtonData1, pushButtonData2, pushButtonData3);
        //}

        ///// <summary>
        ///// Add buttons for the commands we define in this labs. 
        ///// Here we stack three push buttons and repeat it as we get more. 
        ///// </summary>
        //public void AddUILabsCommandButtons(RibbonPanel panel)
        //{
        //    // Get the location of this dll. 
        //    string assembly = GetType().Assembly.Location;

        //    // Create three push buttons to stack up 
        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("UILabsSelection", "Pick Sampler", assembly, _AssemblyName + ".UISelection");
        //    pushButtonData1.Image = NewBitmapImage("basics.ico");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("UILabsCreateHouse", "Create House Pick", assembly, _AssemblyName + ".UICreateHouse");
        //    pushButtonData2.Image = NewBitmapImage("House.ico");

        //    // #3 
        //    PushButtonData pushButtonData3 = new PushButtonData("UILabsTaskDialog", "Dialog Sampler", assembly, _AssemblyName + ".UITaskDialog");
        //    pushButtonData3.Image = NewBitmapImage("basics.ico");

        //    // #4 
        //    PushButtonData pushButtonData4 = new PushButtonData("UILabsCreateHouseDialog", "Create House Dialog", assembly, _AssemblyName + ".UICreateHouseDialog");
        //    pushButtonData4.Image = NewBitmapImage("House.ico");

        //    // #5 
        //    // Make three sets of pull down 
        //    PulldownButtonData pulldownBtnData1 = new PulldownButtonData("UILabsPulldownButton1", "Selection");
        //    PulldownButtonData pulldownBtnData2 = new PulldownButtonData("UILabsPulldownButton2", "Task Dialog");

        //    // Create three item stack. 
        //    IList<RibbonItem> stackedItems = panel.AddStackedItems(pulldownBtnData1, pulldownBtnData2);
        //    PulldownButton pulldownBtn1 = stackedItems[0] as PulldownButton;
        //    PulldownButton pulldownBtn2 = stackedItems[1] as PulldownButton;

        //    pulldownBtn1.Image = NewBitmapImage("Basics.ico");
        //    pulldownBtn2.Image = NewBitmapImage("Basics.ico");

        //    // Add each sub items 
        //    PushButton button1 = pulldownBtn1.AddPushButton(pushButtonData1);
        //    PushButton button2 = pulldownBtn1.AddPushButton(pushButtonData2);
        //    PushButton button3 = pulldownBtn2.AddPushButton(pushButtonData3);
        //    PushButton button4 = pulldownBtn2.AddPushButton(pushButtonData4);

        //    // Note: we need to set the image later. if we do in button data, it won't show in the Ribbon. 
        //    button1.Image = NewBitmapImage("Basics.ico");
        //    button2.Image = NewBitmapImage("Basics.ico");
        //    button3.Image = NewBitmapImage("Basics.ico");
        //    button4.Image = NewBitmapImage("Basics.ico");
        //}

        ///// <summary>
        ///// Add buttons for the commands we define in this labs. 
        ///// Here we stack 2 x 2-push buttons and repeat it as we get more. 
        ///// TBD: still thinking which version is better ... 
        ///// </summary>
        //public void AddUILabsCommandButtons_v2(RibbonPanel panel)
        //{
        //    // Get the location of this dll. 
        //    string assembly = GetType().Assembly.Location;

        //    // Create push buttons to stack up 
        //    // #1 
        //    PushButtonData pushButtonData1 = new PushButtonData("UILabsSelection", "Pick Sampler", assembly, _AssemblyName + ".UISelection");
        //    pushButtonData1.Image = NewBitmapImage("basics.ico");

        //    // #2 
        //    PushButtonData pushButtonData2 = new PushButtonData("UILabsCreateHouseUI", "Create House Pick", assembly, _AssemblyName + ".CreateHouseUI");
        //    pushButtonData2.Image = NewBitmapImage("basics.ico");

        //    // #3 
        //    PushButtonData pushButtonData3 = new PushButtonData("UILabsTaskDialog", "Dialog Sampler", assembly, _AssemblyName + ".UITaskDialog");
        //    pushButtonData3.Image = NewBitmapImage("basics.ico");

        //    // #4 
        //    PushButtonData pushButtonData4 = new PushButtonData("UILabsCreateHouseDialog", "Create House Dialog", assembly, _AssemblyName + ".CreateHouseDialog");
        //    pushButtonData4.Image = NewBitmapImage("basics.ico");

        //    // Create 2 x 2-item stack. 
        //    IList<RibbonItem> stackedItems1 = panel.AddStackedItems(pushButtonData1, pushButtonData2);

        //    IList<RibbonItem> stackedItems2 = panel.AddStackedItems(pushButtonData3, pushButtonData4);
        //}

        ///// <summary>
        ///// Control buttons for Event and Dynamic Model Update 
        ///// </summary>
        //public void AddUILabsCommandButtons2(RibbonPanel panel)
        //{
        //    // Get the location of this dll. 
        //    string assembly = GetType().Assembly.Location;

        //    // Create three toggle buttons for radio button group 
        //    // #1 
        //    ToggleButtonData toggleButtonData1 = new ToggleButtonData("UILabsEventOn", "Event" + "\n Off", assembly, _AssemblyName + ".UIEventOff");
        //    toggleButtonData1.LargeImage = NewBitmapImage("Basics.ico");

        //    // #2 
        //    ToggleButtonData toggleButtonData2 = new ToggleButtonData("UILabsEventOff", "Event" + "\n On", assembly, _AssemblyName + ".UIEventOn");
        //    toggleButtonData2.LargeImage = NewBitmapImage("Basics.ico");

        //    // Create three toggle buttons for radio button group 
        //    // #3 
        //    ToggleButtonData toggleButtonData3 = new ToggleButtonData("UILabsDynUpdateOn", "Center" + "\n Off", assembly, _AssemblyName + ".UIDynamicModelUpdateOff");
        //    toggleButtonData3.LargeImage = NewBitmapImage("Families.ico");

        //    // #4 
        //    ToggleButtonData toggleButtonData4 = new ToggleButtonData("UILabsDynUpdateOff", "Center" + "\n On", assembly, _AssemblyName + ".UIDynamicModelUpdateOn");
        //    toggleButtonData4.LargeImage = NewBitmapImage("Families.ico");

        //    // Make event pn/off radio button group 
        //    RadioButtonGroupData radioBtnGroupData1 = new RadioButtonGroupData("EventNotification");
        //    RadioButtonGroup radioBtnGroup1 = panel.AddItem(radioBtnGroupData1) as RadioButtonGroup;
        //    radioBtnGroup1.AddItem(toggleButtonData1);
        //    radioBtnGroup1.AddItem(toggleButtonData2);

        //    // Make dyn update on/off radio button group 
        //    RadioButtonGroupData radioBtnGroupData2 = new RadioButtonGroupData("WindowDoorCenter");
        //    RadioButtonGroup radioBtnGroup2 = panel.AddItem(radioBtnGroupData2) as RadioButtonGroup;
        //    radioBtnGroup2.AddItem(toggleButtonData3);

        //    radioBtnGroup2.AddItem(toggleButtonData4);
        //}
    }

    #region Helper Classes
    /// <summary>
    /// This lab uses Revit Intro Labs. 
    /// If you prefer to use a dummy command instead, you can do so. 
    /// Providing a command template here. 
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class DummyCommand1 : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            // Write your command implementation here 

            TaskDialog.Show("Dummy command", "You have called Command1");

            return Result.Succeeded;
        }
    }
    #endregion
}
