using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Terraria;

namespace SummonHeart.Extensions
{
    // Token: 0x02000036 RID: 54
    public static class VectorHelper
    {
        // Token: 0x17000007 RID: 7
        // (get) Token: 0x06000161 RID: 353 RVA: 0x000108BE File Offset: 0x0000EABE
        public static Vector2 Random
        {
            get
            {
                return new Vector2(Main.rand.NextFloatRange(1f), Main.rand.NextFloatRange(1f));
            }
        }

        // Token: 0x17000008 RID: 8
        // (get) Token: 0x06000162 RID: 354 RVA: 0x000108E3 File Offset: 0x0000EAE3
        public static Vector2 Up
        {
            get
            {
                return new Vector2(0f, -1f);
            }
        }

        // Token: 0x17000009 RID: 9
        // (get) Token: 0x06000163 RID: 355 RVA: 0x000108F4 File Offset: 0x0000EAF4
        public static Vector2 UpLeft
        {
            get
            {
                return new Vector2(-1f, -1f);
            }
        }

        // Token: 0x1700000A RID: 10
        // (get) Token: 0x06000164 RID: 356 RVA: 0x00010905 File Offset: 0x0000EB05
        public static Vector2 UpRight
        {
            get
            {
                return new Vector2(1f, -1f);
            }
        }

        // Token: 0x1700000B RID: 11
        // (get) Token: 0x06000165 RID: 357 RVA: 0x00010916 File Offset: 0x0000EB16
        public static Vector2 Down
        {
            get
            {
                return new Vector2(0f, 1f);
            }
        }

        // Token: 0x1700000C RID: 12
        // (get) Token: 0x06000166 RID: 358 RVA: 0x00010927 File Offset: 0x0000EB27
        public static Vector2 DownLeft
        {
            get
            {
                return new Vector2(-1f, 1f);
            }
        }

        // Token: 0x1700000D RID: 13
        // (get) Token: 0x06000167 RID: 359 RVA: 0x00010938 File Offset: 0x0000EB38
        public static Vector2 DownRight
        {
            get
            {
                return new Vector2(1f, 1f);
            }
        }

        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000168 RID: 360 RVA: 0x00010949 File Offset: 0x0000EB49
        public static Vector2 Left
        {
            get
            {
                return new Vector2(-1f, 0f);
            }
        }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000169 RID: 361 RVA: 0x0001095A File Offset: 0x0000EB5A
        public static Vector2 Right
        {
            get
            {
                return new Vector2(1f, 0f);
            }
        }

        // Token: 0x0600016A RID: 362 RVA: 0x0001096C File Offset: 0x0000EB6C
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 VelocityToPoint(Vector2 A, Vector2 B, float speed)
        {
            Vector2 move = B - A;
            move *= speed / move.Length();
            if (!move.HasNaNs())
            {
                return move;
            }
            return Vector2.Zero;
        }

        // Token: 0x0600016B RID: 363 RVA: 0x000109A0 File Offset: 0x0000EBA0
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Angle(Vector2 a, Vector2 b)
        {
            return (b - a).ToRotation();
        }

        // Token: 0x0600016C RID: 364 RVA: 0x000109AE File Offset: 0x0000EBAE
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 RandomPointInArea(Vector2 A, Vector2 B)
        {
            return new Vector2(Main.rand.Next((int)A.X, (int)B.X) + 1, Main.rand.Next((int)A.Y, (int)B.Y) + 1);
        }

        // Token: 0x0600016D RID: 365 RVA: 0x000109EB File Offset: 0x0000EBEB
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 PermutateVelocity(float speedX, float speedY, double amount, float multiplierMin = 1f, float multiplierMax = 1f)
        {
            return new Vector2(speedX, speedY).RotatedByRandom(amount) * (multiplierMin != multiplierMax ? Main.rand.NextFloat(multiplierMin, multiplierMax) : multiplierMin);
        }

        // Token: 0x0600016E RID: 366 RVA: 0x00010A14 File Offset: 0x0000EC14
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PermutateVelocity(ref float speedX, ref float speedY, double amount, float multiplierMin = 1f, float multiplierMax = 1f)
        {
            Vector2 vector = PermutateVelocity(speedX, speedY, amount, multiplierMin, multiplierMax);
            speedX = vector.X;
            speedY = vector.Y;
        }

        // Token: 0x0600016F RID: 367 RVA: 0x00010A40 File Offset: 0x0000EC40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Distance(this Vector2 v, Vector2 To)
        {
            float dist = Vector2.Distance(v, To);
            if (!float.IsNaN(dist))
            {
                return dist;
            }
            return 0f;
        }

