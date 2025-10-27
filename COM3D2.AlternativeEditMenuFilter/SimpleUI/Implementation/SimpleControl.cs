using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace COM3D2.SimpleUI.Implementation
{
    public abstract class SimpleControl : MonoBehaviour, IControl, ILayoutComponent
    {
        Vector2 _size;
        Vector2 _position;
        string _text;
        UITexture _texture;
        bool _dirty = true;

        BaseLayout _parent;

        public Vector2 size {
            get => _size;
            set
            {
                this.SetSize(value, true);
            }
        }

        public Vector2 position {
            get => _position;
            set
            {
                this.SetPosition(value, true);
            }
        }

        public string text
        {
            get => this._text;
            set
            {
                this._text = value;
                this.SetDirty();
            }
        }

        public UITexture texture
        {
            get => this._texture;
            set
            {
                this._texture = value;
                this.SetDirty();
            }
        }

        public string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        public string tooltip { get; set; }

        public void SetSize(Vector2 size, bool triggerLayout)
        {
            this._size = size;
            this.SetDirty();
            if (triggerLayout)
            {
                this._parent.SetDirty();
            }
        }

        public void SetPosition(Vector2 position, bool triggerLayout)
        {
            this._position = position;
            if (triggerLayout)
            {
                this._parent.SetDirty();
            }
        }

        public void Init(BaseLayout parent)
        {
            this.gameObject.name = this.GetType().Name;
            this._parent = parent;
            this.InitControl();
        }

        public void SetDirty()
        {
            this._dirty = true;
        }

        public void Update()
        {
            if (this._dirty)
            {
                this._dirty = false;
                this.UpdateUI();
            }
        }

        public abstract void UpdateUI();
        public abstract void InitControl();

        public void Remove()
        {
            if(_parent)
            {
                this._parent.Remove(this);
                this._parent = null;
            }
            Destroy(this.gameObject);
        }

        void OnDestroy()
        {
            if(_parent)
            {
                this._parent.Remove(this);
            }
        }

        public virtual bool Visible
        {
            get
            {
                return this.gameObject.activeSelf;
            }
            set
            {
                this.gameObject.SetActive(value);
                this._parent.SetDirty();
            }
        }
    }
}
