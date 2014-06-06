using System;
using System.Collections.Generic;
using Gtk;
using GtkAFD;
using ERPolacaInversa;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnBtnGenerateClicked (object sender, EventArgs e)
	{
		bool band = true;
		var re = new RegularExpression ();


		String cadena = this.rTxtBNInfija.Text;
		if (cadena != "")
			cadena = re.normaliza (cadena);
		else {
			MessageDialog md = new MessageDialog (null, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, "Error");
			md.Run ();
			md.Destroy();
		}

		if (!band)
			this.rTxtBNormalizada.Text = "ERROR";
		else
			this.rTxtBNormalizada.Text = cadena + ".#";


		if (this.rTxtBNormalizada.Text == "ERROR")
			this.rTxtBPInversa.Text = "ERROR";
		else
		{
			this.rTxtBPInversa.Text = re.polacaInv(cadena) + "#.";//Realiza las operaciones de Notacion Polaca Inversa
			//this.btnArbol.Enabled = true;
		}
	}

	protected void OnBtnCreateTreeClicked (object sender, EventArgs e)
	{
		try
		{
			var arbol = AFD.GeneraArbol(this.rTxtBPInversa.Text);
		}
		catch (Exception)
		{
			MessageDialog md = new MessageDialog (null, 
				DialogFlags.Modal, 
				MessageType.Error, 
				ButtonsType.Ok, 
				"Ha ocurrido un error al generar el arbol");
			md.Run ();
			md.Destroy();   
		}
	}
}
