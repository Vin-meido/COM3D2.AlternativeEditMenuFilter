using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using UnityEngine.Events;

namespace COM3D2.SimpleUI
{
    public interface IControlValue<T>
    {
        T Value { get; set; }

        void AddChangeCallback(UnityAction<T> callback);

        void RemoveChangeCallback(UnityAction<T> callback);
    }

    public interface IBoolControlValue: IControlValue<bool>
    {

    }

    public interface IStringControlValue: IControlValue<string>
    {

    }

    public interface IIntControlValue: IControlValue<int>
    {

    }

    public interface IFloatControlValue: IControlValue<float>
    {

    }

    public interface IObjectControlValue: IControlValue<object>
    {

    }
}
