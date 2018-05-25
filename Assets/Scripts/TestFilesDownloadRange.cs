﻿using System.Text;
using UnityEngine;
using UnityGoogleDrive;

public class TestFilesDownloadRange : MonoBehaviour
{
    public Rect WindowRect = new Rect(10, 10, 300, 200);

    private GoogleDriveFiles.DownloadRequest request;
    private string fileId = string.Empty;
    private string result = string.Empty;
    private RangeInt range = new RangeInt();

    private void OnGUI ()
    {
        GUILayout.Window(0, WindowRect, InfoWindowGUI, "Google Drive Partial Text Downloader");
    }

    private void InfoWindowGUI (int windowId)
    {
        if (request != null && request.IsRunning)
        {
            GUILayout.Label(string.Format("Loading: {0:P2}", request.Progress));
        }
        else
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Text file ID:", GUILayout.Width(70));
            fileId = GUILayout.TextField(fileId, GUILayout.Width(100));
            range.start = int.Parse(GUILayout.TextField(range.start.ToString(), GUILayout.Width(50)));
            GUILayout.Label("-", GUILayout.Width(5));
            range.length = int.Parse(GUILayout.TextField(range.length.ToString(), GUILayout.Width(50)));
            if (GUILayout.Button("Download", GUILayout.Width(100))) DownloadTexture();
            GUILayout.EndHorizontal();
        }

        if (!string.IsNullOrEmpty(result)) GUILayout.TextField(result);
    }

    private void DownloadTexture ()
    {
        request = GoogleDriveFiles.Download(fileId, range.start >= 0 ? (RangeInt?)range : null);
        request.Send().OnDone += SetResult;
    }

    private void SetResult (UnityGoogleDrive.Data.File file)
    {
        result = Encoding.UTF8.GetString(file.Content);
    }
}
