using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Markup;

namespace AutoLayoutPanel
{
    [ContentProperty("Expressions")]
    public class LayoutConstraint : IList
    {
        #region Fields

        #endregion

        #region Constructors

        public LayoutConstraint()
        {
            Constant = 0d;
            Relationship = LayoutRelationship.EqualTo;
            Strength = LayoutConstraintStrength.Required;
            Expressions = new List<LayoutLinearExpression>();
        }

        #endregion

        #region Properties

        public LayoutProperty Property { get; set; }
        public double Constant { get; set; }
        public LayoutRelationship Relationship { get; set; }
        public LayoutConstraintStrength Strength { get; set; }
        public List<LayoutLinearExpression> Expressions { get; set; }

        #endregion

        #region Methods

        #endregion

        #region IList implementation


        #endregion

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable) Expressions).GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection) Expressions).CopyTo(array, index);
        }

        public int Count
        {
            get { return ((IList) Expressions).Count; }
        }

        public object SyncRoot
        {
            get { return ((IList) Expressions).SyncRoot; }
        }

        public bool IsSynchronized
        {
            get { return ((IList) Expressions).IsSynchronized; }
        }

        public int Add(object value)
        {
            return ((IList) Expressions).Add(value);
        }

        public bool Contains(object value)
        {
            return ((IList) Expressions).Contains(value);
        }

        public void Clear()
        {
            ((IList) Expressions).Clear();
        }

        public int IndexOf(object value)
        {
            return ((IList) Expressions).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList) Expressions).Insert(index, value);
        }

        public void Remove(object value)
        {
            ((IList) Expressions).Remove(value);
        }

        public void RemoveAt(int index)
        {
            ((IList) Expressions).RemoveAt(index);
        }

        public object this[int index]
        {
            get { return ((IList) Expressions)[index]; }
            set { ((IList) Expressions)[index] = value; }
        }

        public bool IsReadOnly
        {
            get { return ((IList) Expressions).IsReadOnly; }
        }

        public bool IsFixedSize
        {
            get { return ((IList) Expressions).IsFixedSize; }
        }
    }
}