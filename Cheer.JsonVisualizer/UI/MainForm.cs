using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Cheer.JsonVisualizer.Controller;
using Cheer.JsonVisualizer.CoreServices.Controls;
using Cheer.JsonVisualizer.Localization;
using ScintillaNET;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using Cheer.JsonVisualizer.CoreServices.Helpers;
using System.Globalization;

namespace Cheer.JsonVisualizer.UI
{
    internal partial class MainForm
        : Window
    {
        public MainForm()
        {
            InitializeComponent();

            menuStrip1.Font = Font;

            InitializeEventHandlers();
            LoadLocalizableResources();

            jsonEditor.Lexer = ScintillaNET.Lexer.Cpp;
            var styles = jsonEditor.Styles;

            styles[Style.Cpp.Comment].ForeColor = Color.Red;
            styles[Style.Cpp.CommentDoc].ForeColor = Color.Red;
            styles[Style.Cpp.CommentDocKeyword].ForeColor = Color.Red;
            styles[Style.Cpp.CommentDocKeywordError].ForeColor = Color.Red;
            styles[Style.Cpp.CommentLine].ForeColor = Color.Red;
            styles[Style.Cpp.CommentLineDoc].ForeColor = Color.Red;
            styles[Style.Cpp.String].ForeColor = Color.Blue;
            styles[Style.Cpp.Number].ForeColor = Color.Green;
            styles[Style.Cpp.Word].ForeColor = Color.Purple;
            jsonEditor.SetKeywords(0, "true false");
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            zoomToolStripSplitButton.Text = jsonEditor.Zoom + "";
        }

        private void InitializeTags()
        {
            textViewToolStripMenuItem.Tag = JsonView.Text;
            splitViewToolStripMenuItem.Tag = JsonView.Split;
            visualizationViewToolStripMenuItem.Tag = JsonView.Visualization;
        }

        private void InitializeEventHandlers()
        {
            exitToolStripMenuItem.Click += (sender, e) => Close();

            editToolStripMenuItem.DropDownOpening += (sender, e) =>
            {
                var textViewTabPageSelected = tabControl.SelectedTab == textViewTabPage;
                var hasSelection = !jsonEditor.Selections.IsEmpty; 

                cutToolStripMenuItem.Enabled = 
                    copyAsToolStripMenuItem.Enabled = 
                    copyToolStripMenuItem.Enabled = textViewTabPageSelected && hasSelection;
               
                pasteToolStripMenuItem.Enabled = textViewTabPageSelected && jsonEditor.CanPaste;
                undoToolStripMenuItem.Enabled = textViewTabPageSelected && jsonEditor.CanUndo;
                redoToolStripMenuItem.Enabled = textViewTabPageSelected && jsonEditor.CanRedo;
                selAllToolStripMenuItem.Enabled = textViewTabPageSelected;
            };
            var zoom = jsonEditor.Zoom;
            cutToolStripMenuItem.Click += (sender, e) => CutText();
            copyToolStripMenuItem.Click += (sender, e) => CopyText();
            pasteToolStripMenuItem.Click += (sender, e) => PasteText();
            undoToolStripMenuItem.Click += (sender, e) => EditorUndo();
            redoToolStripMenuItem.Click += (sender, e) => EditorRedo();
            selAllToolStripMenuItem.Click += (sender, e) => EditorSelectAll();

            editorContextMenuStrip.Opening += (sender, e) =>
            {
                var hasSelection = !jsonEditor.Selections.IsEmpty;
                cutToolStripMenuItem2.Enabled = hasSelection;
                copyToolStripMenuItem2.Enabled = hasSelection;
                pasteToolStripMenuItem2.Enabled = jsonEditor.CanPaste;
                undoToolStripMenuItem2.Enabled = jsonEditor.CanUndo;
                redoToolStripMenuItem2.Enabled = jsonEditor.CanRedo;
            };

            cutToolStripMenuItem2.Click += (sender, e) => CutText();
            copyToolStripMenuItem2.Click += (sender, e) => CopyText();
            pasteToolStripMenuItem2.Click += (sender, e) => PasteText();
            undoToolStripMenuItem2.Click += (sender, e) => EditorUndo();
            redoToolStripMenuItem2.Click += (sender, e) => EditorRedo();
            selAllToolStripMenuItem2.Click += (sender, e) => EditorSelectAll();

            viewToolStripMenuItem.DropDownOpening += (sender, e) =>
            {
                var tabPage = tabControl.SelectedTab;
                if(tabPage == textViewTabPage)
                {
                    textViewToolStripMenuItem.Checked = true;
                    splitViewToolStripMenuItem.Checked = false;
                    visualizationViewToolStripMenuItem.Checked = false;
                }
                else if(tabPage == splitViewTabPage)
                {
                    textViewToolStripMenuItem.Checked = false;
                    splitViewToolStripMenuItem.Checked = true;
                    visualizationViewToolStripMenuItem.Checked = false;
                }
                else if(tabPage == visualizationViewTabPage)
                {
                    textViewToolStripMenuItem.Checked = false;
                    splitViewToolStripMenuItem.Checked = false;
                    visualizationViewToolStripMenuItem.Checked = true;
                }

                statusBarToolStripMenuItem.Checked = statusStrip.Visible;
                wordwrapToolStripMenuItem.Checked = jsonEditor.WrapMode != WrapMode.None;
            };
            textViewToolStripMenuItem.Click += (sender, e) => tabControl.SelectedTab = textViewTabPage;
            splitViewToolStripMenuItem.Click += (sender, e) => tabControl.SelectedTab = splitViewTabPage;
            visualizationViewToolStripMenuItem.Click += (sender, e) => tabControl.SelectedTab = visualizationViewTabPage;
            statusBarToolStripMenuItem.Click += (sender, e) => statusStrip.Visible = !statusBarToolStripMenuItem.Checked;

            hideStatusBarToolStripMenuItem.Click += (sender, e) => statusStrip.Visible = false;

        }
        private void EditorSelectAll()
        {
            if(tabControl.SelectedTab == textViewTabPage && jsonEditor.CanUndo)
            {
                jsonEditor.SelectAll();
            }
        }
        private void EditorUndo()
        {
            if(tabControl.SelectedTab == textViewTabPage && jsonEditor.CanUndo)
            {
                jsonEditor.Undo();
            }
        }
        private void EditorRedo()
        {
            if(tabControl.SelectedTab == textViewTabPage && jsonEditor.CanRedo)
            {
                jsonEditor.Redo();
            }
        }
        private void CutText()
        {
            if(tabControl.SelectedTab == textViewTabPage && !jsonEditor.Selections.IsEmpty)
            {
                jsonEditor.Cut();
            }
        }
        private void CopyText()
        {
            if(tabControl.SelectedTab == textViewTabPage && !jsonEditor.Selections.IsEmpty)
            {
                jsonEditor.Copy();
            }
        }
        private void PasteText()
        {
            if(tabControl.SelectedTab == textViewTabPage)
            {
                jsonEditor.Paste();
            }
        }




        private void LoadLocalizableResources()
        {
            Text = Strings.ProductInfo_Name;
            helpToolStripMenuItem.Text = Strings.UI_MainForm_HelpToolStripMenuItem_Text;
            aboutToolStripMenuItem.Text = Strings2.Format("UI_MainForm_AboutToolStripMenuItem_Text_Format",
                Strings.ProductInfo_Name);
        }
        
        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeView.Nodes.Clear();

            // 当前处理的节点
            TreeNode currentNode = null;
            string propertyName = null;

            TextReader textReader = new StringReader(jsonEditor.Text);
            JsonReader jsonReader = new JsonTextReader(textReader);
            while(jsonReader.Read())
            {
                switch(jsonReader.TokenType)
                {
                    case JsonToken.StartObject:
                        var objectNode = new TreeNode
                        {
                            Text = propertyName
                        };
                        if(currentNode != null)
                        {
                            currentNode.Nodes.Add(objectNode);
                        }
                        currentNode = objectNode;
                        break;
                    case JsonToken.PropertyName:
                        propertyName = (string)jsonReader.Value;
                        break;
                    case JsonToken.Boolean:
                        var booleanNode = CreateNode(propertyName, (bool)jsonReader.Value);
                        if(currentNode == null)
                        {
                            currentNode = booleanNode;
                        }
                        else
                        {
                            currentNode.Nodes.Add(booleanNode);
                        }
                        break;
                    case JsonToken.EndObject:
                        if(currentNode != null && currentNode.Parent != null)
                        {
                            currentNode = currentNode.Parent;
                        }
                        break;
                    case JsonToken.Integer:
                    case JsonToken.Float: 
                    case JsonToken.Date:
                    case JsonToken.Bytes:
                        break;
                    case JsonToken.Null:
                        break;
                }
            }
            if(currentNode != null)
            {
                treeView.Nodes.Add(currentNode);
            }
        }
        
        private TreeNode CreateNode(string propertyName, bool value)
        {
            var booleanString = value ? "true" : "false";

            return new TreeNode
            {
                Text = string.IsNullOrEmpty(propertyName) ? booleanString : 
                string.Format(CultureInfo.InvariantCulture, "{0}: {1}", propertyName, booleanString),
                ToolTipText = "布尔值"
            };
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            jsonEditor.Zoom = 0;
        }

        private void 自定义CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var dialog = new CustomZoomDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            jsonEditor.ZoomIn();
            Text = jsonEditor.Zoom + "";
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using(var dialog = new OptionsDialog())
            {
                dialog.ShowDialog(this);
            }
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            var selected = jsonEditor.SelectedText;
            if(!string.IsNullOrEmpty(selected))
            {
                // Clipboard.SetText(selected.CSharpStringEncode(true));
            }
        }
    }
}
