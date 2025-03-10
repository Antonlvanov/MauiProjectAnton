using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Graphics;

namespace MauiProjectAnton
{
    public class TrafficLightDrawable : IDrawable
    {
        public float RedOpacity { get; set; } = 0.3f;
        public float YellowOpacity { get; set; } = 0.3f;
        public float GreenOpacity { get; set; } = 0.3f;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            // Координаты кругов (зависит от размера изображения светофора)
            float centerX = dirtyRect.Width / 2;
            float redY = dirtyRect.Height * 0.2f;
            float yellowY = dirtyRect.Height * 0.5f;
            float greenY = dirtyRect.Height * 0.8f;
            float radius = dirtyRect.Width * 0.2f;

            // Рисуем огни с текущей прозрачностью
            canvas.FillColor = Colors.Red.WithAlpha(RedOpacity);
            canvas.FillCircle(centerX, redY, radius);

            canvas.FillColor = Colors.Yellow.WithAlpha(YellowOpacity);
            canvas.FillCircle(centerX, yellowY, radius);

            canvas.FillColor = Colors.LimeGreen.WithAlpha(GreenOpacity);
            canvas.FillCircle(centerX, greenY, radius);
        }
    }
}
