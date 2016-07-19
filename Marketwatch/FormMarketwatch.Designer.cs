namespace Marketwatch
{
    partial class FormMarketwatch
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMarketwatch));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.itemPreviewColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.itemNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.floatColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.accImageColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.priceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.inspectButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.buyButtonColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.inspectLinkColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.buyLinkColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.searchBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.clearButton = new System.Windows.Forms.Button();
            this.labelTotal = new System.Windows.Forms.Label();
            this.labelCurrent = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.walletAmount = new System.Windows.Forms.Label();
            this.labelSeparator = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.accToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.beendenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extrasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hilfeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officialGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.releaseNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.infoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.donateButton = new System.Windows.Forms.Button();
            this.settingsButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.accLabel = new System.Windows.Forms.Label();
            this.accFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.accFlowLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dataGridView, "dataGridView");
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemPreviewColumn,
            this.itemNameColumn,
            this.floatColumn,
            this.accImageColumn,
            this.priceColumn,
            this.inspectButtonColumn,
            this.buyButtonColumn,
            this.inspectLinkColumn,
            this.buyLinkColumn});
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 32;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // itemPreviewColumn
            // 
            resources.ApplyResources(this.itemPreviewColumn, "itemPreviewColumn");
            this.itemPreviewColumn.Name = "itemPreviewColumn";
            // 
            // itemNameColumn
            // 
            this.itemNameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.itemNameColumn, "itemNameColumn");
            this.itemNameColumn.Name = "itemNameColumn";
            this.itemNameColumn.ReadOnly = true;
            // 
            // floatColumn
            // 
            resources.ApplyResources(this.floatColumn, "floatColumn");
            this.floatColumn.Name = "floatColumn";
            this.floatColumn.ReadOnly = true;
            // 
            // accImageColumn
            // 
            resources.ApplyResources(this.accImageColumn, "accImageColumn");
            this.accImageColumn.Name = "accImageColumn";
            // 
            // priceColumn
            // 
            resources.ApplyResources(this.priceColumn, "priceColumn");
            this.priceColumn.Name = "priceColumn";
            this.priceColumn.ReadOnly = true;
            // 
            // inspectButtonColumn
            // 
            resources.ApplyResources(this.inspectButtonColumn, "inspectButtonColumn");
            this.inspectButtonColumn.Name = "inspectButtonColumn";
            this.inspectButtonColumn.Text = "Inspect";
            // 
            // buyButtonColumn
            // 
            resources.ApplyResources(this.buyButtonColumn, "buyButtonColumn");
            this.buyButtonColumn.Name = "buyButtonColumn";
            // 
            // inspectLinkColumn
            // 
            resources.ApplyResources(this.inspectLinkColumn, "inspectLinkColumn");
            this.inspectLinkColumn.Name = "inspectLinkColumn";
            // 
            // buyLinkColumn
            // 
            resources.ApplyResources(this.buyLinkColumn, "buyLinkColumn");
            this.buyLinkColumn.Name = "buyLinkColumn";
            // 
            // searchBox
            // 
            resources.ApplyResources(this.searchBox, "searchBox");
            this.searchBox.Name = "searchBox";
            // 
            // searchButton
            // 
            resources.ApplyResources(this.searchButton, "searchButton");
            this.searchButton.Name = "searchButton";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.EnabledChanged += new System.EventHandler(this.searchButton_EnabledChanged);
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // clearButton
            // 
            resources.ApplyResources(this.clearButton, "clearButton");
            this.clearButton.Name = "clearButton";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // labelTotal
            // 
            resources.ApplyResources(this.labelTotal, "labelTotal");
            this.labelTotal.Name = "labelTotal";
            // 
            // labelCurrent
            // 
            resources.ApplyResources(this.labelCurrent, "labelCurrent");
            this.labelCurrent.Name = "labelCurrent";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // walletAmount
            // 
            resources.ApplyResources(this.walletAmount, "walletAmount");
            this.walletAmount.Name = "walletAmount";
            // 
            // labelSeparator
            // 
            resources.ApplyResources(this.labelSeparator, "labelSeparator");
            this.labelSeparator.Name = "labelSeparator";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.menuStrip, "menuStrip");
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.accToolStripMenuItem,
            this.extrasToolStripMenuItem,
            this.hilfeToolStripMenuItem});
            this.menuStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // accToolStripMenuItem
            // 
            this.accToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.logoutToolStripMenuItem,
            this.toolStripSeparator1,
            this.beendenToolStripMenuItem});
            this.accToolStripMenuItem.Name = "accToolStripMenuItem";
            resources.ApplyResources(this.accToolStripMenuItem, "accToolStripMenuItem");
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            resources.ApplyResources(this.loginToolStripMenuItem, "loginToolStripMenuItem");
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // logoutToolStripMenuItem
            // 
            resources.ApplyResources(this.logoutToolStripMenuItem, "logoutToolStripMenuItem");
            this.logoutToolStripMenuItem.Name = "logoutToolStripMenuItem";
            this.logoutToolStripMenuItem.Click += new System.EventHandler(this.logoutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // beendenToolStripMenuItem
            // 
            resources.ApplyResources(this.beendenToolStripMenuItem, "beendenToolStripMenuItem");
            this.beendenToolStripMenuItem.Name = "beendenToolStripMenuItem";
            this.beendenToolStripMenuItem.Click += new System.EventHandler(this.beendenToolStripMenuItem_Click);
            // 
            // extrasToolStripMenuItem
            // 
            this.extrasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionenToolStripMenuItem});
            this.extrasToolStripMenuItem.Name = "extrasToolStripMenuItem";
            resources.ApplyResources(this.extrasToolStripMenuItem, "extrasToolStripMenuItem");
            // 
            // optionenToolStripMenuItem
            // 
            this.optionenToolStripMenuItem.Image = global::Marketwatch.Properties.Resources.Gear;
            this.optionenToolStripMenuItem.Name = "optionenToolStripMenuItem";
            resources.ApplyResources(this.optionenToolStripMenuItem, "optionenToolStripMenuItem");
            this.optionenToolStripMenuItem.Click += new System.EventHandler(this.optionenToolStripMenuItem_Click);
            // 
            // hilfeToolStripMenuItem
            // 
            this.hilfeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.officialGroupToolStripMenuItem,
            this.toolStripSeparator2,
            this.checkForUpdateToolStripMenuItem,
            this.releaseNotesToolStripMenuItem,
            this.toolStripSeparator3,
            this.infoToolStripMenuItem});
            this.hilfeToolStripMenuItem.Name = "hilfeToolStripMenuItem";
            resources.ApplyResources(this.hilfeToolStripMenuItem, "hilfeToolStripMenuItem");
            // 
            // officialGroupToolStripMenuItem
            // 
            this.officialGroupToolStripMenuItem.Name = "officialGroupToolStripMenuItem";
            resources.ApplyResources(this.officialGroupToolStripMenuItem, "officialGroupToolStripMenuItem");
            this.officialGroupToolStripMenuItem.Click += new System.EventHandler(this.officialGroupToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // checkForUpdateToolStripMenuItem
            // 
            this.checkForUpdateToolStripMenuItem.Name = "checkForUpdateToolStripMenuItem";
            resources.ApplyResources(this.checkForUpdateToolStripMenuItem, "checkForUpdateToolStripMenuItem");
            this.checkForUpdateToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdateToolStripMenuItem_Click);
            // 
            // releaseNotesToolStripMenuItem
            // 
            this.releaseNotesToolStripMenuItem.Name = "releaseNotesToolStripMenuItem";
            resources.ApplyResources(this.releaseNotesToolStripMenuItem, "releaseNotesToolStripMenuItem");
            this.releaseNotesToolStripMenuItem.Click += new System.EventHandler(this.releaseNotesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // infoToolStripMenuItem
            // 
            this.infoToolStripMenuItem.Image = global::Marketwatch.Properties.Resources.about;
            this.infoToolStripMenuItem.Name = "infoToolStripMenuItem";
            resources.ApplyResources(this.infoToolStripMenuItem, "infoToolStripMenuItem");
            this.infoToolStripMenuItem.Click += new System.EventHandler(this.infoToolStripMenuItem_Click);
            // 
            // donateButton
            // 
            resources.ApplyResources(this.donateButton, "donateButton");
            this.donateButton.BackColor = System.Drawing.SystemColors.Control;
            this.donateButton.BackgroundImage = global::Marketwatch.Properties.Resources.steamDonateLowResMirror1;
            this.donateButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.donateButton.Name = "donateButton";
            this.donateButton.UseVisualStyleBackColor = false;
            this.donateButton.Click += new System.EventHandler(this.donateButton_Click);
            this.donateButton.MouseEnter += new System.EventHandler(this.donateButton_MouseEnter);
            this.donateButton.MouseLeave += new System.EventHandler(this.donateButton_MouseLeave);
            // 
            // settingsButton
            // 
            resources.ApplyResources(this.settingsButton, "settingsButton");
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.UseVisualStyleBackColor = true;
            this.settingsButton.Click += new System.EventHandler(this.settingsButton_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // accLabel
            // 
            resources.ApplyResources(this.accLabel, "accLabel");
            this.accLabel.Name = "accLabel";
            // 
            // accFlowLayoutPanel
            // 
            this.accFlowLayoutPanel.Controls.Add(this.accLabel);
            resources.ApplyResources(this.accFlowLayoutPanel, "accFlowLayoutPanel");
            this.accFlowLayoutPanel.Name = "accFlowLayoutPanel";
            // 
            // FormMarketwatch
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.accFlowLayoutPanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.donateButton);
            this.Controls.Add(this.labelSeparator);
            this.Controls.Add(this.labelTotal);
            this.Controls.Add(this.labelCurrent);
            this.Controls.Add(this.walletAmount);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.settingsButton);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.searchBox);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.dataGridView);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "FormMarketwatch";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MarketwatchForm_FormClosed);
            this.Load += new System.EventHandler(this.FormMarketwatch_Load);
            this.Shown += new System.EventHandler(this.FormMarketwatch_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.accFlowLayoutPanel.ResumeLayout(false);
            this.accFlowLayoutPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.TextBox searchBox;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.Button settingsButton;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCurrent;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label walletAmount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelSeparator;
        private System.Windows.Forms.Button donateButton;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem accToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beendenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extrasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hilfeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem infoToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn itemPreviewColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn floatColumn;
        private System.Windows.Forms.DataGridViewImageColumn accImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceColumn;
        private System.Windows.Forms.DataGridViewButtonColumn inspectButtonColumn;
        private System.Windows.Forms.DataGridViewButtonColumn buyButtonColumn;
        private System.Windows.Forms.DataGridViewLinkColumn inspectLinkColumn;
        private System.Windows.Forms.DataGridViewLinkColumn buyLinkColumn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem logoutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officialGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Label accLabel;
        private System.Windows.Forms.FlowLayoutPanel accFlowLayoutPanel;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem releaseNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}