        // Token: 0x06000170 RID: 368 RVA: 0x00010A64 File Offset: 0x0000EC64
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 GetPosition(this Rectangle rect)
        {
            return new Vector2(rect.X, rect.Y);
        }

        // Token: 0x06000171 RID: 369 RVA: 0x00010A79 File Offset: 0x0000EC79
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Offseted(this Vector2 vec, float x, float y = 0f)
        {
            return new Vector2(vec.X + x, vec.Y + y);
        }

        // Token: 0x06000172 RID: 370 RVA: 0x00010A90 File Offset: 0x0000EC90
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Normal(this Vector2 vec)
        {
            return new Vector2(-vec.Y, vec.X);
        }

        // Token: 0x06000173 RID: 371 RVA: 0x00010AA4 File Offset: 0x0000ECA4
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector2 Normalized(this Vector2 vec)
        {
            return vec.SafeNormalize(Vector2.Zero);
        }

        // Token: 0x06000174 RID: 372 RVA: 0x00010AB1 File Offset: 0x0000ECB1
        public static Color ToColor(this Vector3 vec, float alpha = 1f)
        {
            return new Color(vec.X, vec.Y, vec.Z, alpha);
        }

        // Token: 0x06000175 RID: 373 RVA: 0x00010ACB File Offset: 0x0000ECCB
        public static Vector2 DirectionTo(this Vector2 origin, Vector2 target)
        {
            return Vector2.Normalize(target - origin);
        }

        // Token: 0x06000176 RID: 374 RVA: 0x00010ADC File Offset: 0x0000ECDC
        public static bool IsPastPosition(this Vector2 vel, Vector2 origin, Vector2 target)
        {
            return Math.Sign(vel.X) == origin.X.CompareTo(target.X) && Math.Sign(vel.Y) == origin.Y.CompareTo(target.Y);
        }

        // Token: 0x06000177 RID: 375 RVA: 0x00010B29 File Offset: 0x0000ED29
        public static void Deconstruct(this Vector2 vec, out float x, out float y)
        {
            x = vec.X;
            y = vec.Y;
        }

        // Token: 0x06000178 RID: 376 RVA: 0x00010B3C File Offset: 0x0000ED3C
        public static Vector2 Bezier(Vector2 p0, Vector2 p1, Vector2 p2, float t)
        {
            t = MathHelper.Clamp(t, 0f, 1f);
            float oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * p0 + 2f * oneMinusT * t * p1 + t * t * p2;
        }

        // Token: 0x06000179 RID: 377 RVA: 0x00010B90 File Offset: 0x0000ED90
        private static void RecursiveGetOptimizedDrawingPoints(Vector2 start, Vector2 firstCtrlPoint, Vector2 secondCtrlPoint, Vector2 end, List<Vector2> points, float distanceTolerance)
        {
            Vector2 pt12 = (start + firstCtrlPoint) / 2f;
            Vector2 pt13 = (firstCtrlPoint + secondCtrlPoint) / 2f;
            Vector2 pt14 = (secondCtrlPoint + end) / 2f;
            Vector2 pt15 = (pt12 + pt13) / 2f;
            Vector2 pt16 = (pt13 + pt14) / 2f;
            Vector2 pt17 = (pt15 + pt16) / 2f;
            Vector2 deltaLine = end - start;
            float d2 = Math.Abs((firstCtrlPoint.X - end.X) * deltaLine.Y - (firstCtrlPoint.Y - end.Y) * deltaLine.X);
            float d3 = Math.Abs((secondCtrlPoint.X - end.X) * deltaLine.Y - (secondCtrlPoint.Y - end.Y) * deltaLine.X);
            if ((d2 + d3) * (d2 + d3) < distanceTolerance * (deltaLine.X * deltaLine.X + deltaLine.Y * deltaLine.Y))
            {
                points.Add(pt17);
                return;
            }
            RecursiveGetOptimizedDrawingPoints(start, pt12, pt15, pt17, points, distanceTolerance);
            RecursiveGetOptimizedDrawingPoints(pt17, pt16, pt14, end, points, distanceTolerance);
        }

        // Token: 0x0600017A RID: 378 RVA: 0x00010CD0 File Offset: 0x0000EED0
        public static List<Vector2> BezierPoints(Vector2 start, Vector2 firstCtrlPoint, Vector2 secondCtrlPoint, Vector2 end, float distanceTolerance = 1f)
        {
            List<Vector2> points = new List<Vector2>();
            points.Add(start);
            RecursiveGetOptimizedDrawingPoints(start, firstCtrlPoint, secondCtrlPoint, end, points, distanceTolerance);
            points.Add(end);
            return points;
        }
    }
}
