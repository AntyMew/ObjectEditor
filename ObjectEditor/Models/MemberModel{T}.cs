// <copyright file="MemberModel{T}.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.ComponentModel;
using System.Reflection;
using ObjectEditor.Common;
using ReactiveUI;

namespace ObjectEditor
{
    /// <inheritdoc cref="IMemberModel"/>
    /// <typeparam name="T">The type of the represented member.</typeparam>
    public sealed class MemberModel<T> : IMemberModel<T>, IEquatable<MemberModel<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberModel{T}"/> class.
        /// </summary>
        /// <param name="name">The unqualified name of the represented member.</param>
        public MemberModel(string name, T value = default)
        {
            this.Name = name;
            this.Value = value;
        }

        /// <summary>
        /// Gets the type <typeparamref name="T"/>.
        /// </summary>
        public Type Type => typeof(T);

        /// <summary>
        /// Gets the unqualified name of the represented member.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets the value of the represented member.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the value of the represented member.
        /// </summary>
        object IMemberModel.Value
            => this.Value;

        /// <summary><see cref="Equals(MemberModel{T})"/>.</summary>
        /// <param name="lhs">Left-hand operand.</param>
        /// <param name="rhs">Right-hand operand.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="lhs"/> and <paranref name="rhs"/> are equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator ==(MemberModel<T> lhs, MemberModel<T> rhs)
            => lhs.Equals(rhs);

        /// <summary><see cref="Equals(MemberModel{T})"/>.</summary>
        /// <param name="lhs">Left-hand operand.</param>
        /// <param name="rhs">Right-hand operand.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="lhs"/> and <paranref name="rhs"/> are not equal; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool operator !=(MemberModel<T> lhs, MemberModel<T> rhs) => !lhs.Equals(rhs);

        public static MemberModel<TOut> Mutate<TOut>(IMemberModel<T> other, T value)
            where TOut : T => new MemberModel<TOut>(other.Name, (TOut)value);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="other">An object to compare to this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> is equal to this instance; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public bool Equals(MemberModel<T> other)
            => this.Name == other.Name && this.Value.Equals(other.Value);

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare to this instance.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of <see cref="MemberModel{T}"/> and equal
        /// to this instance; otherwise, <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
            => (obj is MemberModel<T>) && this.Equals(obj as MemberModel<T>);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A hash code for the current <see cref="MemberModel{T}"/>.</returns>
        public override int GetHashCode()
            => HashCode.Combine(this.Name, this.Value);
    }
}
