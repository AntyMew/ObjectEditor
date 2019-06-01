// <copyright file="IMemberViewModel.cs" company="Callie LeFave">
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
    /// Base class for a view model presenting a <see cref="IMemberModel"/>.
    /// </summary>
    public interface IMemberViewModel
    {
        IMemberModel Member { get; }

        ReactiveCommand<Unit, Unit> FlushChanges { get; }

        ReactiveCommand<Unit, Unit> ClearChanges { get; }

        bool IsModified { get; }
    }
}