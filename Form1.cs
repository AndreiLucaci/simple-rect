using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		List<Rectangle> Rects = new List<Rectangle>();

		private int maxRects = 20;

		private void InitRects()
		{
			Random rand = new Random();

			for (int i = 0; i < maxRects; ++i) // Rects[0] is the model
			{
				int x = rand.Next(panel1.Width - 50);
				int y = rand.Next(panel1.Height - 50);

				Rects.Add(new Rectangle(new Point(x, y), new Size(rand.Next(panel1.Width - x), rand.Next(panel1.Height - y))));
			}
		}

		private void DrawRects(Graphics g)
		{
			g.DrawRectangle(Pens.Blue, Rects[0]);
			foreach (var rectangle in CreateInitialRectangle(Rects[0]))
			{
				g.DrawRectangle(Pens.BurlyWood, rectangle);
			};

			g.DrawRectangle(Pens.Black, new Rectangle(10, 40, 10, 10));
		}

		private IEnumerable<Rectangle> CreateInitialRectangle(Rectangle r)
		{
			var a = r.Location;
			var rect = new Rectangle(new Point(0, 0), new Size(a));


			var rects = new List<Rectangle>
			{
				new Rectangle(new Point(0, 0), new Size(r.Location)),
				new Rectangle(new Point(r.X, 0), new Size(r.Width, r.Y)),
				new Rectangle(new Point(r.X + r.Width), new Size(panel1.Width - r.Right - 10, r.Y)),
				new Rectangle(new Point(0, r.Y), new Size(r.Left, r.Height)),
				new Rectangle(new Point(r.Right, r.Top), new Size(panel1.Width - r.Right - 10, r.Height)),
				new Rectangle(new Point(0, r.Bottom), new Size(r.Left, panel1.Height - r.Bottom - 10)),
				new Rectangle(new Point(r.X, r.Y + r.Height), new Size(r.Width, panel1.Height - r.Bottom - 10)),
				new Rectangle(new Point(r.X + r.Width, r.Y + r.Height), new Size(panel1.Width - r.Right - 10, panel1.Height - r.Bottom - 10))
			};

			return rects;
		}

		private bool Solve(Rectangle R)
		{
			// if there is a rectangle containing R
			for (int i = 1; i < Rects.Count; ++i)
			{
				if (Rects[i].Contains(R))
				{
					return true;
				}
			}

			if (R.Width <= 3 && R.Height <= 3)
			{
				return false;
			}

			Rectangle UpperLeft = new Rectangle(new Point(R.X, R.Y), new Size(R.Width / 2, R.Height / 2));
			Rectangle UpperRight = new Rectangle(new Point(R.X + R.Width / 2 + 1, R.Y), new Size(R.Width / 2, R.Height / 2));
			Rectangle LowerLeft = new Rectangle(new Point(R.X, R.Y + R.Height / 2 + 1), new Size(R.Width / 2, R.Height / 2));
			Rectangle LowerRight = new Rectangle(new Point(R.X + R.Width / 2 + 1, R.Y + +R.Height / 2 + 1), new Size(R.Width / 2, R.Height / 2));

			return Solve(UpperLeft) && Solve(UpperRight) && Solve(LowerLeft) && Solve(LowerRight);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(textBox2.Text, out maxRects))
			{
				MessageBox.Show("Invalid number buei!");
				return;
			}

			Graphics g = panel1.CreateGraphics();
			panel1.Hide();
			panel1.Show();
			Rects.Clear();

			InitRects();
			DrawRects(g);
			

			textBox1.Text = Solve(Rects[0]).ToString();
		}
	}
}
