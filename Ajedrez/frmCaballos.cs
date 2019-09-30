using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class frmCaballos : Form
    {
        Casilla[,] tablero = new Casilla[10, 10];
        int selectX, selectY, tam, max = 0, nSol, x, y;
        internal int n = 2;
        ListBox lbSoluciones;
        bool[,] sol;
        List<Solucion> soluciones = new List<Solucion>();
        //List<Point> posiciones = new List<Point>();
        Stack<Point> posiciones = new Stack<Point>();

        internal frmCaballos()
        {
            InitializeComponent();
            inicializarTablero();
        }
        void onButtonClick(object source, EventArgs e)
        {
            Button b = (Button)source;
            int x = b.Location.X / 69, y = b.Location.Y / 69;
            iluminaPosibles(selectX, selectY, "");
            iluminaPosibles(x, y, "l");
            selectX = x; selectY = y;
        }
        void mosaico(int x, int y, String l, bool nxt)
        {
            if (tablero[x, y].pza != null && nxt && l != "")
                l = "r";
            if ((x + y) % 2 == 0)
                tablero[x, y].btn.BackgroundImage = Image.FromFile(l + "wmarbel.png");
            else
                tablero[x, y].btn.BackgroundImage = Image.FromFile(l + "bmarbel.png");
            tablero[x, y].btn.BackgroundImage.Tag = l;
        }
        void iluminar(int x, int y, String l)
        {
            if (!(x < 0 || x > n - 1 || y < 0 || y > n - 1))
                mosaico(x, y, l, true);
        }
        internal void iluminaPosibles(int x, int y, String l)
        {
            if (x > 0)
            {
                if (y > 0)//Esquina Superior Izquierda
                {
                    iluminar(x - 2, y - 1, l);
                    iluminar(x - 1, y - 2, l);
                }
                if (y < n - 1)//Esquina Inferior Izquierda
                {
                    iluminar(x - 2, y + 1, l);
                    iluminar(x - 1, y + 2, l);
                }
            }
            if (x < n - 1)
            {
                if (y > 0)//Esquina Superior Derecha
                {
                    iluminar(x + 1, y - 2, l);
                    iluminar(x + 2, y - 1, l);
                }
                if (y < n - 1)//Esquina Inferior Derecha
                {
                    iluminar(x + 2, y + 1, l);
                    iluminar(x + 1, y + 2, l);
                }
            }
        }
        void inicializarTablero()
        {
            DateTime ti, tf;
            int i, j;
            Tamaño dtam = new Tamaño(this);
            dtam.ShowDialog();
            tam = n * n;
            ti = DateTime.Now;
            caballos(0);
            tf = DateTime.Now;
            TimeSpan tt = new TimeSpan(tf.Ticks - ti.Ticks);
            MessageBox.Show("Tiempo" + tt.ToString());
            this.Size = new System.Drawing.Size(n * 70 + 100, n * 70 + 25);
            lbSoluciones = new System.Windows.Forms.ListBox();
            lbSoluciones.FormattingEnabled = true;
            lbSoluciones.Location = new System.Drawing.Point(n * 69, 0);
            lbSoluciones.Size = new System.Drawing.Size(100, n * 70);
            lbSoluciones.TabIndex = 0;
            lbSoluciones.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Soluciones_MouseClick);
            this.Controls.Add(lbSoluciones);
            nSol = 0;
            i = 1;
            foreach (Solucion s in soluciones)
            {
                if (s.nK == max)
                {
                    lbSoluciones.Items.Add("Solucion: " + i.ToString());
                    i++;
                }
                else
                    nSol++;
            }
            for (i = 0; i < n; i++)//Dibujo Tablero
                for (j = 0; j < n; j++)
                {
                    tablero[i, j] = new Casilla();
            		tablero[i, j].btn=new Button();
                    tablero[i, j].btn.Size = new System.Drawing.Size(70, 70);
                    tablero[i, j].btn.Location = new System.Drawing.Point(i * 69, j * 69);
                    tablero[i, j].btn.FlatAppearance.BorderSize = 0;
                    mosaico(i, j, "", false);
                    tablero[i, j].btn.Click += new EventHandler(onButtonClick);
                    this.Controls.Add(tablero[i, j].btn);
                }
        }
        void comenzarNuevo()
        { 
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    mosaico(i, j, "", false);
                    tablero[i, j].pza = new Pieza("knight", true);
                    tablero[i, j].colocaPieza();
                }
        }
        //---------------------------------------Caballos----------------------------------
        private void Soluciones_MouseClick(object sender, MouseEventArgs e)
        {
            ListBox lbL = (ListBox)sender;
            for (int x = 0; x < n; x++)
                for (int y = 0; y < n; y++)
                {
                    mosaico(x, y, "", false);
                    if (soluciones.ElementAt(nSol + lbL.SelectedIndex).sol[x, y])
                        tablero[x, y].pza = new Pieza("knight", true);
                    else
                        tablero[x, y].pza = null;
                    tablero[x, y].colocaPieza();
                }
        }
        void caballos(int st)
        {
            if (st == tam || (st == tam - 1 && max == posiciones.Count))
            {
                if (posiciones.Count >= max)
                {
                    max = posiciones.Count;
                    sol = new bool[n, n];
                    foreach (Point p in posiciones)
                        sol[p.X, p.Y] = true;
                    soluciones.Add(new Solucion(sol, posiciones.Count));
                }
            }
            else
                for (int p = st; p < tam; p++)
                {
                    x = p % n;
                    y = p / n;
                    if (!atacada())
                    {
                        posiciones.Push(new Point(x, y));
                        caballos(p + 1);
                        posiciones.Pop();
                    }
                }
        }
        bool atacada()
        {
            int dx, dy;
            foreach (Point c in posiciones)
            {
                dx = Math.Abs(c.X - x);
                dy = Math.Abs(c.Y - y);
                if (dx < 3 && dy < 3 && dx + dy == 3)
                    return true;
            }
            return false;
        }
    }
}
