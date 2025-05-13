using DevExpress.Blazor.Reporting.Models;
using Syncfusion.Blazor.PdfViewerServer;
using Syncfusion.Pdf.Graphics;

namespace Demo.Pages
{
    public partial class Pdf : Base
    {
        byte[] DocumentContent { get; set; }

        /// <summary>PDFビュワー</summary>
        SfPdfViewerServer pdfViewer;

        protected override async Task Init()
        {
            LogWriters.LogWriter.Info("MenteMCustomer 画面");

            var PdfPath = @"./wwwroot/download/杜の都銀行.pdf";

            DocumentContent = File.ReadAllBytes(PdfPath);

            await Task.Delay(2);
        }

        protected void OnCustomizeToolbar(ToolbarModel toolbarModel)
        {
            var printToolbarItem = toolbarModel.AllItems.Where(i => i.Id == ToolbarItemId.Print).FirstOrDefault();
            if (printToolbarItem != null)
            {
                printToolbarItem.IconCssClass = "print-btn";
            }
            toolbarModel.AllItems.Clear();

            var downloadToolbarItem = new ToolbarItem
            {
                Text = "Download",
                AdaptiveText = "Download",
                BeginGroup = true,
                Id = "Download",
                IconCssClass = "download-btn",
                Click = async (args) => {
                    await pdfViewer.DownloadAsync();
                }
            };
            toolbarModel.AllItems.Add(printToolbarItem);
            toolbarModel.AllItems.Add(downloadToolbarItem);
        }

    }
}
