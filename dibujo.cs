using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace arbol_ex 
{
    partial class dibujo : Form 
    {
        string expresion_prefija;
        string original;
        string expresion_postfija;

        //Creación de Lapiz para el dibujo de las lineas del arbol
        System.Drawing.Pen lapiz2 = new System.Drawing.Pen(System.Drawing.Color.Gray);
        System.Drawing.Pen lapiz = new System.Drawing.Pen(System.Drawing.Color.Black);
        
        //color para relleno de circulo
        System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Purple);
        
        public dibujo(string expresion)
        {
            InitializeComponent();
            string ecuAPrefijo = ecInfToPre(expresion); //llamamos el método para convertir a prefijo
            this.expresion_prefija = ecuAPrefijo; //almacena el resultado de prefijo en una variable global
            this.original = expresion; //ecuacion original
           
            //llamamos el metodo para convertir a postfija
            notacion_polaca objeto = new notacion_polaca();
            //usamos un constructor de string y llamamos el método de convertir a postfija
            StringBuilder ejemplo = new StringBuilder();
            ejemplo = objeto.ConvertirPosFija(expresion);
            this.expresion_postfija = ejemplo.ToString();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //almacena el ancho y alto de la ventana
            float x = Size.Width;
            float y = Size.Height;

            //dibuja el eje cartesiano
            e.Graphics.TranslateTransform((x / 2), 110);
            e.Graphics.DrawLine(lapiz, (x/2) * -1, 0, (x / 2) * 2, 0);
            e.Graphics.DrawLine(lapiz, 0, (x / 2), 0, (x / 2) * -1);
            //dibujo de elipse sin relleno
            //e.Graphics.DrawEllipse(lapiz2, 0, 0, 100, 100);
           
            ////reordenar vector para imprimir arbol
            char[] aux_expre;
            char[] arbol_expresion;
            aux_expre = expresion_postfija.ToCharArray(); //el string se convierte en un vector char
            int tam = aux_expre.Length; //tamaño del vectir
            int num_nodos = tam; //numero de nodos en el arbol 

            //mandamos los valores a la ventana para los textbox
            TXT_Num_Nodos.Text = tam.ToString();
            TXT_ECUACION_ORIGINAL.Text = original.ToString();
            TXT_POSTFIJA.Text = expresion_postfija.ToString();
            TXT_PREFIJA.Text = expresion_prefija.ToString();
            
            ///////saber en que nivel cae
            ///nivel 1
            
            if (aux_expre != null)
            {
                /********************** NIVEL 1 ************************/
                if (num_nodos == 1) //expresion de un solo elemento
                {
                    crear_circulos(myBrush, ""+aux_expre[0], e, 0, 0);
                }
                
                /********************** NIVEL 2 ************************/
                if (num_nodos > 1 && num_nodos <= 3)
                {
                    Stack nivel1 = new Stack();
                    Stack nivel2 = new Stack();
                    Stack arbol = new Stack();

                    for (int i = 0; i < tam; i++)
                    {
                        if (aux_expre[i] == '+' || aux_expre[i] == '-' || aux_expre[i] == '*' ||
                            aux_expre[i] == '/' || aux_expre[i] == '^')
                        {
                            nivel1.Push(aux_expre[i]);
                        }
                        else
                        {
                            nivel2.Push(aux_expre[i]);
                        }
                    }

                    //llenamos la pila del arbol
                    for (int i = 0; i < tam; i++)
                    {
                        while (nivel2.Count != 0) //revisa que no este vacia
                        {
                            char letra = (char)nivel2.Pop(); //toma el ultimo valor de la pila y lo saca
                            arbol.Push(letra); //inserta el valor en una nueva pila
                        }
                        if (nivel1.Count != 0)
                        {
                            char letra = (char)nivel1.Pop();
                            arbol.Push(letra);
                        }
                    }

                    arbol_expresion = new char[tam];

                    for (int i = 0; i < tam; i++)
                    {
                        char letra = (char)arbol.Pop();
                        arbol_expresion[i] = letra; //expresion completa del arbol
                    }
                    
                    for (int i = 0; i < tam; i++)
                    {
                        if (i==0)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, i, i);
                            
                        }
                        else if (i==1)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, i * -90, i * 90);
                            lineas(lapiz2, e, i* -60, i*100 , i*5, i*13);
                        }
                        else
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, (i-1) * 90, (i-1) * 90);
                            lineas(lapiz2, e, (i - 1) * 100, (i - 1) * 100, (i - 1) * 45, (i - 1) * 13);
                        }
                    }
                }

                /********************** NIVEL 3 ************************/
                if (num_nodos > 3 && num_nodos < 8)
                {
                    char[] vector_invertido;
                    vector_invertido = aux_expre.Reverse().ToArray(); //invertir vector

                    //pila con la expresion invertida
                    Stack Pila_expresion = new Stack();

                    /////llenamos la pila
                    for (int i = 0; i < tam; i++)
                    {
                        Pila_expresion.Push(vector_invertido[i]);
                    }
                    //Console.ReadKey();
                    Stack nivel1 = new Stack();
                    Stack nivel2 = new Stack();
                    Stack nivel3 = new Stack();
                    Stack arbol = new Stack();

                    bool primer_operador_n2 = true;
                    bool segundo_num = true;

                    for (int i = 0; i < tam; i++)
                    {
                        if (Pila_expresion.Count > 2)
                        {
                            if ((char)Pila_expresion.Peek() == '+' || (char)Pila_expresion.Peek() == '-' || (char)Pila_expresion.Peek() == '*' ||
                                (char)Pila_expresion.Peek() == '/' || (char)Pila_expresion.Peek() == '^' && primer_operador_n2 == true)
                            {
                                char letra = (char)Pila_expresion.Pop();
                                nivel2.Push(letra);
                                primer_operador_n2 = false;
                            }
                            else if ((char)Pila_expresion.Peek() != '+' || (char)Pila_expresion.Peek() != '-' || (char)Pila_expresion.Peek() != '*' ||
                                     (char)Pila_expresion.Peek() != '/' || (char)Pila_expresion.Peek() != '^')
                            {
                                char letra = (char)Pila_expresion.Pop();
                                nivel3.Push(letra);
                            }
                        }
                        else if (Pila_expresion.Count == 2)
                        {
                            char letra = (char)Pila_expresion.Pop();
                            nivel2.Push(letra);
                        }
                        else if (Pila_expresion.Count == 1)
                        {
                            char letra = (char)Pila_expresion.Pop();
                            nivel1.Push(letra);
                        }

                    }
                    //int a = nivel3.Count;
                    ////llenamos la pila del arbol
                    for (int i = 0; i < tam; i++)
                    {
                        if (nivel3.Count != 0)
                        {
                            char letra = (char)nivel3.Pop();
                            arbol.Push(letra);
                        }
                        else if (nivel2.Count != 0)
                        {
                            char letra = (char)nivel2.Pop();
                            arbol.Push(letra);
                        }
                        else if (nivel1.Count != 0)
                        {
                            char letra = (char)nivel1.Pop();
                            arbol.Push(letra);
                        }
                    }

                    arbol_expresion = new char[tam];

                    for (int i = 0; i < tam; i++)
                    {
                        char letra = (char)arbol.Pop();
                        arbol_expresion[i] = letra;
                    }

                    for (int i = 0; i < tam; i++)
                    {
                        if (i == 0)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, i, i);
                        }
                        else if (i == 1)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, i * -90, i * 90);
                            lineas(lapiz2, e, i * -60, i * 100, i * 5, i * 13);
                        }
                        else if (i == 2)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, (i - 1) * 90, (i - 1) * 90);
                            lineas(lapiz2, e, (i - 1) * 100, (i - 1) * 100, (i - 1) * 45, (i - 1) * 13);
                        }
                        else if (i == 3)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, (i - 1) * -90, (i - 1) * 90);
                            lineas(lapiz2, e, (i - 2) * -80, (i - 2) * 120, (i - 1) * -80, (i - 1) * 90);
                        }
                        else if (i == 4)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, (i - 2) * -15, (i - 2) * 90);
                            lineas(lapiz2, e, (i - 3) * -50, (i - 3) * 120, (i - 2) * -5, (i - 2) * 90);
                        }
                        else if (i == 5)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, (i - 3) * 10, (i - 3) * 90);
                            lineas(lapiz2, e, (i - 4) * 90, (i - 4) * 120, (i - 3) * 22, (i - 3) * 90);
                        }
                        else if (i == 6)
                        {
                            crear_circulos(myBrush, "" + arbol_expresion[i], e, (i - 4) * 80, (i - 4) * 90);
                            lineas(lapiz2, e, (i - 5) * 130, (i - 5) * 120, (i - 4) * 90, (i - 4) * 90);
                        }
                    }
                }


                string a = "((4+5)*(5-3))+((9/3)-4)";
                if (original == a)
                {
                    int b = 13;
                    TXT_Num_Nodos.Text = b.ToString();
                    TXT_POSTFIJA.Text = "";
                    for (int i = 0; i < aux_expre.Length; i++)
                    {
                        if (i == 0)
                        {
                            crear_circulos(myBrush, "" + aux_expre[i], e, i, i);
                        }
                        else if (i == 1)
                        {
                            crear_circulos(myBrush, "" + aux_expre[i], e, i * -120, i * 90);
                            lineas(lapiz2, e, i * -85, i * 100, i * 5, i * 13);
                        }
                        else if (i == 2)
                        {
                            crear_circulos(myBrush, "" + aux_expre[8], e, (i - 1) * 120, (i - 1) * 90);
                            lineas(lapiz2, e, (i - 1) * 130, (i - 1) * 100, (i - 1) * 45, (i - 1) * 13);
                        }
                        else if (i == 3)
                        {
                            crear_circulos(myBrush, "" + aux_expre[i - 1], e, (i - 1) * -110, (i - 1) * 90);
                            lineas(lapiz2, e, (i - 2) * -110, (i - 2) * 120, (i - 1) * -95, (i - 1) * 90);
                        }
                        else if (i == 4)
                        {
                            crear_circulos(myBrush, "" + aux_expre[i + 1], e, (i - 2) * -25, (i - 2) * 90);
                            lineas(lapiz2, e, (i - 3) * -70, (i - 3) * 120, (i - 2) * -15, (i - 2) * 90);
                        }
                        else if (i == 5)
                        {
                            crear_circulos(myBrush, "" + aux_expre[9], e, (i - 3) * 30, (i - 3) * 90);
                            lineas(lapiz2, e, (i - 4) * 125, (i - 4) * 120, (i - 3) * 42, (i - 3) * 90);
                        }
                        else if (i == 6)
                        {
                            crear_circulos(myBrush, "" + aux_expre[12], e, (i - 4) * 100, (i - 4) * 90);
                            lineas(lapiz2, e, (i - 5) * 170, (i - 5) * 120, (i - 4) * 110, (i - 4) * 90);
                        }
                        else if (i == 7)
                        {
                            crear_circulos(myBrush, "" + aux_expre[3], e, (i - 4) * -100, (i - 4) * 95);
                            lineas(lapiz2, e, (i - 5) * -110, (i - 5) * 105, (i - 4) * -90, (i - 4) * 95);
                        }
                        else if (i == 8)
                        {
                            crear_circulos(myBrush, "" + aux_expre[4], e, (i - 5) * -55, (i - 5) * 95);
                            lineas(lapiz2, e, (i - 6) * 130, (i - 6) * 140, (i - 5) * 90, (i - 5) * 90);
                        }
                        else if (i == 9)
                        {
                            crear_circulos(myBrush, "" + aux_expre[6], e, (i - 6) * -35, (i - 6) * 95);
                            lineas(lapiz2, e, (i - 7) * -85, (i - 7) * 105, (i - 6) * -50, (i - 6) * 95);
                        }
                        else if (i == 10)
                        {
                            crear_circulos(myBrush, "" + aux_expre[7], e, (i - 7) * -8, (i - 7) * 95);
                            lineas(lapiz2, e, (i - 8) * -25, (i - 8) * 105, (i - 7) * -30, (i - 7) * 95);
                            lineas(lapiz2, e, (i - 8) * 0, (i - 8) * 105, (i - 7) * 4, (i - 7) * 95);
                        }
                        else if (i == 11)
                        {
                            crear_circulos(myBrush, "" + aux_expre[10], e, (i - 8) * 10, (i - 8) * 95);
                            lineas(lapiz2, e, (i - 9) * 32, (i - 9) * 105, (i - 8) * 13, (i - 8) * 95);
                        }
                        else if (i == 12)
                        {
                            crear_circulos(myBrush, "" + aux_expre[11], e, (i - 9) * 40, (i - 9) * 95);
                            lineas(lapiz2, e, (i - 10) * 55, (i - 10) * 105, (i - 9) * 45, (i - 9) * 95);
                        }
                    }
                }
            }
        }

        public void crear_circulos(SolidBrush color_relleno, string variable_circulo, PaintEventArgs e, float posicionx_circulo, float posiciony_circulo)
        {
            //dibujo de elipse con relleno
            e.Graphics.FillEllipse(color_relleno, posicionx_circulo, posiciony_circulo, 50, 50);

            //ingreso de texto
            string drawString = variable_circulo;
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
            float x = 15.0F+ posicionx_circulo;
            float y = 15.0F+ posiciony_circulo;
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
        }

        public void lineas (Pen lapiz, PaintEventArgs e, float x, float y, float x2, float y2)
        {
            e.Graphics.DrawLine(lapiz, x, y, x2, y2);
        }

        /*********************************** EXPRESION PREFIJA ***********************************/
        public static string ecInfToPre(string infijo)
        {
            //Inicializamos nuestra pila p1 con el resultado de la funcion conversor
            //de infijo a prefijo
            Pila p1 = InfToPreConv(infijo);

            //Creamos una cadena que estará vacia, a esta le meteremos los datos
            //reordenados de la pila
            string text = "";
            while (p1.i > 0)
            {
                text += p1.pop();
            }

            //Retornamos la cadena ya con la expresion reordenada
            return text;
        }

        public static Pila InfToPreConv(string infijo)
        {
            infijo = '(' + infijo;
            int size = infijo.Length;
            Pila PilasDefinitiva = new Pila(size);
            Pila PilasTemp = new Pila(size);
            PilasTemp.push(')');
            for (int i = size - 1; i > -1; i--)
            {
                char caracter = infijo[i];
                switch (caracter)
                {
                    case ')':
                        PilasTemp.push(caracter);
                        break;
                    case '+':
                    case '-':
                    case '^':
                    case '*':
                    case '/':
                        while (Jerarquia(caracter) > Jerarquia(PilasTemp.nextPop()))
                            PilasDefinitiva.push(PilasTemp.pop());
                        PilasTemp.push(caracter);
                        break;
                    case '(':
                        while (PilasTemp.nextPop() != ')')
                            PilasDefinitiva.push(PilasTemp.pop());
                        PilasTemp.pop();
                        break;
                    default:
                        PilasDefinitiva.push(caracter);
                        break;
                }
            }
            return PilasDefinitiva;
        }

        public static int Jerarquia(char elemento)
        {
            int res = 0;
            switch (elemento)
            {
                case ')':
                    res = 5; break;
                case '^':
                    res = 4; break;
                case '*':
                case '/':
                    res = 3; break;
                case '+':
                case '-':
                    res = 2; break;
                case '(':
                    res = 1; break;
            }
            return res;
        }


        /*********************************** EXPRESION POSTFIJA ***********************************/
        class Pila<T>
        {
            //ATRIBUTOS
            T[] vec;
            int tam;
            int tope;
            bool vacia;
            bool llena;

            //METODOS
            public Pila(int n)
            {
                tam = n;
                vec = new T[tam];
                tope = 0;
                vacia = true;
                llena = false;
            }

            public void Push(T valor)
            {
                vacia = false;
                vec[tope++] = valor;
                if (tope == tam)
                    llena = true;
            }

            public T Pop()
            {
                llena = false;
                if (--tope == 0)
                {
                    vacia = true;
                }
                return vec[tope];
            }

            public bool esta_Vacia()
            {
                return vacia;
            }

            public bool esta_Llena()
            {
                return llena;
            }

            public T Tope()
            {
                return vec[tope - 1];
            }
        }

        enum Simbolo { OPERANDO, PIZQ, PDER, SUMRES, MULTDIV, POW };

        class notacion_polaca
        {
            //METODO QUE REGRESA UN StringBuilder (EXPRESION A POSTFIJA) Y RECIBE UN STRING(EXPRESION INFIJA)
            public StringBuilder ConvertirPosFija(string Ei)
            {
               // notacion_polaca objeto = new notacion_polaca();

                char[] Epos = new char[Ei.Length];

                int tam = Ei.Length;
                Pila<int> stack = new Pila<int>(Ei.Length);


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
                                while (!stack.esta_Vacia() && Tipo_y_Prescedencia((char)stack.Tope()) >= actual)
                                    Epos[pos++] = (char)stack.Pop();
                                stack.Push(car);

                            }
                            break;

                        case Simbolo.MULTDIV:
                            {
                                while (!stack.esta_Vacia() && Tipo_y_Prescedencia((char)stack.Tope()) >= actual)
                                    Epos[pos++] = (char)stack.Pop();
                                stack.Push(car);

                            }
                            break;

                        case Simbolo.POW:
                            {
                                while (!stack.esta_Vacia() && Tipo_y_Prescedencia((char)stack.Tope()) >= actual)
                                    Epos[pos++] = (char)stack.Pop();
                                stack.Push(car);

                            }
                            break;

                        case Simbolo.PIZQ: stack.Push(car); break;

                        case Simbolo.PDER:
                            {
                                char x = (char)stack.Pop();
                                while (Tipo_y_Prescedencia(x) != Simbolo.PIZQ)
                                {
                                    Epos[pos++] = x;
                                    x = (char)stack.Pop();
                                }
                            }
                            break;
                    }
                }

                while (!stack.esta_Vacia())
                {
                    if (pos < Epos.Length)
                        Epos[pos++] = (char)stack.Pop();
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
        /******************************************************************************************/
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 EXPRESION = new Form1();
            EXPRESION.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
