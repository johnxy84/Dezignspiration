using System;
using Android.Content;
using Android.Graphics;
using Android.Support.V4.Content;
using Android.Text;
using DezignSpiration.Helpers;
using Java.IO;
using DezignSpiration.Models;
using Android.Text.Style;
using Android.Widget;
using Android.OS;
using Android.App.Job;
using Java.Lang;

namespace DezignSpiration.Droid
{
    public static class Helper
    {
        public static Android.Graphics.Color GetColorFromHex(string hexColor)
        {
            var rgbColor = Utils.ExtractRGBFromHex(hexColor);
            return Android.Graphics.Color.Rgb(rgbColor.Red, rgbColor.Green, rgbColor.Blue);
        }

        public static Bitmap GetBitmap(Context context, string text, string backgroundColor, string textColor, int height = 3000, int width = 3000)
        {
            Bitmap bitmap = Bitmap.CreateBitmap(width, height, Bitmap.Config.Argb8888);
            Canvas canvas = new Canvas(bitmap);
            canvas.DrawColor(GetColorFromHex(backgroundColor));

            // new antialiased Paint
            TextPaint paint = new TextPaint(PaintFlags.AntiAlias)
            {
                // Set text Color
                Color = GetColorFromHex(textColor)
            };

            Typeface typeFace = Typeface.CreateFromAsset(context.Assets, "Neuton-Bold.ttf");
            paint.SetTypeface(typeFace);


            // text size in pixels
            paint.TextSize = GetFontSize(text);
            paint.TextAlign = Paint.Align.Left;

            // set text width to canvas width minus screen padding(10 percent of width) in dp
            int textWidth = ((int)(canvas.Width - (0.1 * width)));

            var textPreview = new SpannableString(text);
            // Reduce the text of the brand name
            textPreview.SetSpan(new RelativeSizeSpan(0.5f), text.Length - Helpers.Constants.BRAND_NAME.Length, text.Length, SpanTypes.ExclusiveExclusive);

            // init StaticLayout for text
            StaticLayout textLayout;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                textLayout = StaticLayout.Builder.Obtain(textPreview, 0, text.Length, paint, textWidth).Build();
            }
            else
            {
                textLayout = new StaticLayout(textPreview, paint, textWidth, Layout.Alignment.AlignNormal, 1.0f, 0.0f, false);
            }

            // get height of multiline text
            int? textHeight = textLayout?.Height;

            // get position of text's top left corner
            float x = (bitmap.Width - textWidth) / 2;
            float y = (bitmap.Height - textHeight ?? 0) / 2;

            // draw text to the Canvas center
            canvas.Save();
            canvas.Translate(x, y);
            textLayout.Draw(canvas);
            canvas.Restore();

            return bitmap;
        }

        public static void ShareQuote(Context context, DesignQuote designQuote, bool isLongQuote)
        {
            string text = GetImageText(designQuote);
            if (isLongQuote)
            {
                ShareText(context, text);
            }
            else
            {
                Bitmap bitmap = GetBitmap(context, text, designQuote.Color.PrimaryColor, designQuote.Color.SecondaryColor);
                ShareImage(context, bitmap);
            }
        }

        public static string GetImageText(DesignQuote designQuote)
        {
            return $"{designQuote.Quote} \n\n\n{designQuote.Author}\n{Helpers.Constants.BRAND_NAME}";
        }

        public static void ShareText(Context context, string text)
        {
            Intent shareIntent = new Intent(Intent.ActionSend);
            //shareIntent.SetAction(Intent.ActionSend);
            shareIntent.AddFlags(ActivityFlags.ClearWhenTaskReset);
            shareIntent.PutExtra(Intent.ExtraText, text);
            shareIntent.SetType("text/plain");

            context.StartActivity(Intent.CreateChooser(shareIntent, "Share Quote"));
        }

        public static void ShareImage(Context context, Bitmap bitmap)
        {
            try
            {
                File cachePath = new File(context.CacheDir, "images");
                // Create Directory
                cachePath.Mkdirs();
                string fileName = $"quote{DateTime.Now.ToString("yyMMddHHmmss")}.jpeg";
                string filePath = $"{context.CacheDir}/images/{fileName}";
                using (System.IO.FileStream fs = System.IO.File.Open(filePath, System.IO.FileMode.OpenOrCreate))
                {
                    bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fs);
                }

                var authority = $"{context.PackageName}.fileprovider";
                var file = new File(filePath);
                Android.Net.Uri contentUri = FileProvider.GetUriForFile(context, authority, file);

                if (contentUri != null)
                {
                    Intent shareIntent = new Intent(Intent.ActionView);
                    shareIntent.SetAction(Intent.ActionSend);
                    // Temp permission for receiving app to read this file
                    shareIntent.AddFlags(ActivityFlags.GrantReadUriPermission);
                    shareIntent.SetDataAndType(contentUri, context.ContentResolver.GetType(contentUri));
                    shareIntent.PutExtra(Intent.ExtraStream, contentUri);
                    shareIntent.SetFlags(ActivityFlags.NewTask);
                    shareIntent.SetType("image/jpeg");

                    var chooserIntent = Intent.CreateChooser(shareIntent, "Share Quote");
                    chooserIntent.AddFlags(ActivityFlags.NewTask);

                    context.StartActivity(chooserIntent);
                }
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(context, "There was an issue sharing the Quote", ToastLength.Short).Show();
                Utils.LogError(ex, "SharingQuoteException");
            }

        }

        public static void DrawBorder(Canvas canvas, string hexColor)
        {
            Point canvasCenter = new Point(canvas.Width / 2, canvas.Height / 2);
            var borderWidth = canvas.Width - 50;
            var borderHeight = canvas.Height - 50;

            int borderLeft = canvasCenter.X - (borderWidth / 2);
            int borderTop = canvasCenter.Y - (borderHeight / 2);
            int borderRight = canvasCenter.X + (borderWidth / 2);
            int borderBottom = canvasCenter.Y + (borderHeight / 2);

            var border = new Rect(borderLeft, borderTop, borderRight, borderBottom);
            Paint paint = new Paint();
            paint.SetStyle(Paint.Style.Stroke);
            paint.Color = GetColorFromHex(hexColor);
            paint.StrokeWidth = 10f;

            canvas.DrawRect(border, paint);
        }

        public static float GetFontSize(string text)
        {
            return text.Length <= 100 ? 240 : text.Length <= 150 ? 200 : 190;
        }

        public static JobInfo.Builder CreateJobBuilderUsingJobId<T>(this Context context, int jobId) where T : JobService
        {
            var javaClass = Class.FromType(typeof(T));
            var componentName = new ComponentName(context, javaClass);
            return new JobInfo.Builder(jobId, componentName);
        }
    }

}
