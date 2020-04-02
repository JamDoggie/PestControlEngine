using Newtonsoft.Json;
using PestControlEngine.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PestControlEngine.Mapping
{
    public static class ObjectExtract
    {
        /// <summary>
        /// Extracts information about all objects in the game using reflection. This info is compiled into a .PCOI file and is for use in the mapper.
        /// This method is very slow and only meant for developers who want to make an accurate .PCOI file for their game automatically.
        /// This method is not meant to be performed in real time, and only at the press of a button. It's also possible the output file could need some touching up.
        /// </summary>
        /// <param name="path">The path the file should be saved to. (leave blank for local path based on the directory of the game's .exe)</param>
        public static void ExtractInfo(string path)
        {
            // This is mainly the slowest part of this method.
            var gameObjects = (
                    from domainAssembly in AppDomain.CurrentDomain.GetAssemblies()
                    from assemblyType in domainAssembly.GetTypes()
                    where typeof(GameObject).IsAssignableFrom(assemblyType)
                    select assemblyType).ToArray();

            ObjectInformationFile infoFile = new ObjectInformationFile();

            foreach(Type t in gameObjects)
            {
                if (t != typeof(GameObject))
                {
                    Console.WriteLine($"Extracting info about \"{t.FullName}\"...");

                    GameObjectInfo objInfo = new GameObjectInfo();
                    GameObject dummyObject = ((GameObject)Activator.CreateInstance(t));
                    objInfo.Properties = dummyObject.Properties;
                    objInfo.ClassName = t.FullName;
                    objInfo.DefaultAnimation = dummyObject.CurrentAnimation;
                    infoFile.ObjectInfos.Add(objInfo);
                }
            }

            string output = JsonConvert.SerializeObject(infoFile, Formatting.Indented);

            File.WriteAllText(path, output);
        }
    }
}
