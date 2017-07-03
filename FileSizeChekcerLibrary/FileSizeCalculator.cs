using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FileSizeCheckerLibrary.Extensions;

namespace FileSizeCheckerLibrary
{
    public class DirectorySizeInfo
    {
        public long TotalSize { get; set; }
        public long FailedChecks { get; set; }
        public IEnumerable<FileSizeInfo> FileSizeInfos { get; set; }
    }

    public class FileSizeInfo
    {
        public string FullPath { get; set; }
        public long FileSize { get; set; }
        public FileType FileType { get; set; }
    }

    public enum FileType
    {
        Directory,
        File
    }

    public class FileSizeCalculator
    {
        public async Task<DirectorySizeInfo> CalculateAsync( string directoryPath, bool useCache = true )
        {
            return await Task.Run( () => Calculate( directoryPath, useCache ) );
        }

        public DirectorySizeInfo Calculate ( string directoryPath, bool useCache = true )
        {
            if ( useCache && DirectorySizeInfoCache.ContainsKey( directoryPath ) )
            {
                return DirectorySizeInfoCache[directoryPath];
            }

            if ( !Directory.Exists( directoryPath ) )
            {
                throw new FileSizeCalculationException( "Directory not found:" + directoryPath );
            }

            var directoryInfo = new DirectoryInfo( directoryPath );
            var failedChecks = 0;

            var result = new ConcurrentBag<FileSizeInfo>();
            long _totalSize = 0;
            Parallel.ForEach( directoryInfo.GetDirectories(), directory =>
            {
                long directorySize = GetDirectorySize( directory, useCache, out failedChecks );
                result.Add( new FileSizeInfo
                {
                    FileSize = directorySize,
                    FullPath = directory.FullName,
                    FileType = FileType.Directory
                } );
                Interlocked.Add( ref _totalSize, directorySize );
            } );
            Parallel.ForEach( directoryInfo.GetFiles(), fileInfo =>
            {
                result.Add( new FileSizeInfo
                {
                    FileSize = fileInfo.Length,
                    FullPath = fileInfo.FullName,
                    FileType = FileType.File
                } );
                Interlocked.Add( ref _totalSize, fileInfo.Length );
            } );

            var directorySizeInfo = new DirectorySizeInfo
            {
                TotalSize = _totalSize,
                FailedChecks = failedChecks,
                FileSizeInfos = result
            };

            DirectorySizeInfoCache.AddOrUpdate( directoryPath, directorySizeInfo );

            return directorySizeInfo;
        }
        private static readonly Dictionary<string, DirectorySizeInfo> DirectorySizeInfoCache = new Dictionary<string, DirectorySizeInfo>();

        private static long GetDirectorySize ( DirectoryInfo dirInfo, bool useCache, out int failedChecks )
        {
            long size = 0;
            failedChecks = 0;

            try
            {
                size += dirInfo.GetFiles().Sum( fi => fi.Length );

                foreach ( var directoryInfo in dirInfo.GetDirectories() )
                {
                    long dirSize = 0;
                    if ( useCache && RawDirectorySizeCache.ContainsKey( directoryInfo.FullName ) )
                    {
                        dirSize = RawDirectorySizeCache[directoryInfo.FullName];
                    }
                    else
                    {
                        dirSize = GetDirectorySize( directoryInfo, useCache, out failedChecks );
                        RawDirectorySizeCache.AddOrUpdate( directoryInfo.FullName, dirSize );
                    }
                    size += dirSize;
                }
            }
            catch ( Exception )
            {
                Interlocked.Increment( ref failedChecks );
            }

            //結果を返す
            return size;
        }
        private static readonly Dictionary<string, long> RawDirectorySizeCache = new Dictionary<string, long>();
    }

    public class FileSizeCalculationException : Exception
    {
        internal FileSizeCalculationException ( string s ) : base( s ) { }
    }
}
