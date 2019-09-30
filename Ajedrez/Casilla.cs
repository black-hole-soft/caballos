using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Ajedrez
{
	public class Casilla
	{
		public Button btn;
        public Pieza pza;
        public void colocaPieza()
        {
            if (pza == null)
                btn.Image = null;
            else
                btn.Image = Image.FromFile("w" + pza.roll + ".png");
        }
	}
}
