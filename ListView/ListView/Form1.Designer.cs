using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Reflection;
using IWshRuntimeLibrary;
using System.IO;

namespace ListView
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.listView1 = new System.Windows.Forms.ListView();
            this.listViewTo = new System.Windows.Forms.ListView();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_del = new System.Windows.Forms.Button();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_compress = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeView1
            // 
            this.treeView1.Location = new System.Drawing.Point(27, 23);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(269, 415);
            this.treeView1.TabIndex = 0;
            this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(318, 23);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(449, 190);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // listViewTo
            // 
            this.listViewTo.HideSelection = false;
            this.listViewTo.Location = new System.Drawing.Point(318, 248);
            this.listViewTo.Name = "listViewTo";
            this.listViewTo.Size = new System.Drawing.Size(449, 190);
            this.listViewTo.TabIndex = 2;
            this.listViewTo.UseCompatibleStateImageBehavior = false;
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(318, 219);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(75, 23);
            this.btn_add.TabIndex = 3;
            this.btn_add.Text = "追加";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_del
            // 
            this.btn_del.Location = new System.Drawing.Point(399, 219);
            this.btn_del.Name = "btn_del";
            this.btn_del.Size = new System.Drawing.Size(75, 23);
            this.btn_del.TabIndex = 4;
            this.btn_del.Text = "削除";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(648, 219);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(106, 23);
            this.btn_clear.TabIndex = 5;
            this.btn_clear.Text = "追加項目をクリア";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_compress
            // 
            this.btn_compress.Location = new System.Drawing.Point(610, 461);
            this.btn_compress.Name = "btn_compress";
            this.btn_compress.Size = new System.Drawing.Size(75, 23);
            this.btn_compress.TabIndex = 6;
            this.btn_compress.Text = "圧縮";
            this.btn_compress.UseVisualStyleBackColor = true;
            this.btn_compress.Click += new System.EventHandler(this.btn_compress_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(692, 461);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "展開";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 496);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btn_compress);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.btn_del);
            this.Controls.Add(this.btn_add);
            this.Controls.Add(this.listViewTo);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.treeView1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ListView listView1;

        /// <summary>
        /// ファイルサイズを単位付きに変換して返します。
        /// </summary>
        /// <param name="fileSize"></param>
        /// <returns>ファイルサイズ</returns>
        private static string GetFileSize(long fileSize)
        {
            string ret = fileSize + "バイト";
            if (fileSize > (1024f * 1024f * 1024f))
            {
                ret = Math.Round((fileSize / 1024f / 1024f / 1024f), 2)
                          .ToString()
                          + " GB";
            }
            else if (fileSize > (1024f * 1024f))
            {
                ret = Math.Round((fileSize / 1024f / 1024f), 2)
                          .ToString()
                          + " MB";
            }
            else if (fileSize > 1024f)
            {
                ret = Math.Round((fileSize / 1024f))
                          .ToString()
                          + " KB";
            }

            return ret;
        }

        private void SetListItem(string filePath)
        {
            //リストビューのヘッダを設定
            listView1.View = System.Windows.Forms.View.Details;
            listView1.Clear();
            listView1.Columns.Add("名前");
            listView1.Columns.Add("更新日時");
            listView1.Columns.Add("サイズ");

            try
            {
                //フォルダ一覧
                DirectoryInfo dirList = new DirectoryInfo(filePath);
                foreach (DirectoryInfo di in dirList.GetDirectories())
                {
                    ListViewItem item = new ListViewItem(di.Name);

                    //フォルダ種類、アイコンの取得
                    string type = string.Empty;
                    int iconIndex = 0;
                    SHFILEINFO shFileInfo = new SHFILEINFO();
                    IntPtr hSuccess = SHGetFileInfo(di.FullName,
                                                    0,
                                                    out shFileInfo,
                                                    (uint)Marshal.SizeOf(shFileInfo),
                                                    SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_TYPENAME);

                    if (hSuccess != IntPtr.Zero)
                    {
                        type = shFileInfo.szTypeName;
                        iconIndex = shFileInfo.iIcon;
                    }

                    //各列の内容を設定
                    item.Name = di.FullName;
                    item.ImageIndex = iconIndex;
                    item.SubItems.Add(type);
                    item.SubItems.Add(string.Format("{0:yyyy/MM/dd HH:mm:ss}",
                                      di.LastAccessTime));
                    item.SubItems.Add("");

                    //リストに追加
                    listView1.Items.Add(item);
                }

                //ファイル一覧
                List<string> files = Directory.GetFiles(filePath).ToList<string>();
                foreach (string file in files)
                {
                    FileInfo info = new FileInfo(file);
                    ListViewItem item = new ListViewItem(info.Name);

                    //ファイル種類、アイコンの取得
                    string type = string.Empty;
                    int iconIndex = 0;
                    SHFILEINFO shinfo = new SHFILEINFO();
                    IntPtr hSuccess = SHGetFileInfo(info.FullName,
                                                    0,
                                                    out shinfo,
                                                    (uint)Marshal.SizeOf(shinfo),
                                                    SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_TYPENAME);
                    if (hSuccess != IntPtr.Zero)
                    {
                        type = shinfo.szTypeName;
                        iconIndex = shinfo.iIcon;
                    }

                    //各列の内容を設定
                    item.Name = info.FullName;
                    item.ImageIndex = iconIndex;
                    item.SubItems.Add(type);
                    item.SubItems.Add(string.Format("{0:yyyy/MM/dd HH:mm:ss}",
                                                     info.LastAccessTime));
                    item.SubItems.Add(GetFileSize(info.Length));
                    listView1.Items.Add(item);
                }
            }
            catch (IOException ie)
            {
                MessageBox.Show(ie.Message, "選択エラー");
            }

            //列幅を自動調整
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            out SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(
            IntPtr hWnd,
            uint Msg,
            IntPtr wParam,
            IntPtr lParam);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        private struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        };



        //ファイル情報用
        private const int SHGFI_LARGEICON = 0x00000000;
        private const int SHGFI_SMALLICON = 0x00000001;
        private const int SHGFI_USEFILEATTRIBUTES = 0x00000010;
        private const int SHGFI_OVERLAYINDEX = 0x00000040;
        private const int SHGFI_ICON = 0x00000100;
        private const int SHGFI_SYSICONINDEX = 0x00004000;
        private const int SHGFI_TYPENAME = 0x000000400;

        // TreeView用
        private const int TVSIL_NORMAL = 0x0000;
        private const int TVSIL_STATE = 0x0002;
        private const int TVM_SETIMAGELIST = 0x1109;

        // ListView用
        private const int LVSIL_NORMAL = 0;
        private const int LVSIL_SMALL = 1;
        private const int LVM_SETIMAGELIST = 0x1003;

        //選択された項目を保持
        private string selectedItem = string.Empty;
        private System.Windows.Forms.ListView listViewTo;
        private Button btn_add;
        private Button btn_del;
        private Button btn_clear;
        private Button btn_compress;
        private Button button2;

        public static void CopyDirectory(string from, string to)
        {
            //ディレクトリが存在しない場合は作成
            if (!Directory.Exists(to))
            {
                Directory.CreateDirectory(to);
                System.IO.File.SetAttributes(to, System.IO.File.GetAttributes(from));
            }

            //コピー先のディレクトリ名の末尾に"\"をつける
            if (!to.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                to += Path.DirectorySeparatorChar;
            }

            //コピー元のディレクトリにあるファイルをコピー
            string[] files = Directory.GetFiles(from);
            foreach (string file in files)
            {
                System.IO.File.Copy(file, to + Path.GetFileName(file), true);
            }

            //サブディレクトリを再起処理する
            string[] dirs = Directory.GetDirectories(from);
            foreach (string dir in dirs)
            {
                CopyDirectory(dir, to + Path.GetFileName(dir));
            }
        }
    }

}
