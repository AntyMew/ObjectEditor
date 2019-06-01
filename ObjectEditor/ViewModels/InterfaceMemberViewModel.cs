// <copyright file="InterfaceMemberViewModel.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reflection;
using ObjectEditor.Common;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ObjectEditor
{
    public class InterfaceMemberViewModel<T> : BaseMemberViewModel<T>
    {
        public static readonly ImmutableHashSet<Type> Implementers = ImmutableHashSet.Create(AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => asm.ExportedTypes)
            .Where(t => typeof(T).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
            .ToArray());

        static InterfaceMemberViewModel()
        {
            if (!typeof(T).IsInterface)
                throw new ArgumentException($"Type parameter is not an interface type.", nameof(T));
        }

        public InterfaceMemberViewModel(string name, T value = default)
            : base(name, value)
        {
            if (value != default)
                this.Instance = Utilities.CreateViewModel(this.Member);

            this.WhenAnyValue<InterfaceMemberViewModel<T>, bool, IMemberModel<T>>(
                x => x.Instance.Member,
                (model) => model != this.Member)
                .ToPropertyEx(this, x => x.IsModified, initialValue: false);

            this.SelectImplementer = ReactiveCommand.Create<Type, Unit>(type => {
                if (Implementers.Contains(type))
                    this.Instance = Utilities.CreateNongenericViewModel(type, this.Member.Name) as IMemberViewModel<T>;
                return Unit.Default;
            });
        }

        [Reactive]
        public IMemberViewModel<T> Instance { get; set; }

        public ReactiveCommand<Type, Unit> SelectImplementer { get; }

        protected override void Flush()
            => this.Member = this.Instance.Member;

        protected override void Clear()
            => this.Instance = Utilities.CreateViewModel(this.Member);
    }
}
