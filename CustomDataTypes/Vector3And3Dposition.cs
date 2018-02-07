using System;

namespace CS3D.CustomDataTypes
{
    struct Vector3
    {
        public float x, y, z;

        public void Rotate(Position3D pos)
        {
            float x1 = x;
            float y1 = y;
            float z1 = z;

            float returnX1 = x1 * pos.Angle1Computed.v - y1 * pos.Angle1Computed.u;
            y1 = x1 * pos.Angle1Computed.u + y1 * pos.Angle1Computed.v;

            y = y1 * pos.Angle2Computed.v - z1 * pos.Angle2Computed.u;
            z1 = y1 * pos.Angle2Computed.u + z1 * pos.Angle2Computed.v;

            z = z1 * pos.Angle3Computed.v - returnX1 * pos.Angle3Computed.u;
            x = z1 * pos.Angle3Computed.u + returnX1 * pos.Angle3Computed.v;
        }

        public void Perspective(float calculateFov, int halfWidth, int halfHeight)
        {
            float percentCloseToCamera = z / (z + calculateFov);

            x += (halfWidth - x) * percentCloseToCamera;
            y += (halfHeight - y) * percentCloseToCamera;
        }

        public void TranslateWorld(Position3D pos)
        {
            x += pos.X;
            y += pos.Y;
            z += pos.Z;
        }

        public void TranslateLocal(Position3D pos)
        {
            x -= pos.X;
            y -= pos.Y;
            z -= pos.Z;
        }

        public void Scale(float scale)
        {
            x *= scale;
            y *= scale;
            z *= scale;
        }
    }

    class Position3D
    {
        private Vector3 location;
        private Vector3 angle;

        private Vector2 angle1;
        private Vector2 angle2;
        private Vector2 angle3;

        public float X
        {
            get { return this.location.x; }
            set { this.location.x = value; }
        }
        public float Y
        {
            get { return this.location.y; }
            set { this.location.y = value; }
        }
        public float Z
        {
            get { return this.location.z; }
            set { this.location.z = value; }
        }

        public Vector2 Angle1Computed { get { return angle1; } }
        public float Angle1 { get { return DegreesFilter(angle.x); }
        set
            {
                DegreesToAngleVector(ref angle1, angle.x = value);
            }
        }

        public Vector2 Angle2Computed { get { return angle2; } }
        public float Angle2 { get { return DegreesFilter(angle.y); } 
        set
            {
                DegreesToAngleVector(ref angle2, angle.y = value);
            }
        }

        public Vector2 Angle3Computed { get { return angle3; } }
        public float Angle3 { get { return DegreesFilter(angle.z); }
        set {
                DegreesToAngleVector(ref angle3, angle.z = value);
            }
        }

        private void DegreesToAngleVector(ref Vector2 angle, float degrees)
        {
            float tmpRadians = MathTools.DegreesToRadians(degrees);
            angle.u = (float)Math.Sin(tmpRadians);
            angle.v = (float)Math.Cos(tmpRadians);
        }
        private float DegreesFilter(float degrees)
        {
            while (degrees > 360.0f || degrees < 0.0f) degrees += 360.0f * -Math.Sign(degrees);
            return degrees;
        }

        public Position3D(float x, float y, float z, float angle1Degrees, float angle2Degrees, float angle3Degrees)
        {
            X = x;
            Y = y;
            Z = z;

            Angle1 = angle1Degrees;
            Angle2 = angle2Degrees;
            Angle3 = angle3Degrees;
        }
    }
}
