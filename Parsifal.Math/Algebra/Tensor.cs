using System;
using System.Collections;
using System.Collections.Generic;

namespace Parsifal.Math.Algebra
{
    /// <summary>
    /// 张量
    /// </summary>
    public partial class Tensor : IEnumerable<double>, IEquatable<Tensor>, ICloneable
    {
        #region field
        private readonly double[] _element;

        #endregion

        #region property
        public int Rank { get; private set; }
        #endregion

        #region constructor
        public Tensor(double[] element)
        {
            if (element is null)
                ThrowHelper.ThrowArgumentNullException(nameof(element));
            _element = element;
        }
        #endregion

        #region ICloneable
        public Tensor Clone()
        {
            throw new NotImplementedException();
        }
        object ICloneable.Clone()
        {
            return Clone();
        }
        #endregion

        #region IEquatable
        public bool Equals(Tensor other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            //todo
            return true;
        }
        #endregion

        #region IEnumerable
        public IEnumerator<double> GetEnumerator()
        {
            throw new NotImplementedException();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region public
        public override bool Equals(object obj) => obj is Tensor tensor && Equals(tensor);
        public override int GetHashCode() => HashCode.Combine(_element);
        #endregion
    }
}
