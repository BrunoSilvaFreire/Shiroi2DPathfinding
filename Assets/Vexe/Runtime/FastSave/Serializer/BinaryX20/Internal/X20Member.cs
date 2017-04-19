#if UNITY_EDITOR || UNITY_STANDALONE
#define FAST_REFLECTION
#endif

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Vexe.Runtime.Extensions;

namespace BX20Serializer
{
    public class X20Member
    {
#if FAST_REFLECTION
        private MemberSetter<object, object> _setter;
        private MemberGetter<object, object> _getter;
#else
        private Action<object, object> _setter;
        private Func<object, object> _getter;
#endif
        public object Target;

        /// <summary>
        /// The name of the wrapped member
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The type of the wrapped member (FieldInfo.FieldType in case of a field, or PropertyInfo.PropertyType in case of a property)
        /// </summary>
        public readonly Type Type;

        /// <summary>
        /// A reference to the MemberInfo reference of the wrapped member
        /// </summary>
        public readonly MemberInfo Info;

        /// <summary>
        /// The current value of the member in the current target object
        /// </summary>
        public object Value
        {
            get
            {
                return _getter(Target);
            }
            set
            {
                try
                {
#if FAST_REFLECTION
                    _setter(ref Target, value);
#else
                    _setter(Target, value);
#endif
                }
                catch(InvalidCastException)
                {
                    throw new InvalidCastException("Failed casting: " + value + " to type: " + Type);
                }
            }
        }

        private X20Member(MemberInfo memberInfo, Type memberType, object memberTarget)
        {
            Info = memberInfo;
            Type = memberType;
            Target = memberTarget;
            Name = memberInfo.Name;
        }

        /// <summary>
        /// Returns false if the field was constant (literal) while setting 'result' to null.
        /// Otherwise true while setting result to a new RuntimeMember wrapping the specified field
        /// using the appropriate method of building the [s|g]etters (delegates in case of editor/standalone, reflection otherwise)
        /// </summary>
        public static bool TryWrapField(FieldInfo field, object target, out X20Member result)
        {
            if (field.IsLiteral)
            { 
                result = null;
                return false;
            }

            result = new X20Member(field, field.FieldType, target);

#if FAST_REFLECTION
            result._setter = field.DelegateForSet();
            result._getter = field.DelegateForGet();
#else
            result._setter = field.SetValue;
            result._getter = field.GetValue;
#endif
            return true;
        }

        /// <summary>
        /// Returns false if the property isn't readable or if it's an indexer, setting 'result' to null in the process.
        /// Otherwise true while setting result to a new RuntimeMember wrapping the specified property
        /// using the appropriate method of building the [s|g]etters (delegates in case of editor/standalone, reflection otherwise)
        /// Note that readonly properties (getter only) are fine, as the setter will just be an empty delegate doing nothing.
        /// </summary>
        public static bool TryWrapProperty(PropertyInfo property, object target, out X20Member result)
        {
            if (!property.CanRead || property.IsIndexer())
            {
                result = null;
                return false;
            }

            result = new X20Member(property, property.PropertyType, target);

            if (property.CanWrite)
            {
#if FAST_REFLECTION
                result._setter = property.DelegateForSet();
#else
                result._setter = (x, y) => property.SetValue(x, y, null);
#endif
            }
#if FAST_REFLECTION
            else result._setter = delegate(ref object obj, object value) { };
#else
            else result._setter = (x, y) => { };
#endif

#if FAST_REFLECTION
            result._getter = property.DelegateForGet();
#else
            result._getter = x => property.GetValue(x, null);
#endif
            return true;
        }

        /// <summary>
        /// Returns a list of RuntimeMember wrapping whatever is valid from the input members IEnumerable in the specified target
        /// </summary>
        public static List<X20Member> WrapMembers(IEnumerable<MemberInfo> members, object target)
        {
            var result = new List<X20Member>();
            foreach (var member in members)
            { 
                var wrapped = WrapMember(member, target);
                if (wrapped != null)
                    result.Add(wrapped);
            }
            return result;
        }

        /// <summary>
        /// Tries to wrap the specified member.
        /// Returns the wrapped result if it succeeds (valid field/property)
        /// otherwise null
        /// </summary>
        public static X20Member WrapMember(MemberInfo member, object target)
        {
            var field = member as FieldInfo;
            if (field != null)
            {
                X20Member wrappedField;
                if (X20Member.TryWrapField(field, target, out wrappedField))
                    return wrappedField;
            }
            else
            {
                var property = member as PropertyInfo;
                if (property == null)
                    return null;

                X20Member wrappedProperty;
                if (X20Member.TryWrapProperty(property, target, out wrappedProperty))
                    return wrappedProperty;
            }

            return null;
        }

        /// <summary>
        /// Allocates a new array wrapping the specified members (not cached)
        /// </summary>
        public static X20Member[] WrapMembers(Type type, params string[] memberNames)
        {
            //@Todo memoize, somehow...
            var result = new X20Member[memberNames.Length];
            int added = 0;
            for (int i = 0; i < memberNames.Length; i++)
            {
                var name = memberNames[i];
                var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
                var member = type.GetMemberFromAll(name, flags);
                if (member == null)
                {
                    Debug.Log("Couldn't find member: " + name + " inside: " + type.Name);
                    continue;
                }

                var wrapped = WrapMember(member, null);
                if (wrapped == null)
                { 
                    Debug.Log("Couldn't wrap member: " + name);
                    continue;
                }

                result[added++] = wrapped;
            }

            if (added != memberNames.Length)
                Array.Resize(ref result, added);

            return result;
        }

        private static Func<Type, List<X20Member>> _cachedWrapMembers;
        /// <summary>
        /// A cached overload of WrapMembers that returns a list of RuntimeMembers with no target specified (null)
        /// wrapping whatever valid members there are in the specified type argument
        /// (uses cached reflection to get the members)
        /// </summary>
        public static List<X20Member> CachedWrapMembers(Type type)
        {
            if (_cachedWrapMembers == null)
                _cachedWrapMembers = new Func<Type, List<X20Member>>(x =>
                {
                    var members = X20Reflection.CachedGetMembers(x);
                    return X20Member.WrapMembers(members, null);
                }).Memoize();
            return _cachedWrapMembers(type);
        }

        public override string ToString()
        {
            return Type.Name + " " + Name;
        }

        public override int GetHashCode()
        {
            return Info.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var member = obj as X20Member;
            return member != null && this.Info == member.Info;
        }
    }
}
