using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace COM3D2.SimpleUI
{
    public interface IGenericDropdown : IControl, IObjectControlValue
    {
        IGenericDropdown ClearChoices();
        IGenericDropdown Choice<T>(T value, string text="", string selected="");
        IGenericDropdown RemoveChoice<T>(T value);

        T GetValue<T>();
        IGenericDropdown SetValue<T>(T value);

        bool UpdateTextOnValue { get; set; }

        IGenericDropdown SetUpdateTextOnValuechange(bool value);
    }

    public interface IDropdown: IControl, IStringControlValue
    {
        IEnumerable<string> Choices { get; set; }
    }
}
