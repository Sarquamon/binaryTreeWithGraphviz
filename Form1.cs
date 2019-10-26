using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SysBio.dataStructures;

namespace binaryTreeWithGraphviz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        InfToPost infToPostParser = new InfToPost();
        private void StartBtn_Click(object sender, EventArgs e)
        {
            string expresion = ecTxt.Text; //Get the text from the form

            // expresion = InfToPref.Infijo2PrefijoTxt(expresion); //If user wants a prefix expresion, it can uncomment this and use it
            // expresion = infToPostParser.ConvertirPosFija(expresion).ToString();//If user wants a postfix expresion, it can uncomment this and use it

            TBinarySTree bt = generatesTree(expresion); //Generates us a binary tree with binaryTree library

            Console.WriteLine(bt.drawTree() + "\n"); //Writes to console the final tree on string form

            Console.WriteLine(treeToGraphviz(bt)); //Writes to console the result gotten from the converter of tree to graphviz

            var fileName = "graph.txt"; //Create a string for the txt file name

            SaveToFile(bt, fileName); //We save the file

            System.Diagnostics.Process.Start("notepad.exe", fileName); //Opens a txt file with the graphviz result

            string path = Directory.GetCurrentDirectory(); //Gets the current path

            GenerateGraph(fileName, path); //Generates the actual jpg from the tree string

            var graphFileName = "graph.jpg"; //Create a string for the jpg file name

            System.Diagnostics.Process.Start(graphFileName); //Opens a jpg file with the graphviz generated tree

        }

        //Method to generate the actual jpg from the tree string
        private static void GenerateGraph(string fileName, string path)
        {
            try
            {
                //Run command to run a dot command
                var toGraphvizComm = string.Format("dot -T jpg {0} -o {1}", Path.Combine(path, fileName), Path.Combine(path, fileName.Replace(".txt", ".jpg")));

                //Concat the cmd run command and dot command
                var processStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/C " + toGraphvizComm);

                //Makes all the above commands run
                var process = new System.Diagnostics.Process();

                //Runs all command
                process.StartInfo = processStartInfo;

                process.Start();

                process.WaitForExit();

            }
            catch (Exception x)
            {
                System.Console.WriteLine(x);
            }
        }

        //Method to save all data to a txt file
        private static void SaveToFile(TBinarySTree bt, string fileName)
        {
            TextWriter textWriter = new StreamWriter(fileName); //Create a Textwriter var

            textWriter.WriteLine(treeToGraphviz(bt)); //Writes to txt file the body content

            textWriter.Close(); //Close the TextWriter function
        }

        //A method that gets the original expresion and parses it to a binary tree
        public static TBinarySTree generatesTree(string expresion)
        {
            TBinarySTree binaryTree = new TBinarySTree(); //Create a TbinarySTree var

            //Inserts each one of the expresion's values
            for (int i = 0; i < expresion.Length; i++)
            {

                binaryTree.insert("" + expresion[i], expresion[i]);
            }

            return binaryTree;
        }

        // This method is used to transform the tree string into an actual graphviz
        public static string treeToGraphviz(TBinarySTree tree)
        {
            StringBuilder stringBuilder = new StringBuilder(); //Create a stringbuilder var

            stringBuilder.Append("digraph G {" + Environment.NewLine); //Append the graphviz prefix to initialize the txt file

            stringBuilder.Append(toGraphviz(tree.GetRoot())); //Here we fill the actual content of the txt graphviz with the toGraphviz method

            stringBuilder.Append("}"); //Closing curly braces to indicate the end of a graphviz file

            return stringBuilder.ToString(); //return as string the result of our stringbuilder var
        }

        //A recursive method which is used to transform the tree nodes
        // into a string that graphviz can interpretade
        public static string toGraphviz(TTreeNode node)
        {
            StringBuilder stringBuilder = new StringBuilder(); //Create a stringbuilder var

            //Check if the node on it's left and right fields are not empty
            if (node.left != null)
            {
                //Append a new format for each node for graphviz to be able to read it
                stringBuilder.AppendFormat("{0}->{1}{2}", node.ToString(), node.left.ToString(), Environment.NewLine);
                stringBuilder.Append(toGraphviz(node.left));
            }

            if (node.right != null)
            {
                stringBuilder.AppendFormat("{0}->{1}{2}", node.ToString(), node.right.ToString(), Environment.NewLine);
                stringBuilder.Append(toGraphviz(node.right));
            }
            return stringBuilder.ToString();
        }

    }
}
