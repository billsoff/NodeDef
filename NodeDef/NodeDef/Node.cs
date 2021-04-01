using System.Collections.Generic;

namespace NodeDef
{
    public class Node<TElement> where TElement : IElement, new()
    {
        public Node(TElement elemnt, string code)
        {
            Element = elemnt;
            Code = code;
        }

        public Node(Node<TElement> parent, TElement element, string code)
            : this(element, code)
        {
            Parent = parent;
        }

        private Dictionary<string, TElement> _lookup;

        private Dictionary<string, TElement> Lookup
        {
            get
            {
                if (_lookup == null)
                {
                    _lookup = new Dictionary<string, TElement>();
                }

                return _lookup;
            }
        }

        public TElement Element { get; private set; }

        public Node<TElement> Parent { get; private set; }

        private List<Node<TElement>> _children;

        public List<Node<TElement>> Children
        {
            get
            {
                if (_children == null)
                {
                    _children = new List<Node<TElement>>();
                }

                return _children;
            }
        }

        public bool HasChildren
        {
            get
            {
                return (_children == null) || (_children.Count == 0);
            }
        }

        public string Code { get; private set; }

        public bool OpenChild(string code)
        {
            if (Lookup.ContainsKey(code))
            {
                return false;
            }

            Node<TElement> child = new Node<TElement>(this, new TElement(), code)
            {
                _lookup = Lookup
            };

            Children.Add(child);
            Lookup.Add(code, child.Element);

            return true;
        }

        public void Close()
        {
            if (HasChildren)
            {
                foreach (Node<TElement> child in Children)
                {
                    child.Close();
                }
            }

            Element.Close();

            if (HasChildren)
            {
                _children = null;
            }

            if (Parent != null)
            {
                Parent.Children.Remove(this);

                //if (Parent.Children.Count == 0)
                //{
                //    Parent.Close();
                //}
            }
        }
    }
}