using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace COM3D2.SimpleUI
{
    public interface ILayout
    {
        int contentWidth { get; }
        int contentHeight { get; }

        bool Draggable { get; set; }

        void AddLayoutCallback(UnityAction callback);

        void RemoveLayoutCallback(UnityAction callback);
    }
}
