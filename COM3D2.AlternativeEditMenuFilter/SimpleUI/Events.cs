using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine.Events;

namespace COM3D2.SimpleUI.Events
{
    public class ToolbarSelectedEvent: UnityEvent<int>
    {
    }

    public class ToggleEvent: UnityEvent<bool>
    {

    }

    public class SliderEvent: UnityEvent<float>
    {

    }

    public class TextChangeEvent: UnityEvent<string>
    {

    }

    public class ClickEvent: UnityEvent
    {

    }
}
