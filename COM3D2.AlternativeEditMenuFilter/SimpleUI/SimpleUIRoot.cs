using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


using COM3D2.SimpleUI.Implementation;

namespace COM3D2.SimpleUI
{

    public class SimpleUIRoot
    {
        public static IFixedLayout Create()
        {
            GameObject uiroot = GameObject.Find("SystemUI Root");
            var go = NGUITools.AddChild<SimpleFixedLayout>(uiroot);
            go.InitRoot();
            return go;
        }

        public static IFixedLayout Create(GameObject parent, int width, int height)
        {
            var layout = NGUITools.AddChild<SimpleFixedLayout>(parent);
            layout.Init(null);
            layout.gameObject.GetComponent<UIPanel>().depth = 1000;
            return layout;
        }
    }
}
