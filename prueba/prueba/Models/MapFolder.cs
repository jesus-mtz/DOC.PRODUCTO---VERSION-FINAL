using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace prueba.Models
{
    public class MapFolder
    {
        public string Folder { get; set; }
        public string FolderPath { get; set; }
        public List<MapFile> Files { get; set; }
        public List<MapFolder> Folders { get; set; }

        public MapFolder()
        {
            Files = new List<MapFile>();
            Folders = new List<MapFolder>();
        }
    }

    public class FolderMap
    {
        public string Folder { get; set; }
        public string FolderPath { get; set; }
        public List<MapFile> Files { get; set; }
        public List<FolderMap> Folders { get; set; }
        public List<MapFile> Images { get; set; }
        public FolderMap()
        {
            Files = new List<MapFile>();

            Images = new List<MapFile>();

            Folders = new List<FolderMap>();

        }
    }

    public class MapFile
    {
        public string Name { get; set; }    
        public string NamePath { get; set; }    
        public string NameUrl { get; set; }    
        public string NoParte { get; set; }    
    }
}