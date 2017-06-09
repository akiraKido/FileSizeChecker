using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly:InternalsVisibleTo("FileSizeCheckerTests")]
namespace FileSizeChecker
{
    internal class HistoryManagerOptions
    {
        internal bool UseCache = true;
        internal bool PushHistory = true;
    }

    internal class HistoryManager
    {
        private List<string> history = new List<string>();
        private int index = -1;
        internal bool CanReturn => index > 0;
        internal bool CanMoveNext => index < history.Count - 1;

        internal void Push( string s )
        {
            if(string.IsNullOrWhiteSpace( s )) return;
            if ( index == history.Count - 1 )
            {
                history.Add( s );
                index++;
            }
            else
            {
                index++;
                history[index] = s;
            }

        }

        internal string Current
        {
            get
            {
                if ( index >= 0 )
                {
                    return history[index];
                }
                else
                {
                    throw new InvalidOperationException("No history found.");
                }
            }
        }

        internal string Back()
        {
            if ( index > 0 )
            {
                return history[--index];
            }
            else
            {
                throw new InvalidOperationException("Index is out of range.");
            }
        }

        internal string Next()
        {
            if ( index < history.Count )
            {
                return history[++index];
            }
            else
            {
                throw new InvalidOperationException("Index is out of range.");
            }
        }
    }
}
