using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileSizeChecker.Extensions;

namespace FileSizeChecker
{
    class DirectorySizeInfo
    {
        internal long TotalSize;
        internal long FailedChecks;
        internal IEnumerable<FileSizeInfo> FileSizeInfos;
    }

    class FileSizeInfo
    {
        internal string FullPath { get; set; }
        internal long FileSize { get; set; }
    }

    class FileSizeCalculator
    {

        private static long failedChecks;
        private static readonly Dictionary<string, DirectorySizeInfo> Cache = new Dictionary<string, DirectorySizeInfo>();

        internal DirectorySizeInfo Calculate( string directoryPath, bool useCache = true )
        {
            if ( useCache && Cache.ContainsKey( directoryPath ) )
            {
                return Cache[directoryPath];
            }

            if ( !Directory.Exists( directoryPath ) )
            {
                throw new FileSizeCalculationException( "Directory not found:" + directoryPath );
            }

            var directoryInfo = new DirectoryInfo( directoryPath );
            failedChecks = 0;

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

            var directorySizeInfo = new DirectorySizeInfo
            {
                TotalSize = _totalSize,
                FailedChecks = failedChecks,
                FileSizeInfos = result
            };

            Cache.AddOrUpdate( directoryPath, directorySizeInfo );

            return directorySizeInfo;
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
