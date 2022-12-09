using prueba.Models;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web.Mvc;

namespace DocumentacionProducto.Controllers
{

    public class Ejemplo : Controller
    {
        private List<FolderMap> _folders;
        public Ejemplo()
        {
            _folders = new List<FolderMap>();
            //MapFolder = new MapFolder();
        }

        [HttpGet]
        public ActionResult SelectFile(string noMaquina, string noParte, string file)
        {

            ViewBag.noMaquina = noMaquina;
            ViewBag.noParte = noParte;
            ViewBag.File = file;
            string root = @"C:\Users\Jesús Martínez\Documents\Proyectos visual studio\prueba-20221002T171822Z-001\prueba\prueba\DOCUMENTACION";
            var fileInfo = new DirectoryInfo($"{root}\\{file}");
            var ruta = fileInfo.ToString();

            WebClient client = new WebClient();
            client.DownloadFile(ruta, Path.Combine(Server.MapPath($"/DOCUMENTO.PDF")));

            return RedirectToAction("Index", new { noMaquina, noParte, file });
        }


        [HttpGet]
        public ActionResult Index(string noMaquina, string noParte, string file)
        {
            if (string.IsNullOrWhiteSpace(noMaquina) || string.IsNullOrWhiteSpace(noParte))
            {
                return View(_folders);
            }

            string rootFolder = Server.MapPath("~/DOCUMENTACION/");
            MapFolders(rootFolder, noMaquina, noParte);

            if (!string.IsNullOrWhiteSpace(file))
            {
                ViewBag.File = file;
            }

            ViewBag.noMaquina = noMaquina;
            ViewBag.noParte = noParte;

            return View(_folders);
        }

        //private void MapFolders(string folderRoot, string filePattern)
        //{
        //    var dir = new DirectoryInfo(folderRoot);

        //    foreach (var directory in dir.GetDirectories())
        //    {
        //        var newFolder = new FolderMap();
        //        newFolder.Folder = directory.Name;
        //        newFolder.FolderPath = directory.FullName;
        //        RecorrerCarpetas(newFolder, newFolder.FolderPath, filePattern, newFolder.Folder);
        //        _folders.Add(newFolder);
        //    }
        //}

        private void MapFolders(string folderRoot, string noMaquina, string noParte)
        {
            var dir = new DirectoryInfo(folderRoot);

            foreach (var directory in dir.GetDirectories())
            {
                var newFolder = new FolderMap();
                newFolder.Folder = directory.Name;
                newFolder.FolderPath = directory.FullName;
                RecorrerPor(newFolder, noMaquina, noParte);
                RecorrerPorImg(newFolder, noMaquina, noParte);
                _folders.Add(newFolder);
            }
        }

        private void RecorrerPor(FolderMap folder, string noMaquina, string noParte)
        {
            string actualFolder = $"{folder.FolderPath}\\{noMaquina}\\{noParte}";
            var dir = new DirectoryInfo(actualFolder);

            if (dir.Exists)
            {
                RecorrerFiles(folder, dir, noMaquina, noParte, new[] { "pdf" });
            }
        }

        private void RecorrerFiles(FolderMap folder, DirectoryInfo dir, string noMaquina, string noParte, string[] extensions)
        {
            foreach (var extension in extensions)
            {
                FileInfo[] fileNames = dir.GetFiles($"*.{extension}");
                foreach (var file in fileNames)
                {
                    folder.Files.Add(new MapFile
                    {
                        Name = file.Name,
                        NamePath = file.FullName,
                        NameUrl = $"{folder.Folder}\\{noMaquina}\\{noParte}\\{file.Name}",
                        NoMaquina = noMaquina,
                        NoParte = noParte,
                    });
                }
            }

        }

        private void RecorrerPorImg(FolderMap folder, string noMaquina, string noParte)
        {
            string actualFolder = $"{folder.FolderPath}\\{noMaquina}\\{noParte}";
            var dir = new DirectoryInfo(actualFolder);

            if (dir.Exists)
            {
                RecorrerImages(folder, dir, noMaquina, noParte, new[] { "png", "jpg" });
            }
        }

        

        private void RecorrerImages(FolderMap folder, DirectoryInfo dir, string noMaquina, string noParte, string[] extensions)
        {
            foreach (var extension in extensions)
            {
                FileInfo[] fileNames = dir.GetFiles($"*.{extension}");
                foreach (var file in fileNames)
                {
                    folder.Images.Add(new MapFile
                    {
                        Name = file.Name,
                        NamePath = file.FullName,
                        NameUrl = $"{folder.Folder}\\{noMaquina}\\{noParte}\\{file.Name}",
                        NoMaquina = noMaquina,
                        NoParte = noParte,
                    });
                }
            }

        }

        //private void RecorrerCarpetas(FolderMap folder, string Subfolder, string filePattern, string lastFolder)
        //{
        //    var dir = new DirectoryInfo(Subfolder);
        //    string[] carpetas = Directory.GetDirectories(Subfolder);
        //    foreach (var item in carpetas)
        //    {
        //        RecorrerCarpetas(folder, item, filePattern, dir.Name);
        //    }

        //    if (dir.Name.ToLower().Contains(filePattern.ToLower()))
        //    {
        //        FileInfo[] fileNames = dir.GetFiles($"*.pdf");
        //        foreach (var file in fileNames)
        //        {
        //            folder.Files.Add(new MapFile
        //            {
        //                Name = file.Name,
        //                NamePath = file.FullName,
        //                NameUrl = $"{lastFolder}\\{dir.Name}\\{file.Name}"
        //            });
        //        }
        //    }
        //}

    }
}