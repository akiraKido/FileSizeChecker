using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using FileSizeChecker.Extensions;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileSizeChecker
{
    public partial class MainForm : Form
    {
        private readonly List<string> history = new List<string>();
        private int currentIndex = -1;

        public MainForm ()
        {
            InitializeComponent();
            refreshButton.Enabled = false;
            backButton.Enabled = false;
            filePathTextBox.KeyUp += ( sender, e ) =>
            {
                if ( e.KeyCode == Keys.Enter )
                {
                    DisplayData( filePathTextBox.Text );
                }
            };
        }

        private void searchButton_Click ( object sender, EventArgs e )
        {
            DisplayData(filePathTextBox.Text);
        }
        
        private void DisplayData(string filePath)
        {
            if ( !Directory.Exists( filePath ) )
            {
                MessageBox.Show( "Directory not found." );
                return;
            }

            filePathTextBox.Text = filePath;

            dataGridView.Rows.Clear();
            EnableForm(false);
            totalSizeLabel.Text = "計算中...";
            

            var worker = new BackgroundWorker();
            IEnumerable<FileSizeInfo> data = null;
            long totalSize = 0;
            long failedChecks = 0;
            worker.DoWork += ( sender, e ) =>
            {
                var fileSizeCalculator = new FileSizeCalculator();
                data = fileSizeCalculator.Calculate( filePath, out totalSize, out failedChecks );
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if(data == null) throw new Exception();
                foreach ( var entry in data )
                {
                    dataGridView.Rows.Add(
                        "削除", 
                        Path.GetFileName( entry.FullPath ),
                        entry.FileSize.ToSizeSuffix( 2 ),
                        entry.FileSize, entry.FullPath );
                }
                dataGridView.Sort( dataGridView.Columns[3], ListSortDirection.Descending );
                totalSizeLabel.Text = "合計: " + totalSize.ToSizeSuffix( 2 ) + " (取得できなかったファイル数: " + failedChecks + ")";

                history.Add( filePath );
                currentIndex++;


                EnableForm( true );
            };

            worker.RunWorkerAsync();

        }

        private void EnableForm(bool value)
        {
            refreshButton.Enabled = value;
            dataGridView.Enabled = value;
            searchButton.Enabled = value;
            openFolderButton.Enabled = value;

            if ( value == true )
            {
                if ( currentIndex > 0 ) backButton.Enabled = true;
            }
            else
            {
                backButton.Enabled = false;
            }
        }

        #region DataGridView
        private void dataGridView1_CellContentClick ( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.ColumnIndex == 0 )
            {
                if ( MessageBox.Show( "削除しますか", "確認",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes ) return;
                if ( MessageBox.Show( "本当に削除されます、復元できません。よろしいですか？", "確認",
                         MessageBoxButtons.YesNo, MessageBoxIcon.Question ) != DialogResult.Yes ) return;

                // 削除 
                var fullFilePath = dataGridView.Rows[e.RowIndex].Cells["FullPath"].Value.ToString();
                try
                {
                    File.Delete( fullFilePath );
                    MessageBox.Show( "ファイルを削除しました:" + Path.GetFileName( fullFilePath ) );
                }
                catch ( UnauthorizedAccessException )
                {
                    MessageBox.Show( "権限が無いため削除できませんでした。" );
                }
                catch ( Exception )
                {
                    MessageBox.Show( "削除できませんでした。" );
                }
            }

        }

        private void DataGridView_CellContentDoubleClick ( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.ColumnIndex == 0 || e.RowIndex < 0 ) return;

            string fullFilePath = dataGridView.Rows[e.RowIndex].Cells["FullPath"].Value.ToString();
            if ( !Directory.Exists( fullFilePath ) ) return;
            DisplayData( fullFilePath );
        }

        private void DataGridView_CellMouseClick ( object sender, DataGridViewCellMouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Right && e.RowIndex >= 0 )
            {
                dataGridView.ClearSelection();
                OpenExplorerStripMenuItem.Enabled =
                    Directory.Exists( dataGridView.Rows[e.RowIndex].Cells[4].Value as string );
                dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];

                contextMenuStrip.Show( Cursor.Position );
            }
        }

        #endregion


        private void OpenFolderButton_Click ( object sender, EventArgs e )
        {
            var openFolderDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            };
            if(openFolderDialog.ShowDialog() != CommonFileDialogResult.Ok) return;
            DisplayData( openFolderDialog.FileName );
        }

        private void RefreshButton_Click ( object sender, EventArgs e )
        {
            DisplayData( history[currentIndex] );
        }

        private void BackButton_Click ( object sender, EventArgs e )
        {
            history.RemoveAt( history.Count - 1 );
            DisplayData( history[--currentIndex] );
        }

        private void OpenExplorerStripMenuItem_Click ( object sender, EventArgs e )
        {
            var filePath = dataGridView.CurrentRow.Cells["FullPath"].Value as string;
            Process.Start( filePath );
        }

    }
}
