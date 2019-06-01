// <copyright file="PrimitiveMemberViewModel.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reflection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ObjectEditor
{
    /// <summary>
    /// A view model presenting a primitive type, or any other type which allows explicit conversion to and from a
    /// <see langword="string"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// <typeparamref name="T"/> must be convertible to and from <see langword="string"/>. To use a custom class as the
    /// type parameter, use <see cref="TypeConverterAttribute"/>.
    /// </remarks>
    public class PrimitiveMemberViewModel<T> : BaseMemberViewModel<T>
    {
        private static readonly IConverter<T, string> ConverterToString;
        private static readonly IConverter<string, T> ConverterFromString;

        static PrimitiveMemberViewModel()
        {
            MethodInfo FindConversionOperator<TIn, TOut>(IEnumerable<MethodInfo> methods)
            {
                return methods.First(method =>
                {
                    if (method.ReturnType != typeof(TOut))
                        return false;
                    Type inType = method.GetParameters()[0].ParameterType;
                    return inType == typeof(TIn) || inType == typeof(TIn).MakeByRefType();
                });
            }

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter.CanConvertTo(typeof(string)))
                ConverterToString = new TypeConverterToWrapper(converter);
            if (converter.CanConvertFrom(typeof(string)))
                ConverterFromString = new TypeConverterFromWrapper(converter);

            if (ConverterToString != null && ConverterFromString != null)
                return;

            var convOperators = typeof(T).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Where(x => x.Name == "op_Implicit" || x.Name == "op_Explicit");
            ConverterToString = ConverterToString ?? new ConversionOperatorWrapper<T, string>(FindConversionOperator<T, string>(convOperators));
            ConverterFromString = ConverterFromString ?? new ConversionOperatorWrapper<string, T>(FindConversionOperator<string, T>(convOperators));

            if (ConverterToString == null)
                throw new ArgumentException("Type parameter is not convertible to string.", nameof(T));
            if (ConverterFromString == null)
                throw new ArgumentException("Type parameter is not convertible from string.", nameof(T));
        }

        public PrimitiveMemberViewModel(string name, T value = default)
            : base(name, value)
        {
            this.TextField = Convert.ToString(this.Member.Value);

            this.WhenAnyValue(
                x => x.HasFocus,
                x => x.TextField,
                (hasFocus, textField) => !(string.IsNullOrEmpty(textField) || textField == ConverterToString.Convert(this.Member.Value)) || hasFocus)
                .ToPropertyEx(this, x => x.IsModified, initialValue: false);
        }

        private interface IConverter<TIn, TOut>
        {
            TOut Convert(TIn input);
        }

        [Reactive]
        public string TextField { get; set; }

        protected override void Flush() => this.Member = new MemberModel<T>(this.Member.Name, ConverterFromString.Convert(this.TextField));

        protected override void Clear() => this.TextField = ConverterToString.Convert(this.Member.Value);

        private class TypeConverterToWrapper : IConverter<T, string>
        {
            private readonly TypeConverter converter;

            public TypeConverterToWrapper(TypeConverter converter)
            {
                this.converter = converter;
            }

            public string Convert(T input)
                => converter.ConvertToInvariantString(input);
        }

        private class TypeConverterFromWrapper : IConverter<string, T>
        {
            private readonly TypeConverter converter;

            public TypeConverterFromWrapper(TypeConverter converter)
            {
                this.converter = converter;
            }

            public T Convert(string input)
                => (T)converter.ConvertFromInvariantString(input);
        }

        private class ConversionOperatorWrapper<TIn, TOut> : IConverter<TIn, TOut>
        {
            private readonly MethodInfo convOperator;
            private readonly bool inByRef;

            public ConversionOperatorWrapper(MethodInfo convOperator)
            {
                this.convOperator = convOperator;
            }

            public TOut Convert(TIn input)
                => (TOut)convOperator.Invoke(null, new object[] { input });
        }
    }
}
