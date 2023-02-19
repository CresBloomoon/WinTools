using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ListView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //イメージリストの設定
            SHFILEINFO shFileInfo = new SHFILEINFO();
            IntPtr imageListHandle = SHGetFileInfo(
                string.Empty,
                0,
                out shFileInfo,
                (uint)Marshal.SizeOf(shFileInfo),
                SHGFI_SMALLICON | SHGFI_SYSICONINDEX);

            //TreeView
            SendMessage(treeView1.Handle,
                TVM_SETIMAGELIST,
                new IntPtr(TVSIL_NORMAL),
                imageListHandle);

            //ListViewFrom
            SendMessage(listView1.Handle,
                LVM_SETIMAGELIST,
                new IntPtr(LVSIL_SMALL),
                imageListHandle);

            //ListViewTo
            SendMessage(listViewTo.Handle,
                        LVM_SETIMAGELIST,
                        new IntPtr(LVSIL_SMALL),
                        imageListHandle);



            //ドライブ一覧を走査してツリーに追加
            foreach (string drive in Environment.GetLogicalDrives())
            {
                //アイコンの取得
                int iconIndex = 0;
                IntPtr hSuccess = SHGetFileInfo(drive,
                                                0,
                                                out shFileInfo,
                                                (uint)Marshal.SizeOf(shFileInfo)
                                                , SHGFI_SYSICONINDEX);
                if (hSuccess != IntPtr.Zero)
                {
                    iconIndex = shFileInfo.iIcon;
                }

                //新規ノード作成
                //プラスボタンを表示するため空のノードを追加しておく
                TreeNode node = new TreeNode(drive, iconIndex, iconIndex);
                node.Nodes.Add(new TreeNode());
                treeView1.Nodes.Add(node);
            }

            //初期選択ドライブの内容を表示
            SetListItem(Environment.GetLogicalDrives().First());

            //リストビューのヘッダを設定
            listViewTo.View = View.Details;
            listViewTo.Clear();
            listViewTo.Columns.Add("名前");
            listViewTo.Columns.Add("種類");
            listViewTo.Columns.Add("更新日時");
            listViewTo.Columns.Add("サイズ");

        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            TreeNode node = e.Node;
            node.Nodes.Clear();

            try
            {
                DirectoryInfo dirList = new DirectoryInfo(node.FullPath);
                foreach (DirectoryInfo di in dirList.GetDirectories())
                {
                    //フォルダのアイコンを取得
                    SHFILEINFO shinfo = new SHFILEINFO();
                    int iconIndex = 0;
                    IntPtr hSuccess = SHGetFileInfo(di.FullName,
                                                    0,
                                                    out shinfo,
                                                    (uint)Marshal.SizeOf(shinfo),
                                                    SHGFI_ICON | SHGFI_LARGEICON | SHGFI_SMALLICON | SHGFI_SYSICONINDEX | SHGFI_TYPENAME);
                    if (hSuccess != IntPtr.Zero)
                    {
                        iconIndex = shinfo.iIcon;
                    }

                    //子を追加してノードを展開
                    TreeNode child = new TreeNode(di.Name, shinfo.iIcon, shinfo.iIcon);
                    child.ImageIndex = iconIndex;
                    child.Nodes.Add(new TreeNode());
                    node.Nodes.Add(child);
                }
            }
            catch (IOException ie)
            {
                MessageBox.Show(ie.Message, "選択エラー");
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            SetListItem(e.Node.FullPath);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            var selectedItems = listView1.SelectedItems;
            foreach (ListViewItem item in selectedItems)
            {
                //項目をコピーして名前を再設定
                ListViewItem copy = (ListViewItem)item.Clone();
                copy.Name = item.Name;

                //すでにリストに含まれていないパスであれば追加
                if (listViewTo.Items.Find(copy.Name, false).Length == 0)
                {
                    listViewTo.Items.Add(copy);
                }
            }

            //列幅を自動調整
            listViewTo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void btn_del_Click(object sender, EventArgs e)
        {
            var selectedItems = listViewTo.SelectedItems;
            foreach (ListViewItem item in selectedItems)
            {
                listViewTo.Items.Remove(item);
            }

            //列幅を自動調整
            listViewTo.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            listViewTo.Items.Clear();
        }

        private void btn_compress_Click(object sender, EventArgs e)
        {
            //実行ディレクトリパスの取得
            Assembly mainAssemply = Assembly.GetExecutingAssembly();

            //実行フォルダを取得
            string appDir = Path.GetDirectoryName(mainAssemply.Location);

            //一時フォルダパス
            string copyTo = appDir + "\\Compress";

            try
            {
                //存在しなければ作成
                if (!Directory.Exists(copyTo))
                {
                    Directory.CreateDirectory(copyTo);
                }

                //選択されたファイルを一時ディレクトリにコピー
                var selectedItems = listView1.SelectedItems;
                foreach (ListViewItem item in selectedItems)
                {
                    //ファイル・ディレクトリを判定
                    if (File.Exists(item.Name))
                    {
                        //存在する場合は上書き
                        File.Copy(item.Name, copyTo + "\\" + item.Text, true);
                    }
                    else if (Directory.Exists(item.Name))
                    {
                        //サブディレクトリを再起処理でコピー
                        CopyDirectory(item.Name, copyTo);
                    }
                }

                //圧縮処理
                IntPtr hWnd = this.Handle;
                StringBuilder strShortPath = new StringBuilder(1024);
                GetShortPathName(copyTo, strShortPath, 1024);

                //zip圧縮
                string strCommandLine = "a -tzip archive.zip "
                                        + strShortPath.ToString()
                                        + "\\*";

                StringBuilder strOutPut = new StringBuilder(1024);
                int ret = SevenZip(hWnd, strCommandLine, strOutPut, 1024);
                if (0 == ret)
                {
                    MessageBox.Show(appDir + "に archive.zipを作成しました。", "圧縮処理");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "圧縮エラー");
            }
            finally
            {

            }
        }

        [DllImport("7-zip32.dll", CharSet = CharSet.Ansi)]
        private static extern int SevenZip(
            IntPtr hWnd,
            string strCommandLine,
            StringBuilder strOutPut,
            uint outputSize);

        [DllImport("kernel32.dll")]
        private static extern uint GetShortPathName(
            string strLongPath,
            StringBuilder strShortPath,
            uint buf);
    }
}
