// <copyright file="EnumMemberViewModel.cs" company="Callie LeFave">
// Copyright (c) Callie LeFave 2019
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.
// </copyright>

using System;
using System.Reactive;
using System.Reflection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace ObjectEditor
{
    public class EnumMemberViewModel<T> : BaseMemberViewModel<T>
        where T : System.Enum
    {
        public EnumMemberViewModel(string name, T value = default)
            : base(name, value)
        {
            this.ChosenValue = this.Member.Value;

            this.WhenAnyValue<EnumMemberViewModel<T>, bool, T>(
                x => x.ChosenValue,
                (chosenValue) => !chosenValue.Equals(this.Member.Value))
                .ToPropertyEx(this, x => x.IsModified, initialValue: false);
        }

        public string[] Names
            => Enum.GetNames(typeof(T));

        public T[] Values
            => (T[])Enum.GetValues(typeof(T));

        [Reactive]
        public T ChosenValue { get; set; }

        protected override void Flush()
            => this.Member = new MemberModel<T>(this.Member.Name, this.ChosenValue);

        protected override void Clear()
            => this.ChosenValue = this.Member.Value;
    }
}
