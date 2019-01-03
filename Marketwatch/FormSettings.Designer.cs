namespace Marketwatch
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.settingTextBox1 = new System.Windows.Forms.TextBox();
            this.settingLabel2 = new System.Windows.Forms.Label();
            this.maxRequestTrackBar = new System.Windows.Forms.TrackBar();
            this.currencySelector = new System.Windows.Forms.ComboBox();
            this.settingLabel1 = new System.Windows.Forms.Label();
            this.groupBoxGeneral = new System.Windows.Forms.GroupBox();
            this.updateCheckBox = new System.Windows.Forms.CheckBox();
            this.languageSelector = new System.Windows.Forms.ComboBox();
            this.settingLabel3 = new System.Windows.Forms.Label();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.settingTextBox2 = new System.Windows.Forms.TextBox();
            this.msRequestTrackBar = new System.Windows.Forms.TrackBar();
            this.settingLabel4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.maxRequestTrackBar)).BeginInit();
            this.groupBoxGeneral.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msRequestTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // settingTextBox1
            // 
            resources.ApplyResources(this.settingTextBox1, "settingTextBox1");
            this.settingTextBox1.Name = "settingTextBox1";
            // 
            // settingLabel2
            // 
            resources.ApplyResources(this.settingLabel2, "settingLabel2");
            this.settingLabel2.BackColor = System.Drawing.Color.Transparent;
            this.settingLabel2.Name = "settingLabel2";
            // 
            // maxRequestTrackBar
            // 
            resources.ApplyResources(this.maxRequestTrackBar, "maxRequestTrackBar");
            this.maxRequestTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.maxRequestTrackBar.Maximum = 21;
            this.maxRequestTrackBar.Minimum = 1;
            this.maxRequestTrackBar.Name = "maxRequestTrackBar";
            this.maxRequestTrackBar.TickFrequency = 2;
            this.maxRequestTrackBar.Value = 10;
            this.maxRequestTrackBar.Scroll += new System.EventHandler(this.maxRequestTrackBar_Scroll);
            // 
            // currencySelector
            // 
            resources.ApplyResources(this.currencySelector, "currencySelector");
            this.currencySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.currencySelector.FormattingEnabled = true;
            this.currencySelector.Name = "currencySelector";
            // 
            // settingLabel1
            // 
            resources.ApplyResources(this.settingLabel1, "settingLabel1");
            this.settingLabel1.Name = "settingLabel1";
            // 
            // groupBoxGeneral
            // 
            resources.ApplyResources(this.groupBoxGeneral, "groupBoxGeneral");
            this.groupBoxGeneral.Controls.Add(this.updateCheckBox);
            this.groupBoxGeneral.Controls.Add(this.languageSelector);
            this.groupBoxGeneral.Controls.Add(this.settingLabel3);
            this.groupBoxGeneral.Controls.Add(this.settingLabel1);
            this.groupBoxGeneral.Controls.Add(this.currencySelector);
            this.groupBoxGeneral.Name = "groupBoxGeneral";
            this.groupBoxGeneral.TabStop = false;
            // 
            // updateCheckBox
            // 
            resources.ApplyResources(this.updateCheckBox, "updateCheckBox");
            this.updateCheckBox.BackColor = System.Drawing.SystemColors.Control;
            this.updateCheckBox.Name = "updateCheckBox";
            this.updateCheckBox.UseVisualStyleBackColor = false;
            // 
            // languageSelector
            // 
            resources.ApplyResources(this.languageSelector, "languageSelector");
            this.languageSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.languageSelector.FormattingEnabled = true;
            this.languageSelector.Items.AddRange(new object[] {
            resources.GetString("languageSelector.Items"),
            resources.GetString("languageSelector.Items1")});
            this.languageSelector.Name = "languageSelector";
            this.languageSelector.SelectionChangeCommitted += new System.EventHandler(this.languageSelector_SelectionChangeCommitted);
            // 
            // settingLabel3
            // 
            resources.ApplyResources(this.settingLabel3, "settingLabel3");
            this.settingLabel3.Name = "settingLabel3";
            // 
            // buttonAccept
            // 
            resources.ApplyResources(this.buttonAccept, "buttonAccept");
            this.buttonAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonCancel
            // 
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.settingTextBox2);
            this.groupBox1.Controls.Add(this.msRequestTrackBar);
            this.groupBox1.Controls.Add(this.settingLabel4);
            this.groupBox1.Controls.Add(this.settingLabel2);
            this.groupBox1.Controls.Add(this.settingTextBox1);
            this.groupBox1.Controls.Add(this.maxRequestTrackBar);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // settingTextBox2
            // 
            resources.ApplyResources(this.settingTextBox2, "settingTextBox2");
            this.settingTextBox2.Name = "settingTextBox2";
            // 
            // msRequestTrackBar
            // 
            resources.ApplyResources(this.msRequestTrackBar, "msRequestTrackBar");
            this.msRequestTrackBar.BackColor = System.Drawing.SystemColors.Control;
            this.msRequestTrackBar.LargeChange = 25;
            this.msRequestTrackBar.Maximum = 1000;
            this.msRequestTrackBar.Name = "msRequestTrackBar";
            this.msRequestTrackBar.TickFrequency = 100;
            this.msRequestTrackBar.Value = 100;
            this.msRequestTrackBar.Scroll += new System.EventHandler(this.msRequestTrackBar_Scroll);
            // 
            // settingLabel4
            // 
            resources.ApplyResources(this.settingLabel4, "settingLabel4");
            this.settingLabel4.BackColor = System.Drawing.Color.Transparent;
            this.settingLabel4.Name = "settingLabel4";
            // 
            // FormSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.groupBoxGeneral);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            ((System.ComponentModel.ISupportInitialize)(this.maxRequestTrackBar)).EndInit();
            this.groupBoxGeneral.ResumeLayout(false);
            this.groupBoxGeneral.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.msRequestTrackBar)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox settingTextBox1;
        private System.Windows.Forms.Label settingLabel2;
        private System.Windows.Forms.TrackBar maxRequestTrackBar;
        private System.Windows.Forms.ComboBox currencySelector;
        private System.Windows.Forms.Label settingLabel1;
        private System.Windows.Forms.GroupBox groupBoxGeneral;
        private System.Windows.Forms.ComboBox languageSelector;
        private System.Windows.Forms.Label settingLabel3;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox updateCheckBox;
        private System.Windows.Forms.TextBox settingTextBox2;
        private System.Windows.Forms.TrackBar msRequestTrackBar;
        private System.Windows.Forms.Label settingLabel4;
    }
}