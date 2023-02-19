using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyProgress
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            const string sourcePath = { your_source_path };
            const string destPath = { your_destination_path };

            SHFILEOPSTRUCT shellOpStruct = new SHFILEOPSTRUCT();

            shellOpStruct.hWnd = this.Handle;
            shellOpStruct.wfunc = FO_COPY;
            shellOpStruct.pfrom = Marshal.StringToCoTaskMemAnsi(sourcePath);
            shellOpStruct.pto = Marshal.StringToCoTaskMemAnsi(destPath);
            SHFileOperation(ref shellOpStruct);

            Marshal.FreeCoTaskMem(shellOpStruct.pfrom);
            Marshal.FreeCoTaskMem(shellOpStruct.pto);
        }

        struct SHFILEOPSTRUCT
        {
            public IntPtr hWnd;
            public int wfunc;
            public IntPtr pfrom;
            public IntPtr pto;
            public int fFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fAnyOperationsAborted;
            public IntPtr hNamemappings;
            public IntPtr lpszProgressTitle;
        }

        private const int FO_COPY = 0x02;
        [DllImport("shell32.dll", EntryPoint = "SHFileOperationA")]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

    }
}
