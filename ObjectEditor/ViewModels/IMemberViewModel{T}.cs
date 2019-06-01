// <copyright file="IMemberViewModel{T}.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.ComponentModel;
using System.Reactive;
using System.Reflection;
using System.Runtime.Serialization;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ObjectEditor
{
    /// <summary>
    /// Base class for a view model presenting a <see cref="IMemberModel{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the member represented by the presented <see cref="IMemberModel{T}"/>.</typeparam>
    public interface IMemberViewModel<T> : IMemberViewModel
    {
        IMemberModel<T> Member { get; set; }
    }
}
