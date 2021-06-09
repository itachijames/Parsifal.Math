namespace Parsifal.Math
{
    public enum AngleType
    {
        /// <summary>锐角</summary>
        /// <remarks>(0, π/2)</remarks>
        Acute,
        /// <summary>直角</summary>
        /// <remarks>π/2</remarks>
        Right,
        /// <summary>钝角</summary>
        /// <remarks>(π/2, π)</remarks>
        Obtuse,
        /// <summary>平角</summary>
        /// <remarks>π</remarks>
        Straight,
        /// <summary>反射角</summary>
        /// <remarks>(π, 2π)</remarks>
        Reflex,
        /// <summary>全角 </summary>
        /// <remarks>2π</remarks>
        Rotation
    }
}
