using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.SimpleUI.Implementation
{
    public interface ILayoutComponent
    {
        GameObject gameObject { get; }
        Vector2 position { get; }
        Vector2 size { get; }
        void SetSize(Vector2 size, bool triggerLayout);
        void SetPosition(Vector2 size, bool triggerLayout);
        void Init(BaseLayout parent);
        string Name { get; }
        bool Visible { get; }
    }
}
