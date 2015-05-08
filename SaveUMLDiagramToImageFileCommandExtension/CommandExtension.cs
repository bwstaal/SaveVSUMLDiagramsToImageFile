using System.ComponentModel.Composition; // for [Import], [Export]
using System.Drawing; // for Bitmap
using System.Drawing.Imaging; // for ImageFormat
using System.Linq; // for collection extensions
using System.Windows.Forms; // for SaveFileDialog
using Microsoft.VisualStudio.Modeling.Diagrams; // for Diagram
using Microsoft.VisualStudio.Modeling.ExtensionEnablement; // for IGestureExtension, ICommandExtension, ILinkedUndoContext
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation; // for IDiagramContext

// for designer extension attributes
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
    class CommandExtension : ICommandExtension
    {
        [Import]
        IDiagramContext Context { get; set; }

        public void Execute(IMenuCommand command)
        {
            // Get the diagram of the underlying implementation.
            Diagram dslDiagram = Context.CurrentDiagram.GetObject<Diagram>();
            if (dslDiagram != null)
            {
                string imageFileName = FileNameFromUser();
                if (!string.IsNullOrEmpty(imageFileName))
                {
                    Bitmap bitmap = dslDiagram.CreateBitmap(
                     dslDiagram.NestedChildShapes,
                     Diagram.CreateBitmapPreference.FavorClarityOverSmallSize);
                    bitmap.Save(imageFileName, GetImageType(imageFileName));
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
            command.Enabled = Context.CurrentDiagram != null
              && Context.CurrentDiagram.ChildShapes.Count() > 0;
        }

        /// <summary>
        /// Menu text.
        /// </summary>
        public string Text
        {
            get { return "Save To Image..."; }
        }


        /// <summary>
        /// Ask the user for the path of an image file.
        /// </summary>
        /// <returns>image file path, or null</returns>
        private string FileNameFromUser()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.AddExtension = true;
            dialog.DefaultExt = "image.bmp";
            dialog.Filter = "Bitmap ( *.bmp )|*.bmp|JPEG File ( *.jpg )|*.jpg|Enhanced Metafile (*.emf )|*.emf|Portable Network Graphic ( *.png )|*.png";
            dialog.FilterIndex = 1;
            dialog.Title = "Save Diagram to Image";
            return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
        }

        /// <summary>
        /// Return the appropriate image type for a file extension.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private ImageFormat GetImageType(string fileName)
        {
            string extension = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            ImageFormat result = ImageFormat.Bmp;
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
            }
            return result;
        }
    }
}