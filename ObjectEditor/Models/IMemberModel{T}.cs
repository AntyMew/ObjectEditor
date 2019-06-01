// <copyright file="IMemberModel{T}.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;

namespace ObjectEditor
{
    /// <summary>
    /// A model representing a member of a class or a struct.
    /// </summary>
    /// <typeparam name="T">The type represented by the <see cref="IMemberModel{T}"/>.</typeparam>
    public interface IMemberModel<out T> : IMemberModel
    {
        /// <summary>
        /// Gets the value of the represented member.
        /// </summary>
        new T Value { get; }
    }
}
