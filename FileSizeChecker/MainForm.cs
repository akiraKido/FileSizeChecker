using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
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

            dataGridView1.Rows.Clear();
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
                    dataGridView1.Rows.Add(
                        "削除", 
                        Path.GetFileName( entry.FullPath ),
                        SizeSuffix( entry.FileSize ),
                        entry.FileSize, entry.FullPath );
                }
                dataGridView1.Sort( dataGridView1.Columns[3], ListSortDirection.Descending );
                totalSizeLabel.Text = "合計: " + SizeSuffix( totalSize, 2 ) + " (取得できなかったファイル数: " + failedChecks + ")";

                history.Add( filePath );
                currentIndex++;


                EnableForm( true );
            };

            worker.RunWorkerAsync();

        }

        private void EnableForm(bool value)
        {
            refreshButton.Enabled = value;
            dataGridView1.Enabled = value;
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
        

        static readonly string[] SizeSuffixes =
            { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        static string SizeSuffix ( Int64 value, int decimalPlaces = 1 )
        {
            if ( value < 0 ) { return "-" + SizeSuffix( -value ); }
            if ( value == 0 ) { return "0.0 bytes"; }

            // mag is 0 for bytes, 1 for KB, 2, for MB, etc.
            int mag = (int)Math.Log(value, 1024);

            // 1L << (mag * 10) == 2 ^ (10 * mag) 
            // [i.e. the number of bytes in the unit corresponding to mag]
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            // make adjustment when the value is large enough that
            // it would round up to 1000 or more
            if ( Math.Round( adjustedSize, decimalPlaces ) >= 1000 )
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format( "{0:n" + decimalPlaces + "} {1}",
                adjustedSize,
                SizeSuffixes[mag] );
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
                var fullFilePath = dataGridView1.Rows[e.RowIndex].Cells["FullPath"].Value.ToString();
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

        private void dataGridView1_CellContentDoubleClick ( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.ColumnIndex == 0 ) return;

            var fullFilePath = dataGridView1.Rows[e.RowIndex].Cells["FullPath"].Value.ToString();
            if ( !Directory.Exists( fullFilePath ) ) return;
            DisplayData( fullFilePath );
        }
        private void dataGridView1_CellMouseClick ( object sender, DataGridViewCellMouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Right )
            {
                dataGridView1.ClearSelection();
                OpenExplorerStripMenuItem.Enabled =
                    Directory.Exists( dataGridView1.Rows[e.RowIndex].Cells[4].Value as string );
                dataGridView1.CurrentCell = dataGridView1[e.ColumnIndex, e.RowIndex];

                contextMenuStrip.Show( Cursor.Position );
            }
        }

        #endregion


        private void openFolderButton_Click ( object sender, EventArgs e )
        {
            var openFolderDialog = new CommonOpenFileDialog()
            {
                IsFolderPicker = true
            };
            if(openFolderDialog.ShowDialog() != CommonFileDialogResult.Ok) return;
            DisplayData( openFolderDialog.FileName );
        }

        private void refreshButton_Click ( object sender, EventArgs e )
        {
            DisplayData( history[currentIndex] );
        }

        private void backButton_Click ( object sender, EventArgs e )
        {
            history.RemoveAt( history.Count - 1 );
            DisplayData( history[--currentIndex] );
        }

        private void OpenExplorerStripMenuItem_Click ( object sender, EventArgs e )
        {
            var filePath = dataGridView1.CurrentRow.Cells["FullPath"].Value as string;
            Process.Start( filePath );
        }

    }
}
