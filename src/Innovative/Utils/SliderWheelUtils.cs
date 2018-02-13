using System;

namespace PSY.Innovative.Utils
{
    public class SliderWheelUtils
    {
        public static int SliderAngleToValue(int angle, int startAngle, int sliderAngle, int minValue, int maxValue,
            bool clockwise = true)
        {
            var deltaAngle = ((angle - startAngle + 360) % 360);

            if (angle == -1)
                return 0;
            if (clockwise)
            {
                if (deltaAngle > sliderAngle)
                {
                    return 0;
                }
            }
            else
            {
                if (deltaAngle < (360 - sliderAngle))
                {
                    return 0;
                }
            }
            return minValue + ((deltaAngle * (maxValue - minValue)) / sliderAngle);
        }

        public static int ValueToSliderAngle(int value, int startAngle, int sliderAngle, int minValue, int maxValue,
            bool clockwise = true)
        {
            var angle = sliderAngle * (double)((double)(value - minValue) / (double)(maxValue - minValue));

            if (clockwise)
            {
                return (startAngle + (int)angle) % 360;
            }
            return (startAngle - (int)angle + 360) % 360;
        }

        public static double ConvertToDegrees(double val)
        {
            return val * (180.0 / Math.PI);
        }

        public static double ConvertToRadians(double rad)
        {
            return (rad * Math.PI) / 180.0;
        }

        public static double GetTouchDegrees(float xPos, float yPos, float cxPos, float cyPos, bool clockWise = true)
        {
            var x = xPos - cxPos;
            var y = yPos - cyPos;

            //invert the x-coord if we are rotating anti-clockwise
            x = (clockWise) ? x : -x;

            // convert to arc Angle
            var angle = ConvertToDegrees(Math.Atan2(y, x) + (Math.PI / 2));

            if (angle < 0)
            {
                angle = 360 + angle;
            }
            angle -= 0; //_startAngle;
            return angle;
        }

        public static double GetDistanceBetweenPoints(float p1X, float p1Y, float p2X, float p2Y)
        {
            double a = p1X - p2X;
            double b = p1Y - p2Y;
            var distance = Math.Sqrt(a * a + b * b);
            return distance;
        }
    }
}