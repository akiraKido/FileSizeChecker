using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FileSizeChecker
{
    class FileSizeInfo
    {
        internal string FullPath { get; set; }
        internal long FileSize { get; set; }
    }

    class FileSizeCalculator
    {

        private static long failedChecks;

        internal IEnumerable<FileSizeInfo> Calculate( string directoryPath, out long totalSize, out long failedChecks )
        {
            if ( !Directory.Exists( directoryPath ) )
            {
                throw new FileSizeCalculationException("Directory not found:" + directoryPath);
            }

            var directoryInfo = new DirectoryInfo( directoryPath );
            FileSizeCalculator.failedChecks = 0;

            var result = new ConcurrentBag<FileSizeInfo>();
            long _totalSize = 0;
            Parallel.ForEach( directoryInfo.GetDirectories(), directory =>
            {
                long directorySize = GetDirectorySize( directory );
                result.Add( new FileSizeInfo {FileSize = directorySize , FullPath = directory.FullName} );
                Interlocked.Add( ref _totalSize, directorySize );
            } );
            Parallel.ForEach( directoryInfo.GetFiles(), fileInfo =>
            {
                result.Add( new FileSizeInfo {FileSize = fileInfo.Length, FullPath = fileInfo.FullName} );
                Interlocked.Add( ref _totalSize, fileInfo.Length );
            } );

            failedChecks = FileSizeCalculator.failedChecks;
            totalSize = _totalSize;

            return result;
        }

        private static long GetDirectorySize ( DirectoryInfo dirInfo )
        {
            long size = 0;

            try
            {
                size += dirInfo.GetFiles().Sum( fi => fi.Length );
                size += dirInfo.GetDirectories().Sum( di => GetDirectorySize( di ) );
            }
            catch ( Exception )
            {
                Interlocked.Increment( ref failedChecks );
            }

            //結果を返す
            return size;
        }
    }

    class FileSizeCalculationException : Exception
    {
        internal FileSizeCalculationException( string s ) : base(s){}
    }
}
