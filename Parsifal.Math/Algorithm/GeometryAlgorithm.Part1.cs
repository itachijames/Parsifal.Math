using Parsifal.Math.Geometry;

namespace Parsifal.Math.Algorithm
{
    public partial class GeometryAlgorithm
    {
        /// <summary>
        /// 是否共线
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="third"></param>
        /// <returns>共线时返回true;否则false</returns>
        public static bool IsCollinearity(Point2D first, Point2D second, Point2D third)
        {
            //共线条件：(x2-x1)(y3-y1)=(x3-x1)(y2-y1)
            var temp1 = (second.X - first.X) * (third.Y - first.Y);
            var temp2 = (third.X - first.X) * (second.Y - first.Y);
            return temp1.IsEqual(temp2);
        }
        /// <summary>
        /// 是否为凸多边形
        /// </summary>
        /// <param name="vertexes">多边形顶点</param>
        /// <returns>如果是凸多边形则返回true;否则false</returns>
        public static bool IsConvexPolygon(Point2D[] vertexes)
        {
            //任意两领边所组成的向量做叉乘，如果符号全部相同，则为凸多边形，有任一异号则为凹多边形
            int count = vertexes.Length;
            Point2D current, pre, next;
            int preCrossSign = 0;//上一叉积的符号，由于相邻三点不共线则不可能为0
            for (int i = 0; i < count; i++)
            {
                current = vertexes[i];
                if (i == 0)
                    pre = vertexes[count - 1];
                else
                    pre = vertexes[i - 1];
                if (i == count - 1)
                    next = vertexes[0];
                else
                    next = vertexes[i + 1];

                var v1 = current - pre;
                var v2 = next - current;
                var cross = Vector2D.CrossProduct(v1, v2);
                if (i == 0)
                    preCrossSign = System.Math.Sign(cross);
                else
                {
                    if (!cross.IsSameSign(preCrossSign))//分别与第一次做符号对比
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 多边形是否自相交
        /// </summary>
        /// <param name="vertexes">多边形顶点</param>
        /// <returns>如果自相交则返回true;否则false</returns>
        public static bool IsSelfIntersected(Point2D[] vertexes)
        {
            int count = vertexes.Length;
            if (count <= 3)
                return false;
            int j, k, l, m;
            int comparaTime = (int)System.Math.Ceiling(count / 2d);//仅需比较半数的边
            for (int i = 0; i < comparaTime; i++)
            {
                //每条边分别与非邻边的边做相交判断，最大比较次数 n(n-3)/2
                j = (i == count - 1) ? 0 : i + 1;
                var currentSegment = (vertexes[i], vertexes[j]);
                k = (j == count - 1) ? 0 : j + 1;
                for (m = 0; m < count - 3; m++, k = (k == (count - 1)) ? 0 : k + 1)
                {
                    l = (k == count - 1) ? 0 : k + 1;
                    var compareSegment = (vertexes[k], vertexes[l]);
                    if (IsSegmentIntersection(currentSegment, compareSegment))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
