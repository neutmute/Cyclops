using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TheSprocker.Core.ExtensionMethods;

namespace TheSprocker.Core.Mapping
{
    public class TypeReflector
    {
        private IList<PropertyInfo> membersForMapping = new List<PropertyInfo>();

        internal IEnumerable<PropertyInfo> ReflectType(Type type)
        {
            foreach (PropertyInfo property in typeof(Type).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                yield return property;
        }

        public IList<PropertyInfo> LocateMappingCandidates(Type type)
        {
            // TODO: needs a filter. 

            IEnumerable<PropertyInfo> propertys = ReflectType(type).Where(p => p.MemberType == MemberTypes.Property);
            propertys.Each(m => membersForMapping.Add(m));

            return membersForMapping;
        }
    }
}
