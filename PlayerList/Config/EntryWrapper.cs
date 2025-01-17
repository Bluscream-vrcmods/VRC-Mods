﻿using System;
using MelonLoader;
using PlayerList.Utilities;

namespace PlayerList.Config 
{
    public class EntryWrapper
    {
        public event Action OnValueChangedUntyped;

        protected void RunOnValueChangedUntyped() => OnValueChangedUntyped?.SafeInvoke();
    }
    public class EntryWrapper<T> : EntryWrapper
    {
        public MelonPreferences_Entry<T> pref;

        public T Value
        {
            get { return pref.Value; }
            set 
            {
                if (!value.Equals(Value))
                {
                    OnValueChanged?.SafeInvoke(pref.Value, value);
                    pref.Value = value;
                    RunOnValueChangedUntyped();
                }
            }
        }

        public event Action<T, T> OnValueChanged;
    }
}
