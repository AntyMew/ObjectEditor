// <copyright file="StringMemberViewModel.cs" company="Callie LeFave">
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
    public class StringMemberViewModel : BaseMemberViewModel<string>
    {
        public StringMemberViewModel(string name, string value = default)
            : base(name, value)
        {
            this.TextField = this.Member.Value;

            this.WhenAnyValue(
                x => x.HasFocus,
                x => x.TextField,
                (hasFocus, textField) => !(string.IsNullOrEmpty(textField) || textField == this.Member.Value) || hasFocus)
                .ToPropertyEx(this, x => x.IsModified, initialValue: false);
        }

        [Reactive]
        public string TextField { get; set; }

        protected override void Flush() => this.Member = new MemberModel<string>(this.Member.Name, this.TextField);

        protected override void Clear() => this.TextField = this.Member.Value;
    }
}
