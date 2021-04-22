using System;

namespace Parsifal.Math
{
    using Math = System.Math;
    /// <summary>
    /// 转换辅助
    /// </summary>
    public class ConversionHelper
    {
        /// <summary>
        /// π
        /// </summary>
        public const double Pi = Math.PI;
        /// <summary>
        /// 2π
        /// </summary>
        public const double TwoPi = 2d * Math.PI;

        /// <summary>
        /// 角转换到[-π, π]
        /// </summary>
        /// <param name="angle">角</param>
        /// <returns>在[-π, π]区间对应的角</returns>
        public static Angle AngleWrapToPi(Angle angle)
        {
            return new Angle(RadianWrapToPi(angle.Radians));
        }
        /// <summary>
        /// 角转换到[0, 2π]
        /// </summary>
        /// <param name="angle">角</param>
        /// <returns>在[, 2π]区间对应的角</returns>
        public static Angle AngleWrapTo2Pi(Angle angle)
        {
            return new Angle(RadianWrapTo2Pi(angle.Radians));
        }
        /// <summary>
        /// 弧度转角度
        /// </summary>
        /// <param name="radian">弧度</param>
        /// <returns>角度</returns>
        public static double RadianToDegree(double radian)
        {
            return radian * (180d / Pi);
        }
        /// <summary>
        /// 角度转弧度
        /// </summary>
        /// <param name="degree">角度</param>
        /// <returns>弧度</returns>
        public static double DegreeToRadian(double degree)
        {
            return degree * (Pi / 180d);
        }
        /// <summary>
        /// 角转换到[-π, π]
        /// </summary>
        /// <param name="radian">弧度</param>
        /// <returns>在[-π, π]区间对应的角</returns>
        public static double RadianWrapToPi(double radian)
        {
            var rad = Math.IEEERemainder(radian, TwoPi);
            if (rad <= -Pi)
                rad += TwoPi;
            else if (rad > Pi)
                rad -= TwoPi;
            return rad;
        }
        /// <summary>
        /// 角转换到[0, 2π]
        /// </summary>
        /// <param name="radian">弧度</param>
        /// <returns>在[, 2π]区间对应的角</returns>
        public static double RadianWrapTo2Pi(double radian)
        {
            var rad = Math.IEEERemainder(radian, Pi);
            if (rad < 0d)
                rad += TwoPi;
            return rad;
        }
        /// <summary>
        /// 角转换到[-180, 180]
        /// </summary>
        /// <param name="degree">角度</param>
        /// <returns>在[-180, 180]区间对应的角</returns>
        public static double DegreeWrapTo180(double degree)
        {
            var deg = Math.IEEERemainder(degree, 360d);
            if (deg <= -180)
                deg += 360;
            else if (deg > 180)
                deg -= 360;
            return deg;
        }
        /// <summary>
        /// 角转换到[0, 360]
        /// </summary>
        /// <param name="degree">角度</param>
        /// <returns>在[0, 360]区间对应的角</returns>
        public static double DegreeWrapTo360(double degree)
        {
            var deg = Math.IEEERemainder(degree, 360d);
            if (deg < 0)
                deg += 360d;
            return deg;
        }
        /// <summary>
        /// 度转度分
        /// </summary>
        /// <remarks>注意输出格式:度分格式的分在整数部分中占两位</remarks>
        /// <param name="angleInDegree">度形式的角,格式:ddd.mmmmmm</param>
        /// <returns>角度的度分值,格式:dddmm.mmmm</returns>
        /// <exception cref="ArgumentOutOfRangeException">角度过大或过小</exception>
        public static double DegreeToDM(double angleInDegree)
        {
            if (angleInDegree > int.MaxValue || angleInDegree < int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(angleInDegree));
            var intPart = Math.Truncate(angleInDegree);//整数部分
            var decPart = Math.Abs(angleInDegree - intPart);//小数部分,可能存在精度丢失
            return intPart * 100 + Math.Sign(angleInDegree) * (decPart * 60);
        }
        /// <summary>
        /// 度分转度
        /// </summary>
        /// <remarks>注意输入格式:度分格式的分在整数部分中占两位</remarks>
        /// <param name="angleInDM">度分形式的角,格式:dddmm.mmmm</param>
        /// <returns>角的度值,格式:ddd.mmmmmm</returns>
        /// <exception cref="ArgumentOutOfRangeException">角度过大或过小</exception>
        public static double DMToDegree(double angleInDM)
        {
            if (angleInDM > int.MaxValue || angleInDM < int.MinValue)
                throw new ArgumentOutOfRangeException(nameof(angleInDM));
            var degree = Math.Truncate(angleInDM / 100);//度
            var minute = Math.Abs(angleInDM - degree * 100);//分
            return degree + Math.Sign(angleInDM) * (minute / 60);
        }
    }
}
