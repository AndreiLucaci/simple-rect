using System;
using System.Collections;
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

		private Rectangle _initialRectangle;

		private int _maxRects = 20;

		private void InitRect()
		{
			var rand = new Random();

			var x = rand.Next(panel1.Width - 50);
			var y = rand.Next(panel1.Height - 50);

			_initialRectangle = new Rectangle(new Point(x, y), new Size(rand.Next(panel1.Width - x), rand.Next(panel1.Height - y)));
		}

		private void DrawRects(Graphics g, IEnumerable<Rectangle> rects)
		{
			panel1.Invoke(new Action(() =>
			{
				foreach (var rectangle in rects)
				{
					g.DrawRectangle(Pens.BurlyWood, rectangle);
				}

				g.DrawRectangle(Pens.Blue, _initialRectangle);
			}));
		}

		private IEnumerable<Rectangle> SplitRectangles(IEnumerable<Rectangle> initialList)
		{
			var list = new List<Rectangle>();
			var rnd = new Random();

			var initList = initialList.ToList();
			initList.Remove(_initialRectangle);

			for (var i = 0; i < _maxRects - initList.Count; i++)
			{
				if (initList.Count == 0)
				{
					initList = new List<Rectangle>(list);
				}
				var rect = initList.RandomPop();
				for (var j = 0; j < 2; j++)
				{
					var rndnr = 2;
					list.Add(new Rectangle(rect.X + rect.Width / rndnr * j, rect.Y, rect.Width / rndnr, rect.Height));
				}
			}

			list.AddRange(initList);
			return list;
		}

		private IEnumerable<Rectangle> CreateInitialRectangles()
		{
			var rects = new List<Rectangle>
			{
				new Rectangle(new Point(0, 0), new Size(_initialRectangle.Location)),
				new Rectangle(new Point(_initialRectangle.X, 0), new Size(_initialRectangle.Width, _initialRectangle.Y)),
				new Rectangle(new Point(_initialRectangle.X + _initialRectangle.Width),
					new Size(panel1.Width - _initialRectangle.Right - 10, _initialRectangle.Y)),
				new Rectangle(new Point(0, _initialRectangle.Y), new Size(_initialRectangle.Left, _initialRectangle.Height)),
				new Rectangle(new Point(_initialRectangle.Right, _initialRectangle.Top),
					new Size(panel1.Width - _initialRectangle.Right - 10, _initialRectangle.Height)),
				new Rectangle(new Point(0, _initialRectangle.Bottom),
					new Size(_initialRectangle.Left, panel1.Height - _initialRectangle.Bottom - 10)),
				new Rectangle(new Point(_initialRectangle.X, _initialRectangle.Y + _initialRectangle.Height),
					new Size(_initialRectangle.Width, panel1.Height - _initialRectangle.Bottom - 10)),
				new Rectangle(
					new Point(_initialRectangle.X + _initialRectangle.Width, _initialRectangle.Y + _initialRectangle.Height),
					new Size(panel1.Width - _initialRectangle.Right - 10, panel1.Height - _initialRectangle.Bottom - 10))
			};

			return rects;
		}

		private IEnumerable<Rectangle> Solve()
		{
			var parts = CreateInitialRectangles();
			parts = SplitRectangles(parts);
			return parts;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (!int.TryParse(textBox2.Text, out _maxRects))
			{
				MessageBox.Show("Invalid number buei!");
				return;
			}

			var g = panel1.CreateGraphics();
			panel1.Hide();
			panel1.Show();
			InitRect();

			var items = Solve();
			DrawRects(g, items);
		}
	}

	public static class ListExtensions
	{
		public static T RandomPop<T>(this List<T> list)
		{
			var random = new Random();
			var randomNumber = random.Next(list.Count - 1);
			var item = list[randomNumber];
			list.RemoveAt(randomNumber);
			return item;
		}
	}
}
