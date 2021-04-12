using System;

namespace Parsifal.Math.Algebra
{
    public partial class Tensor
    {
        public static bool operator ==(Tensor left, Tensor right)
        {
            if (left is null || right is null)
                return false;
            return true;
        }
        public static bool operator !=(Tensor left, Tensor right)
        {
            return !(left == right);
        }
        public static Tensor operator +(Tensor left, Tensor right)
        {
            throw new NotImplementedException();
        }
        public static Tensor operator -(Tensor left, Tensor right)
        {
            throw new NotImplementedException();
        }
        public static Tensor operator *(Tensor tensor, double scalar)
        {
            throw new NotImplementedException();
        }
        public static Tensor operator *(double scalar, Tensor tensor)
        {
            return tensor * scalar;
        }
        public static Tensor operator /(Tensor left, double scalar)
        {
            throw new NotImplementedException();
        }
    }
}
