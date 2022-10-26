using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CadastrPlanApp
{
    /// <summary>
    /// Главная форма.
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// Словарь объектов.
        /// </summary>
        private Dictionary<string, CadastrObject> dataSet;

        /// <summary>
        /// Список идентификаторов отмеченных объектов.
        /// </summary>
        private List<string> checkedObjectId;

        /// <summary>
        /// XML-документ.
        /// </summary>
        private XDocument document;

        /// <summary>
        /// Путь открываемого файла.
        /// </summary>
        private string pathFile;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Формирует кадастровое дерево.
        /// </summary>
        /// <param name="document">XML-документ.</param>
        private void FillTreeView(XDocument document)
        {
            TreeNode argentinaNode = new TreeNode { Text = "Объекты" };
            treeView.CheckBoxes = true;
            var lands = new TreeNode { Text = "Земельный участок" };
            var builds = new TreeNode { Text = "Здания" };
            var construction = new TreeNode { Text = "Сооружение" };
            var spatial = new TreeNode { Text = "Пространнственные данные" };
            var munitipal = new TreeNode { Text = "Муниципальная земля" };
            var zone = new TreeNode { Text = "Зоны" };

            argentinaNode.Nodes.Add(lands);
            argentinaNode.Nodes.Add(builds);
            argentinaNode.Nodes.Add(construction);
            argentinaNode.Nodes.Add(spatial);
            argentinaNode.Nodes.Add(munitipal);
            argentinaNode.Nodes.Add(zone);

            dataSet = ParserCadastrXML.ParsingData(document);
            foreach (var elem in dataSet)
            {
                switch (elem.Value.Type)
                {
                    case "Land":
                        lands.Nodes.Add(new TreeNode { Text = elem.Key });
                        break;
                    case "Build":
                        builds.Nodes.Add(new TreeNode { Text = elem.Key });
                        break;
                    case "Construction":
                        construction.Nodes.Add(new TreeNode { Text = elem.Key });
                        break;
                    case "Spatial":
                        spatial.Nodes.Add(new TreeNode { Text = elem.Key });
                        break;
                    case "Municipal":
                        munitipal.Nodes.Add(new TreeNode { Text = elem.Key });
                        break;
                    case "Zone":
                        zone.Nodes.Add(new TreeNode { Text = elem.Key });
                        break;
                }
            }

            treeView.Nodes.Add(argentinaNode);
        }

        /// <summary>
        /// Обрабатывает нажатие на элемент дерева.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_BeforeSelect(Object sender, TreeViewCancelEventArgs e)
        {
            try
            {
                richTextBox.Text = dataSet[e.Node.Text].ToString();
            }
            catch { }
            
        }

        /// <summary>
        /// Возвращает строку с выделенными элементами.
        /// </summary>
        /// <param name="tnc">Коллекция узлов дерева.</param>
        /// <returns></returns>
        private string GetCheckedNodes(TreeNodeCollection tnc)
        {
            StringBuilder sb = new StringBuilder();
            checkedObjectId = new List<string>();

            foreach (TreeNode tn in tnc[0].Nodes)
            {
                foreach (TreeNode th in tn.Nodes)
                {
                    if (th.Checked)
                    {
                        checkedObjectId.Add(th.Text);
                        string res = th.FullPath;
                        sb.AppendLine(res);
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Обрабатывает нажатие на кнопку "Сохранить".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.Cancel)
                return;
            
            string fileName = saveFileDialog.FileName;
            SaveHelperXML.SaveToFile(checkedObjectId, document, fileName);
        }

        /// <summary>
        /// Обрабатывает нажатие на кнопку "Посмотреть".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCheckButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(GetCheckedNodes(treeView.Nodes));
        }

        /// <summary>
        /// Обрабатывает нажатие на кнопку "Открыть".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            pathFile = openFileDialog.FileName;
            document = XDocument.Load(pathFile);

            try
            {
                FillTreeView(document);
                SetEnabledComponents();
            }
            catch
            {
                MessageBox.Show("Некорректный файл");
            }
        }

        /// <summary>
        /// Выделяет/снимает выделание с дочерних узлов.
        /// </summary>
        /// <param name="node">Узел дерева.</param>
        private void TreeViewChangeCheckBox(TreeNode node)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].Checked = node.Checked;
                TreeViewChangeCheckBox(node.Nodes[i]);
            }
        }

        /// <summary>
        /// Устанавливает видимость элементов.
        /// </summary>
        private void SetEnabledComponents()
        {
            treeView.Enabled = true;
            richTextBox.Enabled = true;
            SaveButton.Enabled = true;
            ViewCheckButton.Enabled = true;
        }

        /// <summary>
        /// Обрабатывает изменение выделания узла.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            GetCheckedNodes(treeView.Nodes);
            TreeViewChangeCheckBox(e.Node);
        }
    }
}
