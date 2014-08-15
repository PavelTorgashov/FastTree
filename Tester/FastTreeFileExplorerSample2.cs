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
    public partial class FastTreeFileExplorerSample2 : Form
    {
        public FastTreeFileExplorerSample2()
        {
            InitializeComponent();

            ft.Build(null);
        }

        private void ft_NodeIconNeeded(object sender, FastTreeNS.ImageNodeEventArgs e)
        {
            var path = e.Node as string;

            e.Result = File.Exists(path) ? Resources.default_icon : Resources.folder;
        }

        private void ft_NodeTextNeeded(object sender, FastTreeNS.StringNodeEventArgs e)
        {
            var path = e.Node as string;

            e.Result = Path.GetFileName(path);
            if (string.IsNullOrEmpty(e.Result))
                e.Result = e.Node as string;
        }

        private void ft_NodeChildrenNeeded(object sender, FastTreeNS.NodeChildrenNeededEventArgs e)
        {
            var path = e.Node as string;

            //root ?
            if (path == null)
            {
                //return driver's list
                e.Children = DriveInfo.GetDrives().Select(d => d.RootDirectory.FullName).ToList();
                return;
            }

            //path is not dir ?
            if (!Directory.Exists(path))
                return;

            //get subdirs and files
            string[] dirs, files;

            try
            {
                dirs = Directory.GetDirectories(path);
                files = Directory.GetFiles(path);
            }
            catch
            {
                return;//UnauthorizedAccessException
            }

            e.Children = dirs.Concat(files);
        }

        #region Routines

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            (propertyGrid1.SelectedObject as Control).Invalidate();
        }

        #endregion
    }
}
