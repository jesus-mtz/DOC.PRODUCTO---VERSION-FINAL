using prueba.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace DocumentacionProducto.Controllers
{
    public class DocumentacionController : Controller
    {
        private List<FolderMap> _folders;

        public DocumentacionController()
        {
            _folders = new List<FolderMap>();
        }

        public ActionResult Principal()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Index(string noParte, string file)
        {
            if (string.IsNullOrWhiteSpace(noParte))
            {
                return View(_folders);
            }

            string rootFolder = Server.MapPath("~/DOCUMENTACION/");

            MapFolders(rootFolder, noParte);

            if (!string.IsNullOrWhiteSpace(file))
            {
                ViewBag.archivo = noParte;
            }

            ViewBag.noParte = noParte;

            return View(_folders);
        }

        private void MapFolders(string folderRoot, string noParte)
        {
            var dir = new DirectoryInfo($"{folderRoot}");

            foreach (var directory in dir.GetDirectories())
            {
                if (directory.Name.StartsWith(noParte))
                {
                    var newFolder = new FolderMap();
                    newFolder.Folder = directory.Name;
                    newFolder.FolderPath = directory.FullName;
                    RecorrerCarpetas(newFolder, directory.Name);
                    RecorrerImages(newFolder, new DirectoryInfo(directory.FullName), directory.Name, new[] { "jpeg", "png", "jpg" });
                    _folders.Add(newFolder);
                }
            }
        }

        private void RecorrerCarpetas(FolderMap folder, string noParte)
        {
            //obtener directorio seleccionado
            var dir = new DirectoryInfo(folder.FolderPath);

            //recorrer carpetas del directorio seleccionado
            foreach (var carpeta in dir.GetDirectories())
            {
                var newFolder = new FolderMap
                {
                    Folder = carpeta.Name,
                    FolderPath = carpeta.FullName
                };

                //recorrer nuevo directorio en subcarpeta
                RecorrerCarpetas(newFolder, noParte);

                //agregar subdirectorio al directorio padre
                folder.Folders.Add(newFolder);
            }
            //leer archivos actuales
            RecorrerFiles(folder, dir, noParte, new[] { "pdf" });
        }

        private void RecorrerFiles(FolderMap folder, DirectoryInfo dir, string noParte, string[] extensions)
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
                        NameUrl = $"{noParte}\\{folder.Folder}\\{file.Name}",
                        NoParte = noParte
                    });
                }
            }
        }

        private void RecorrerImages(FolderMap folder, DirectoryInfo dir, string noParte, string[] extensions)
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
                        NameUrl = $"{folder.Folder}\\{file.Name}",
                        NoParte = noParte
                    });
                }
            }
        }
    }
}