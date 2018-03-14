using System;
using static RS.Core.Const.Enum;

namespace RS.Core.Lib
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RSFilterAttribute : Attribute
    {
        private readonly SearchType _searchType;
        private readonly string _dbName = null;
        private readonly bool _ignore = false;

        public RSFilterAttribute(SearchType searchType = SearchType.Equal, string dbName = null, bool ignore = false)
        {
            _searchType = searchType;
            _dbName = dbName;
            _ignore = ignore;
        }

        public SearchType SearchType
        {
            get { return _searchType; }
        }
        public string DbName
        {
            get { return _dbName; }
        }
        public bool Ignore
        {
            get { return _ignore; }
        }
    }
}