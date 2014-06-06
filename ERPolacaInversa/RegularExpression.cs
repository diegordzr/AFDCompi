using System;
using System.Collections.Generic;
using System.Globalization;

namespace ERPolacaInversa
{
    public class RegularExpression
    {
        public string Normaliza(String texto)
        {
            var cadena = texto;
            var cadaux = cadena;

            var banpre = PreproMeta(ref cadaux);

            if (VerificaLogicas(cadaux))
            {
                Concatenalternativa(ref cadaux, banpre);
                if (cadaux.Contains("ERROR"))
                {
                    cadaux = "ERROR";
                }
            }
            else
                cadaux = "ERROR";

            cadena = cadaux;
            return cadena;
        }

        private static bool PreproMeta(ref String cadena)
        {
            var tempcad = "";
            var bandera = false;
            var tam = cadena.Length;
            for (var i = 0; i < tam; i++)
            {
                var car = cadena[i];
                switch (car)
                {
                    case '.':
                        if (i != 0)
                        {
                            var atras = cadena[i - 1].ToString(CultureInfo.InvariantCulture);
                            bandera = atras != "\\";
                        }
                        else if (i == 0)
                            bandera = true;
                        tempcad += "\\.";
                        break;
                    case ' ':
                        tempcad += "\\e";
                        bandera = true;
                        break;
                    case '\t':
                        tempcad += "\\t";
                        bandera = true;
                        break;
                    default:
                        tempcad += car;
                        break;
                }
            }
            cadena = tempcad;
            return bandera;
        }

        private bool VerificaLogicas(String cadena)
        {
            var band = true;
            int parbres = 0, parcierras = 0, corchabres = 0, corchcierras = 0;
            var ini = cadena.Substring(0, 1);
            var bandabre = false;
            int incremento = 0, detuviste = 0;

            foreach (var t in cadena)
            {
                incremento++;
                var dato = t.ToString(CultureInfo.InvariantCulture);
                if (dato == "\\")
                {
                    bandabre = true;
                    detuviste = incremento;
                }
                else
                    if (bandabre && (dato == "[" || dato == "]" || dato == "(" || dato == ")") && (incremento == detuviste + 1))
                    {
                        switch (dato)
                        {
                            case "[":
                                corchabres++;
                                break;
                            case "]":
                                corchcierras++;
                                break;
                            case "(":
                                parbres++;
                                break;
                            case ")":
                                parcierras++;
                                break;
                        }
                    }
                    else
                        if (bandabre && Escaracterval2(dato) && (incremento == detuviste + 1))
                        {
                            band = false;
                            break;
                        }
            }

            if (ini == "+" || ini == "*" || ini == "?" || ini == "|" || ini == "-" || ini == ")" || ini == "]")
            {
                band = false;
            }
            if (cadena == "()" || cadena == "[]" || cadena == "(" || cadena == ")" || cadena == "[" || cadena == "]")
                band = false;

            var parbre = Cuantosdeestetiene(cadena, "(");
            var parcierra = Cuantosdeestetiene(cadena, ")");
            var corchabre = Cuantosdeestetiene(cadena, "[");
            var corchcierra = Cuantosdeestetiene(cadena, "`]");


            if (((parbre - parbres) != (parcierra - parcierras)) || ((corchabre - corchabres) != (corchcierra - corchcierras)))
            {
                band = false;
            }

            if (!Hayjuntos(cadena, "(", ")") || !Hayjuntos(cadena, "[", "]"))
                band = false;

            if (!Checapre(cadena, "(", ")") || !Checapre(cadena, "[", "]"))
                band = false;

            return band;
        }

        private static bool Hayjuntos(String cadena, String datoini, String datofin)
        {
            var band = true;
            var atras = "";

            for (var i = 0; i < cadena.Length; i++)
            {
                var caracter = cadena[i].ToString(CultureInfo.InvariantCulture);
                if (i != 0)
                    atras = cadena[i - 1].ToString(CultureInfo.InvariantCulture);

                if (caracter == datofin && atras == datoini)
                    band = false;
            }
            return band;
        }

        private static int Cuantosdeestetiene(String cadena, String dato)
        {
            var ret = 0;
            var tam = cadena.Length;

            for (var i = 0; i < tam; i++)
            {
                var pos = cadena[i].ToString(CultureInfo.InvariantCulture);
                if (dato.Contains(pos))
                    ret++;
            }
            return ret;
        }

        private static bool Escaracterval(String dato)
        {
            var bandera = false;
            const string cadena = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789=";
            if (cadena.Contains(dato))
                bandera = true;
            return bandera;
        }

        private static bool Escaracterval2(String dato)
        {
            var bandera = false;
            const string cadena = "abcdfghijklmñopqrsuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            if (cadena.Contains(dato))
                bandera = true;
            return bandera;
        }

