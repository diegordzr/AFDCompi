
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.Fixed fixed4;
	
	private global::Gtk.Button btnGenerate;
	
	private global::Gtk.Entry rTxtBPInversa;
	
	private global::Gtk.Entry rTxtBNormalizada;
	
	private global::Gtk.Entry rTxtBNInfija;
	
	private global::Gtk.Button btnCreateTree;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.fixed4 = new global::Gtk.Fixed ();
		this.fixed4.Name = "fixed4";
		this.fixed4.HasWindow = false;
		// Container child fixed4.Gtk.Fixed+FixedChild
		this.btnGenerate = new global::Gtk.Button ();
		this.btnGenerate.CanFocus = true;
		this.btnGenerate.Name = "btnGenerate";
		this.btnGenerate.UseUnderline = true;
		this.btnGenerate.Label = global::Mono.Unix.Catalog.GetString ("Generar");
		this.fixed4.Add (this.btnGenerate);
		global::Gtk.Fixed.FixedChild w1 = ((global::Gtk.Fixed.FixedChild)(this.fixed4 [this.btnGenerate]));
		w1.X = 35;
		w1.Y = 113;
		// Container child fixed4.Gtk.Fixed+FixedChild
		this.rTxtBPInversa = new global::Gtk.Entry ();
		this.rTxtBPInversa.CanFocus = true;
		this.rTxtBPInversa.Name = "rTxtBPInversa";
		this.rTxtBPInversa.IsEditable = true;
		this.rTxtBPInversa.InvisibleChar = '●';
		this.fixed4.Add (this.rTxtBPInversa);
		global::Gtk.Fixed.FixedChild w2 = ((global::Gtk.Fixed.FixedChild)(this.fixed4 [this.rTxtBPInversa]));
		w2.X = 26;
		w2.Y = 74;
		// Container child fixed4.Gtk.Fixed+FixedChild
		this.rTxtBNormalizada = new global::Gtk.Entry ();
		this.rTxtBNormalizada.CanFocus = true;
		this.rTxtBNormalizada.Name = "rTxtBNormalizada";
		this.rTxtBNormalizada.IsEditable = true;
		this.rTxtBNormalizada.InvisibleChar = '●';
		this.fixed4.Add (this.rTxtBNormalizada);
		global::Gtk.Fixed.FixedChild w3 = ((global::Gtk.Fixed.FixedChild)(this.fixed4 [this.rTxtBNormalizada]));
		w3.X = 24;
		w3.Y = 43;
		// Container child fixed4.Gtk.Fixed+FixedChild
		this.rTxtBNInfija = new global::Gtk.Entry ();
		this.rTxtBNInfija.CanFocus = true;
		this.rTxtBNInfija.Name = "rTxtBNInfija";
		this.rTxtBNInfija.IsEditable = true;
		this.rTxtBNInfija.InvisibleChar = '●';
		this.fixed4.Add (this.rTxtBNInfija);
		global::Gtk.Fixed.FixedChild w4 = ((global::Gtk.Fixed.FixedChild)(this.fixed4 [this.rTxtBNInfija]));
		w4.X = 22;
		w4.Y = 13;
		// Container child fixed4.Gtk.Fixed+FixedChild
		this.btnCreateTree = new global::Gtk.Button ();
		this.btnCreateTree.CanFocus = true;
		this.btnCreateTree.Name = "btnCreateTree";
		this.btnCreateTree.UseUnderline = true;
		this.btnCreateTree.Label = global::Mono.Unix.Catalog.GetString ("Crear Arbol");
		this.fixed4.Add (this.btnCreateTree);
		global::Gtk.Fixed.FixedChild w5 = ((global::Gtk.Fixed.FixedChild)(this.fixed4 [this.btnCreateTree]));
		w5.X = 143;
		w5.Y = 113;
		this.Add (this.fixed4);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 430;
		this.DefaultHeight = 300;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.btnGenerate.Clicked += new global::System.EventHandler (this.OnBtnGenerateClicked);
		this.btnCreateTree.Clicked += new global::System.EventHandler (this.OnBtnCreateTreeClicked);
	}
}
