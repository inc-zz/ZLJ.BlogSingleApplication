using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Numerics;

namespace Blogs.Common
{
    /// <summary>
    /// 图片水印服务
    /// </summary>
    public class ImageWatermarkHelper
    {
        /// <summary>
        /// 添加微信样式水印（底部右侧）
        /// </summary>
        public async Task AddWatermarkAsync(Stream inputStream, Stream outputStream, string watermarkText)
        {
            using var image = await Image.LoadAsync(inputStream);

            // 创建字体
            var font = SystemFonts.Get("Microsoft YaHei").CreateFont(24, FontStyle.Regular);

            // 测量文字
            var textOptions = new TextOptions(font)
            {
                Origin = new PointF(0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var textSize = TextMeasurer.MeasureSize(watermarkText, textOptions);

            // 计算位置（底部右侧，带边距）
            var position = new PointF(
                image.Width - textSize.Width - 30,
                image.Height - textSize.Height - 30
            );

            // 绘制水印
            image.Mutate(x =>
            {
                x.DrawText(watermarkText, font, Color.FromRgba(255, 255, 255, 180), position);
            });
            var imageSharp = image.Metadata.DecodedImageFormat ?? SixLabors.ImageSharp.Formats.Png.PngFormat.Instance;
            await image.SaveAsync(outputStream, imageSharp);
        }

        /// <summary>
        /// 添加斜角平铺水印（防复制）
        /// </summary>
        public async Task AddTiledWatermarkAsync(Stream inputStream, Stream outputStream, string watermarkText)
        {
            using var image = await Image.LoadAsync(inputStream);

            // 创建字体
            var font = SystemFonts.Get("Microsoft YaHei").CreateFont(36, FontStyle.Regular);

            // 测量文字
            var textOptions = new TextOptions(font)
            {
                Origin = new PointF(0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };

            var textSize = TextMeasurer.MeasureSize(watermarkText, textOptions);

            // 计算行数和列数
            int cols = (int)Math.Ceiling(image.Width / (float)(textSize.Width + 150));
            int rows = (int)Math.Ceiling(image.Height / (float)(textSize.Height + 150));

            // -45度转换为弧度
            float radians = (float)(-45 * Math.PI / 180);

            image.Mutate(x =>
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        float xPos = j * (textSize.Width + 150);
                        float yPos = i * (textSize.Height + 150);

                        // 创建绘制选项并设置旋转
                        var drawingOptions = new DrawingOptions();
                        var center = new Vector2(xPos + textSize.Width / 2, yPos + textSize.Height / 2);
                        drawingOptions.Transform = Matrix3x2.CreateRotation(radians, center);

                        // 绘制旋转的水印
                        x.DrawText(drawingOptions, watermarkText, font, Color.FromRgba(255, 255, 255, 80), new PointF(xPos, yPos));
                    }
                }
            });
            var imageSharp = image.Metadata.DecodedImageFormat ?? SixLabors.ImageSharp.Formats.Png.PngFormat.Instance;
            await image.SaveAsync(outputStream, imageSharp);
        }


        private readonly Random _random = new Random();
        /// <summary>
        /// 添加自定义水印
        /// </summary>
        /// <param name="inputStream">源文件路径</param>
        /// <param name="outputStream">加工后的图片存储路径-带文件名</param>
        /// <param name="watermarkText">水印文本</param>
        /// <param name="count">水印数量</param>
        /// <param name="fontSize">字体大小</param>
        /// <param name="fontOpacity">透明度1-255</param>
        /// <param name="fontRotation">倾斜角度</param>
        /// <returns></returns>
        public async Task AddRandomWatermarksAsync(
            Stream inputStream,
            Stream outputStream,
            string watermarkText = "blog.zhenglijun.com",
            int count = 5,
            int fontSize = 30,
            int fontOpacity = 1,
            int fontRotation = -45)
        {
            using var image = await Image.LoadAsync(inputStream);
            //定义字体颜色
            byte red = 200, green = 200, blue = 200; 

            for (int i = 0; i < count; i++)
            {
                // 获取字体
                var font = GetFont("Microsoft YaHei", fontSize);

                // 测量文字大小
                var textOptions = new TextOptions(font)
                {
                    Origin = new PointF(0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top
                };

                var textSize = TextMeasurer.MeasureSize(watermarkText, textOptions);

                // 生成随机位置
                float x = (float)(_random.NextDouble() * (image.Width - textSize.Width));
                float y = (float)(_random.NextDouble() * (image.Height - textSize.Height));
                var position = new PointF(x, y);

                // 设置字体颜色与透明度
                var color = Color.FromRgba(red, green, blue, (byte)fontOpacity);

                // 绘制水印
                if (fontRotation == 0)
                {
                    image.Mutate(ctx =>
                    {
                        ctx.DrawText(watermarkText, font, color, position);
                    });
                }
                else
                {
                    var drawingOptions = new DrawingOptions();
                    float radians = (float)(fontRotation * Math.PI / 180);

                    // 计算旋转中心
                    var center = new Vector2(
                        position.X + textSize.Width / 2,
                        position.Y + textSize.Height / 2
                    );

                    drawingOptions.Transform = Matrix3x2.CreateRotation(radians, center);

                    image.Mutate(ctx =>
                    {
                        ctx.DrawText(drawingOptions, watermarkText, font, color, position);
                    });
                }
            }

            // 保存图片
            var encoder = GetEncoderBasedOnInput(image);
            await image.SaveAsync(outputStream, encoder);

        }
        /// <summary>
        /// 根据输入图片获取编码器
        /// </summary>
        private IImageEncoder GetEncoderBasedOnInput(Image image)
        {
            var format = image.Metadata.DecodedImageFormat;

            if (format == PngFormat.Instance)
            {
                return new PngEncoder();
            }
            else if (format == JpegFormat.Instance)
            {
                return new JpegEncoder { Quality = 90 };
            }
            else if (format == SixLabors.ImageSharp.Formats.Bmp.BmpFormat.Instance)
            {
                return new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder();
            }
            else if (format == SixLabors.ImageSharp.Formats.Gif.GifFormat.Instance)
            {
                return new SixLabors.ImageSharp.Formats.Gif.GifEncoder();
            }
            else if (format == SixLabors.ImageSharp.Formats.Webp.WebpFormat.Instance)
            {
                return new SixLabors.ImageSharp.Formats.Webp.WebpEncoder();
            }

            // 默认使用JPEG格式
            return new JpegEncoder { Quality = 90 };
        }

        /// <summary>
        /// 获取字体
        /// </summary>
        private Font GetFont(string fontFamily, float fontSize)
        {
            if (SystemFonts.TryGet(fontFamily, out var fontFamilyObj))
            {
                return fontFamilyObj.CreateFont(fontSize, FontStyle.Regular);
            }
            var defaultFont = SystemFonts.Families.First();
            return defaultFont.CreateFont(fontSize, FontStyle.Regular);
        }

    }
}