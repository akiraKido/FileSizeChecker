﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using FileSizeChecker.Extensions;
using FileSizeChecker.Properties;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileSizeChecker
{
    public partial class MainForm : Form
    {
        private readonly HistoryManager history = new HistoryManager();
        private string _currentDirectory;
        private static readonly Lazy<Bitmap> fileIcon = new Lazy<Bitmap>( () =>
        {
            using ( var image = new Bitmap( Resources._1497023987_1_01 ) )
            {
                return ResizeIcon( image, 28, 28 );
            }
        });

        private static readonly Lazy<Bitmap> directoryIcon = new Lazy<Bitmap>( () =>
        {
            using ( var image = new Bitmap( Resources._1497023994_10_Folder ) )
            {
                return ResizeIcon( image, 20, 20 );
            }
        });

        private static Bitmap ResizeIcon( Bitmap image, int width, int height )
        {
            var newImage = new Bitmap( width, height );

            using ( var graphics = Graphics.FromImage( newImage ) )
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage( image, 0, 0, width, height );
            }

            return newImage;
        }

        private string CurrentDirectory
        {
            get { return _currentDirectory; }
            set
            {
                filePathTextBox.Text = value;
                _currentDirectory = value;
            }
        }

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
        
        private void DisplayData(string filePath, HistoryManagerOptions options = null)
        {
            if ( !Directory.Exists( filePath ) )
            {
                MessageBox.Show( "Directory not found." );
                return;
            }
            if(options == null) options = new HistoryManagerOptions();

            CurrentDirectory = filePath;
            if(options.PushHistory) history.Push( CurrentDirectory );

            dataGridView.Rows.Clear();
            EnableForm(false);
            totalSizeLabel.Text = "計算中...";
            

            var worker = new BackgroundWorker();
            DirectorySizeInfo data = null;
            long totalSize = 0;
            long failedChecks = 0;
            worker.DoWork += ( sender, e ) =>
            {
                try
                {

                    var fileSizeCalculator = new FileSizeCalculator();
                    data = fileSizeCalculator.Calculate( filePath, options.UseCache );
                }
                catch ( Exception exception )
                {
                    Debug.WriteLine( exception );
                    throw;
                }
            };

            worker.RunWorkerCompleted += (sender, e) =>
            {
                if(data == null) throw new Exception();
                foreach ( var entry in data.FileSizeInfos )
                {
                    dataGridView.Rows.Add(
                        "削除",
                        entry.FileType == FileSizeChecker.FileType.Directory
                            ? directoryIcon.Value
                            : fileIcon.Value, 
                        Path.GetFileName( entry.FullPath ),
                        entry.FileSize.ToSizeSuffix( 2 ),
                        entry.FileSize,
                        entry.FullPath );
                    totalSize += entry.FileSize;
                }
                dataGridView.Sort( dataGridView.Columns[4], ListSortDirection.Descending );
                totalSizeLabel.Text = "合計: " + totalSize.ToSizeSuffix( 2 ) + " (取得できなかったファイル数: " + failedChecks + ")";

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
                if ( history.CanReturn ) backButton.Enabled = true;
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
            DisplayData( CurrentDirectory, new HistoryManagerOptions {UseCache = false, PushHistory = false} );
        }

        private void BackButton_Click ( object sender, EventArgs e )
        {
            DisplayData( history.Back(), new HistoryManagerOptions {PushHistory = false} );
        }

        private void OpenExplorerStripMenuItem_Click ( object sender, EventArgs e )
        {
            var filePath = dataGridView.CurrentRow.Cells["FullPath"].Value as string;
            if ( !Directory.Exists( filePath ) )
            {
                filePath = Path.GetDirectoryName( filePath );
            }
            Process.Start( filePath );
        }

        private void ExtractOldFilesToolStripMenuItem_Click ( object sender, EventArgs e )
        {
            new OldFilesForm( dataGridView.CurrentRow.Cells["FullPath"].Value as string ).Show();
        }

        private void ExtractOldFilesButton_Click ( object sender, EventArgs e )
        {
            new OldFilesForm( CurrentDirectory ).Show();
        }
    }
}
