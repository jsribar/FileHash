using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using DamienG.Security.Cryptography;

namespace Sha512
{
    public partial class FormMain : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_CHANGEUISTATE = 0x127;
        private const int UIS_SET = 1;
        private const int UISF_HIDEFOCUS = 0x1;

        private int MakeLong(int wLow, int wHigh)
        {
            int low = (int)IntLoWord(wLow);
            short high = IntLoWord(wHigh);
            int product = 0x10000 * (int)high;
            int mkLong = (int)(low | product);
            return mkLong;
        }

        private short IntLoWord(int word)
        {
            return (short)(word & short.MaxValue);
        }

        public FormMain()
        {
            InitializeComponent();

            SendMessage(listView.Handle, WM_CHANGEUISTATE, MakeLong(UIS_SET, UISF_HIDEFOCUS), 0);
        }

        public FormMain(string filename) : this()
        {
            EvaluateFileData(filename);
        }

        string EvaluateHash(Stream stream, Func<HashAlgorithm> func)
        {
            using (HashAlgorithm algorithm = func.Invoke())
            {
                stream.Position = 0;
                return ByteArrayToString(algorithm.ComputeHash(stream));
            }
        }

        private void EvaluateFileData(string filename)
        {
            try
            {
                using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    long length = fs.Length;

                    listView.Items.Add(new ListViewItem(new string[] { "File name", Path.GetFileName(filename) }));
                    listView.Items.Add(new ListViewItem(new string[] { "Full path", filename }));
                    listView.Items.Add(new ListViewItem(new string[] { "Size", $"{length} bytes ({ length / 1024 } KB)" }));

                    listView.Items.Add(new ListViewItem(new string[] { "CRC32", EvaluateHash(fs, DamienG.Security.Cryptography.Crc32.Create) }));
                    listView.Items.Add(new ListViewItem(new string[] { "MD5", EvaluateHash(fs, MD5.Create) }));
                    listView.Items.Add(new ListViewItem(new string[] { "SHA1", EvaluateHash(fs, SHA1.Create) }));
                    listView.Items.Add(new ListViewItem(new string[] { "SHA256", EvaluateHash(fs, SHA256.Create) }));
                    listView.Items.Add(new ListViewItem(new string[] { "SHA384", EvaluateHash(fs, SHA384.Create) }));
                    listView.Items.Add(new ListViewItem(new string[] { "SHA512", EvaluateHash(fs, SHA512.Create) }));

                    listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                    listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        string ByteArrayToString(byte[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                sb.Append($"{array[i]:x2}");
            }
            return sb.ToString();
        }

        private void ListViewDoubleClick(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count == 0)
                return;
            using (var nameValueForm = new FormNameValue(listView.SelectedItems[0].SubItems[0].Text, listView.SelectedItems[0].SubItems[1].Text))
                nameValueForm.ShowDialog(this);
        }

        private void ButtonCloseClick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
