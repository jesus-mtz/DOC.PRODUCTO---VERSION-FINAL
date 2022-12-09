using prueba.Models;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace DocumentacionProducto.Controllers
{

    public class HomeController : Controller
    {
        public List<string> items { get; set; }

        public MapFolder MapFolder { get; set; }

        private List<MapFile> _archivos = new List<MapFile>();

        public HomeController()
        {
            items = new List<string>();
            MapFolder = new MapFolder();
        }

        [HttpGet]
        
        public ActionResult Principal()
        {

            return View();
        }

        public ActionResult Menu(string patron)
        {
            if (string.IsNullOrWhiteSpace(patron))
            {
                return View(MapFolder);
            }
            string rootFolder = Server.MapPath("~/DOCUMENTACION/");
            var Resul = RecorrerCarpetas(rootFolder, patron, "");
            return View(Resul);
        }


        public ActionResult Documentacion(string patron)
        {
            if (string.IsNullOrWhiteSpace(patron))
            {
                return View(items);
            }
            string rootFolder = Server.MapPath("~/DOCUMENTACION/");
            RecorrerCarpetas2(rootFolder, patron);
            return View(items);
        }


        private MapFolder RecorrerCarpetas(string folder, string filePattern, string lastFolder)
        {
            MapFolder mapFolder = new MapFolder();
            var dir = new DirectoryInfo(folder);
            mapFolder.Folder = dir.Name;
            mapFolder.FolderPath = dir.FullName;

            string[] carpetas = Directory.GetDirectories(folder);
            foreach (var item in carpetas)
            {
                mapFolder.Folders.Add(RecorrerCarpetas(item, filePattern, mapFolder.Folder));
            }

            if (mapFolder.Folder.ToLower().Contains(filePattern.ToLower()))
            {
                FileInfo[] fileNames = dir.GetFiles($"*.pdf");
                foreach (var file in fileNames)
                {
                    mapFolder.Files.Add(new MapFile
                    {
                        Name = file.Name,
                        NamePath = file.FullName,
                        NameUrl = $"{file.Name}"
                    });
                }
            }
            return mapFolder;
        }

        private void RecorrerCarpetas2(string folder, string filePattern, string lastFolder)
        {
            var dir = new DirectoryInfo(folder);

            //if (dir.Name.ToLower().Trim().Contains(filePattern.ToLower()) || dir.Name.ToLower() ==filePattern.ToLower().Trim())
            //{
                FileInfo[] fileNames = dir.GetFiles($"*.pdf");
                foreach (var file in fileNames)
                {
                    _archivos.Add(new MapFile
                    {
                        Name = file.Name,
                        NamePath = file.FullName,
                        NameUrl = $"{file.Name}"
                    });
                }
            //}

            string[] carpetas = Directory.GetDirectories(folder);
            foreach (var item in carpetas)
            {
                RecorrerCarpetas(item, filePattern, folder);
            }
        }

        private void RecorrerCarpetas2(string folder, string filePattern)
        {
            string[] carpetas = Directory.GetDirectories(folder);
            foreach (var item in carpetas)
            {
                RecorrerCarpetas2(item, filePattern);
            }

            var dir = new DirectoryInfo(folder);

            FileInfo[] fileNames = dir.GetFiles($"*{filePattern}*.pdf");
            foreach (var file in fileNames)
            {
                items.Add(file.FullName);
            }
        }


    }
}