using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace COM3D2.AlternativeEditMenuFilter
{
    public class PresetPanelItem
    {
        public GameObject gameObject;

        public bool Visible
        {
            get => gameObject.activeSelf;
            set => gameObject.SetActive(value);
        }

        GameObject Label
        {
            get
            {
                UITexture component = gameObject.GetComponent<UITexture>();
                if (component)
                {
                    var childLabel = gameObject.GetComponentInChildren<UILabel>();
                    if (childLabel)
                    {
                        return childLabel.gameObject;
                    }
                    else
                    {
                        Font font = GameObject.Find("SystemUI Root").GetComponentsInChildren<UILabel>()[0].trueTypeFont;
                        UILabel uilabel = NGUITools.AddChild<UILabel>(gameObject);
                        if (uilabel)
                        {
                            uilabel.trueTypeFont = font;
                            uilabel.fontSize = 22;
                            uilabel.width = component.width;
                            uilabel.height = component.height;
                            uilabel.pivot = UIWidget.Pivot.TopLeft;
                            uilabel.overflowMethod = UILabel.Overflow.ResizeHeight;
                            uilabel.effectStyle = UILabel.Effect.Outline;
                            uilabel.text = "[00FF00]" + gameObject.name.Replace(".preset", "");
                            uilabel.depth = component.depth + 2;
                            return uilabel.gameObject;
                        }

                    }
                }
                return null;
            }
        }

        public string Name
        {
            get => gameObject.name;
        }


        public bool IsClothesBody { get; private set; }
        public bool IsClothes { get; private set; }
        public bool IsBody { get; private set; }

        public PresetPanelItem(GameObject go)
        {
            this.gameObject = go;

            UISprite typeUISprite = go.GetComponentInChildren<UISprite>(true);
            var spriteName = typeUISprite.spriteName;

            this.IsClothesBody = spriteName.Contains("kindicon_clothes_body");
            this.IsClothes = spriteName.Contains("kindicon_clothes");
            this.IsBody = spriteName.Contains("kindicon_body");
        }

        public void ShowName()
        {
            Label.SetActive(true);
        }

        public void HideName()
        {
            Label.SetActive(false);
        }
    }
}
