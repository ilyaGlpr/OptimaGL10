using System.ComponentModel;
using System.Windows.Forms;

namespace OptimaGL
{
    sealed partial class MainForm
    {

        private IContainer components = null;
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabPageMain = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.LogOutputTextBox = new System.Windows.Forms.RichTextBox();
            this.StatusCommandsLable = new System.Windows.Forms.Label();
            this.ProgressBarStatus = new System.Windows.Forms.ProgressBar();
            this.btnDestroyWindowsSpying = new System.Windows.Forms.Button();
            this.FormTabsControl = new System.Windows.Forms.TabControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.ProgressBarStatus1 = new System.Windows.Forms.ProgressBar();
            this.Win10SettingsPanel = new System.Windows.Forms.Panel();
            this.disable_fonapps_button = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.delete_edge_button = new System.Windows.Forms.Button();
            this.delete_hiber_button = new System.Windows.Forms.Button();
            this.btnDeleteOneDrive = new System.Windows.Forms.Button();
            this.create_compactos_button = new System.Windows.Forms.Button();
            this.delete_trash_button = new System.Windows.Forms.Button();
            this.startup_button = new System.Windows.Forms.Button();
            this.groupBoxWindowsUpdate = new System.Windows.Forms.GroupBox();
            this.btnEnableWindowsUpdate = new System.Windows.Forms.Button();
            this.btnDisableWindowsUpdate = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxCreateSystemRestorePoint = new System.Windows.Forms.CheckBox();
            this.checkBoxKeyLoggerAndTelemetry = new System.Windows.Forms.CheckBox();
            this.checkBoxSPYTasks = new System.Windows.Forms.CheckBox();
            this.checkBoxDisableWindowsDefender = new System.Windows.Forms.CheckBox();
            this.checkBoxDisablePrivateSettings = new System.Windows.Forms.CheckBox();
            this.checkBoxAddToHosts = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxDeleteWindows10Apps = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteApp3d = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteMailCalendarMaps = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteAppBing = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteAppXBOX = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteAppZune = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteAppPeopleOneNote = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteAppPhone = new System.Windows.Forms.CheckBox();
            this.checkBoxDeleteAppSolit = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.delete_hyperv_button = new System.Windows.Forms.Button();
            this.enable_hyperv_button = new System.Windows.Forms.Button();
            this.disable_hyperv_button = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkedListBoxUpdatesW78 = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPageMain.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.FormTabsControl.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.Win10SettingsPanel.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBoxWindowsUpdate.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageMain
            // 
            this.tabPageMain.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(this.tabPageMain, "tabPageMain");
            this.tabPageMain.Controls.Add(this.groupBox3);
            this.tabPageMain.Controls.Add(this.StatusCommandsLable);
            this.tabPageMain.Controls.Add(this.ProgressBarStatus);
            this.tabPageMain.Controls.Add(this.btnDestroyWindowsSpying);
            this.tabPageMain.Name = "tabPageMain";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.LogOutputTextBox);
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            this.groupBox3.Enter += new System.EventHandler(this.groupBox3_Enter);
            // 
            // LogOutputTextBox
            // 
            this.LogOutputTextBox.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.LogOutputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.LogOutputTextBox, "LogOutputTextBox");
            this.LogOutputTextBox.ForeColor = System.Drawing.Color.DarkBlue;
            this.LogOutputTextBox.Name = "LogOutputTextBox";
            this.LogOutputTextBox.ReadOnly = true;
            this.LogOutputTextBox.TextChanged += new System.EventHandler(this.LogOutputTextBox_TextChanged);
            // 
            // StatusCommandsLable
            // 
            this.StatusCommandsLable.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.StatusCommandsLable, "StatusCommandsLable");
            this.StatusCommandsLable.Name = "StatusCommandsLable";
            this.StatusCommandsLable.Click += new System.EventHandler(this.StatusCommandsLable_Click);
            // 
            // ProgressBarStatus
            // 
            this.ProgressBarStatus.BackColor = System.Drawing.SystemColors.Control;
            resources.ApplyResources(this.ProgressBarStatus, "ProgressBarStatus");
            this.ProgressBarStatus.Name = "ProgressBarStatus";
            this.ProgressBarStatus.Click += new System.EventHandler(this.ProgressBarStatus_Click);
            // 
            // btnDestroyWindowsSpying
            // 
            this.btnDestroyWindowsSpying.BackColor = System.Drawing.Color.LightBlue;
            this.btnDestroyWindowsSpying.Cursor = System.Windows.Forms.Cursors.Arrow;
            resources.ApplyResources(this.btnDestroyWindowsSpying, "btnDestroyWindowsSpying");
            this.btnDestroyWindowsSpying.Name = "btnDestroyWindowsSpying";
            this.btnDestroyWindowsSpying.UseVisualStyleBackColor = false;
            this.btnDestroyWindowsSpying.Click += new System.EventHandler(this.btnDestroyWindowsSpying_Click);
            // 
            // FormTabsControl
            // 
            this.FormTabsControl.Controls.Add(this.tabPageMain);
            this.FormTabsControl.Controls.Add(this.tabPageSettings);
            this.FormTabsControl.Controls.Add(this.tabPage1);
            resources.ApplyResources(this.FormTabsControl, "FormTabsControl");
            this.FormTabsControl.Name = "FormTabsControl";
            this.FormTabsControl.SelectedIndex = 0;
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.BackColor = System.Drawing.SystemColors.ActiveCaption;
            resources.ApplyResources(this.tabPageSettings, "tabPageSettings");
            this.tabPageSettings.Controls.Add(this.ProgressBarStatus1);
            this.tabPageSettings.Controls.Add(this.Win10SettingsPanel);
            this.tabPageSettings.Name = "tabPageSettings";
            // 
            // ProgressBarStatus1
            // 
            resources.ApplyResources(this.ProgressBarStatus1, "ProgressBarStatus1");
            this.ProgressBarStatus1.Name = "ProgressBarStatus1";
            // 
            // Win10SettingsPanel
            // 
            this.Win10SettingsPanel.BackColor = System.Drawing.Color.Transparent;
            this.Win10SettingsPanel.Controls.Add(this.disable_fonapps_button);
            this.Win10SettingsPanel.Controls.Add(this.groupBox4);
            this.Win10SettingsPanel.Controls.Add(this.startup_button);
            this.Win10SettingsPanel.Controls.Add(this.groupBoxWindowsUpdate);
            this.Win10SettingsPanel.Controls.Add(this.groupBox2);
            this.Win10SettingsPanel.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.Win10SettingsPanel, "Win10SettingsPanel");
            this.Win10SettingsPanel.Name = "Win10SettingsPanel";
            // 
            // disable_fonapps_button
            // 
            this.disable_fonapps_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.disable_fonapps_button, "disable_fonapps_button");
            this.disable_fonapps_button.ForeColor = System.Drawing.Color.Black;
            this.disable_fonapps_button.Name = "disable_fonapps_button";
            this.disable_fonapps_button.UseVisualStyleBackColor = false;
            this.disable_fonapps_button.Click += new System.EventHandler(this.disable_fonapps_button_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.delete_edge_button);
            this.groupBox4.Controls.Add(this.delete_hiber_button);
            this.groupBox4.Controls.Add(this.btnDeleteOneDrive);
            this.groupBox4.Controls.Add(this.create_compactos_button);
            this.groupBox4.Controls.Add(this.delete_trash_button);
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // delete_edge_button
            // 
            this.delete_edge_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.delete_edge_button, "delete_edge_button");
            this.delete_edge_button.ForeColor = System.Drawing.Color.Black;
            this.delete_edge_button.Name = "delete_edge_button";
            this.delete_edge_button.UseVisualStyleBackColor = false;
            this.delete_edge_button.Click += new System.EventHandler(this.delete_edge_button_Click);
            // 
            // delete_hiber_button
            // 
            this.delete_hiber_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.delete_hiber_button, "delete_hiber_button");
            this.delete_hiber_button.ForeColor = System.Drawing.Color.Black;
            this.delete_hiber_button.Name = "delete_hiber_button";
            this.delete_hiber_button.UseVisualStyleBackColor = false;
            this.delete_hiber_button.Click += new System.EventHandler(this.delete_hiber_button_Click);
            // 
            // btnDeleteOneDrive
            // 
            this.btnDeleteOneDrive.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnDeleteOneDrive, "btnDeleteOneDrive");
            this.btnDeleteOneDrive.ForeColor = System.Drawing.Color.Black;
            this.btnDeleteOneDrive.Name = "btnDeleteOneDrive";
            this.btnDeleteOneDrive.UseVisualStyleBackColor = false;
            this.btnDeleteOneDrive.Click += new System.EventHandler(this.btnDeleteOneDrive_Click);
            // 
            // create_compactos_button
            // 
            this.create_compactos_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.create_compactos_button, "create_compactos_button");
            this.create_compactos_button.ForeColor = System.Drawing.Color.Black;
            this.create_compactos_button.Name = "create_compactos_button";
            this.create_compactos_button.UseVisualStyleBackColor = false;
            this.create_compactos_button.Click += new System.EventHandler(this.create_compactos_button_Click);
            // 
            // delete_trash_button
            // 
            this.delete_trash_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.delete_trash_button, "delete_trash_button");
            this.delete_trash_button.ForeColor = System.Drawing.Color.Black;
            this.delete_trash_button.Name = "delete_trash_button";
            this.delete_trash_button.UseVisualStyleBackColor = false;
            this.delete_trash_button.Click += new System.EventHandler(this.delete_trash_button_Click);
            // 
            // startup_button
            // 
            this.startup_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.startup_button, "startup_button");
            this.startup_button.ForeColor = System.Drawing.Color.Black;
            this.startup_button.Name = "startup_button";
            this.startup_button.UseVisualStyleBackColor = false;
            this.startup_button.Click += new System.EventHandler(this.startup_button_Click);
            // 
            // groupBoxWindowsUpdate
            // 
            this.groupBoxWindowsUpdate.BackColor = System.Drawing.Color.Transparent;
            this.groupBoxWindowsUpdate.Controls.Add(this.btnEnableWindowsUpdate);
            this.groupBoxWindowsUpdate.Controls.Add(this.btnDisableWindowsUpdate);
            resources.ApplyResources(this.groupBoxWindowsUpdate, "groupBoxWindowsUpdate");
            this.groupBoxWindowsUpdate.Name = "groupBoxWindowsUpdate";
            this.groupBoxWindowsUpdate.TabStop = false;
            // 
            // btnEnableWindowsUpdate
            // 
            this.btnEnableWindowsUpdate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnEnableWindowsUpdate, "btnEnableWindowsUpdate");
            this.btnEnableWindowsUpdate.Name = "btnEnableWindowsUpdate";
            this.btnEnableWindowsUpdate.UseVisualStyleBackColor = false;
            this.btnEnableWindowsUpdate.Click += new System.EventHandler(this.btnEnableWindowsUpdate_Click);
            // 
            // btnDisableWindowsUpdate
            // 
            this.btnDisableWindowsUpdate.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.btnDisableWindowsUpdate, "btnDisableWindowsUpdate");
            this.btnDisableWindowsUpdate.Name = "btnDisableWindowsUpdate";
            this.btnDisableWindowsUpdate.UseVisualStyleBackColor = false;
            this.btnDisableWindowsUpdate.Click += new System.EventHandler(this.btnDisableWindowsUpdate_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxCreateSystemRestorePoint);
            this.groupBox2.Controls.Add(this.checkBoxKeyLoggerAndTelemetry);
            this.groupBox2.Controls.Add(this.checkBoxSPYTasks);
            this.groupBox2.Controls.Add(this.checkBoxDisableWindowsDefender);
            this.groupBox2.Controls.Add(this.checkBoxDisablePrivateSettings);
            this.groupBox2.Controls.Add(this.checkBoxAddToHosts);
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // checkBoxCreateSystemRestorePoint
            // 
            resources.ApplyResources(this.checkBoxCreateSystemRestorePoint, "checkBoxCreateSystemRestorePoint");
            this.checkBoxCreateSystemRestorePoint.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCreateSystemRestorePoint.Checked = true;
            this.checkBoxCreateSystemRestorePoint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCreateSystemRestorePoint.Name = "checkBoxCreateSystemRestorePoint";
            this.checkBoxCreateSystemRestorePoint.UseVisualStyleBackColor = false;
            // 
            // checkBoxKeyLoggerAndTelemetry
            // 
            resources.ApplyResources(this.checkBoxKeyLoggerAndTelemetry, "checkBoxKeyLoggerAndTelemetry");
            this.checkBoxKeyLoggerAndTelemetry.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxKeyLoggerAndTelemetry.Checked = true;
            this.checkBoxKeyLoggerAndTelemetry.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxKeyLoggerAndTelemetry.Name = "checkBoxKeyLoggerAndTelemetry";
            this.checkBoxKeyLoggerAndTelemetry.UseVisualStyleBackColor = false;
            // 
            // checkBoxSPYTasks
            // 
            resources.ApplyResources(this.checkBoxSPYTasks, "checkBoxSPYTasks");
            this.checkBoxSPYTasks.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxSPYTasks.Checked = true;
            this.checkBoxSPYTasks.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSPYTasks.Name = "checkBoxSPYTasks";
            this.checkBoxSPYTasks.UseVisualStyleBackColor = false;
            // 
            // checkBoxDisableWindowsDefender
            // 
            resources.ApplyResources(this.checkBoxDisableWindowsDefender, "checkBoxDisableWindowsDefender");
            this.checkBoxDisableWindowsDefender.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDisableWindowsDefender.Checked = true;
            this.checkBoxDisableWindowsDefender.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDisableWindowsDefender.Name = "checkBoxDisableWindowsDefender";
            this.checkBoxDisableWindowsDefender.UseVisualStyleBackColor = false;
            // 
            // checkBoxDisablePrivateSettings
            // 
            resources.ApplyResources(this.checkBoxDisablePrivateSettings, "checkBoxDisablePrivateSettings");
            this.checkBoxDisablePrivateSettings.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDisablePrivateSettings.Checked = true;
            this.checkBoxDisablePrivateSettings.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDisablePrivateSettings.Name = "checkBoxDisablePrivateSettings";
            this.checkBoxDisablePrivateSettings.ThreeState = true;
            this.checkBoxDisablePrivateSettings.UseVisualStyleBackColor = false;
            // 
            // checkBoxAddToHosts
            // 
            resources.ApplyResources(this.checkBoxAddToHosts, "checkBoxAddToHosts");
            this.checkBoxAddToHosts.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxAddToHosts.Checked = true;
            this.checkBoxAddToHosts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAddToHosts.Name = "checkBoxAddToHosts";
            this.checkBoxAddToHosts.UseVisualStyleBackColor = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxDeleteWindows10Apps);
            this.groupBox1.Controls.Add(this.checkBoxDeleteApp3d);
            this.groupBox1.Controls.Add(this.checkBoxDeleteMailCalendarMaps);
            this.groupBox1.Controls.Add(this.checkBoxDeleteAppBing);
            this.groupBox1.Controls.Add(this.checkBoxDeleteAppXBOX);
            this.groupBox1.Controls.Add(this.checkBoxDeleteAppZune);
            this.groupBox1.Controls.Add(this.checkBoxDeleteAppPeopleOneNote);
            this.groupBox1.Controls.Add(this.checkBoxDeleteAppPhone);
            this.groupBox1.Controls.Add(this.checkBoxDeleteAppSolit);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // checkBoxDeleteWindows10Apps
            // 
            resources.ApplyResources(this.checkBoxDeleteWindows10Apps, "checkBoxDeleteWindows10Apps");
            this.checkBoxDeleteWindows10Apps.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteWindows10Apps.Checked = true;
            this.checkBoxDeleteWindows10Apps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteWindows10Apps.Name = "checkBoxDeleteWindows10Apps";
            this.checkBoxDeleteWindows10Apps.UseVisualStyleBackColor = false;
            this.checkBoxDeleteWindows10Apps.CheckedChanged += new System.EventHandler(this.checkBoxDeleteWindows10Apps_CheckedChanged);
            // 
            // checkBoxDeleteApp3d
            // 
            resources.ApplyResources(this.checkBoxDeleteApp3d, "checkBoxDeleteApp3d");
            this.checkBoxDeleteApp3d.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteApp3d.Checked = true;
            this.checkBoxDeleteApp3d.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteApp3d.Name = "checkBoxDeleteApp3d";
            this.checkBoxDeleteApp3d.UseVisualStyleBackColor = false;
            // 
            // checkBoxDeleteMailCalendarMaps
            // 
            resources.ApplyResources(this.checkBoxDeleteMailCalendarMaps, "checkBoxDeleteMailCalendarMaps");
            this.checkBoxDeleteMailCalendarMaps.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteMailCalendarMaps.Checked = true;
            this.checkBoxDeleteMailCalendarMaps.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteMailCalendarMaps.Name = "checkBoxDeleteMailCalendarMaps";
            this.checkBoxDeleteMailCalendarMaps.UseVisualStyleBackColor = false;
            // 
            // checkBoxDeleteAppBing
            // 
            resources.ApplyResources(this.checkBoxDeleteAppBing, "checkBoxDeleteAppBing");
            this.checkBoxDeleteAppBing.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteAppBing.Checked = true;
            this.checkBoxDeleteAppBing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteAppBing.Name = "checkBoxDeleteAppBing";
            this.checkBoxDeleteAppBing.UseVisualStyleBackColor = false;
            // 
            // checkBoxDeleteAppXBOX
            // 
            resources.ApplyResources(this.checkBoxDeleteAppXBOX, "checkBoxDeleteAppXBOX");
            this.checkBoxDeleteAppXBOX.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteAppXBOX.Checked = true;
            this.checkBoxDeleteAppXBOX.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteAppXBOX.Name = "checkBoxDeleteAppXBOX";
            this.checkBoxDeleteAppXBOX.UseVisualStyleBackColor = false;
            this.checkBoxDeleteAppXBOX.CheckedChanged += new System.EventHandler(this.checkBoxDeleteAppXBOX_CheckedChanged);
            // 
            // checkBoxDeleteAppZune
            // 
            resources.ApplyResources(this.checkBoxDeleteAppZune, "checkBoxDeleteAppZune");
            this.checkBoxDeleteAppZune.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteAppZune.Checked = true;
            this.checkBoxDeleteAppZune.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteAppZune.Name = "checkBoxDeleteAppZune";
            this.checkBoxDeleteAppZune.UseVisualStyleBackColor = false;
            // 
            // checkBoxDeleteAppPeopleOneNote
            // 
            resources.ApplyResources(this.checkBoxDeleteAppPeopleOneNote, "checkBoxDeleteAppPeopleOneNote");
            this.checkBoxDeleteAppPeopleOneNote.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteAppPeopleOneNote.Checked = true;
            this.checkBoxDeleteAppPeopleOneNote.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteAppPeopleOneNote.Name = "checkBoxDeleteAppPeopleOneNote";
            this.checkBoxDeleteAppPeopleOneNote.UseVisualStyleBackColor = false;
            // 
            // checkBoxDeleteAppPhone
            // 
            resources.ApplyResources(this.checkBoxDeleteAppPhone, "checkBoxDeleteAppPhone");
            this.checkBoxDeleteAppPhone.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteAppPhone.Checked = true;
            this.checkBoxDeleteAppPhone.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteAppPhone.Name = "checkBoxDeleteAppPhone";
            this.checkBoxDeleteAppPhone.UseVisualStyleBackColor = false;
            // 
            // checkBoxDeleteAppSolit
            // 
            resources.ApplyResources(this.checkBoxDeleteAppSolit, "checkBoxDeleteAppSolit");
            this.checkBoxDeleteAppSolit.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxDeleteAppSolit.Checked = true;
            this.checkBoxDeleteAppSolit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeleteAppSolit.Name = "checkBoxDeleteAppSolit";
            this.checkBoxDeleteAppSolit.UseVisualStyleBackColor = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.progressBar1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.delete_hyperv_button);
            this.groupBox5.Controls.Add(this.enable_hyperv_button);
            this.groupBox5.Controls.Add(this.disable_hyperv_button);
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // delete_hyperv_button
            // 
            this.delete_hyperv_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.delete_hyperv_button, "delete_hyperv_button");
            this.delete_hyperv_button.ForeColor = System.Drawing.Color.Black;
            this.delete_hyperv_button.Name = "delete_hyperv_button";
            this.delete_hyperv_button.UseVisualStyleBackColor = false;
            this.delete_hyperv_button.Click += new System.EventHandler(this.delete_hyperv_button_Click);
            // 
            // enable_hyperv_button
            // 
            this.enable_hyperv_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.enable_hyperv_button, "enable_hyperv_button");
            this.enable_hyperv_button.ForeColor = System.Drawing.Color.Black;
            this.enable_hyperv_button.Name = "enable_hyperv_button";
            this.enable_hyperv_button.UseVisualStyleBackColor = false;
            this.enable_hyperv_button.Click += new System.EventHandler(this.enable_hyperv_button_Click);
            // 
            // disable_hyperv_button
            // 
            this.disable_hyperv_button.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.disable_hyperv_button, "disable_hyperv_button");
            this.disable_hyperv_button.ForeColor = System.Drawing.Color.Black;
            this.disable_hyperv_button.Name = "disable_hyperv_button";
            this.disable_hyperv_button.UseVisualStyleBackColor = false;
            this.disable_hyperv_button.Click += new System.EventHandler(this.disable_hyperv_button_Click);
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            // 
            // checkedListBoxUpdatesW78
            // 
            resources.ApplyResources(this.checkedListBoxUpdatesW78, "checkedListBoxUpdatesW78");
            this.checkedListBoxUpdatesW78.Name = "checkedListBoxUpdatesW78";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.button1, "button1");
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.FormTabsControl);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DestroyWindowsSpyingMainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DestroyWindowsSpyingMainForm_FormClosed);
            this.tabPageMain.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.FormTabsControl.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.Win10SettingsPanel.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBoxWindowsUpdate.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TabPage tabPageMain;
        private RichTextBox LogOutputTextBox;
        private Label StatusCommandsLable;
        private ProgressBar ProgressBarStatus;
        private Button btnDestroyWindowsSpying;
        private TabControl FormTabsControl;
        private TabPage tabPageSettings;
        private Panel Win10SettingsPanel;
        private CheckedListBox checkedListBoxUpdatesW78;
        private CheckBox checkBoxCreateSystemRestorePoint;
        private CheckBox checkBoxKeyLoggerAndTelemetry;
        private CheckBox checkBoxAddToHosts;
        private CheckBox checkBoxDeleteAppXBOX;
        private CheckBox checkBoxDisablePrivateSettings;
        private CheckBox checkBoxDisableWindowsDefender;
        private CheckBox checkBoxDeleteAppSolit;
        private CheckBox checkBoxSPYTasks;
        private CheckBox checkBoxDeleteAppPhone;
        private CheckBox checkBoxDeleteWindows10Apps;
        private CheckBox checkBoxDeleteAppPeopleOneNote;
        private CheckBox checkBoxDeleteApp3d;
        private CheckBox checkBoxDeleteAppZune;
        private CheckBox checkBoxDeleteAppBing;
        private CheckBox checkBoxDeleteMailCalendarMaps;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBoxWindowsUpdate;
        private Button btnEnableWindowsUpdate;
        private Button btnDisableWindowsUpdate;
        private Button btnDeleteOneDrive;
        private Button delete_trash_button;
        private Button startup_button;
        private Button create_compactos_button;
        private Button delete_hiber_button;
        private GroupBox groupBox4;
        private Button delete_edge_button;
        private Button disable_fonapps_button;
        private ProgressBar ProgressBarStatus1;
        private TabPage tabPage1;
        private Button button1;
        private Button disable_hyperv_button;
        private ProgressBar progressBar1;
        private GroupBox groupBox5;
        private Button delete_hyperv_button;
        private Button enable_hyperv_button;
    }
}

