namespace FileSizeChecker
{
    partial class OldFilesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenExplorerStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DeleteButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.FileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastUpdateDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FullPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
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
            this.LastUpdateDate,
            this.FullPath});
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(12, 12);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 21;
            this.dataGridView.Size = new System.Drawing.Size(746, 459);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_CellMouseClick);
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
            // LastUpdateDate
            // 
            this.LastUpdateDate.HeaderText = "LastUpdateDate";
            this.LastUpdateDate.Name = "LastUpdateDate";
            // 
            // FullPath
            // 
            this.FullPath.HeaderText = "FullPath";
            this.FullPath.Name = "FullPath";
            // 
            // OldFilesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 483);
            this.Controls.Add(this.dataGridView);
            this.Name = "OldFilesForm";
            this.Text = "OldFilesForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem OpenExplorerStripMenuItem;
        private System.Windows.Forms.DataGridViewButtonColumn DeleteButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn FileName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastUpdateDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullPath;
    }
}