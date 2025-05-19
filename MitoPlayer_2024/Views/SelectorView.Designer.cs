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
            this.pnlTagComponent = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.tagValueEditorPanel = new System.Windows.Forms.Panel();
            this.btnClearTagValueFilter = new System.Windows.Forms.Button();
            this.txtbFilter = new System.Windows.Forms.TextBox();
            this.lblSelectedItemsLengthInSelector = new System.Windows.Forms.Label();
            this.lblSelectedItemsCountInSelector = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToM3UToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.btnSetTracklistToActive = new System.Windows.Forms.Button();
            this.btnSetSelectorToActive = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chbBestFit = new System.Windows.Forms.CheckBox();
            this.chbMove = new System.Windows.Forms.CheckBox();
            this.rdbPlaylist = new System.Windows.Forms.RadioButton();
            this.rdbDatabase = new System.Windows.Forms.RadioButton();
            this.cbbResultSize = new System.Windows.Forms.ComboBox();
            this.dgvSelectorTrackList = new MitoPlayer_2024.Helpers.CustomDataGridView();
            this.dgvTrackList = new MitoPlayer_2024.Helpers.CustomDataGridView();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            this.pnlTagComponent.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectorTrackList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
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
            this.lblActualPlaylistName.Location = new System.Drawing.Point(12, 12);
            this.lblActualPlaylistName.Name = "lblActualPlaylistName";
            this.lblActualPlaylistName.Size = new System.Drawing.Size(183, 23);
            this.lblActualPlaylistName.TabIndex = 58;
            this.lblActualPlaylistName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(282, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(32, 23);
            this.btnSave.TabIndex = 57;
            this.btnSave.Text = "💾";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
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
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
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
            this.btnClearTagValueFilter.Click += new System.EventHandler(this.btnClearTagValueFilter_Click);
            // 
            // txtbFilter
            // 
            this.txtbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbFilter.Location = new System.Drawing.Point(42, 8);
            this.txtbFilter.Name = "txtbFilter";
            this.txtbFilter.Size = new System.Drawing.Size(148, 20);
            this.txtbFilter.TabIndex = 42;
            this.txtbFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtbFilter_KeyUp);
            // 
            // lblSelectedItemsLengthInSelector
            // 
            this.lblSelectedItemsLengthInSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedItemsLengthInSelector.Location = new System.Drawing.Point(866, 659);
            this.lblSelectedItemsLengthInSelector.Name = "lblSelectedItemsLengthInSelector";
            this.lblSelectedItemsLengthInSelector.Size = new System.Drawing.Size(106, 13);
            this.lblSelectedItemsLengthInSelector.TabIndex = 66;
            this.lblSelectedItemsLengthInSelector.Text = "Length: 00:00:00";
            // 
            // lblSelectedItemsCountInSelector
            // 
            this.lblSelectedItemsCountInSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedItemsCountInSelector.Location = new System.Drawing.Point(866, 646);
            this.lblSelectedItemsCountInSelector.Name = "lblSelectedItemsCountInSelector";
            this.lblSelectedItemsCountInSelector.Size = new System.Drawing.Size(106, 13);
            this.lblSelectedItemsCountInSelector.TabIndex = 67;
            this.lblSelectedItemsCountInSelector.Text = "0 item selected";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createPlaylistToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator3,
            this.exportToM3UToolStripMenuItem1,
            this.exportToTxtToolStripMenuItem,
            this.exportToDirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(148, 164);
            // 
            // createPlaylistToolStripMenuItem
            // 
            this.createPlaylistToolStripMenuItem.Name = "createPlaylistToolStripMenuItem";
            this.createPlaylistToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.createPlaylistToolStripMenuItem.Text = "New";
            this.createPlaylistToolStripMenuItem.Click += new System.EventHandler(this.menuStripCreatePlaylist_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.menuStripLoadPlaylist_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.renameToolStripMenuItem.Text = "Edit";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.menuStripRenamePlaylist_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.menuStripDeletePlaylist_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(144, 6);
            // 
            // exportToM3UToolStripMenuItem1
            // 
            this.exportToM3UToolStripMenuItem1.Name = "exportToM3UToolStripMenuItem1";
            this.exportToM3UToolStripMenuItem1.Size = new System.Drawing.Size(147, 22);
            this.exportToM3UToolStripMenuItem1.Text = "Export to m3u";
            this.exportToM3UToolStripMenuItem1.Click += new System.EventHandler(this.menuStripExportToM3uToolStripMenuItem_Click);
            // 
            // exportToTxtToolStripMenuItem
            // 
            this.exportToTxtToolStripMenuItem.Name = "exportToTxtToolStripMenuItem";
            this.exportToTxtToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportToTxtToolStripMenuItem.Text = "Export to txt";
            this.exportToTxtToolStripMenuItem.Click += new System.EventHandler(this.menuStripExportToTxtToolStripMenuItem_Click);
            // 
            // exportToDirToolStripMenuItem
            // 
            this.exportToDirToolStripMenuItem.Name = "exportToDirToolStripMenuItem";
            this.exportToDirToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.exportToDirToolStripMenuItem.Text = "Export to directory";
            this.exportToDirToolStripMenuItem.Click += new System.EventHandler(this.exportToDirectoryToolStripMenuItem_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(978, 646);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(274, 26);
            this.lblMessage.TabIndex = 69;
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(866, 340);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(106, 13);
            this.lblSelectedItemsLength.TabIndex = 70;
            this.lblSelectedItemsLength.Text = "Length: 00:00:00";
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(866, 327);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(106, 13);
            this.lblSelectedItemsCount.TabIndex = 71;
            this.lblSelectedItemsCount.Text = "0 item selected";
            // 
            // btnSetTracklistToActive
            // 
            this.btnSetTracklistToActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetTracklistToActive.Location = new System.Drawing.Point(201, 12);
            this.btnSetTracklistToActive.Name = "btnSetTracklistToActive";
            this.btnSetTracklistToActive.Size = new System.Drawing.Size(75, 23);
            this.btnSetTracklistToActive.TabIndex = 72;
            this.btnSetTracklistToActive.Text = "Set Active";
            this.btnSetTracklistToActive.UseVisualStyleBackColor = true;
            this.btnSetTracklistToActive.Click += new System.EventHandler(this.btnSetTrackListToActive_Click);
            // 
            // btnSetSelectorToActive
            // 
            this.btnSetSelectorToActive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetSelectorToActive.Location = new System.Drawing.Point(201, 327);
            this.btnSetSelectorToActive.Name = "btnSetSelectorToActive";
            this.btnSetSelectorToActive.Size = new System.Drawing.Size(75, 23);
            this.btnSetSelectorToActive.TabIndex = 73;
            this.btnSetSelectorToActive.Text = "Set Active";
            this.btnSetSelectorToActive.UseVisualStyleBackColor = true;
            this.btnSetSelectorToActive.Click += new System.EventHandler(this.btnSetSelectorToActive_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(282, 332);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 75;
            this.label1.Text = "Playlist Source:";
            // 
            // chbBestFit
            // 
            this.chbBestFit.AutoSize = true;
            this.chbBestFit.Location = new System.Drawing.Point(633, 331);
            this.chbBestFit.Name = "chbBestFit";
            this.chbBestFit.Size = new System.Drawing.Size(61, 17);
            this.chbBestFit.TabIndex = 76;
            this.chbBestFit.Text = "Best Fit";
            this.chbBestFit.UseVisualStyleBackColor = true;
            this.chbBestFit.CheckedChanged += new System.EventHandler(this.chbBestFit_CheckedChanged);
            // 
            // chbMove
            // 
            this.chbMove.AutoSize = true;
            this.chbMove.Location = new System.Drawing.Point(700, 331);
            this.chbMove.Name = "chbMove";
            this.chbMove.Size = new System.Drawing.Size(53, 17);
            this.chbMove.TabIndex = 76;
            this.chbMove.Text = "Move";
            this.chbMove.UseVisualStyleBackColor = true;
            this.chbMove.CheckedChanged += new System.EventHandler(this.chbMove_CheckedChanged);
            // 
            // rdbPlaylist
            // 
            this.rdbPlaylist.AutoSize = true;
            this.rdbPlaylist.Checked = true;
            this.rdbPlaylist.Location = new System.Drawing.Point(368, 331);
            this.rdbPlaylist.Name = "rdbPlaylist";
            this.rdbPlaylist.Size = new System.Drawing.Size(57, 17);
            this.rdbPlaylist.TabIndex = 77;
            this.rdbPlaylist.TabStop = true;
            this.rdbPlaylist.Text = "Playlist";
            this.rdbPlaylist.UseVisualStyleBackColor = true;
            this.rdbPlaylist.CheckedChanged += new System.EventHandler(this.rdbPlaylist_CheckedChanged);
            // 
            // rdbDatabase
            // 
            this.rdbDatabase.AutoSize = true;
            this.rdbDatabase.Location = new System.Drawing.Point(431, 331);
            this.rdbDatabase.Name = "rdbDatabase";
            this.rdbDatabase.Size = new System.Drawing.Size(71, 17);
            this.rdbDatabase.TabIndex = 77;
            this.rdbDatabase.Text = "Database";
            this.rdbDatabase.UseVisualStyleBackColor = true;
            this.rdbDatabase.CheckedChanged += new System.EventHandler(this.rdbDatabase_CheckedChanged);
            // 
            // cbbResultSize
            // 
            this.cbbResultSize.FormattingEnabled = true;
            this.cbbResultSize.Items.AddRange(new object[] {
            "50",
            "100",
            "150",
            "200",
            "500",
            "750",
            "1000",
            "1500"});
            this.cbbResultSize.Location = new System.Drawing.Point(577, 329);
            this.cbbResultSize.Name = "cbbResultSize";
            this.cbbResultSize.Size = new System.Drawing.Size(50, 21);
            this.cbbResultSize.TabIndex = 78;
            this.cbbResultSize.Text = "50";
            this.cbbResultSize.SelectedIndexChanged += new System.EventHandler(this.cbbResultSize_SelectedIndexChanged);
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
            this.dgvSelectorTrackList.Location = new System.Drawing.Point(201, 356);
            this.dgvSelectorTrackList.Name = "dgvSelectorTrackList";
            this.dgvSelectorTrackList.ReadOnly = true;
            this.dgvSelectorTrackList.RowHeadersVisible = false;
            this.dgvSelectorTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectorTrackList.Size = new System.Drawing.Size(771, 287);
            this.dgvSelectorTrackList.TabIndex = 60;
            this.dgvSelectorTrackList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSelectorTrackList_ColumnHeaderMouseClick);
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
            // dgvTrackList
            // 
            this.dgvTrackList.AllowDrop = true;
            this.dgvTrackList.AllowUserToAddRows = false;
            this.dgvTrackList.AllowUserToDeleteRows = false;
            this.dgvTrackList.AllowUserToResizeRows = false;
            this.dgvTrackList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
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
            this.dgvTrackList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragOver);
            this.dgvTrackList.DragLeave += new System.EventHandler(this.dgvTrackList_DragLeave);
            this.dgvTrackList.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvTrackList_Paint);
            this.dgvTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyDown);
            this.dgvTrackList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyUp);
            this.dgvTrackList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDoubleClick);
            this.dgvTrackList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDown);
            this.dgvTrackList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseMove);
            this.dgvTrackList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(508, 333);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "Result Size:";
            // 
            // SelectorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbbResultSize);
            this.Controls.Add(this.rdbDatabase);
            this.Controls.Add(this.rdbPlaylist);
            this.Controls.Add(this.chbMove);
            this.Controls.Add(this.chbBestFit);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSetSelectorToActive);
            this.Controls.Add(this.btnSetTracklistToActive);
            this.Controls.Add(this.lblSelectedItemsLength);
            this.Controls.Add(this.lblSelectedItemsCount);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblSelectedItemsLengthInSelector);
            this.Controls.Add(this.lblSelectedItemsCountInSelector);
            this.Controls.Add(this.pnlTagComponent);
            this.Controls.Add(this.dgvSelectorTrackList);
            this.Controls.Add(this.dgvTrackList);
            this.Controls.Add(this.lblActualPlaylistName);
            this.Controls.Add(this.lblTrackSumLength);
            this.Controls.Add(this.lblTrackCount);
            this.Controls.Add(this.dgvPlaylistList);
            this.Name = "SelectorView";
            this.Text = "SelectorView";
            this.Shown += new System.EventHandler(this.SelectorView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            this.pnlTagComponent.ResumeLayout(false);
            this.pnlTagComponent.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectorTrackList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTrackSumLength;
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.Label lblActualPlaylistName;
        private System.Windows.Forms.Button btnSave;
        private Helpers.CustomDataGridView dgvTrackList;
        private Helpers.CustomDataGridView dgvSelectorTrackList;
        private System.Windows.Forms.Panel pnlTagComponent;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Panel tagValueEditorPanel;
        private System.Windows.Forms.Button btnClearTagValueFilter;
        private System.Windows.Forms.TextBox txtbFilter;
        private System.Windows.Forms.Label lblSelectedItemsLengthInSelector;
        private System.Windows.Forms.Label lblSelectedItemsCountInSelector;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem createPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem exportToM3UToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToTxtToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToDirToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private System.Windows.Forms.Button btnSetTracklistToActive;
        private System.Windows.Forms.Button btnSetSelectorToActive;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chbBestFit;
        private System.Windows.Forms.CheckBox chbMove;
        private System.Windows.Forms.RadioButton rdbPlaylist;
        private System.Windows.Forms.RadioButton rdbDatabase;
        private System.Windows.Forms.ComboBox cbbResultSize;
        private System.Windows.Forms.Label label2;
    }
}