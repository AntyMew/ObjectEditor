// <copyright file="Utilities.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.ComponentModel;
using System.Reflection;

namespace ObjectEditor.Common
{
    public static class Utilities
    {
        public static IMemberViewModel<T> CreateViewModel<T>(IMemberModel<T> member)
        {
            return CreateViewModel(member.Name, member.Value);
        }

        public static IMemberViewModel<T> CreateViewModel<T>(string name, T value = default)
        {
            IMemberViewModel<T> viewModel = CreateNongenericViewModel(typeof(T), name, value) as IMemberViewModel<T>;
            return viewModel;
        }

        /// <summary>
        /// Creates a new <see cref="IMemberViewModel"/> from the passed <see cref="MemberInfo"/>. A derived type is
        /// automatically selected via reflection based on the type represented by the <see cref="MemberInfo"/>.
        /// </summary>
        /// <remarks>
        /// Initializes the contained <see cref="IMemberModel"/> to its <see cref="DefaultValueAttribute"/> if defined.
        /// </remarks>
        /// <param name="member">The member the new instance will represent.</param>
        /// <returns>The new <see cref="IMemberViewModel"/>.</returns>
        public static IMemberViewModel ToViewModel(this MemberInfo member)
        {
            Type type;
            string name;

            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    var eventInfo = member as EventInfo;
                    type = eventInfo.EventHandlerType;
                    name = eventInfo.Name;
                    break;
                case MemberTypes.Property:
                    var propInfo = member as PropertyInfo;
                    type = propInfo.PropertyType;
                    name = propInfo.Name;
                    break;
                case MemberTypes.Field:
                    var fieldInfo = member as FieldInfo;
                    type = fieldInfo.FieldType;
                    name = fieldInfo.Name;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported member type '{member.MemberType}'.");
            }

            var defaultAttr = (DefaultValueAttribute)member.GetCustomAttribute(typeof(DefaultValueAttribute));
            IMemberViewModel viewModel = CreateNongenericViewModel(type, name, defaultAttr?.Value);

            return viewModel;
        }

        internal static IMemberViewModel CreateNongenericViewModel(Type type, string name, object value = null)
        {
            IMemberViewModel ConstructGenericViewModel(Type genericType, Type modelType, string modelName, object modelValue)
            {
                Type constructedViewModel = genericType.MakeGenericType(modelType);
                ConstructorInfo ctor = constructedViewModel.GetConstructor(new Type[] { typeof(string), modelType });
                return ctor.Invoke(new object[] { modelName, modelValue }) as IMemberViewModel;
            }

            IMemberViewModel viewModel;

            if (type == typeof(string))
                viewModel = new StringMemberViewModel(name, (string)value);
            else if (type.IsPrimitive)
                viewModel = ConstructGenericViewModel(typeof(PrimitiveMemberViewModel<>), type, name, value);
            else if (type.IsEnum)
                viewModel = ConstructGenericViewModel(typeof(EnumMemberViewModel<>), type, name, value);
            else if (type.IsInterface)
                viewModel = ConstructGenericViewModel(typeof(InterfaceMemberViewModel<>), type, name, value);
            else if (type.IsValueType)
                throw new NotImplementedException("Custom struct type handling not yet implemented.");
            else if (type.IsClass)
                throw new NotImplementedException("Class type handling not yet implemented.");
            else
                throw new NotSupportedException($"Unsupported type '{type}'.");

            return viewModel;
        }
    }
}