using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Resources;
using System.Windows.Forms;

namespace Pseudo
{
	public class MainForm : Form
	{
		private OpenFileDialog m_OpenFile;

		private int numProcedimientos;

		private string tabString = "    ";

		private TextReader reader;

		private TextWriter writter;

		private MainForm.TipoElemento[] fines;

		private IContainer components;

		private Label label1;

		private TextBox tbRutaArchivo;

		private Button btnSeleccionarArchivo;

		private Button btnConvertir;

		private TextBox tbResultado;

		private Label label2;

		public MainForm()
		{
			this.InitializeComponent();
			this.m_OpenFile = new OpenFileDialog()
			{
				Filter = "Archivo DFD (*.dfd)|*.dfd|Todos los archivos|*"
			};
			MainForm.TipoElemento[] tipoElementoArray = new MainForm.TipoElemento[] { MainForm.TipoElemento.FIN_BUCLE_FOR, MainForm.TipoElemento.FIN_BUCLE_WHILE, MainForm.TipoElemento.FIN_LADO_IF, MainForm.TipoElemento.FIN_MAIN, MainForm.TipoElemento.FIN_SUBPROGRAMA };
			this.fines = tipoElementoArray;
		}

		private void btnConvertir_Click(object sender, EventArgs e)
		{
			try
			{
				try
				{
					this.reader = new StreamReader(this.tbRutaArchivo.Text);
					this.writter = new StringWriter();
					this.comprobarEncabezado(this.reader);
					int num = 0;
					while (num < this.numProcedimientos)
					{
						switch (Convert.ToInt32(this.reader.ReadLine()))
						{
							case 0:
							{
								this.leerMain();
								goto case 1;
							}
							case 1:
							{
								this.writter.WriteLine();
								num++;
								continue;
							}
							case 2:
							{
								this.leerSubprograma();
								goto case 1;
							}
							default:
							{
								goto case 1;
							}
						}
					}
					this.tbResultado.Text = this.writter.ToString();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					MessageBox.Show(string.Concat("No se puede convertir el archivo \"", this.tbRutaArchivo.Text, "\"\nMensaje:\n", exception.Message), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
			}
			finally
			{
				if (this.writter != null)
				{
					this.writter.Close();
				}
				if (this.reader != null)
				{
					this.reader.Close();
				}
			}
		}

		private void btnSeleccionarArchivo_Click(object sender, EventArgs e)
		{
			this.m_OpenFile.FileName = this.tbRutaArchivo.Text;
			if (this.m_OpenFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				this.tbRutaArchivo.Text = this.m_OpenFile.FileName;
			}
		}

		private void comprobarEncabezado(TextReader reader)
		{
			string str = reader.ReadLine();
			if (str != "\u0004 Dfd \u0002\b(c)")
			{
				throw new Exception("No se trata de un archivo DFD");
			}
			str = reader.ReadLine();
			if (Convert.ToInt32(str) != 1)
			{
				throw new Exception(string.Concat("No se soporta el formato de archivo ", str));
			}
			str = reader.ReadLine();
			if (Convert.ToInt32(str) != 1)
			{
				throw new Exception("Excelente, un BOOL_3 que no es 1. Revisalo!");
			}
			this.numProcedimientos = Convert.ToInt32(reader.ReadLine());
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void escibirEspacios(int profundidad)
		{
			for (int i = 0; i < profundidad; i++)
			{
				this.writter.Write(this.tabString);
			}
		}

		private bool esFin(MainForm.TipoElemento tipo)
		{
			for (int i = 0; i < (int)this.fines.Length; i++)
			{
				if (tipo == this.fines[i])
				{
					return true;
				}
			}
			return false;
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainForm));
			this.label1 = new Label();
			this.tbRutaArchivo = new TextBox();
			this.btnSeleccionarArchivo = new Button();
			this.btnConvertir = new Button();
			this.tbResultado = new TextBox();
			this.label2 = new Label();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(11, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(68, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Archivo DFD";
			this.tbRutaArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.tbRutaArchivo.Location = new Point(86, 12);
			this.tbRutaArchivo.Name = "tbRutaArchivo";
			this.tbRutaArchivo.Size = new System.Drawing.Size(520, 20);
			this.tbRutaArchivo.TabIndex = 1;
			this.btnSeleccionarArchivo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
			this.btnSeleccionarArchivo.Location = new Point(612, 11);
			this.btnSeleccionarArchivo.Name = "btnSeleccionarArchivo";
			this.btnSeleccionarArchivo.Size = new System.Drawing.Size(25, 23);
			this.btnSeleccionarArchivo.TabIndex = 2;
			this.btnSeleccionarArchivo.Text = "...";
			this.btnSeleccionarArchivo.UseVisualStyleBackColor = true;
			this.btnSeleccionarArchivo.Click += new EventHandler(this.btnSeleccionarArchivo_Click);
			this.btnConvertir.Location = new Point(12, 40);
			this.btnConvertir.Name = "btnConvertir";
			this.btnConvertir.Size = new System.Drawing.Size(75, 23);
			this.btnConvertir.TabIndex = 4;
			this.btnConvertir.Text = "Convertir";
			this.btnConvertir.UseVisualStyleBackColor = true;
			this.btnConvertir.Click += new EventHandler(this.btnConvertir_Click);
			this.tbResultado.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.tbResultado.Font = new System.Drawing.Font("Arial", 11.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.tbResultado.Location = new Point(12, 69);
			this.tbResultado.Multiline = true;
			this.tbResultado.Name = "tbResultado";
			this.tbResultado.ReadOnly = true;
			this.tbResultado.ScrollBars = ScrollBars.Both;
			this.tbResultado.Size = new System.Drawing.Size(626, 210);
			this.tbResultado.TabIndex = 5;
			this.tbResultado.WordWrap = false;
			this.label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(12, 286);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(553, 91);
			this.label2.TabIndex = 6;
			this.label2.Text = componentResourceManager.GetString("label2.Text");
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(651, 386);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.tbResultado);
			base.Controls.Add(this.btnConvertir);
			base.Controls.Add(this.btnSeleccionarArchivo);
			base.Controls.Add(this.tbRutaArchivo);
			base.Controls.Add(this.label1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Name = "MainForm";
			this.Text = "Convertidor de DFD a PseudoCódigo © Gabriel Guillermo Gómez Puentes Mayo/2009";
			base.WindowState = FormWindowState.Maximized;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		private MainForm.TipoElemento leerBloque(int profundidad)
		{
			MainForm.TipoElemento num;
			profundidad++;
			while (true)
			{
				num = (MainForm.TipoElemento)Convert.ToInt32(this.reader.ReadLine());
				if (!this.esFin(num))
				{
					this.escibirEspacios(profundidad);
				}
				switch (num)
				{
					case MainForm.TipoElemento.SALIDA:
					{
						this.writter.WriteLine(string.Concat("Imprima ", this.leerCadena(true)));
						continue;
					}
					case MainForm.TipoElemento.LECTURA:
					{
						this.writter.WriteLine(string.Concat("Lea ", this.leerCadena(true)));
						continue;
					}
					case MainForm.TipoElemento.ASIGNACIÓN:
					{
						int num1 = Convert.ToInt32(this.reader.ReadLine());
						for (int i = 0; i < num1; i++)
						{
							if (i > 0)
							{
								this.escibirEspacios(profundidad);
							}
							this.writter.WriteLine(string.Concat(this.leerCadena(false), " ← ", this.leerCadena(false)));
						}
						continue;
					}
					case MainForm.TipoElemento.BUCLE_IF:
					{
						this.writter.Write("Si ");
						bool flag = Convert.ToBoolean(Convert.ToInt32(this.reader.ReadLine()));
						if (!flag)
						{
							this.writter.Write(" No (");
						}
						this.writter.Write(this.leerCadena(true));
						if (!flag)
						{
							this.writter.Write(")");
						}
						this.writter.WriteLine(":");
						if (this.leerBloque(profundidad) != MainForm.TipoElemento.FIN_LADO_IF)
						{
							throw new Exception("Falta FIN_LADO_IF");
						}
						this.escibirEspacios(profundidad);
						this.writter.WriteLine("Sino:");
						if (this.leerBloque(profundidad) != MainForm.TipoElemento.FIN_LADO_IF)
						{
							throw new Exception("Falta FIN_LADO_IF");
						}
						this.escibirEspacios(profundidad);
						this.writter.WriteLine("FinSi");
						continue;
					}
					case MainForm.TipoElemento.BUCLE_FOR:
					{
						this.writter.Write("Para ");
						this.writter.Write(string.Concat(this.leerCadena(true), ", "));
						this.writter.Write(string.Concat(this.leerCadena(true), ", "));
						this.writter.Write(string.Concat(this.leerCadena(true), ", "));
						this.writter.Write(this.leerCadena(true));
						this.writter.WriteLine(":");
						if (this.leerBloque(profundidad) != MainForm.TipoElemento.FIN_BUCLE_FOR)
						{
							throw new Exception("Falta FIN_BUCLE_FOR");
						}
						this.escibirEspacios(profundidad);
						this.writter.WriteLine("FinPara");
						continue;
					}
					case MainForm.TipoElemento.BUCLE_WHILE:
					{
						this.writter.Write("Mientras ");
						this.writter.WriteLine(string.Concat(this.leerCadena(true), ":"));
						if (this.leerBloque(profundidad) != MainForm.TipoElemento.FIN_BUCLE_WHILE)
						{
							throw new Exception("Falta FIN_BUCLE_WHILE");
						}
						this.escibirEspacios(profundidad);
						this.writter.WriteLine("FinMientras");
						continue;
					}
					case MainForm.TipoElemento.FIN_BUCLE_FOR:
					case MainForm.TipoElemento.FIN_BUCLE_WHILE:
					{
						return num;
					}
					case MainForm.TipoElemento.INVOCACION:
					{
						this.writter.WriteLine(string.Concat(this.leerCadena(true), "( ", this.leerCadena(true), " )"));
						continue;
					}
					default:
					{
						return num;
					}
				}
			}
			throw new Exception("Falta FIN_LADO_IF");
			return num;
		}

		private string leerCadena(bool revisar)
		{
			bool flag;
			flag = (!revisar ? true : Convert.ToBoolean(Convert.ToInt32(this.reader.ReadLine())));
			if (!flag)
			{
				return string.Empty;
			}
			string str = this.reader.ReadLine();
			int num = Convert.ToInt32(str);
			str = this.reader.ReadLine();
			if (num != str.Length)
			{
				object[] length = new object[] { "Cadena de tamaño incorrecto: \"", str, "\" [", str.Length, "]\n" };
				throw new Exception(string.Concat(length));
			}
			return str;
		}

		private void leerMain()
		{
			string str = this.leerCadena(true);
			this.writter.WriteLine(string.Concat("# ", str));
			this.writter.WriteLine("Funcion Main():");
			if (this.leerBloque(0) != MainForm.TipoElemento.FIN_MAIN)
			{
				throw new Exception("Main sin finalizar");
			}
			this.writter.WriteLine("FinFuncion");
		}

		private void leerSubprograma()
		{
			string str = this.leerCadena(true);
			string str1 = this.leerCadena(true);
			string str2 = this.leerCadena(true);
			this.writter.WriteLine(string.Concat("# ", str2));
			TextWriter textWriter = this.writter;
			string[] strArrays = new string[] { "Funcion ", str, "(", str1, "):" };
			textWriter.WriteLine(string.Concat(strArrays));
			if (this.leerBloque(0) != MainForm.TipoElemento.FIN_SUBPROGRAMA)
			{
				throw new Exception("SUBPROGRAMA sin finalizar");
			}
			this.writter.WriteLine("FinFuncion");
		}

		private enum TipoElemento
		{
			MAIN,
			FIN_MAIN,
			SUBPROGRAMA,
			FIN_SUBPROGRAMA,
			SALIDA,
			LECTURA,
			ASIGNACIÓN,
			BUCLE_IF,
			BUCLE_FOR,
			BUCLE_WHILE,
			FIN_BUCLE_FOR,
			FIN_BUCLE_WHILE,
			INVOCACION,
			FIN_LADO_IF
		}
	}
}