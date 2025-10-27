using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace COM3D2.SimpleUI.Extensions
{
    public static class IControlExtensions
    {
        public static R VisibleWhen<R>(this R instance, IBoolControlValue target, bool value=true)
            where R: IControl
        {
            target.AddChangeCallback(v =>
            {
                instance.Visible = (v == value);
            });

            return instance;
        }

        public static R VisibleWhen<R>(this R instance, IStringControlValue target, string value, bool equal=true)
            where R : IControl
        {
            target.AddChangeCallback(v =>
            {
                instance.Visible = equal ? v == value : v != value;
            });

            return instance;
        }


        public static R VisibleWhen<R>(this R instance, IStringControlValue target, Func<string, bool> condition)
            where R : IControl
        {
            target.AddChangeCallback(v =>
            {
                instance.Visible = condition(v);
            });

            return instance;
        }

        public static R VisibleWhen<R>(this R instance, IIntControlValue target, int value, bool equal=true)
            where R : IControl
        {
            target.AddChangeCallback(v =>
            {
                instance.Visible = equal ? v == value : v != value;
            });

            return instance;
        }

        public static R VisibleWhen<R>(this R instance, IIntControlValue target, Func<int, bool> condition)
            where R : IControl
        {
            target.AddChangeCallback(v =>
            {
                instance.Visible = condition(v);
            });

            return instance;
        }

        public static R VisibleWhen<R>(this R instance, IFloatControlValue target, Func<float, bool> condition)
            where R : IControl
        {
            target.AddChangeCallback(v =>
            {
                instance.Visible = condition(v);
            });

            return instance;
        }
    }

}