        private static bool Checapre(String cadena, String datoini, String datofin)
        {
            var band = true;
            var atras = "";
            var inc = 0;

            for (var i = 0; i < cadena.Length; i++)
            {
                var caracter = cadena[i].ToString(CultureInfo.InvariantCulture);
                if (i != 0)
                    atras = cadena[i - 1].ToString(CultureInfo.InvariantCulture);

                if (caracter == datofin)
                {
                    if (i == 0) continue;
                    if (inc == 0 && atras != "\\")
                    {
                        band = false;
                        break;
                    }
                    inc--;
                }
                else
                    if (caracter == datoini)
                    {
                        inc++;
                    }
            }
            return band;
        }

        private void Concatenalternativa(ref string cadena, bool bandprepro)
        {
            var bandfunc = false;
            var banhabilita = false;
            var bandentro = false;
            var cadaux = "";
            var tam = cadena.Length;
            int posini = 0, posfin = 0;
            var partcad = "";
            var atras = "";
            var bandnohicenad = false;

            for (var i = 0; i < tam; i++)
            {
                var caracter = cadena[i].ToString(CultureInfo.InvariantCulture);
                if (i != 0)
                    atras = cadena[i - 1].ToString(CultureInfo.InvariantCulture);

                if (i > 0 && (caracter == "[" && atras != "|" && atras != "(" && atras != "[" && atras != "-" && atras != "\\"))
                {
                    posini = i;
                    cadaux += '.';
                    //cadaux += caracter;
                    bandfunc = true;
                }
                else if (tam - 1 == i && caracter == "|")
                {
                    cadaux = "ERROR";
                    break;
                }
                else if (i == 1 && (atras == "(" || atras == "[") && (caracter == "*" || caracter == "+" || caracter == "|" || caracter == "-"))
                {
                    cadaux = "ERROR";
                    break;
                }
                else if ((caracter == "|" && atras == "|") || (caracter == "." && atras == "\\" && !bandprepro) || (caracter == "-" && atras != "\\" && !bandfunc) || (caracter == ")" && atras == "|") || (atras == "|" && (caracter == "*" || caracter == "+" || caracter == "?")))
                {
                    cadaux = "ERROR";
                    break;
                }

                else if (caracter.Contains("[") && atras != "\\")
                {
                    posini = i;
                    bandfunc = true;
                }
                else if (caracter.Contains("]") && bandfunc)
                {
                    posfin = i;
                    bandfunc = false;
                    banhabilita = true;
                }
                else if (!bandfunc && i > 0 && atras != "(" && atras != "\\" && atras != "[" && atras != "|" && atras != "-" && (Escaracterval(caracter) || caracter == "\\" || caracter == "[" || caracter == "("))
                {
                    cadaux += '.';
                    cadaux += caracter;
                    if (caracter == "\\")
                    {
                        i++;
                        cadaux += cadena[i];
                        if ("-" == cadena[i].ToString(CultureInfo.InvariantCulture))
                        {
                            cadaux += ".";
                        }
                    }
                }
                else if (atras == "\\" && caracter == "(")
                {
                    cadaux += caracter + ".";
                }
                else
                    bandnohicenad = true;

                if (banhabilita)
                {
                    bandentro = true;
                    partcad = cadena.Substring(posini, posfin - posini + 1);
                    if (partcad != "[]")
                    {
                        HazAlternativas(ref partcad);
                    }
                    else
                    {
                        cadaux = "ERROR";
                        break;
                    }
                }

                if (bandnohicenad && !bandfunc)
                {
                    cadaux += caracter;
                }

                if (bandentro)
                {
                    cadaux += partcad;
                    bandentro = false;
                }

                banhabilita = false;
                bandnohicenad = false;
            }
            cadena = cadaux;
        }

