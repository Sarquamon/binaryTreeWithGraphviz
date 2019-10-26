using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace binaryTreeWithGraphviz
{
    enum Simbolo { OPERANDO, PIZQ, PDER, SUMRES, MULTDIV, POW };
    class InfToPost
    {

        //METODO QUE REGRESA UN StringBuilder (EXPRESION A POSTFIJA) Y RECIBE UN STRING(EXPRESION INFIJA)
        public StringBuilder ConvertirPosFija(string Ei)
        {
            // notacion_polaca objeto = new notacion_polaca();

            char[] Epos = new char[Ei.Length];

            int tam = Ei.Length;
            Pila stack = new Pila(Ei.Length);


            int i, pos = 0;
            for (i = 0; i < Epos.Length; i++)
            {
                char car = Ei[i];
                Simbolo actual = Tipo_y_Prescedencia(car);
                switch (actual)
                {
                    case Simbolo.OPERANDO: Epos[pos++] = car; break;

                    case Simbolo.SUMRES:
                        {
                            while (!stack.pilaIsEmpty() && Tipo_y_Prescedencia((char)stack.headValue()) >= actual)
                                Epos[pos++] = (char)stack.pop();
                            stack.push(car);

                        }
                        break;

                    case Simbolo.MULTDIV:
                        {
                            while (!stack.pilaIsEmpty() && Tipo_y_Prescedencia((char)stack.headValue()) >= actual)
                                Epos[pos++] = (char)stack.pop();
                            stack.push(car);

                        }
                        break;

                    case Simbolo.POW:
                        {
                            while (!stack.pilaIsEmpty() && Tipo_y_Prescedencia((char)stack.headValue()) >= actual)
                                Epos[pos++] = (char)stack.pop();
                            stack.push(car);

                        }
                        break;

                    case Simbolo.PIZQ: stack.push(car); break;

                    case Simbolo.PDER:
                        {
                            char x = (char)stack.pop();
                            while (Tipo_y_Prescedencia(x) != Simbolo.PIZQ)
                            {
                                Epos[pos++] = x;
                                x = (char)stack.pop();
                            }
                        }
                        break;
                }
            }

            while (!stack.pilaIsEmpty())
            {
                if (pos < Epos.Length)
                    Epos[pos++] = (char)stack.pop();
                else
                    break;
            }

            StringBuilder regresa = new StringBuilder(Ei);

            for (int r = 0; r < Epos.Length; r++)
                regresa[r] = Epos[r];


            return regresa;

        }

        //METODO QUE REGRESA UN ENUM Y RECIBE UN char (operador)
        public Simbolo Tipo_y_Prescedencia(char n)
        {
            Simbolo simbolo;
            switch (n)
            {
                case '+': simbolo = Simbolo.SUMRES; break;
                case '-': simbolo = Simbolo.SUMRES; break;
                case '*': simbolo = Simbolo.MULTDIV; break;
                case '/': simbolo = Simbolo.MULTDIV; break;
                case '(': simbolo = Simbolo.PIZQ; break;
                case ')': simbolo = Simbolo.PDER; break;
                case '^': simbolo = Simbolo.POW; break;
                default: simbolo = Simbolo.OPERANDO; break;

            }

            return simbolo;
        }

    }
}
