using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace kurs
{
    class Counter : IImpactPoint
    {
        public Action<ParticleColorful> DestroyParticle;

        public float Diameter = 60;
        public Color color = Color.Red;
        public Color oldColor = Color.Black;
        public ushort Count = 0;
        public override void ImpactParticle(Particle particle)
        {
            if (Overlaps(particle))
            {
                if (particle is ParticleColorful particleColorful)
                {
                    DestroyParticle?.Invoke(particleColorful);
                }
            }
        }

        public override bool Overlaps(Particle particle)
        {
            float gX = X - particle.X;
            float gY = Y - particle.Y;

            double distance = Math.Sqrt(gX * gX + gY * gY);

            return distance <= Diameter / 2;
        }
        public override void Render(Graphics g)
        {
            Random random = new Random();
            if ((float)(Count % 1080) / 1080 > 0.99)
            {
                oldColor = color;
                color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255));
            }

            g.FillEllipse(new SolidBrush(oldColor), X - Diameter / 2, Y - Diameter / 2, Diameter, Diameter);
            g.FillPie(new SolidBrush(color), X - Diameter / 2, Y - Diameter / 2, Diameter, Diameter, -90, Count % 1080 / 3);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;


            if (Count > 0)
            {
                g.DrawString(
                    $"{Count}",
                    new Font("Comic Sans MS", 10), // шрифт и размер
                    new SolidBrush(Color.White), // цвет шрифта
                    X, Y, stringFormat);
            }
        }
    }
}
