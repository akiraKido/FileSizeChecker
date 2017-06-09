using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSizeChecker.Extensions;

namespace FileSizeChecker
{
    public partial class OldFilesForm : Form
    {
        private readonly DateTime today = DateTime.Today;

        public OldFilesForm (string directory)
        {
            InitializeComponent();
            DisplayData( directory );
        }

        private void DisplayData(string directory)
        {
            var worker = new BackgroundWorker();
            IEnumerable<FileSystemInfo> data = null;
            long totalSize = 0;
            long failedChecks = 0;
            worker.DoWork += ( sender, e ) =>
            {
                try
                {
                    data = GetDirectorySize( new DirectoryInfo( directory ) );
                }
                catch ( Exception exception )
                {
                    Debug.WriteLine( exception );
                    throw;
                }
            };

            worker.RunWorkerCompleted += ( sender, e ) =>
            {
                if ( data == null ) throw new Exception();
                foreach ( var entry in data )
                {
                    dataGridView.Rows.Add(
                        "削除",
                        Path.GetFileName( entry.FullName ),
                        entry.LastAccessTime,
                        entry.FullName);
                }
                dataGridView.Sort( dataGridView.Columns[3], ListSortDirection.Descending );
            };

            worker.RunWorkerAsync();
        }

        private static int failedChecks = 0;

        private static IEnumerable<FileSystemInfo> GetDirectorySize ( DirectoryInfo directoryInfo )
        {
            var oldFiles = new List<FileSystemInfo>();

            try
            {
                oldFiles.AddRange( directoryInfo.GetFiles().Where( fileInfo => ( DateTime.Today - fileInfo.LastAccessTime ).Days > 180 ) );
                //foreach ( var directory in directoryInfo.GetDirectories() )
                //{
                //    oldFiles.AddRange( GetDirectorySize( directory ) );
                //}
                oldFiles.AddRange( directoryInfo.GetDirectories().Where( directory => ( DateTime.Today - directory.LastAccessTime ).Days > 180 ) );
            }
            catch ( Exception )
            {
                Interlocked.Increment( ref failedChecks );
            }

            //結果を返す
            return oldFiles;
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

        private void dataGridView_CellMouseClick ( object sender, DataGridViewCellMouseEventArgs e )
        {
            if ( e.Button == MouseButtons.Right && e.RowIndex >= 0 )
            {
                dataGridView.ClearSelection();
                //OpenExplorerStripMenuItem.Enabled =
                //    Directory.Exists( dataGridView.Rows[e.RowIndex].Cells[4].Value as string );
                dataGridView.CurrentCell = dataGridView[e.ColumnIndex, e.RowIndex];

                contextMenuStrip.Show( Cursor.Position );
            }
        }
    }
}
