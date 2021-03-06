using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace kurs
{
    public partial class Form1 : Form
    {
        Emitter emitter; // добавим поле для эмиттера
        Collector collector = new Collector();
        public Form1()
        {
            InitializeComponent();
            picDisplay.MouseWheel += picDisplay_MouseWheel;
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            this.emitter = new Emitter // создаю эмиттер и привязываю его к полю emitter
            {
                Direction = 0,
                Spreading = 10,
                SpeedMin = 10,
                SpeedMax = 10,
                ColorFrom = Color.Gold,
                ColorTo = Color.FromArgb(0, Color.Red),
                ParticlesPerTick = 10,
                X = picDisplay.Width / 2,
                Y = picDisplay.Height / 2,
            };

            collector.PaintParticle += (particle) =>
            {
                particle.FromColor = collector.color;
                particle.ToColor = collector.color;
            };

            collector.ReturnColor += (particle) =>
            {
                particle.FromColor = emitter.ColorFrom;
                particle.ToColor = emitter.ColorTo;
            };
            emitter.impactPoints.Add(collector);
        }

        private void UpdateCounters()
        {
            foreach (var impactPoint in emitter.impactPoints.ToList())
            {
                if (impactPoint is Counter counter)
                {
                    counter.DestroyParticle += (particle) =>
                    {
                        emitter.particles.Remove(particle);
                        counter.Count++;
                    };
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            emitter.UpdateState(); // каждый тик обновляем систему
            using (var g = Graphics.FromImage(picDisplay.Image))
            {
                g.Clear(Color.Black);
                emitter.Render(g); // рендерим систему
            }
            picDisplay.Invalidate();

        }
        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // в обработчике заносим положение мыши в переменные для хранения положения мыши
            collector.X = e.X;
            collector.Y = e.Y;
        }

        private void tbDirection_Scroll(object sender, EventArgs e)
        {
            emitter.Direction = tbDirection.Value;
            //lblDirection.Text = $"{tbDirection.Value}°";
        }

        private void tbSpreading_Scroll(object sender, EventArgs e)
        {
            emitter.Spreading = tbSpreading.Value;
        }

        private void tbSpeed_Scroll(object sender, EventArgs e)
        {
            emitter.SpeedMax = tbSpeed.Value;
        }

        private void tbGravitation_Scroll(object sender, EventArgs e)
        {
            emitter.GravitationY = tbGravitation.Value;
        }

        private void tbParticles_Scroll(object sender, EventArgs e)
        {
            emitter.ParticlesPerTick = tbParticles.Value;
        }

        private void tbLife_Scroll(object sender, EventArgs e)
        {
            emitter.LifeMax = tbLife.Value;
        }

        private void picDisplay_MouseWheel(object sender, MouseEventArgs e)
        {
            collector.Diameter += e.Delta * 0.1f;
            if (collector.Diameter < 25)
                collector.Diameter = 25;
        }

        private void btfromColor_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();
            emitter.ColorFrom = colorDialog1.Color;
        }

        private void bttoColor_Click(object sender, EventArgs e)
        {
            colorDialog2.ShowDialog();
            emitter.ColorTo = colorDialog2.Color;
        }

        private void picDisplay_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Random random = new Random();
                Counter counter = new Counter
                {
                    X = e.X,
                    Y = e.Y,
                    color = Color.FromArgb(random.Next(255), random.Next(255), random.Next(255)),
                };
                emitter.impactPoints.Add(counter);
                emitter.impactPoints.Add(new GravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                });
                UpdateCounters();
            }
            else if (e.Button == MouseButtons.Right)
            {
                foreach (var impactPoint in emitter.impactPoints.ToList())
                {
                    if (impactPoint is Counter counter)
                    {
                        if (counter.Diameter / 2 >= Math.Sqrt(Math.Pow(e.X - counter.X, 2) + Math.Pow(e.Y - counter.Y, 2)))
                        {
                            emitter.impactPoints.Remove(counter);
                        }
                    }
                }
                foreach (var impactPoints in emitter.impactPoints.ToList())
                {
                    if (impactPoints is GravityPoint gravityPoint)
                    {
                        if (gravityPoint.Power / 2 >= Math.Sqrt(Math.Pow(e.X - gravityPoint.X, 2) + Math.Pow(e.Y - gravityPoint.Y, 2)))
                        {
                            emitter.impactPoints.Remove(gravityPoint);
                        }
                    }
                }
                foreach (var impactPoints in emitter.impactPoints.ToList())
                {
                    if (impactPoints is AntiGravityPoint antiGravityPoint)
                    {
                        if (antiGravityPoint.Power / 2 >= Math.Sqrt(Math.Pow(e.X - antiGravityPoint.X, 2) + Math.Pow(e.Y - antiGravityPoint.Y, 2)))
                        {
                            emitter.impactPoints.Remove(antiGravityPoint);
                        }
                    }
                }
                UpdateCounters();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                emitter.impactPoints.Add(new AntiGravityPoint
                {
                    X = e.X,
                    Y = e.Y,
                });

                UpdateCounters();
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            ParticleColorful.size = trackBar1.Value;
        }

    }
}
