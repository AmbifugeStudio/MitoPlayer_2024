using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Helpers
{
    internal class Tmp
    {
    }
}


dgvTrackList_MouseDown
private void dgvTrackList_MouseDown(object sender, MouseEventArgs e)
{
    var hitTestInfo = dgvTrackList.HitTest(e.X, e.Y);
    if (hitTestInfo.RowIndex >= 0 && e.Button == MouseButtons.Left)
    {
        lastMouseDownRowIndex = hitTestInfo.RowIndex; // Capture the index
        isCoverBrowserUpdateEnabled = false; // Disable cover browser update
        if (ModifierKeys.HasFlag(Keys.Shift) || ModifierKeys.HasFlag(Keys.Control))
        {
            // Handle multi-row selection with Shift or Ctrl key
            return;
        }

        if (dgvTrackList.SelectedRows.Count > 1)
        {
            // Multiple rows are selected, start drag-and-drop immediately
            isDragAndDropInProgress = true; // Set the flag
            StartDragAndDrop(e.Location);
        }
        else if (dgvTrackList.SelectedRows.Cast<DataGridViewRow>().Any(row => row.Index == hitTestInfo.RowIndex))
        {
            // Single row is already selected, start drag-and-drop
            isDragAndDropInProgress = true; // Set the flag
            PrepareForDrag(e.Location, hitTestInfo.RowIndex);
        }
        else
        {
            // Row is not selected, select it and start drag-and-drop
            dgvTrackList.ClearSelection();
            dgvTrackList.Rows[hitTestInfo.RowIndex].Selected = true;
            isDragAndDropInProgress = true; // Set the flag
            PrepareForDrag(e.Location, hitTestInfo.RowIndex);
        }
    }
}

dgvTrackList_MouseMove
private void dgvTrackList_MouseMove(object sender, MouseEventArgs e)
{
    if (isMouseDown && !isDragging)
    {
        if (Math.Abs(e.X - dragStartPoint.X) > SystemInformation.DragSize.Width ||
        Math.Abs(e.Y - dragStartPoint.Y) > SystemInformation.DragSize.Height)
        {
            isDragging = true;
            tracklistClickTimer.Stop();
            dgvTrackList.DoDragDrop(new DataObject(TrackListDataFormat, dgvTrackList.SelectedRows.Cast<DataGridViewRow>().ToArray()), DragDropEffects.Move | DragDropEffects.Copy);
        }
    }
}

dgvTrackList_DragDrop
private void dgvTrackList_DragDrop(object sender, DragEventArgs e)
{
    if (isFilterEnabled)
    {
        return;
    }

    autoScrollTimer.Stop();

    Point clientPoint = dgvTrackList.PointToClient(new Point(e.X, e.Y));
    var hitTestInfo = dgvTrackList.HitTest(clientPoint.X, clientPoint.Y);
    int targetIndex = hitTestInfo.RowIndex >= 0 ? hitTestInfo.RowIndex : dgvTrackList.Rows.Count;

    if (targetIndex < 0 || targetIndex > dgvTrackList.Rows.Count) // Allow dropping at the end
    {
        return;
    }

    if (e.Data.GetDataPresent(DataFormats.FileDrop))
    {
        // Handle files dropped from a directory
        string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

        // Call your function to process the files
        this.ExternalDragAndDropIntoTracklistEvent?.Invoke(this, new ListEventArgs() { DragAndDropFilePathArray = files, IntegerField1 = targetIndex });

        // Clear the insertion line
        insertionLineIndex = -1;
        dgvTrackList.Invalidate(); // Clear the line
    }
    else if (dgvTrackList.SelectedRows.Count > 0)
    {
        // Handle reordering of existing rows
        List<int> selectedIndices = dgvTrackList.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).Select(r => r.Index).ToList();

        this.MoveTracklistRowsEvent?.Invoke(this, new ListEventArgs() { SelectedIndices = selectedIndices, IntegerField1 = targetIndex });

        // Re-select the moved rows using BeginInvoke to ensure it runs after the DataGridView has rendered
        dgvTrackList.BeginInvoke(new Action(() =>
        {
            dgvTrackList.ClearSelection();
            int newIndex = targetIndex;
            foreach (int index in selectedIndices)
            {
                if (newIndex < dgvTrackList.Rows.Count)
                {
                    dgvTrackList.Rows[newIndex].Selected = true;
                    newIndex++;
                }
            }
        }));

        // Clear the insertion line
        insertionLineIndex = -1;
        dgvTrackList.Invalidate(); // Clear the line
    }

    isCoverBrowserUpdateEnabled = true; // Enable cover browser update
    if (lastMouseDownRowIndex >= 0)
    {
        UpdateCoverBrowser(lastMouseDownRowIndex); // Update the cover browser with the captured index
    }
}

Additional Methods
CreateTemporaryFile
private string CreateTemporaryFile(DataGridViewRow row)
{
    string tempFilePath = Path.GetTempFileName();
    string trackPath = row.Cells["Path"].Value.ToString();

    // Copy the track file to the temporary file path
    File.Copy(trackPath, tempFilePath, true);

    return tempFilePath;
}

StartFileDrop
private void StartFileDrop()
{
    if (dgvTrackList.SelectedRows.Count == 1)
    {
        DataGridViewRow selectedRow = dgvTrackList.SelectedRows[0];
        string tempFilePath = CreateTemporaryFile(selectedRow);

        // Initiate the file drop operation
        DataObject dataObject = new DataObject(DataFormats.FileDrop, new string[] { tempFilePath });
        DragDropEffects effect = DoDragDrop(dataObject, DragDropEffects.Copy);

        // Clean up the temporary file if necessary
        if (effect == DragDropEffects.None)
        {
            File.Delete(tempFilePath);
        }
    }
}