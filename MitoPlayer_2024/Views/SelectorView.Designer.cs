namespace MitoPlayer_2024.Views
{
    partial class SelectorView
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
            this.components = new System.ComponentModel.Container();
            this.lblTrackSumLength = new System.Windows.Forms.Label();
            this.lblTrackCount = new System.Windows.Forms.Label();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.lblActualPlaylistName = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPlaylistListPanelToggle = new System.Windows.Forms.Button();
            this.dgvTrackList = new MitoPlayer_2024.Helpers.CustomDataGridView();
            this.dgvSelectorTrackList = new MitoPlayer_2024.Helpers.CustomDataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdbShowBestFitFromDatabase = new System.Windows.Forms.RadioButton();
            this.rdbShowBestFitFromSelected = new System.Windows.Forms.RadioButton();
            this.rdbShowSelected = new System.Windows.Forms.RadioButton();
            this.pnlTagComponent = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.tagValueEditorPanel = new System.Windows.Forms.Panel();
            this.btnClearTagValueFilter = new System.Windows.Forms.Button();
            this.txtbFilter = new System.Windows.Forms.TextBox();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToM3UToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblMessage = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectorTrackList)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.pnlTagComponent.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTrackSumLength
            // 
            this.lblTrackSumLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackSumLength.Location = new System.Drawing.Point(9, 659);
            this.lblTrackSumLength.Name = "lblTrackSumLength";
            this.lblTrackSumLength.Size = new System.Drawing.Size(181, 13);
            this.lblTrackSumLength.TabIndex = 18;
            this.lblTrackSumLength.Text = "Length: 00:00:00";
            this.lblTrackSumLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrackCount
            // 
            this.lblTrackCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackCount.AutoSize = true;
            this.lblTrackCount.Location = new System.Drawing.Point(9, 646);
            this.lblTrackCount.Name = "lblTrackCount";
            this.lblTrackCount.Size = new System.Drawing.Size(116, 13);
            this.lblTrackCount.TabIndex = 19;
            this.lblTrackCount.Text = "1000 item(s) in [Playlist]";
            this.lblTrackCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgvPlaylistList
            // 
            this.dgvPlaylistList.AllowDrop = true;
            this.dgvPlaylistList.AllowUserToAddRows = false;
            this.dgvPlaylistList.AllowUserToDeleteRows = false;
            this.dgvPlaylistList.AllowUserToResizeRows = false;
            this.dgvPlaylistList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvPlaylistList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlaylistList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPlaylistList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPlaylistList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPlaylistList.Location = new System.Drawing.Point(12, 41);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.RowHeadersVisible = false;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(183, 602);
            this.dgvPlaylistList.TabIndex = 20;
            this.dgvPlaylistList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPlaylistList_DataBindingComplete);
            this.dgvPlaylistList.SelectionChanged += new System.EventHandler(this.dgvPlaylistList_SelectionChanged);
            this.dgvPlaylistList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragDrop);
            this.dgvPlaylistList.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragEnter);
            this.dgvPlaylistList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragOver);
            this.dgvPlaylistList.DragLeave += new System.EventHandler(this.dgvPlaylistList_DragLeave);
            this.dgvPlaylistList.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvPlaylistList_Paint);
            this.dgvPlaylistList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPlaylistList_KeyDown);
            this.dgvPlaylistList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseClick);
            this.dgvPlaylistList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseDoubleClick);
            this.dgvPlaylistList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseDown);
            this.dgvPlaylistList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseMove);
            this.dgvPlaylistList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseUp);
            // 
            // lblActualPlaylistName
            // 
            this.lblActualPlaylistName.Location = new System.Drawing.Point(87, 12);
            this.lblActualPlaylistName.Name = "lblActualPlaylistName";
            this.lblActualPlaylistName.Size = new System.Drawing.Size(108, 23);
            this.lblActualPlaylistName.TabIndex = 58;
            this.lblActualPlaylistName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(49, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(32, 23);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "💾";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnPlaylistListPanelToggle
            // 
            this.btnPlaylistListPanelToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlaylistListPanelToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlaylistListPanelToggle.Image = global::MitoPlayer_2024.Properties.Resources.Arrow_Left_20_20;
            this.btnPlaylistListPanelToggle.Location = new System.Drawing.Point(12, 12);
            this.btnPlaylistListPanelToggle.Name = "btnPlaylistListPanelToggle";
            this.btnPlaylistListPanelToggle.Size = new System.Drawing.Size(31, 23);
            this.btnPlaylistListPanelToggle.TabIndex = 56;
            this.btnPlaylistListPanelToggle.UseVisualStyleBackColor = true;
            // 
            // dgvTrackList
            // 
            this.dgvTrackList.AllowDrop = true;
            this.dgvTrackList.AllowUserToAddRows = false;
            this.dgvTrackList.AllowUserToDeleteRows = false;
            this.dgvTrackList.AllowUserToResizeRows = false;
            this.dgvTrackList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrackList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrackList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTrackList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTrackList.Location = new System.Drawing.Point(201, 41);
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.RowHeadersVisible = false;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(771, 280);
            this.dgvTrackList.TabIndex = 59;
            this.dgvTrackList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTrackList_ColumnHeaderMouseClick);
            this.dgvTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackList_DataBindingComplete);
            this.dgvTrackList.SelectionChanged += new System.EventHandler(this.dgvTrackList_SelectionChanged);
            this.dgvTrackList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragDrop);
            this.dgvTrackList.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragEnter);
            this.dgvTrackList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragOver);
            this.dgvTrackList.DragLeave += new System.EventHandler(this.dgvTrackList_DragLeave);
            this.dgvTrackList.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvTrackList_Paint);
            this.dgvTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyDown);
            this.dgvTrackList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyUp);
            this.dgvTrackList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDoubleClick);
            this.dgvTrackList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDown);
            this.dgvTrackList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseMove);
            this.dgvTrackList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseUp);
            // 
            // dgvSelectorTrackList
            // 
            this.dgvSelectorTrackList.AllowDrop = true;
            this.dgvSelectorTrackList.AllowUserToAddRows = false;
            this.dgvSelectorTrackList.AllowUserToDeleteRows = false;
            this.dgvSelectorTrackList.AllowUserToResizeRows = false;
            this.dgvSelectorTrackList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSelectorTrackList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelectorTrackList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSelectorTrackList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvSelectorTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectorTrackList.Location = new System.Drawing.Point(201, 386);
            this.dgvSelectorTrackList.Name = "dgvSelectorTrackList";
            this.dgvSelectorTrackList.ReadOnly = true;
            this.dgvSelectorTrackList.RowHeadersVisible = false;
            this.dgvSelectorTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectorTrackList.Size = new System.Drawing.Size(771, 257);
            this.dgvSelectorTrackList.TabIndex = 60;
            this.dgvSelectorTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvSelectorTrackList_DataBindingComplete);
            this.dgvSelectorTrackList.SelectionChanged += new System.EventHandler(this.dgvSelectorTrackList_SelectionChanged);
            this.dgvSelectorTrackList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvSelectorTrackList_DragDrop);
            this.dgvSelectorTrackList.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvSelectorTrackList_DragEnter);
            this.dgvSelectorTrackList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvSelectorTrackList_DragOver);
            this.dgvSelectorTrackList.DragLeave += new System.EventHandler(this.dgvSelectorTrackList_DragLeave);
            this.dgvSelectorTrackList.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvSelectorTrackList_Paint);
            this.dgvSelectorTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvSelectedTrackList_KeyDown);
            this.dgvSelectorTrackList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvSelectorTrackList_KeyUp);
            this.dgvSelectorTrackList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvSelectorTrackList_MouseDoubleClick);
            this.dgvSelectorTrackList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvSelectorTrackList_MouseDown);
            this.dgvSelectorTrackList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvSelectorTrackList_MouseMove);
            this.dgvSelectorTrackList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvSelectorTrackList_MouseUp);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdbShowBestFitFromDatabase);
            this.groupBox1.Controls.Add(this.rdbShowBestFitFromSelected);
            this.groupBox1.Controls.Add(this.rdbShowSelected);
            this.groupBox1.Location = new System.Drawing.Point(201, 327);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(771, 53);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playlist Mode";
            // 
            // rdbShowBestFitFromDatabase
            // 
            this.rdbShowBestFitFromDatabase.AutoSize = true;
            this.rdbShowBestFitFromDatabase.Location = new System.Drawing.Point(246, 19);
            this.rdbShowBestFitFromDatabase.Name = "rdbShowBestFitFromDatabase";
            this.rdbShowBestFitFromDatabase.Size = new System.Drawing.Size(135, 17);
            this.rdbShowBestFitFromDatabase.TabIndex = 2;
            this.rdbShowBestFitFromDatabase.TabStop = true;
            this.rdbShowBestFitFromDatabase.Text = "Best Fit From Database";
            this.rdbShowBestFitFromDatabase.UseVisualStyleBackColor = true;
            // 
            // rdbShowBestFitFromSelected
            // 
            this.rdbShowBestFitFromSelected.AutoSize = true;
            this.rdbShowBestFitFromSelected.Location = new System.Drawing.Point(109, 19);
            this.rdbShowBestFitFromSelected.Name = "rdbShowBestFitFromSelected";
            this.rdbShowBestFitFromSelected.Size = new System.Drawing.Size(131, 17);
            this.rdbShowBestFitFromSelected.TabIndex = 1;
            this.rdbShowBestFitFromSelected.TabStop = true;
            this.rdbShowBestFitFromSelected.Text = "Best Fit From Selected";
            this.rdbShowBestFitFromSelected.UseVisualStyleBackColor = true;
            // 
            // rdbShowSelected
            // 
            this.rdbShowSelected.AutoSize = true;
            this.rdbShowSelected.Location = new System.Drawing.Point(6, 19);
            this.rdbShowSelected.Name = "rdbShowSelected";
            this.rdbShowSelected.Size = new System.Drawing.Size(97, 17);
            this.rdbShowSelected.TabIndex = 0;
            this.rdbShowSelected.TabStop = true;
            this.rdbShowSelected.Text = "Show Selected";
            this.rdbShowSelected.UseVisualStyleBackColor = true;
            // 
            // pnlTagComponent
            // 
            this.pnlTagComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTagComponent.Controls.Add(this.lblFilter);
            this.pnlTagComponent.Controls.Add(this.btnFilter);
            this.pnlTagComponent.Controls.Add(this.tagValueEditorPanel);
            this.pnlTagComponent.Controls.Add(this.btnClearTagValueFilter);
            this.pnlTagComponent.Controls.Add(this.txtbFilter);
            this.pnlTagComponent.Location = new System.Drawing.Point(978, 42);
            this.pnlTagComponent.Name = "pnlTagComponent";
            this.pnlTagComponent.Size = new System.Drawing.Size(274, 601);
            this.pnlTagComponent.TabIndex = 65;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(4, 11);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 43;
            this.lblFilter.Text = "Filter:";
            // 
            // btnFilter
            // 
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Location = new System.Drawing.Point(196, 6);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(66, 23);
            this.btnFilter.TabIndex = 46;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            // 
            // tagValueEditorPanel
            // 
            this.tagValueEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagValueEditorPanel.AutoScroll = true;
            this.tagValueEditorPanel.Location = new System.Drawing.Point(0, 34);
            this.tagValueEditorPanel.Name = "tagValueEditorPanel";
            this.tagValueEditorPanel.Size = new System.Drawing.Size(274, 535);
            this.tagValueEditorPanel.TabIndex = 36;
            // 
            // btnClearTagValueFilter
            // 
            this.btnClearTagValueFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearTagValueFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearTagValueFilter.Location = new System.Drawing.Point(196, 575);
            this.btnClearTagValueFilter.Name = "btnClearTagValueFilter";
            this.btnClearTagValueFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearTagValueFilter.TabIndex = 45;
            this.btnClearTagValueFilter.Text = "Clear Filter";
            this.btnClearTagValueFilter.UseVisualStyleBackColor = true;
            // 
            // txtbFilter
            // 
            this.txtbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbFilter.Location = new System.Drawing.Point(42, 8);
            this.txtbFilter.Name = "txtbFilter";
            this.txtbFilter.Size = new System.Drawing.Size(148, 20);
            this.txtbFilter.TabIndex = 42;
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(198, 659);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(106, 13);
            this.lblSelectedItemsLength.TabIndex = 66;
            this.lblSelectedItemsLength.Text = "Length: 00:00:00";
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsCount.AutoSize = true;
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(198, 646);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(78, 13);
            this.lblSelectedItemsCount.TabIndex = 67;
            this.lblSelectedItemsCount.Text = "0 item selected";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createPlaylistToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator3,
            this.toolStripMenuItem1,
            this.toolStripSeparator4,
            this.exportToM3UToolStripMenuItem1,
            this.exportToTxtToolStripMenuItem,
            this.exportToDirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 192);
            // 
            // createPlaylistToolStripMenuItem
            // 
            this.createPlaylistToolStripMenuItem.Name = "createPlaylistToolStripMenuItem";
            this.createPlaylistToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.createPlaylistToolStripMenuItem.Text = "New";
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.loadToolStripMenuItem.Text = "Load";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.renameToolStripMenuItem.Text = "Edit";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.deleteToolStripMenuItem.Text = "Remove";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(144, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            this.toolStripMenuItem1.Text = "Train Model";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(144, 6);
            // 
            // exportToM3UToolStripMenuItem1
            // 
            this.exportToM3UToolStripMenuItem1.Name = "exportToM3UToolStripMenuItem1";
            this.exportToM3UToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            this.exportToM3UToolStripMenuItem1.Text = "Export to m3u";
            // 
            // exportToTxtToolStripMenuItem
            // 
            this.exportToTxtToolStripMenuItem.Name = "exportToTxtToolStripMenuItem";
            this.exportToTxtToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportToTxtToolStripMenuItem.Text = "Export to txt";
            // 
            // exportToDirToolStripMenuItem
            // 
            this.exportToDirToolStripMenuItem.Name = "exportToDirToolStripMenuItem";
            this.exportToDirToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportToDirToolStripMenuItem.Text = "Export to directory";
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(978, 646);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(274, 26);
            this.lblMessage.TabIndex = 69;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(201, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Load Selected";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SelectorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblSelectedItemsLength);
            this.Controls.Add(this.lblSelectedItemsCount);
            this.Controls.Add(this.pnlTagComponent);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvSelectorTrackList);
            this.Controls.Add(this.dgvTrackList);
            this.Controls.Add(this.lblActualPlaylistName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPlaylistListPanelToggle);
            this.Controls.Add(this.lblTrackSumLength);
            this.Controls.Add(this.lblTrackCount);
            this.Controls.Add(this.dgvPlaylistList);
            this.Name = "SelectorView";
            this.Text = "SelectorView";
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectorTrackList)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlTagComponent.ResumeLayout(false);
            this.pnlTagComponent.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTrackSumLength;
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.Label lblActualPlaylistName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPlaylistListPanelToggle;
        private Helpers.CustomDataGridView dgvTrackList;
        private Helpers.CustomDataGridView dgvSelectorTrackList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdbShowBestFitFromDatabase;
        private System.Windows.Forms.RadioButton rdbShowBestFitFromSelected;
        private System.Windows.Forms.RadioButton rdbShowSelected;
        private System.Windows.Forms.Panel pnlTagComponent;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Panel tagValueEditorPanel;
        private System.Windows.Forms.Button btnClearTagValueFilter;
        private System.Windows.Forms.TextBox txtbFilter;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exportToM3UToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToTxtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToDirToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button button1;
    }
}