using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace COM3D2.SimpleUI
{
    public interface IButton: IContentControl
    {
        bool isEnabled { get; set; }

        UnityEvent Click { get; }

        Color defaultColor { get; set; }
        Color hoverColor { get; set; }
        Color disabledColor { get; set; }
        Color activeColor { get; set; }
    }
}