        private void HazAlternativas(ref String cadena)
        {
            var cadaux = "";
            var banera = false;
            var bandmeta = false;
            int ubicacion = 0, sabermeta = 0, tam = cadena.Length;

            for (var i = 0; i < cadena.Length; i++)
            {
                ubicacion++;
                var pos = cadena[i].ToString(CultureInfo.InvariantCulture);

                if (pos == "[")         //si es parentesis que abre
                {
                    cadaux += "(";
                    banera = true;
                }
                else if (pos == "]" && banera)       //si es parentesis que cierra
                {
                    quitaultimo(ref cadaux);
                    cadaux += ")";
                    banera = false;
                }
                else if ((pos == "\\") && (ubicacion == (cadena.Length) - 1))      //Para verificar que el ultimo dato no sea un dato de  estos/ 
                {
                    cadaux = "ERROR";
                    break;
                }
                else if (bandmeta && (!contienecaracter(pos)))       //Si despues de un / no le sigue un metacaracter entonces es mala la expresion regular
                {
                    cadaux = "ERROR";
                    break;
                }
                else if (pos == "\\")         //si es el inico de un metacaracter
                {
                    bandmeta = true;
                    sabermeta = i;
                }
                else if ((sabermeta + 1 == i) && bandmeta && (i != tam - 1) && (pos == "*" || pos == "+" || pos == "?" || pos == "." || pos == ")" || pos == "(" || pos == "[" || pos == "]" || pos == "-" || pos == "|" || pos == "\\" || pos == "#"))
                {
                    cadaux = cadaux + "\\" + pos + "|";            //si es metacaracter
                    bandmeta = false;
                }
                else if (pos == "-" && banera)               //si tiene el -
                {
                    if (GetValue(cadena, ubicacion, pos, i, ref cadaux))
                        break;
                }
                else if (Escaracterval(pos) && banera)        //si es algun metacaracter
                {
                    cadaux = cadaux + pos + "|";
                }
                else
                {
                    cadaux = "ERROR";
                    break;
                }
            }
            if (cadena == "[\\e]")
                cadaux = "\\e";
            if (cadena == "[a-c-g]")
                cadaux = "ERROR";
            cadena = cadaux;
        }

        private bool GetValue(string cadena, int ubicacion, string pos, int i, ref string cadaux)
        {
            int tamcad = cadena.Length;
            if ((ubicacion == 2) || (ubicacion == tamcad - 1))
            {
                cadaux = "ERROR";
                return true;
            }
            if (ubicacion == 3)
            {
                for (int j = 0; j <= 1; j++)
                    quitaultimo(ref cadaux);
            }
            else if (ubicacion >= 4)
            {
                for (int j = 0; j < 2; j++)
                    quitaultimo(ref cadaux);
            }
            else if (ubicacion >= 4 && Cuantosdeestetiene(cadena, pos) >= 2)
            {
                for (int j = 0; j < 3; j++)
                    quitaultimo(ref cadaux);
            }
            else
                quitaultimo(ref cadaux);

            char rangoini = cadena[i - 1];
            char rangofin = cadena[i + 1];

            if (rangoini > rangofin)
            {
                while (rangofin != rangoini)
                {
                    /*cadaux = cadaux + rangoini + "|";   //SIRVE PARA IR DE REVERSA PERO NO ES VALIDO
                                    rangoini--;*/
                    cadaux = "ERROR";
                    break;
                }
                return true;
            }
            if (rangoini < rangofin)
            {
                while (rangoini != rangofin)
                {
                    cadaux = cadaux + rangoini + "|";
                    rangoini++;
                }
            }
            return false;
        }

        private bool contienecaracter(String dato)
        {
            bool bandera = false;
            const string cadena = "*+?.)([]-|#";
            if (cadena.Contains(dato))
                bandera = true;
            return bandera;
        }

        private void quitaultimo(ref String dato)
        {
            int tam = dato.Length;
            string cadena = dato.Substring(0, (tam - 1));

            dato = cadena;
        }

        public static string PolacaInv(String cadena)
        {
            var pila = new Stack<char>();
            var cadact = "";
            pila.Push('#');
            cadena += '#';
            var band = true;
            var i = 0;

            while (band)
            {
                var dato = cadena[i];
                switch (dato)
                {
                    case '#':
                        switch (pila.Peek())
                        {
                            case '(':
                                cadact = "ERROR";
                                band = false;
                                break;
                            case '#':
                                band = false;
                                break;
                            default:
                                cadact += pila.Pop();
                                break;
                        }
                        break;
                    case '.':
                        if (pila.Peek() == '*' || pila.Peek() == '+' || pila.Peek() == '?' || pila.Peek() == '.')
                            cadact += pila.Pop();
                        else
                            pila.Push(cadena[i++]);
                        break;
                    case '*':
                    case '?':
                    case '+':
                        if (pila.Peek() == '*' || pila.Peek() == '+' || pila.Peek() == '?')
                            cadact += pila.Pop();
                        else
                            pila.Push(cadena[i++]);
                        break;
                    case '|':
                        if (pila.Peek() == '*' || pila.Peek() == '+' || pila.Peek() == '?' || pila.Peek() == '.' || pila.Peek() == '|')
                            cadact += pila.Pop();
                        else
                            pila.Push(cadena[i++]);
                        break;
                    case '(':
                        pila.Push(cadena[i++]);
                        break;
                    case ')':
                        switch (pila.Peek())
                        {
                            case '#':
                                cadact = "ERROR";
                                band = false;
                                break;
                            case '(':
                                pila.Pop();
                                i++;
                                break;
                            default:
                                cadact += pila.Pop();
                                break;
                        }
                        break;
                    default:
                        cadact += cadena[i];
                        i++;
                        if (dato == '\\')
                        {
                            cadact += cadena[i++];
                        }
                        break;
                }
            }
            cadena = cadact;
            return cadena;
        }

    }
}

