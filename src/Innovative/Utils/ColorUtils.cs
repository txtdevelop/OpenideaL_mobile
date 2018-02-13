using System;
using Xamarin.Forms;

namespace PSY.Innovative.Utils
{
    public class ColorUtil
    {
        #region Position to color
        /// \brief Calculate the color depending on the position.
        ///
        /// \param parSize        Size of the color feld
        /// \param parX           x coordinate (zero if left) 
        /// \param parY           y coordinate (zero is on the top)
        ///
        /// \param parR           Will be filled with the red part 0...255
        /// \param parG           Will be filled with the green part 0...255
        /// \param parB           Will be filled with the Blue part 0...255
        ///
        /// \retval               Return true, if point inside of circle and color value valid.
        static public bool GetColorByPos(int parSize, int parX, int parY, ref int parR, ref int parG, ref int parB)
        {
            double H = 0;
            double S = 0;

            if (XY_to_HS(parSize / 2, parX, parY, ref H, ref S) == true)
            {
                HSV_to_RGB(H, S, 1, ref parR, ref parG, ref parB);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// \brief Calculation from x, y to angle and distance from origin
        ///
        /// Origin in the coordinates [parRadius, parRadius].
        ///
        /// \param parRadius      Radius of the circle
        /// \param parX           x coordinate (zero if left) 
        /// \param parY           y coordinate (zero is on the top)
        /// \param parH           Will be filled with the angle in the range 0...360
        /// \param parS           Will be filled with relative distance from the origin in the range 0...1
        ///
        /// \retval               Return true, if point inside of circle (parS <= 1)
        static public bool XY_to_HS(int parRadius, int parX, int parY, ref double parH, ref double parS)
        {
            double x = parX - parRadius;
            double y = parY - parRadius;

            parH = Math.Atan2(y, x);
            parH = parH * 180 / Math.PI + 180;

            if (parH == 360)
            {
                parH = 0;
            }

            parS = Math.Sqrt(x * x + y * y) / parRadius;

            if (parS <= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// \brief Calculation from HSV to RGB
        ///
        /// \param H              Angle in range 0...360
        /// \param S              Saturation in range 0...1
        /// \param V              Intensity/Level in range 0...1
        ///
        /// \param parR           Will be filled with the red part 0...255
        /// \param parG           Will be filled with the green part 0...255
        /// \param parB           Will be filled with the Blue part 0...255
        ///
        static public void HSV_to_RGB(double H, double S, double V, ref int parR, ref int parG, ref int parB)
        {
            double R = 0;
            double G = 0;
            double B = 0;

            int h_i = (int)H / 60;
            double f = (H / 60 - h_i);

            double p = (1 - S);                                                   //p = V*(1-S)
            double q = (1 - S * f);                                               //q = V*(1-S*f)
            double t = (1 - S * (1 - f));                                         //t = V*(1-S*(1-f))

            switch (h_i)
            {
                default:
                case 0:
                    R = V;
                    G = t;
                    B = p;
                    break;

                case 1:
                    R = q;
                    G = V;
                    B = p;
                    break;

                case 2:
                    R = p;
                    G = V;
                    B = t;
                    break;

                case 3:
                    R = p;
                    G = q;
                    B = V;
                    break;

                case 4:
                    R = t;
                    G = p;
                    B = V;
                    break;

                case 5:
                    R = V;
                    G = p;
                    B = q;
                    break;
            }

            int z = 255;
            parR = (int)Math.Round(R * z, 0);
            parG = (int)Math.Round(G * z, 0);
            parB = (int)Math.Round(B * z, 0);
        }
        #endregion

        #region Make bitmap
        /*
        static public  Bitmap MakeBitmap( int parSize, Color parBackColor, bool flag )
        {
          Bitmap colorFeldBitmap = new System.Drawing.Bitmap( parSize, parSize );

          for( int x = 0; x < parSize; x++ ) {
            for( int y = 0; y < parSize; y++ ) {
              int r = 0;
              int g = 0;
              int b = 0;
              if( GetColorByPos( parSize, x, y, ref r, ref g, ref b ) == true ) {

                if( flag == true ) {
                  int colorNumber = RgbToColorNumber( r, g, b );
                  RgbToColorNumber( colorNumber, ref r, ref g, ref b );
                }

                Color color = Color.FromArgb( r, g, b );
                colorFeldBitmap.SetPixel( x, y, color );
              }
              else {
                colorFeldBitmap.SetPixel( x, y, parBackColor );
              }
            }
          }

          return colorFeldBitmap;
        }
        */
        #endregion

        #region Color to position
        /// \brief Calculate from color to position.
        ///
        /// \param parSize        Size of the color feld
        /// \param parR           Red part 0...255
        /// \param parG           Green part 0...255
        /// \param parB           Blue part 0...255
        /// \param parX           Will be filled with the x coordinate (zero if left) 
        /// \param parY           yWill be filled with the coordinate (zero is on the top)
        ///
        static public void GetPosByColorNumber(int parSize, int parR, int parG, int parB, ref int parX, ref int parY)
        {
            double H = 0;
            double S = 0;

            RGB_to_HSV(parR, parG, parB, ref H, ref S);
            HS_to_XY(parSize / 2, H, S, ref parX, ref parY);
        }


        /// \brief Calculation from H (angle 0...360) und S (distance from the origin 0...1) to x, y 
        ///
        ///
        /// \param parRadius      Radius of the circle
        /// \param parH           Angle inthe range 0...360°
        /// \param parS           Relative distance from the origin in the range 0...1
        /// \param parX           Will be filled with x coordinate (zero if left) 
        /// \param parY           Will be filled with y coordinate (zero is on the top)
        static public void HS_to_XY(int parRadius, double parH, double parS, ref int parX, ref int parY)
        {
            parX = (int)Math.Round(parRadius - parRadius * parS * Math.Cos(parH * Math.PI / 180), 0);
            parY = (int)Math.Round(parRadius - parRadius * parS * Math.Sin(parH * Math.PI / 180), 0);
        }


        /// \brief Calculate RGB to HSV, the V part will be ignored!
        ///
        /// \param parR           Red part in range 0...255
        /// \param parG           Green part in range 0...255
        /// \param parB           Blue part in range 0...255
        /// \param parH           Will be filled with the angle in the range 0...360
        /// \param parS           Relative distance from the origin in the range 0...1
        ///
        static public void RGB_to_HSV(int parR, int parG, int parB, ref double parH, ref double parS)
        {
            if ((parR == parG) && (parG == parB))
            {
                parH = 0;
                parS = 0;
                return;
            }

            //-> Wertebereich 0..255
            double max = parR;
            double min = parR;
            byte maxId = 0;                                                           //0-rot, 1-grün, 2-blau

            if (max < parG)
            {
                max = parG;
                maxId = 1;
            }
            if (max < parB)
            {
                max = parB;
                maxId = 2;
            }

            if (min > parG)
            {
                min = parG;
            }
            if (min > parB)
            {
                min = parB;
            }
            //<-

            //-> zu Wertebereich 0...1
            double r = parR / max;
            double g = parG / max;
            double b = parB / max;


            min = min / max;
            max = 1;                                                                  //durch die Normung      
                                                                                      //<-

            switch (maxId)
            {
                case 0:
                    parH = 60 * ((g - b) / (max - min));
                    break;
                case 1:
                    parH = 60 * (2 + (b - r) / (max - min));
                    break;
                case 2:
                    parH = 60 * (4 + (r - g) / (max - min));
                    break;
            }

            if (parH < 0)
            {
                parH += 360.0;
            }

            parS = (max - min) / max;
        }
        #endregion

        #region Color number
        /// \brief Calculate RGB to color number.
        ///
        /// \param parR           Red part 0...255
        /// \param parG           Green part 0...255
        /// \param parB           Blue part 0...255
        ///
        /// \retval               Return color number 0xRRGG and 255 = R + G + B
        static public int RgbToColorNumber(int parR, int parG, int parB)
        {
            int retval = 0;

            double sum = parR + parB + parG;

            if (sum != 0)
            {
                byte r = (byte)Math.Round(parR * 255 / sum, 0);
                byte g = (byte)Math.Round(parG * 255 / sum, 0);
                retval = (r << 8) | g;
            }

            return retval;
        }


        /// \brief Calculate color number to RGB (with maximal intensity.
        ///
        /// \param parColorNumber Color number
        /// \param parR           Will be filled with the red part 0...255
        /// \param parG           Will be filled with the green part 0...255
        /// \param parB           Will be filled with the Blue part 0...255
        ///
        static public void ColorNumberToRgb(int parColorNumber, ref int parR, ref int parG, ref int parB)
        {
            parR = (byte)((parColorNumber & 0xFF00) >> 8);
            parG = (byte)(parColorNumber & 0x00FF);
            parB = 255 - parR - parG;

            double max = parR;
            if (max < parG)
            {
                max = parG;
            }
            if (max < parB)
            {
                max = parB;
            }

            parR = (byte)Math.Round(parR * 255 / max, 0);
            parG = (byte)Math.Round(parG * 255 / max, 0);
            parB = (byte)Math.Round(parB * 255 / max, 0);
        }
        #endregion

        public static int ColorNumberToAngle2(int colorNumber)
        {
            int r = 0,  g = 0, b = 0;
            ColorNumberToRgb(colorNumber, ref r, ref g, ref b);
            return RGBToAngle2(r, g, b);
        }
        public static int RGBToAngle2(int r, int g, int b)
        {
            var color = Color.FromRgb(r/255.0, g/255.0, b/255.0);
            return (int) (color.Hue*360);
        }

        public static Color AngleToRGB2(int angle)
        {
            return Xamarin.Forms.Color.FromHsla(angle/360.0, 1.0, 0.5);
        }

        public static int AngleToColorNumber2(int angle)
        {
            var color = AngleToRGB2(angle);
            return RgbToColorNumber((int)(color.R * 255), (int)(color.G * 255), (int)(color.B * 255));
        }

        public static Color AngleToRGB(int angle)
        {
            int regionsize = 60;
            int region = (int)angle / regionsize;
            int rest = 360 - (regionsize / region);
            int restLevel = (rest * 0xFF) / region;

            int R = 0, G = 0, B = 0;

            switch ((int)angle / 60)
            {
                case 6:
                case 0: //R -> RB
                    R = 0xFF;
                    G = 0;
                    B = restLevel;
                    break;
                case 1://RB -> B
                    R = 0xFF - restLevel;
                    G = 0;
                    B = 0xFF;
                    break;
                case 2://B -> BG
                    R = 0;
                    G = restLevel;
                    B = 0xFF;
                    break;
                case 3://BG -> G
                    R = 0;
                    G = 0xFF;
                    B = 0xFF - restLevel;
                    break;
                case 4://G -> GR
                    R = restLevel;
                    G = 0xFF;
                    B = 0;
                    break;
                case 5://GR -> R
                    R = 0xFF;
                    G = 0xFF - restLevel;
                    B = 0;
                    break;
            }
            return Color.FromRgb(R/255.0, G/255.0, B/255.0);
        }

        public static int AngleToColorNumber(int angle)
        {
            var color = AngleToRGB(angle);
            return RgbToColorNumber((int) (color.R*255), (int) (color.G*255), (int) (color.B*255));
        }
    }
}
