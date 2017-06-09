namespace FileSizeChecker
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.DeleteButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FileSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActualSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filePathTextBox = new System.Windows.Forms.TextBox();
            this.openFolderButton = new System.Windows.Forms.Button();
            this.searchButton = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.totalSizeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.refreshButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenExplorerStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DeleteButton,
            this.FileName,
            this.FileSize,
            this.ActualSize,
            this.FullPath});
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(12, 66);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.Size = new System.Drawing.Size(625, 560);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView_CellContentDoubleClick);
            this.dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DataGridView_CellMouseClick);
            // 
            // DeleteButton
            // 
            this.DeleteButton.HeaderText = "削除";
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DeleteButton.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // FileName
            // 
            this.FileName.HeaderText = "FileName";
            this.FileName.Name = "FileName";
            // 
            // FileSize
            // 
            this.FileSize.HeaderText = "FileSize";
            this.FileSize.Name = "FileSize";
            // 
            // ActualSize
            // 
            this.ActualSize.HeaderText = "ActualSize";
            this.ActualSize.Name = "ActualSize";
            // 
            // FullPath
            // 
            this.FullPath.HeaderText = "FullPath";
            this.FullPath.Name = "FullPath";
            // 
            // filePathTextBox
            // 
            this.filePathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filePathTextBox.Location = new System.Drawing.Point(12, 12);
            this.filePathTextBox.Name = "filePathTextBox";
            this.filePathTextBox.Size = new System.Drawing.Size(600, 19);
            this.filePathTextBox.TabIndex = 1;
            // 
            // openFolderButton
            // 
            this.openFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.openFolderButton.Location = new System.Drawing.Point(618, 12);
            this.openFolderButton.Name = "openFolderButton";
            this.openFolderButton.Size = new System.Drawing.Size(19, 19);
            this.openFolderButton.TabIndex = 2;
            this.openFolderButton.Text = "...";
            this.openFolderButton.UseVisualStyleBackColor = true;
            this.openFolderButton.Click += new System.EventHandler(this.OpenFolderButton_Click);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.searchButton.Location = new System.Drawing.Point(497, 37);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(140, 23);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "🔍 検索";
            this.searchButton.UseVisualStyleBackColor = false;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.totalSizeLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 629);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(649, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // totalSizeLabel
            // 
            this.totalSizeLabel.Name = "totalSizeLabel";
            this.totalSizeLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(420, 37);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(71, 23);
            this.refreshButton.TabIndex = 5;
            this.refreshButton.Text = "🔄 更新";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // backButton
            // 
            this.backButton.Location = new System.Drawing.Point(12, 37);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 6;
            this.backButton.Text = "⇐ 戻る";
            this.backButton.UseVisualStyleBackColor = true;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenExplorerStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(155, 26);
            // 
            // OpenExplorerStripMenuItem
            // 
            this.OpenExplorerStripMenuItem.Name = "OpenExplorerStripMenuItem";
            this.OpenExplorerStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.OpenExplorerStripMenuItem.Text = "Explorer で開く";
            this.OpenExplorerStripMenuItem.Click += new System.EventHandler(this.OpenExplorerStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(649, 651);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.refreshButton);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.openFolderButton);
            this.Controls.Add(this.filePathTextBox);
            this.Controls.Add(this.dataGridView);
            this.Name = "MainForm";
            this.Text = "ファイルサイズチェッカー";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.TextBox filePathTextBox;
        private System.Windows.Forms.Button openFolderButton;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel totalSizeLabel;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ActualSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullPath;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem OpenExplorerStripMenuItem;
    }
}

