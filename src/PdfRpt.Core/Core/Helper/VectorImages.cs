using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PdfRpt.Core.Helper
{
    /// <summary>
    /// Contains the definition of some useful vector images
    /// </summary>
    public static class VectorImages
    {
        /// <summary>
        /// A vector checkmark image's definition
        /// </summary>
        /// <param name="position">position of the drawing</param>
        /// <param name="contentByte">drawing canvas</param>
        /// <param name="color">fill color</param>
        /// <returns>a vector image</returns>
        public static Image DrawCheckmark(Rectangle position, PdfContentByte contentByte, System.Drawing.Color color)
        {
            var img = CheckmarkImage(contentByte, color);
            DrawCheckmarkImageAtPosition(img, position);
            return img;
        }

        /// <summary>
        /// Draws the images at the specified location.
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="position">position of the drawing</param>
        /// <param name="newWidth">ScaleAbsolute's newWidth</param>
        /// <param name="newHeight">ScaleAbsolute's newHeight</param>
        public static Image DrawCheckmarkImageAtPosition(this Image img, Rectangle position, float newWidth = 40, float newHeight = 40)
        {
            img.ScaleAbsolute(newWidth, newHeight);
            img.SetAbsolutePosition(
                    (position.Right + position.Left - (img.PlainWidth / 4)) / 2,
                    ((position.Bottom + position.Top) / 2) - (img.PlainHeight / 4) - 2
                    );
            return img;
        }

        /// <summary>
        /// A vector checkmark image's definition
        /// </summary>
        /// <param name="contentByte">drawing canvas</param>
        /// <param name="color">fill color</param>
        /// <returns>a vector image</returns>
        public static Image CheckmarkImage(PdfContentByte contentByte, System.Drawing.Color color)
        {
            var template = contentByte.CreateTemplate(260, 260);

            template.MoveTo(38.33889376f, 67.35513328f);
            template.CurveTo(39.90689547f, 67.35509017f, 41.09296342f, 66.03921993f, 41.89711165f, 63.40748424f);
            template.CurveTo(43.50531445f, 58.47289182f, 44.65118131f, 56.00562195f, 45.33470755f, 56.0056459f);
            template.CurveTo(45.85735449f, 56.00562195f, 46.40013944f, 56.41682961f, 46.96305772f, 57.23928802f);
            template.CurveTo(58.2608517f, 75.74384316f, 68.7143666f, 90.71198997f, 78.32362116f, 102.14379168f);
            template.CurveTo(80.81631349f, 105.10443984f, 84.77658911f, 106.58480942f, 90.20445269f, 106.58489085f);
            template.CurveTo(91.49097185f, 106.58480942f, 92.35539361f, 106.46145048f, 92.79773204f, 106.21480444f);
            template.CurveTo(93.23991593f, 105.96799555f, 93.4610547f, 105.65958382f, 93.46113432f, 105.28956447f);
            template.CurveTo(93.4610547f, 104.71379041f, 92.7976618f, 103.58294901f, 91.47094155f, 101.89705463f);
            template.CurveTo(75.95141033f, 82.81670149f, 61.55772504f, 62.66726353f, 48.28984822f, 41.44869669f);
            template.CurveTo(47.36506862f, 39.96831273f, 45.47540199f, 39.22812555f, 42.62081088f, 39.22813992f);
            template.CurveTo(39.72597184f, 39.22812555f, 38.0172148f, 39.35149407f, 37.49457722f, 39.5982407f);
            template.CurveTo(36.12755286f, 40.2150402f, 34.51931728f, 43.36081778f, 32.66987047f, 49.03557823f);
            template.CurveTo(30.57914689f, 55.32711903f, 29.53378743f, 59.27475848f, 29.53381085f, 60.87852533f);
            template.CurveTo(29.53378743f, 62.60558406f, 30.94099884f, 64.27099685f, 33.75542165f, 65.87476369f);
            template.CurveTo(35.48425582f, 66.86164481f, 37.01207517f, 67.35509017f, 38.33889376f, 67.35513328f);

            template.SetRgbColorFill(color.R, color.G, color.B);
            template.Fill();

            var img = Image.GetInstance(template);
            return img;
        }

        /// <summary>
        /// A vector cross sign image's definition
        /// </summary>
        /// <param name="position">position of the drawing</param>
        /// <param name="cb">drawing canvas</param>
        /// <param name="color">fill color</param>
        /// <returns>a vector image</returns>
        public static Image DrawCross(Rectangle position, PdfContentByte cb, System.Drawing.Color color)
        {
            var img = CrossImage(cb, color);
            DrawCrossImageAtPosition(img, position);
            return img;
        }

        /// <summary>
        /// Draws the images at the specified location.
        /// </summary>
        /// <param name="img">Image</param>
        /// <param name="position">position of the drawing</param>
        /// <param name="newWidth">ScaleAbsolute's newWidth</param>
        /// <param name="newHeight">ScaleAbsolute's newHeight</param>
        public static Image DrawCrossImageAtPosition(this Image img, Rectangle position, float newWidth = 30, float newHeight = 30)
        {
            img.ScaleAbsolute(newWidth, newHeight);
            img.SetAbsolutePosition(
                    (position.Right + position.Left - (img.PlainWidth / 4)) / 2,
                    ((position.Bottom + position.Top) / 2) - (img.PlainHeight / 4)
                    );
            return img;
        }

        /// <summary>
        /// A vector cross sign image's definition
        /// </summary>
        /// <param name="cb">drawing canvas</param>
        /// <param name="color">fill color</param>
        /// <returns>a vector image</returns>
        public static Image CrossImage(PdfContentByte cb, System.Drawing.Color color)
        {
            var template = cb.CreateTemplate(240, 240);

            template.NewPath();
            template.MoveTo(13.57143f, 76.42857262f);
            template.LineTo(23.57143f, 86.42857262f);
            template.LineTo(48.57143f, 61.42857262f);
            template.LineTo(73.57143f, 86.42857262f);
            template.LineTo(83.57143f, 76.42857262f);
            template.LineTo(58.57143f, 51.42857262f);
            template.LineTo(83.57143f, 26.42857262f);
            template.LineTo(73.57143f, 16.42857262f);
            template.LineTo(48.57143f, 41.42857262f);
            template.LineTo(23.57143f, 16.42857262f);
            template.LineTo(13.57143f, 26.42857262f);
            template.LineTo(38.57143f, 51.42857262f);
            template.ClosePath();

            template.NewPath();
            template.MoveTo(13.57143f, 76.42857262f);
            template.LineTo(23.57143f, 86.42857262f);
            template.LineTo(48.57143f, 61.42857262f);
            template.LineTo(73.57143f, 86.42857262f);
            template.LineTo(83.57143f, 76.42857262f);
            template.LineTo(58.57143f, 51.42857262f);
            template.LineTo(83.57143f, 26.42857262f);
            template.LineTo(73.57143f, 16.42857262f);
            template.LineTo(48.57143f, 41.42857262f);
            template.LineTo(23.57143f, 16.42857262f);
            template.LineTo(13.57143f, 26.42857262f);
            template.LineTo(38.57143f, 51.42857262f);
            template.ClosePath();

            template.NewPath();
            template.MoveTo(13.57143f, 76.42857262f);
            template.LineTo(23.57143f, 86.42857262f);
            template.LineTo(48.57143f, 61.42857262f);
            template.LineTo(73.57143f, 86.42857262f);
            template.LineTo(83.57143f, 76.42857262f);
            template.LineTo(58.57143f, 51.42857262f);
            template.LineTo(83.57143f, 26.42857262f);
            template.LineTo(73.57143f, 16.42857262f);
            template.LineTo(48.57143f, 41.42857262f);
            template.LineTo(23.57143f, 16.42857262f);
            template.LineTo(13.57143f, 26.42857262f);
            template.LineTo(38.57143f, 51.42857262f);
            template.ClosePath();

            template.SetRgbColorFill(color.R, color.G, color.B);
            template.Fill();

            var img = Image.GetInstance(template);
            return img;
        }
    }
}
