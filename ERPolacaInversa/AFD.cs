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

        public Nodo Arbol
        {
            get { return _arbol; }
        }

        public AFD(string expresionRegular)
        {
            _arbol = GeneraArbol(expresionRegular);
            EsNuleable(_arbol);
            GeneraPrimeraPos(_arbol);
            GeneraUltimaPos(_arbol);
            GeneraSiguientePos();
        }

        public void EsNuleable(Nodo nodo)
        {
            if(nodo == null)
                return;
            EsNuleable(nodo.Izquierdo);
            EsNuleable(nodo.Derecho);
            _nodos.Add(nodo);
            if (EsUnario(nodo.Id) && (nodo.Id != '+') ||
                (nodo.Id == '.' && ((nodo.Izquierdo.EsAnulable && nodo.Derecho.EsAnulable))) ||
                (nodo.Id == '|' && (nodo.Izquierdo.EsAnulable || nodo.Derecho.EsAnulable)))
            {
                nodo.EsAnulable = true;
            }
        }

        public void GeneraPrimeraPos(Nodo nodo)
        {
            if (nodo == null)
                return;
            GeneraPrimeraPos(nodo.Izquierdo);
            GeneraPrimeraPos(nodo.Derecho);

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

        public void GeneraUltimaPos(Nodo nodo)
        {
            if (nodo == null)
                return;
            GeneraUltimaPos(nodo.Izquierdo);
            GeneraUltimaPos(nodo.Derecho);

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

        public void GeneraSiguientePos()
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

        public Nodo GeneraArbol(string expresionRegular)
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

        public Nodo Resolver(char operador, Nodo op1, Nodo op2 = null)
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

		public bool EsMetacaracter(char dato)
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

        public bool EsUnario(char operador)
        {
            return (operador == '+' || operador == '*' || operador == '?') ? true : false; 
        }

    }
}
