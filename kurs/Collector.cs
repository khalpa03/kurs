using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace kurs
{
    class Collector : IImpactPoint
    {
        public float Diameter = 100;
        private ushort _count = 0;
        public Color color = Color.Blue;

        public Action<ParticleColorful> PaintParticle;
        public Action<ParticleColorful> ReturnColor;

        public override void ImpactParticle(Particle particle)
        {
            if (Overlaps(particle))
            {
                _count++;
                if (particle is ParticleColorful particleColorful)
                {
                    PaintParticle?.Invoke(particleColorful);
                }

            }
            else
            {
                if (particle is ParticleColorful particleColorful)
                {
                    ReturnColor?.Invoke(particleColorful);
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
            g.FillEllipse(new SolidBrush(Color.GhostWhite), X - Diameter / 2, Y - Diameter / 2, Diameter, Diameter);
            //g.DrawEllipse(new Pen(color, 3), X - Diameter / 2, Y - Diameter / 2, Diameter, Diameter);

            var stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;


            if (_count > 0)
            {
                g.DrawString(
                    $"{_count}",
                    new Font("Comic Sans MS", 18), // шрифт и размер
                    new SolidBrush(Color.Red), // цвет шрифта
                    X, Y, stringFormat);
            }

            _count = 0;
        }
    }
}
