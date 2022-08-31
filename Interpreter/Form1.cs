using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;


namespace Interpretador
{
    public partial class canvas : Form
    {
        Dictionary<string, int> identificador = new Dictionary<string, int>();
        Dictionary<string, int> labels = new Dictionary<string, int>();
        string[] memory = new string[1024];
        int[] dataMem = new int[1024];

        int AC;
        int AC2;
        int AC3;
        int PC;
        int N = -1;
        int Z = 0;

        Cursor cursor = new Cursor();

        Graphics gObject;
        Color color;

        Random random = new Random();

        public canvas()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.WordWrap = true;
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void painel1_Paint(object sender, PaintEventArgs e)
        {

            //test
            gObject = painel1.CreateGraphics();
            //pen = new Pen(Color.DeepPink);
            //pen.Color = Color.FromArgb(255, 255, 255, 255);

            //gObject.DrawLine(test, 10, 10, 350, 280);
            //gObject.DrawLine(pen, 1, 1, 2, 1);

        }

        private void Execute_Click(object sender, EventArgs e)
        {
            string str;
            int i = 0;
            while(i==0)
            {
                if (memory[PC] != null)
                {
                    str = memory[PC];

                    //highlight
                    //listView1.Items[PC].Selected = true;

                    if (str == "LD")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            AC = dataMem[memID];
                        }
                        PC += 2;
                    }
                    else if (str == "LD2")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            AC2 = dataMem[memID];
                        }
                        PC += 2;
                    }
                    else if (str == "LD3")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            AC3 = dataMem[memID];
                        }
                        PC += 2;
                    }

                    else if (str == "ST")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            dataMem[memID] = AC;
                        }
                        PC += 2;
                    }
                    else if (str == "ST2")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            dataMem[memID] = AC2;
                        }
                        PC += 2;
                    }
                    else if (str == "ST3")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            dataMem[memID] = AC3;
                        }
                        PC += 2;
                    }

                    else if (str == "ADD")
                    {
                        int b = Int32.Parse(memory[PC + 1]);
                        AC = AC + b;
                        PC += 2;

                        CheckVars();
                    }
                    else if (str == "SUB")
                    {
                        string id = memory[PC + 1];
                        int memID;
                        int b;

                        if (identificador.TryGetValue(id, out memID))
                        {
                            b = dataMem[memID];
                        }
                        else
                        {
                            b = Int32.Parse(memory[PC + 1]);
                        }
                        
                        AC = AC - b;
                        PC += 2;

                        CheckVars();
                    }
                    else if (str == "JMP")
                    {
                        string id = memory[PC + 1];
                        int memID;

                        if (labels.TryGetValue(id, out memID))
                        {
                            PC = memID;
                        }
                    }
                    else if (str == "JN")
                    {
                        if (N == 1)
                        {
                            string id = memory[PC + 1];
                            int memID;

                            if (labels.TryGetValue(id, out memID))
                            {
                                PC = memID;
                            }
                        }
                        else
                        {
                            PC++;
                        }
                    }
                    else if (str == "JP")
                    {
                        if (N == 0)
                        {
                            string id = memory[PC + 1];
                            int memID;

                            if (labels.TryGetValue(id, out memID))
                            {
                                PC = memID;
                            }
                        }
                        else
                        {
                            PC++;
                        }
                    }
                    else if (str == "JZ")
                    {
                        if (Z == 1)
                        {
                            string id = memory[PC + 1];
                            int memID;

                            if (labels.TryGetValue(id, out memID))
                            {
                                PC = memID;
                            }
                        }
                        else
                        {
                            PC++;
                        }
                    }
                    else if (str == "JNZ")
                    {
                        if (Z == 0)
                        {
                            string id = memory[PC + 1];
                            int memID;

                            if (labels.TryGetValue(id, out memID))
                            {
                                PC = memID;
                            }
                        }
                        else
                        {
                            PC++;
                        }
                    }
                    else if (str == "HALT")
                    {
                        PC = 0;
                        i = 1;
                    }

                    else if (str == "POS")
                    {
                        cursor.posX = AC;
                        cursor.posY = AC2;
                        PC++;
                    }
                    else if(str == "PXL")
                    {
                        color = Color.FromArgb(255, AC, AC2, AC3);
                        SolidBrush brush = new SolidBrush(color);
                        gObject.FillRectangle(brush, cursor.posX, cursor.posY,1,1);
                        PC++;
                    }
                    else if (str == "RND")
                    {
                        string id = memory[PC + 1];
                        int memID;
                        if (identificador.TryGetValue(id, out memID))
                        {
                            
                            dataMem[memID] = random.Next(AC, AC2);
                        }
                        PC += 2;
                    }
                    else if (str == "CLR")
                    {
                        gObject.Clear(Color.White);
                        PC++;
                    }
                    else if (str == "COS")
                    {
                        int b = Int32.Parse(memory[PC + 1]);
                        double cos = Math.Cos(b);
                        AC = b * AC2;
                        PC += 2;

                    }
                    else if (str == "SIN")
                    {
                        int b = Int32.Parse(memory[PC + 1]);
                        double sin = Math.Sin(b);
                        AC = b * AC2;
                        PC += 2;
                    }
                    else if (str == "IN")
                    {
                        string id = memory[PC + 1];
                        int memID;
                        if (identificador.TryGetValue(id, out memID))
                        {

                        }
                    }
                    else
                    {
                        PC++;
                    }
                }
                else
                {
                    break;
                }
            }

            //mostrar AC tela
            label_ac.Text = AC.ToString();
            label_ac2.Text = AC2.ToString();
            label_ac3.Text = AC3.ToString();
            label5.Text = N.ToString();
            label7.Text = Z.ToString();

            listView2.Clear();

            foreach (KeyValuePair<string, int> id in identificador)
            {
                //keys.Add(id.Key);
                listView2.Items.Add(id.Key + ": " + dataMem[id.Value]);
            }
        }

        public void CheckVars()
        {
            if (AC < 0)
            {
                N = 1;
            }
            else if (AC > 0)
            {
                N = 0;
            }
            else if (AC == 0)
            {
                Z = 1;
            }
            if (AC != 0)
            {
                Z = 0;
            }
        }

        public void ParseStrings(string strings)
        {
            string buffer = "";
            int counter = 0;

            //type of code control variables
            bool data = false;
            bool code = false;

            //.data control variables
            string varName = "";
            bool db = false;
            int pos = 0;
            bool posFilled = false;

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < strings.Length; i++)
            {
                sb.Append(strings[i]);
                
                if (strings[i] == ' ' || strings[i] == '\r' || strings[i] == '\n' || strings[i] == '#' || strings[i] == ',' || i == strings.Length-1 || strings[i] == ':')
                {
                    int l = buffer.Length;
                    if (l > 1 || i == strings.Length-1 || strings[i] == ',' || strings[i] == '\r')
                    {
                        if (data && buffer != ".cod")
                        {
                            if (buffer == ".data")
                            {
                                buffer = "";
                                sb.Clear();
                            }
                            if (buffer != "")
                            {
                                //adicionar variável no identificador e etc
                                if (db == false && buffer != "")
                                {
                                    if (buffer == "db" || buffer == "DB")
                                    {
                                        db = true;
                                        buffer = "";
                                        sb.Clear();
                                    }
                                    else
                                    {

                                        varName = buffer;
                                        buffer = "";
                                        sb.Clear();
                                    }
                                }
                                else if (posFilled == false)
                                {
                                    pos = Int32.Parse(buffer);
                                    identificador.Add(varName, pos);
                                    posFilled = true;
                                    buffer = "";
                                    sb.Clear();
                                }
                                else
                                {
                                    if (strings[i] != '\r' && strings[i] != ' ')
                                    {
                                        buffer = sb.ToString();
                                    }
                                    dataMem[pos] = Int32.Parse(buffer);
                                    db = false;
                                    posFilled = false;
                                    buffer = "";
                                    sb.Clear();
                                }
                            }

                        }
                        else if (code)
                        {
                            //.code colocar tudo na memória
                            if(buffer == ".code")
                            {
                                buffer = "";
                                sb.Clear();
                            }
                            if (buffer != "")
                            {
                                if (buffer != "" && buffer != ".code")
                                {
                                    if(strings[i] == ':')
                                    {
                                        labels.Add(buffer, counter);
                                        memory[counter] = buffer;
                                        counter++;
                                        buffer = "";
                                        sb.Clear();
                                    }
                                    else if(i == strings.Length - 1)
                                    {
                                        buffer = sb.ToString();
                                        memory[counter] = buffer;
                                        counter++;
                                        buffer = "";
                                        sb.Clear();
                                    }
                                    else
                                    {
                                        memory[counter] = buffer;
                                        counter++;
                                        buffer = "";
                                        sb.Clear();
                                    }
                                   
                                }
                                else
                                {
                                    sb.Append(strings[i]);
                                }
                            }
                        }
                        else
                        {
                            buffer = "";
                            sb.Clear();
                        }
                    }
                    else
                    {
                        buffer = "";
                        sb.Clear();
                    }
                }
                else
                {
                    buffer = sb.ToString();
                    if (buffer != " ")
                    {
                        //verificar .data
                        if (buffer == ".data")
                        {
                            data = true;
                            code = false;
                        }

                        if (buffer == ".code")
                        {
                            data = false;
                            code = true;
                        }
                        if (buffer == ".enddata")
                        {
                            data = false;
                            code = false;
                        }
                        if (buffer == ".endcode")
                        {
                            data = false;
                            code = false;
                        }

                        //if (data)
                        //{
                        //        if (buffer == ".data")
                        //        {
                        //            buffer = "";
                        //            sb.Clear();
                        //        }
                               

                        //}
                        //else if (code)
                        //{
                        //    //.code colocar tudo na memória
                        //    if (buffer != "")
                        //    {
                        //        memory[counter] = buffer;
                        //        counter++;
                        //        sb.Clear();
                        //    }
                        //    else
                        //    {
                        //        sb.Append(strings[i]);
                        //    }
                        //}
                    }
                }
            }

            //desenhar variaveis memoria
            for (int i = 0; i < memory.Length; i++)
            {
                if (memory[i] != null)
                {

                    listView1.Items.Add("Mem " + i.ToString() +": " + memory[i]);

                }
                else
                {
                    break;
                }
            }

            //desenhar variaveis
            //List<String> keys = new List<String>();
            foreach (KeyValuePair<string, int> id in identificador)
            {
                //keys.Add(id.Key);
                listView2.Items.Add(id.Key + ": " + id.Value);
            }

           


        }
        private void button1_Click(object sender, EventArgs e)
        {
            string test = textBox1.Text;
            ParseStrings(test);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }


    public struct Cursor
    {
        public int posX;
        public int posY;
    }
}
