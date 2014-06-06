using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPolacaInversa
{
    public class AFD
    {
        private readonly Nodo _arbol;
        private readonly List<Nodo> _nodos = new List<Nodo>();

		/// <summary>
		/// obtiene el arbol AFD
		/// </summary>
		/// <value>The arbol.</value>
        public Nodo Arbol
        {
            get { return _arbol; }
        }

		/// <summary>
		/// Inicializa AFD a partir de una expresion regular
		/// </summary>
		/// <param name="expresionRegular">Expresion regular.</param>
        public AFD(string expresionRegular)
        {
            _arbol = GeneraArbol(expresionRegular);
			Inicializa(_arbol);
            GeneraSiguientePos();
        }

		/// <summary>
		/// Inicializa el arbol con Nulleables, Primera y Ultima Pos
		/// </summary>
		/// <param name="nodo">Nodo cabeza del arbol</param>
		private void Inicializa(Nodo nodo)
        {
            if(nodo == null)
                return;
			Inicializa(nodo.Izquierdo);
			Inicializa(nodo.Derecho);
            
			_nodos.Add(nodo);
			EsNulleable (nodo);
			GeneraPrimeraPos (nodo);
			GeneraUltimaPos (nodo);
        }

		/// <summary>
		/// Establece si es nulleable un arbol
		/// </summary>
		/// <param name="nodo">Nodo.</param>
		private void EsNulleable(Nodo nodo){
			if (EsUnario(nodo.Id) && (nodo.Id != '+') ||
				(nodo.Id == '.' && ((nodo.Izquierdo.EsAnulable && nodo.Derecho.EsAnulable))) ||
				(nodo.Id == '|' && (nodo.Izquierdo.EsAnulable || nodo.Derecho.EsAnulable)))
			{
				nodo.EsAnulable = true;
			}
		}

		/// <summary>
		/// Genera el calculo de Primera Pos
		/// </summary>
		/// <param name="nodo">Nodo cabeza del arbol</param>
		private void GeneraPrimeraPos(Nodo nodo)
        {
            if (nodo.GetType() == typeof (Hoja))
            {
                var hoja = (Hoja) nodo;
                nodo.PrimeraPos = new[] { hoja.Numero };
            }
            if (EsUnario(nodo.Id) || 
                (nodo.Id == '.' && !nodo.Izquierdo.EsAnulable))
            {
                nodo.PrimeraPos = nodo.Izquierdo.PrimeraPos;
            }
            if (nodo.Id == '|' ||
                (nodo.Id == '.' && nodo.Izquierdo.EsAnulable))
            {
                nodo.PrimeraPos = nodo.Izquierdo.PrimeraPos.Union(nodo.Derecho.PrimeraPos).ToArray();
            }
        }

		/// <summary>
		/// Genera UltimaPos
		/// </summary>
		/// <param name="nodo">Nodo cabeza</param>
		private void GeneraUltimaPos(Nodo nodo)
        {
            if (nodo.GetType() == typeof(Hoja))
            {
                var hoja = (Hoja)nodo;
                nodo.UltimaPos = new[] { hoja.Numero };
            }
            if (EsUnario(nodo.Id))                 
            {
                nodo.UltimaPos = nodo.Izquierdo.UltimaPos;
            }
            if (nodo.Id == '|')                
            {
                nodo.UltimaPos = nodo.Izquierdo.UltimaPos.Union(nodo.Derecho.UltimaPos).ToArray();
            }
            if (nodo.Id == '.')
            {
                if (nodo.Derecho.EsAnulable)
                {
                    nodo.UltimaPos = nodo.Izquierdo.UltimaPos.Union(nodo.Derecho.UltimaPos).ToArray();
                }
                else
                {
                    nodo.UltimaPos = nodo.Derecho.UltimaPos;
                }
            }
        }

		/// <summary>
		/// Genera siguiente pos y almacena el resultado en las hojas del arbol
		/// </summary>
		private void GeneraSiguientePos()
        {
            var nodosPunto = (from n in _nodos where n.Id == '.' select n).ToArray();
            var nodosOtros = (from n in _nodos where (n.Id == '+' || n.Id == '*') select n).ToArray();
            var hojas = (from h in _nodos where h.GetType() == typeof(Hoja) select (Hoja)h).ToArray();

            foreach (var nodo in nodosPunto)
            {
                foreach (var i in nodo.Izquierdo.UltimaPos)
                {
                    hojas[i - 1].SigPos = hojas[i - 1].SigPos.Union(nodo.Derecho.PrimeraPos).ToArray();
                } 
            }

            foreach (var nodo in nodosOtros)
            {
                foreach (var i in nodo.PrimeraPos)
                {
                    hojas[i - 1].SigPos = hojas[i - 1].SigPos.Union(nodo.Izquierdo.UltimaPos).ToArray();
                }
            }
        }

		/// <summary>
		/// Genera un arbol con una expresion regular
		/// </summary>
		/// <returns>El arbol.</returns>
		/// <param name="expresionRegular">Expresion regular polaca inversa</param>
		private Nodo GeneraArbol(string expresionRegular)
        {
            int number = 1;
            var pila = new Stack<Nodo>();
            
            foreach(var c in expresionRegular)          
            {
                if (c != '#' && EsMetacaracter(c))
                {
                    var op1 = pila.Pop();
                    Nodo op2 = null;
                    if (!EsUnario(c)) // es operador binario
                    {
                        op2 = pila.Pop();
                    }
                    var resultado = Resolver(c, op1, op2);
                    pila.Push(resultado);
                }
                else // operando ó token
                {
                    pila.Push(new Hoja { Id = c, Numero = number++, SigPos = new int[]{} });
                }
            }
            return pila.Pop();
        }

		/// <summary>
		/// Evalua una expresion
		/// </summary>
		/// <param name="operador">Operador.</param>
		/// <param name="op1">Op1.</param>
		/// <param name="op2">Op2.</param>
		private Nodo Resolver(char operador, Nodo op1, Nodo op2 = null)
        {
            Nodo nodoOp;
            if (op2 == null || EsUnario(operador))
            {
                nodoOp = new Nodo { Id = operador, Izquierdo = op1 };
            }
            else
            {
                nodoOp = new Nodo { Id = operador, Derecho = op1, Izquierdo = op2 };
                op2.Padre = nodoOp;
            }
            op1.Padre = nodoOp;
            return nodoOp;
        }

		/// <summary>
		/// Es Metacaracter
		/// </summary>
		/// <returns><c>true</c>, Si fue metacaracter, <c>false</c> si no lo es.</returns>
		/// <param name="dato">Dato.</param>
		private bool EsMetacaracter(char dato)
		{
		    return dato == '*' || 
                dato == '+' || 
                dato == '?' || 
                dato == '.' || 
                dato == ')' || 
                dato == '(' || 
                dato == '[' || 
                dato == ']' || 
                dato == '-' || 
                dato == '|' || 
                dato == '\\' || 
                dato == '#';
		}

		/// <summary>
		/// Es unario.
		/// </summary>
		/// <returns><c>true</c>, Si fue unario, <c>false</c> si no lo es.</returns>
		/// <param name="operador">Operador.</param>
		private bool EsUnario(char operador)
        {
            return (operador == '+' || operador == '*' || operador == '?') ? true : false; 
        }

    }
}
