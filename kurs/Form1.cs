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
        List<Particle> particles = new List<Particle>();
        Emitter emitter = new Emitter(); // добавили эмиттер
        public Form1()
        {
            InitializeComponent();
            picDisplay.Image = new Bitmap(picDisplay.Width, picDisplay.Height);

            emitter.gravityPoints.Add(new Point(
            picDisplay.Width / 2, picDisplay.Height / 2
        ));
            // добавил еще две        
            emitter.gravityPoints.Add(new Point(
              (int)(picDisplay.Width * 0.75), picDisplay.Height / 2
           ));

            emitter.gravityPoints.Add(new Point(
               (int)(picDisplay.Width * 0.25), picDisplay.Height / 2
           ));

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
        // добавляем переменные для хранения положения мыши
        private int MousePositionX = 0;
        private int MousePositionY = 0;
        private void picDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            // в обработчике заносим положение мыши в переменные для хранения положения мыши
            emitter.MousePositionX = e.X;
            emitter.MousePositionY = e.Y;
        }
    }
}
