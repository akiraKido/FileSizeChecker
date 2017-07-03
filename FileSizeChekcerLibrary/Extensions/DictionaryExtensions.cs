using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSizeCheckerLibrary.Extensions
{
    internal static class DictionaryExtensions
    {
        internal static void AddOrUpdate<TKey, TValue>( this Dictionary<TKey, TValue> dictionary, TKey key, TValue value )
        {
            if ( dictionary.ContainsKey( key ) )
            {
                dictionary.Remove( key );
                dictionary.Add( key, value );
            }
            else
            {
                dictionary.Add( key, value );
            }
        }
    }
}
