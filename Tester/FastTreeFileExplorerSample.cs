using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tester.DataModel;
using Tester.Properties;

namespace Tester
{
    public partial class FastTreeFileExplorerSample : Form
    {
        public FastTreeFileExplorerSample()
        {
            InitializeComponent();

            var drivers = DriveInfo.GetDrives().Select(d=>new FileNode(d.RootDirectory.FullName, true)).ToList();
            ft.Build(drivers);
        }

        private void ft_NodeIconNeeded(object sender, FastTreeNS.ImageNodeEventArgs e)
        {
            e.Result = (e.Node as FileNode).IsDir ? Resources.folder : Resources.default_icon;
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }

    public class FileNode : IEnumerable<FileNode>
    {
        public string Path { get; private set; }
        public bool IsDir { get; private set; }

        public FileNode(string path, bool isDir)
        {
            this.Path = path;
            this.IsDir = isDir;
        }

        public string Name
        {
            get 
            { 
                var name = System.IO.Path.GetFileName(Path);
                if (string.IsNullOrEmpty(name))
                    return Path;
                return name;
            }
        }

        public bool HasChildren
        {
            get { return true; }
        }

        public IEnumerator<FileNode> GetEnumerator()
        {
            if (!IsDir)
                yield break;

            string[] dirs, files;

            try
            {
                dirs = Directory.GetDirectories(Path);
                files = Directory.GetFiles(Path);
            }catch
            {
                yield break;//UnauthorizedAccessException
            }

            foreach (var path in dirs)
                yield return new FileNode(path, true);

            foreach (var path in files)
                yield return new FileNode(path, false);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return Path.Equals((obj as FileNode).Path);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
