using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CLB_PROTOTYPE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button2.Click += new System.EventHandler(this.button1_Click);
            this.button3.Click += new System.EventHandler(this.button1_Click);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
                string Output = "Out.exe";
                Button ButtonObject = (Button)sender;

                textBox2.Text = "";
                CompilerParameters parameters = new CompilerParameters();
                //Make sure we generate an EXE, not a DLL
                parameters.GenerateExecutable = true;
                parameters.OutputAssembly = Output;
                CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, textBox1.Text);

                if (results.Errors.Count > 0)
                {
                    textBox2.ForeColor = Color.Red;
                    foreach (CompilerError CompErr in results.Errors)
                    {
                        textBox2.Text = textBox2.Text +
                                    "Line number " + CompErr.Line +
                                    ", Error Number: " + CompErr.ErrorNumber +
                                    ", '" + CompErr.ErrorText + ";" +
                                    Environment.NewLine + Environment.NewLine;
                    }
                }
                else
                {
                    //Successful Compile
                    textBox2.ForeColor = Color.Blue;
                    textBox2.Text = "Success!";
                    //If we clicked run then launch our EXE
                    if (ButtonObject.Text == "Run" || ButtonObject.Text == "Run Test Case ")
                    {
                        var proc = new Process
                        {
                            StartInfo = new ProcessStartInfo
                            {
                                FileName = Output,
                                Arguments = (txtTestInput.Text == "") ? "1 2 3" : txtTestInput.Text,
                                UseShellExecute = false,
                                RedirectStandardOutput = true,
                                RedirectStandardError=true,
                                CreateNoWindow = true
                            }
                        };

                        proc.Start();
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        System.Text.StringBuilder pb = new System.Text.StringBuilder();
                        while (!proc.StandardError.EndOfStream)
                        {
                            pb.Append(proc.StandardError.ReadLine());
                            // do something with line
                        }
                        while (!proc.StandardOutput.EndOfStream)
                        {
                            sb.Append(proc.StandardOutput.ReadLine());
                            // do something with line
                        }
                        textBox2.Text = sb.ToString();
                        textBox2.AppendText(pb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }

}
