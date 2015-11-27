using System.Drawing;
using System.Drawing.Imaging;

namespace IsaacAnimator
{
    public class Coloring
    {
        private ColorMatrix CMOffset = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {0,0,0,0,0}
        });

        private ColorMatrix CMTint = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {0,0,0,0,0}
        });

        private ColorMatrix CMAlpha = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {0,0,0,0,0}
        });

        private ColorMatrix combineColorMatrix()
        {
            ColorMatrix CM = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,CMAlpha.Matrix33,0},
            new float[] {
                CMOffset.Matrix40+CMTint.Matrix40,
                CMOffset.Matrix41+CMTint.Matrix41,
                CMOffset.Matrix42+CMTint.Matrix42,
                0,
                1
            }
        });
            return CM;
        }

        public void Reset()
        {
            CMAlpha = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {0,0,0,0,0}
        });
            CMOffset = CMAlpha;
            CMTint = CMAlpha;
        }

        public Bitmap Tint(Bitmap bmpSource, Color clrScaleColor)
        {
            Bitmap bmpTemp = new Bitmap(bmpSource.Width, bmpSource.Height); //Create Temporary Bitmap To Work With

            ImageAttributes iaImageProps = new ImageAttributes(); //Contains information about how bitmap and metafile colors are manipulated during rendering.

            CMTint = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {
                (float)clrScaleColor.R / -255,
                (float)clrScaleColor.G / -255,
                (float)clrScaleColor.B / -255,
                0,
                1
            }
        });

            iaImageProps.SetColorMatrix(combineColorMatrix()); //Apply Matrix

            Graphics grpGraphics = Graphics.FromImage(bmpTemp); //Create Graphics Object and Draw Bitmap Onto Graphics Object

            grpGraphics.DrawImage(bmpSource, new Rectangle(0, 0, bmpSource.Width, bmpSource.Height), 0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, iaImageProps);

            return bmpTemp;
        }

        public Bitmap setTransparent(Bitmap bmpSource, int alpha)
        {
            Bitmap bmpTemp = new Bitmap(bmpSource.Width, bmpSource.Height); //Create Temporary Bitmap To Work With

            ImageAttributes iaImageProps = new ImageAttributes(); //Contains information about how bitmap and metafile colors are manipulated during rendering.

            CMAlpha = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,(float)alpha/255,0},
            new float[] {0,0,0,0,1}
        });

            iaImageProps.SetColorMatrix(combineColorMatrix()); //Apply Matrix

            Graphics grpGraphics = Graphics.FromImage(bmpTemp); //Create Graphics Object and Draw Bitmap Onto Graphics Object

            grpGraphics.DrawImage(bmpSource, new Rectangle(0, 0, bmpSource.Width, bmpSource.Height), 0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, iaImageProps);

            return bmpTemp;
        }

        public Bitmap Offset(Bitmap bmpSource, Color clrScaleColor)
        {
            Bitmap bmpTemp = new Bitmap(bmpSource.Width, bmpSource.Height); //Create Temporary Bitmap To Work With

            ImageAttributes iaImageProps = new ImageAttributes(); //Contains information about how bitmap and metafile colors are manipulated during rendering.

            CMOffset = new ColorMatrix(new float[][] {
            new float[] {1,0,0,0,0},
            new float[] {0,1,0,0,0},
            new float[] {0,0,1,0,0},
            new float[] {0,0,0,1,0},
            new float[] {
                (float)clrScaleColor.R / 255,
                (float)clrScaleColor.G / 255,
                (float)clrScaleColor.B / 255,
                0,
                1
            }
        });
            iaImageProps.SetColorMatrix(combineColorMatrix()); //Apply Matrix

            Graphics grpGraphics = Graphics.FromImage(bmpTemp); //Create Graphics Object and Draw Bitmap Onto Graphics Object

            grpGraphics.DrawImage(bmpSource, new Rectangle(0, 0, bmpSource.Width, bmpSource.Height), 0, 0, bmpSource.Width, bmpSource.Height, GraphicsUnit.Pixel, iaImageProps);

            return bmpTemp;
        }

        public Bitmap Resize(Bitmap bmpSource, int width, int height)
        {
            Bitmap temp = new Bitmap(bmpSource, (int)((float)bmpSource.Width / 100 * width), (int)((float)bmpSource.Height / 100 * height));
            return temp; //Create Temporary Bitmap To Work With
        }
    }
}