// <copyright file="MemberViewModel{T}.cs" company="Callie LeFave">
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
    /// Base class for a view model presenting a <see cref="MemberModel{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseMemberViewModel<T> : ReactiveObject, IMemberViewModel<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMemberViewModel{T}"/> class.
        /// </summary>
        /// <typeparam name="T">The type of the member the new instance will represent.</typeparam>
        /// <param name="name">The name of the member the new instance will represent.</param>
        protected BaseMemberViewModel(string name, T value)
        {
            this.Member = new MemberModel<T>(name, value);
            this.FlushChanges = ReactiveCommand.Create(this.Flush);
            this.ClearChanges = ReactiveCommand.Create(this.Clear);
        }

        public IMemberModel<T> Member { get; set; }

        IMemberModel IMemberViewModel.Member
            => this.Member;

        public ReactiveCommand<Unit, Unit> FlushChanges { get; }

        public ReactiveCommand<Unit, Unit> ClearChanges { get; }

        [ObservableAsProperty]
        public bool IsModified { get; }

        [Reactive]
        public bool HasFocus { get; set; }

        protected abstract void Flush();

        protected abstract void Clear();
    }
}
