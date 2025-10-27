using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using COM3D2.SimpleUI.Events;


namespace COM3D2.SimpleUI
{
    public interface IToolbar: IControl, IIntControlValue
    {
        Color defaultColor { get; set; }
        Color selectedColor { get; set; }
        Color hoverColor { get; set; }
        Color disabledColor { get; set; }
    }
}
