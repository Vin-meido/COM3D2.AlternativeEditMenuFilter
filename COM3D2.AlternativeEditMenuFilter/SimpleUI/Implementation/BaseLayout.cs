using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace COM3D2.SimpleUI.Implementation
{
    public abstract class BaseLayout : MonoBehaviour, ILayoutComponent
    {
        protected readonly List<ILayoutComponent> layoutComponents = new List<ILayoutComponent>();

        protected bool _dirty = true;

        protected Vector2 _position;
        public virtual Vector2 position
        {
            get => _position;
            set
            {
                SetPosition(value, true);
            }
        }

        protected Vector2 _size;
        public virtual Vector2 size
        {
            get => _size;
            set
            {
                SetSize(value, true);
            }
        }

        protected BaseLayout _parent;

        protected UIDragObject uiDragObj;
        protected BoxCollider boxCollider;
        protected UIPanel uiPanel;

        readonly UnityEvent onLayout = new UnityEvent();

        public virtual int width => Mathf.FloorToInt(_size.x + 0.5f);

        public virtual int height => Mathf.FloorToInt(_size.y + 0.5f);

        public virtual int contentWidth
        {
            get; protected set;
        }

        public virtual int contentHeight
        {
            get; protected set;
        }

        public string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        protected abstract void Relayout();

        public void Init(BaseLayout parent)
        {
            this.gameObject.name = this.GetType().Name;
            UnityEngine.Debug.Log($"Name is {this.gameObject.name}");
            var parentPanel = gameObject.GetComponentInParent<UIPanel>();
            uiPanel = this.gameObject.AddComponent<UIPanel>();
            uiPanel.depth = parentPanel.depth + 1;

            this._parent = parent;
            this.InitLayout();
        }

        public virtual void InitLayout()
        {
        }

        protected T Child<T>() where T: MonoBehaviour, ILayoutComponent
        {
            GameObject go = NGUITools.AddChild(this.gameObject);
            T component = go.AddComponent<T>();
            component.Init(this);
            this.layoutComponents.Add(component);
            SetDirty();
            return component;
        }

        protected T Child<T>(Vector2 size) where T : MonoBehaviour, ILayoutComponent
        {
            var control = Child<T>();
            control.SetSize(size, false);
            return control;
        }

        protected T Child<T>(Rect rect) where T : MonoBehaviour, ILayoutComponent
        {
            var control = Child<T>(new Vector2(rect.width, rect.height));
            control.SetPosition(new Vector2(rect.x, rect.y), false);
            return control;
        }

        public IEnumerable<T> GetChildren<T>() where T : IControl
        {
            foreach (var component in layoutComponents)
            {
                if (component is T control)
                {
                    yield return control;
                }
            }
        }

        public IEnumerable<IControl> GetChildren()
        {
            return GetChildren<IControl>();
        }

        public virtual void SetDirty()
        {
            this._dirty = true;
        }

        protected virtual void Update()
        {
            if (this._dirty)
            {
                UnityEngine.Debug.Log($"{this} dirty");

                this._dirty = false;

                if(boxCollider != null)
                {
                    boxCollider.size = this.size;
                }

                this.Relayout();
                this.onLayout.Invoke();
            }
        }

        public virtual void SetSize(Vector2 size, bool triggerLayout)
        {
            this._size = size;
            SetDirty();
            if(triggerLayout && _parent != null)
            {
                _parent.SetDirty();
            }
        }

        public virtual void SetPosition(Vector2 position, bool triggerLayout)
        {
            this._position = position;
            SetDirty();
            if (triggerLayout && _parent != null)
            {
                _parent.SetDirty();
            }
        }

        public virtual void Remove(ILayoutComponent component)
        {
            var components = layoutComponents
                .Find(c => c == component);

            if (layoutComponents.Contains(component))
            {
                layoutComponents.Remove(component);
                Destroy(component.gameObject);
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

        public bool Draggable
        {
            get
            {
                return uiDragObj != null && uiDragObj.enabled;
            }
            set
            {
                if (!(this._parent is SimpleFixedLayout))
                {
                    UnityEngine.Debug.Log("Dragging ended");
                    return;
                }

                if (value && uiDragObj is null)
                {
                    SetupDraggable();
                }
                
                if(uiDragObj != null)
                {
                    uiDragObj.enabled = value;
                    SetDirty();
                }
            }
        }

        protected void SetupDraggable()
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
            uiDragObj = gameObject.AddComponent<UIDragObject>();
            uiDragObj.target = this.gameObject.transform;
            uiDragObj.dragEffect = UIDragObject.DragEffect.None;
            uiDragObj.contentRect = this.uiPanel;
            uiDragObj.panelRegion = this._parent.uiPanel;

            var listener = gameObject.AddComponent<UIEventListener>();
            var wasDragging = false;
            listener.onDrag = (go, v) =>
            {
                wasDragging = true;
            };
            listener.onPress = (go, pressed) =>
            {
                if (!pressed && wasDragging)
                {
                    wasDragging = false;
                    this._position = this.gameObjectLocalToPosition;
                }
            };
        }

        protected Vector2 gameObjectLocalToPosition
        {
            get
            {
                return _parent.TransformLocalToPostion(this.gameObject.transform.localPosition, this.size);
            }
        }

        protected Vector2 TransformLocalToPostion(Vector3 local, Vector2 size)
        {
            var posx = local.x + (this._size.x / 2f) - (size.x / 2f);
            var posy = -(local.y - (this._size.y / 2f) + (size.y / 2f));
            return new Vector2(posx, posy);
        }

        protected Vector3 PostionToLocalTransform(Vector2 position, Vector2 size)
        {
            var xlocal = -(this._size.x / 2f) + (size.x / 2f) + position.x;
            var ylocal = (this._size.y / 2f) - (size.y / 2f) - position.y;
            return new Vector3(xlocal, ylocal);

        }

        public void AddLayoutCallback(UnityAction callback)
        {
            this.onLayout.AddListener(callback);
        }

        public void RemoveLayoutCallback(UnityAction callback)
        {
            this.onLayout.RemoveListener(callback);
        }
    }
}
