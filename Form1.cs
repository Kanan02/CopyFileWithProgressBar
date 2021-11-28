namespace WinFormsApp2
{
    public partial class Form1 : Form
    {

        public delegate void ProgressChangeDelegate(double Percentage, ref bool Cancel);
        public delegate void Completedelegate();
        public event ProgressChangeDelegate OnProgressChanged;
        public event Completedelegate OnComplete;
        public Form1()
        {
            InitializeComponent();
            progressBar1.Maximum=100;
            progressBar1.Minimum = 0;

        }

        void CopyFile()
        {
            byte[] bytes;
            int c = 0;
            bool cancelFlag = false;

            using (FileStream fs = new FileStream(Directory.GetCurrentDirectory()+"\\"+textBox1.Text, FileMode.Open))
            {
                
                bytes = new byte[fs.Length];
                long fileLength = fs.Length;
                using (FileStream dest = new FileStream(Directory.GetCurrentDirectory()+"\\..\\"+textBox1.Text, FileMode.CreateNew, FileAccess.Write))
                {
                    long totalBytes = 0;
                    int currentBlockSize = 0;

                    while ((currentBlockSize = fs.Read(bytes, 0, bytes.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double percentage = (double)totalBytes * 100.0 / fileLength;

                        dest.Write(bytes, 0, currentBlockSize);

                        cancelFlag = false;
                        OnProgressChanged(percentage, ref cancelFlag);

                        if (cancelFlag == true)
                        {
                            File.Delete(Directory.GetCurrentDirectory() + "/../" + textBox1.Text);
                            break;
                        }
                    }
                }
              
            }
            //Console.WriteLine(Encoding.UTF8.GetString(bytes));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OnProgressChanged += ChangeProgress;
            CopyFile();

        }
        void ChangeProgress(double Percentage,ref bool Cancel)
        {
            progressBar1.Value = (int)Percentage;
            if (Cancel)
            {
                progressBar1.Value = 0;
            }
            if (progressBar1.Value==100)
            {
                MessageBox.Show("Finished!");
            }
        }
    }
}