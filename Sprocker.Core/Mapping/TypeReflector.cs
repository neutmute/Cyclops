using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TheSprocker.Core.ExtensionMethods;

namespace TheSprocker.Core.Mapping
{
    public class TypeReflector<TType>
    {
        private IList<PropertyInfo> membersForMapping = new List<PropertyInfo>();

        internal IEnumerable<PropertyInfo> ReflectType<TType>()
        {
            foreach (PropertyInfo property in typeof(TType).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                yield return property;
        }

        public IList<PropertyInfo> LocateMappingCandidates()
        {
            IEnumerable<PropertyInfo> propertys = ReflectType<TType>().Where(p => p.MemberType == MemberTypes.Property);
            propertys.Each(m => membersForMapping.Add(m));

            return membersForMapping;
        }
    }
}
