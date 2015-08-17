using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq; 
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Layer;

namespace SaveUMLDiagramToImageFileCommandExtension
{
    /// <summary>
    /// Called when the user clicks the menu item.
    /// </summary>
    // Context menu command applicable to any UML diagram
    [Export(typeof(ICommandExtension))]
    [ClassDesignerExtension]
    [UseCaseDesignerExtension]
    [SequenceDesignerExtension]
    [ComponentDesignerExtension]
    [ActivityDesignerExtension]
    [LayerDesignerExtension]
    public class CommandExtension : ICommandExtension
    {
        private static WeakReference<SaveFileDialog> _saveDialogReference = new WeakReference<SaveFileDialog>(null);

        [Import]
        IDiagramContext Context { get; set; }

        public void Execute(IMenuCommand command)
        {
            // Get the diagram of the underlying implementation.
            Diagram dslDiagram = Context.CurrentDiagram.GetObject<Diagram>();
            if (dslDiagram == null) return;

            var model = dslDiagram;
            SaveFileDialog dialog = GetSaveDialog();
            dialog.FileName = model.Name;

            string imageFileName = dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
            if (!string.IsNullOrEmpty(imageFileName))
            {
                switch (dialog.FilterIndex)
                {
                    case 1:
                    case 2:
                    case 4:
                        Bitmap bitmap = dslDiagram.CreateBitmap(
                            dslDiagram.NestedChildShapes,
                            Diagram.CreateBitmapPreference.FavorClarityOverSmallSize);
                        bitmap.Save(imageFileName, GetImageType(imageFileName));
                        break;
                    case 3:
                        Metafile metafile = dslDiagram.CreateMetafile(
                            dslDiagram.NestedChildShapes);
                        metafile.Save(imageFileName, GetImageType(imageFileName));
                        break;
                }

            }
        }

        /// <summary>
        /// Called when the user right-clicks the diagram.
        /// Set Enabled and Visible to specify the menu item status.
        /// </summary>
        /// <param name="command"></param>
        public void QueryStatus(IMenuCommand command)
        {
            command.Enabled = Context.CurrentDiagram != null && Context.CurrentDiagram.ChildShapes.Any();
        }

        /// <summary>
        /// Menu text.
        /// </summary>
        public string Text
        {
            get { return "Save To Image..."; }
        }

        private static SaveFileDialog GetSaveDialog()
        {
            SaveFileDialog dialog;
            if (!_saveDialogReference.TryGetTarget(out dialog))
            {
                dialog = CreateSaveDialog();
                _saveDialogReference.SetTarget(dialog);
            }
            return dialog;
        }

        private static SaveFileDialog CreateSaveDialog()
        {
            SaveFileDialog dialog = new SaveFileDialog
            {
                AddExtension = true,
                Filter = "JPEG File (*.jpg)|*.jpg|Portable Network Graphic (*.png)|*.png|Enhanced Metafile (*.emf)|*.emf|Bitmap (*.bmp)|*.bmp",
                FilterIndex = 1,
                Title = "Save Diagram to Image"
            };
            return dialog;
        }

        /// <summary>
        /// Return the appropriate image type for a file extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static ImageFormat GetImageType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            ImageFormat result;
            switch (extension)
            {
                case ".jpg":
                    result = ImageFormat.Jpeg;
                    break;
                case ".emf":
                    result = ImageFormat.Emf;
                    break;
                case ".png":
                    result = ImageFormat.Png;
                    break;
                case ".bmp":
                    result = ImageFormat.Bmp;
                    break;
                default:
                    result = ImageFormat.Jpeg;
                    break;
            }
            return result;
        }
    }
}