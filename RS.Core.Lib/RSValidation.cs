using System;

namespace RS.Core.Lib
{
    public static class RSValidation
    {
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return (guid == Guid.Empty || guid == null);
        }
        public static bool IsNullOrEmpty(this Guid guid)
        {
            return (guid == Guid.Empty);
        }
    }
}
