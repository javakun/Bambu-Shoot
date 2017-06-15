﻿using System;
using System.Threading.Tasks;

namespace Plugin.ShareFile.Abstractions
{
  /// <summary>
  /// Interface for ShareFile
  /// </summary>
  public interface IShareFile
  {
      /// <summary>
      /// Simply share a local file on compatible services
      /// </summary>
      /// <param name="localFilePath">path to local file</param>
      /// <param name="title">Title of popup on share (not included in message)</param>
      /// <returns>awaitable Task</returns>
      void ShareLocalFile(string localFilePath, string title = "");

      /// <summary>
      /// Simply share a file from a remote resource on compatible services
      /// </summary>
      /// <param name="fileUri">uri to external file</param>
      /// <param name="fileName">name of the file</param>
      /// <param name="title">Title of popup on share (not included in message)</param>
      /// <returns>awaitable bool</returns>
      Task ShareRemoteFile(string fileUri, string fileName, string title = "");
  }
}
