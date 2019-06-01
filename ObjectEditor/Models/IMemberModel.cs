// <copyright file="IMemberModel.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.ComponentModel;
using System.Reflection;
using ReactiveUI;

namespace ObjectEditor
{
    /// <summary>
    /// A model representing a member of a class or a struct.
    /// </summary>
    public interface IMemberModel
    {
        /// <summary>
        /// Gets the type of the represented member.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the unqualified name of the represented member.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the value of the represented member.
        /// </summary>
        object Value { get; }
    }
}
