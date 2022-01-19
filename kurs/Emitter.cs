﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace kurs
{
    class Emitter
    {
        List<Particle> particles = new List<Particle>();
        public int MousePositionX;
        public int MousePositionY;

        public void UpdateState()
        {
            foreach (var particle in particles)
            {
                particle.Life -= 1; // уменьшаю здоровье
                                    // если здоровье кончилось
                if (particle.Life < 0)
                {
                    // восстанавливаю здоровье
                    particle.Life = 20 + Particle.rand.Next(100);
                    // перемещаю частицу в центр
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                    /*
                    particle.Direction = Particle.rand.Next(360);
                    particle.Speed = 1 + Particle.rand.Next(10);
                    */
                    var direction = (double)Particle.rand.Next(360);
                    var speed = 1 + Particle.rand.Next(10);

                    particle.SpeedX = (float)(Math.Cos(direction / 180 * Math.PI) * speed);
                    particle.SpeedY = -(float)(Math.Sin(direction / 180 * Math.PI) * speed);

                    particle.Radius = 2 + Particle.rand.Next(10);
                }
                else
                {
                    /*
                    // а это наш старый код
                    var directionInRadians = particle.Direction / 180 * Math.PI;
                    particle.X += (float)(particle.Speed * Math.Cos(directionInRadians));
                    particle.Y -= (float)(particle.Speed * Math.Sin(directionInRadians));
                    */
                    particle.X += particle.SpeedX;
                    particle.Y += particle.SpeedY;
                }
            }
            // добавил генерацию частиц
            // генерирую не более 10 штук за тик
            for (var i = 0; i < 10; ++i)
            {
                if (particles.Count < 500) // пока частиц меньше 500 генерируем новые
                {
                    var particle = new ParticleColorful();
                    particle.FromColor = Color.Yellow;
                    particle.ToColor = Color.FromArgb(0, Color.Magenta);
                    particle.X = MousePositionX;
                    particle.Y = MousePositionY;
                    particles.Add(particle);
                }
                else
                {
                    break; // а если частиц уже 500 штук, то ничего не генерирую
                }
            }
        }

        public void Render(Graphics g)
        {
            // ну тут так и быть уж сам впишу...
            // это то же самое что на форме в методе Render
            foreach (var particle in particles)
            {
                particle.Draw(g);
            }
        }
    }
}
